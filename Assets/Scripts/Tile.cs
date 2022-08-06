using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public TileOwner tileOwner;
    public bool canMoveLeft, canMoveRight, canMoveUp, canMoveDown;
    public bool isOccupied;
    public int row, col;
    public SpriteRenderer borderColor;
    public SpriteRenderer tileColor;
    public GameObject summonIndicator;
    public Color selectableTileColor, defaultTileColor, playerDefendedTileColor, enemyDefendedTileColor;
    public bool tileIsSelectable = false;
    public bool tileIsDefended = false;
    public Moveable occupyingUnit; // Unit occupying the tile
    public Moveable defendingUnit; // Unit defending the tile

    private void Awake() {
        tileColor = this.GetComponent<SpriteRenderer>();
        ColorUtility.TryParseHtmlString("#969696", out selectableTileColor);
        ColorUtility.TryParseHtmlString("#333030", out defaultTileColor);
        ColorUtility.TryParseHtmlString("#00A6FF", out playerDefendedTileColor);
        ColorUtility.TryParseHtmlString("#FF0000", out enemyDefendedTileColor);
    }

    //constructor and set data
    public Tile()  
    {  

    }  
    public Tile(TileOwner _tileOwner, bool cmLeft, bool cmRight, bool cmUp, bool cmDown, bool _isOccupied, int _row, int _col)  
    {  
        tileOwner = _tileOwner;
        canMoveLeft = cmLeft;
        canMoveRight = cmRight;
        canMoveUp = cmUp;
        canMoveDown = cmDown;
        isOccupied = _isOccupied;
        row = _row;
        col = _col;
    }

    public void SetTileData(TileOwner _tileOwner, bool cmLeft, bool cmRight, bool cmUp, bool cmDown, bool _isOccupied, int _row, int _col) {
        tileOwner = _tileOwner;
        canMoveLeft = cmLeft;
        canMoveRight = cmRight;
        canMoveUp = cmUp;
        canMoveDown = cmDown;
        isOccupied = _isOccupied;
        row = _row;
        col = _col;

        if(tileOwner == TileOwner.Enemy) {
            borderColor.color = Color.red;
        }
    }
    //constructor and set data

    private void OnTriggerEnter2D(Collider2D other) {
        switch(other.gameObject.tag) {
            case "Moveable":
            case "Player":
            case "Boss":
                Moveable unit = other.GetComponent<Moveable>();
                unit.currentTile = this;
                if (!occupyingUnit) {
                    occupyingUnit = unit;
                    isOccupied = true;
                    // print(unit.gameObject.name + " enter empty tile");
                }
                break;
            case "NeighbourNode":
                other.GetComponent<NeighbourTileChecker>().tile = this;
                break;
            default:
                break;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == "Moveable" || other.gameObject.tag == "Player" || other.gameObject.tag == "Boss") {
            Moveable unit = other.GetComponent<Moveable>();
            unit.previousTile = this;
            if (occupyingUnit == unit) {
                occupyingUnit = null;
                isOccupied = false;
                // print(unit.gameObject.name + " exit empty tile");
            }
        }
    }

    private void OnMouseUp() {
        switch(MasterController.Instance.gamePhase) {
            case GamePhase.Player_Summon:
                if(summonIndicator.activeSelf && !CanvasManager.Instance.summoningPanel.activeSelf) {
                    MasterController.Instance.SummonMonster(this);
                    CanvasManager.Instance.cancelSummonButton.SetActive(false);
                }
                break;
            case GamePhase.Player_Command:
                if(tileIsSelectable) {
                    // reset tile after select
                    foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                        // 1. Set tile color back to normal, unless its a defended tile
                        if (!tile.tileIsDefended)
                            tile.SetTileDefault();
                        else if (tile.tileOwner == TileOwner.Player)
                            tile.SetTilePlayerDefended();
                        else if (tile.tileOwner == TileOwner.Enemy)
                            tile.SetTileEnemyDefended();
                            
                        // 2. Show remaining units with action points
                        Moveable[] playerSummonsOnField = FindObjectsOfType<Moveable>();
                        foreach(Moveable moveable in playerSummonsOnField) {
                            if (moveable.movableOwner == MovableOwner.Player) {
                                switch(moveable.commandState) {
                                    case CommandState.None:
                                        moveable.GetComponentInChildren<FloatingUI>().actionIndicator.SetActive(true);
                                        break;
                                    case CommandState.Attack:
                                        moveable.PlayerAttack(this);
                                        moveable.commandState = CommandState.ActionComplete;
                                        break;
                                    case CommandState.Capture:
                                        CaptureEnemyTile(moveable);
                                        moveable.commandState = CommandState.ActionComplete;
                                        break;
                                    case CommandState.Defend:
                                        SetTilePlayerDefended(moveable);
                                        moveable.commandState = CommandState.ActionComplete;
                                        break;
                                    case CommandState.Move:
                                        moveable.MoveToSelectedTile(this);
                                        moveable.commandState = CommandState.ActionComplete;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                    }
                }
                break;
            default:
                break;
        }
        
    }

    public void SetTileSelectable() {
        tileColor.color = selectableTileColor;
        tileIsSelectable = true;
    }

    public void SetTileDefault() {
        tileColor.color = defaultTileColor;
        tileIsSelectable = false;
        tileIsDefended = false;
        defendingUnit = null;
    }

    //capturing
    public void CaptureEnemyTile(Moveable _capturingUnit) {
        if (tileOwner == TileOwner.Player) { return; }

        tileOwner = TileOwner.Player;
        borderColor.color = playerDefendedTileColor;
        tileColor.color = defaultTileColor;
        
        _capturingUnit.summonsAbility.CaptureEffect(this);
        AudioManager.Instance.Play("Capture_Defend");

        if (tileIsDefended) {
            _capturingUnit.hp -= defendingUnit.attack;
            _capturingUnit.GetComponentInChildren<FloatingUI>().hp.text = _capturingUnit.hp.ToString();
            defendingUnit.animator.enabled = true;
            defendingUnit.animator.SetTrigger("EnemyAttack");
            AudioManager.Instance.Play("Attack");

            if( _capturingUnit.hp <= 0) {  _capturingUnit.Death(); }
            
            GameObject defendInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, defendingUnit.transform.position, Quaternion.identity);
            defendInfo.GetComponent<FloatingInfoText>().EnemyDefendingInfo();

            GameObject damageInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, _capturingUnit.transform.position, Quaternion.identity);
            damageInfo.GetComponent<FloatingInfoText>().DamageInfo(defendingUnit.attack);
        } else {
            GameObject captureInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, _capturingUnit.transform.position, Quaternion.identity);
            captureInfo.GetComponent<FloatingInfoText>().EnemyCapturingInfo();
        }

        tileIsDefended = false;
        defendingUnit = null;
    }

    public void CapturePlayerTile(Moveable _capturingUnit) {
        if (tileOwner == TileOwner.Enemy) { return; }

        tileOwner = TileOwner.Enemy;
        borderColor.color = Color.red;
        tileColor.color = defaultTileColor;

        _capturingUnit.summonsAbility.CaptureEffect(this);

        if (tileIsDefended) {
            _capturingUnit.hp -= defendingUnit.attack;
            _capturingUnit.GetComponentInChildren<FloatingUI>().hp.text = _capturingUnit.hp.ToString();
            defendingUnit.animator.enabled = true;
            defendingUnit.animator.SetTrigger("PlayerAttack");

            if( _capturingUnit.hp <= 0) {  _capturingUnit.Death(); }
            
            GameObject defendInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, defendingUnit.transform.position, Quaternion.identity);
            defendInfo.GetComponent<FloatingInfoText>().EnemyDefendingInfo();

            GameObject damageInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, _capturingUnit.transform.position, Quaternion.identity);
            damageInfo.GetComponent<FloatingInfoText>().DamageInfo(defendingUnit.attack);
        }

        tileIsDefended = false;
        defendingUnit = null;
    }
    //capturing

    //defending
    public void SetTilePlayerDefended() {
        tileColor.color = playerDefendedTileColor;
        tileIsSelectable = false;

        tileIsDefended = true;
    }

    public void SetTileEnemyDefended() {
        tileColor.color = enemyDefendedTileColor;
        tileIsSelectable = false;

        tileIsDefended = true;
    }

    public void SetTilePlayerDefended(Moveable _defendingUnit) {
        tileColor.color = playerDefendedTileColor;
        tileIsSelectable = false;

        tileIsDefended = true;
        defendingUnit = _defendingUnit;
        defendingUnit.defendedTile = this;

        _defendingUnit.summonsAbility.DefendEffect(this);
        
        AudioManager.Instance.Play("Capture_Defend");
    }

    public void SetTileEnemyDefended(Moveable _defendingUnit) {
        tileColor.color = enemyDefendedTileColor;
        tileIsSelectable = false;

        tileIsDefended = true;
        defendingUnit = _defendingUnit;
        defendingUnit.defendedTile = this;

        _defendingUnit.summonsAbility.DefendEffect(this);
    }
    //defending
}

public enum TileOwner {
    Player,
    Enemy
}
