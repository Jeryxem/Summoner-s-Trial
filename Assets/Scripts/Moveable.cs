using UnityEngine;

public class Moveable : MonoBehaviour
{
    public Animator animator;
    float lerpTime = .25f;
    float currentLerpTime;
 
    Vector3 startPos;
    Vector3 endPos;

    public MovableOwner movableOwner;
    public Tile currentTile;
    public Tile previousTile;
    public NeighbourTileChecker top;
    public NeighbourTileChecker right;
    public NeighbourTileChecker left;
    public NeighbourTileChecker bottom;
    private SpriteRenderer spriteRenderer;

    [Header("Summon Data")]
    public Summon summon;
    public int hp;
    public int attack;
    public int moveDistance;
    public int attackDistance;
    public SummonsAbility summonsAbility;

    [Header("Current Turn Status")]
    public CommandState commandState;
    public Tile defendedTile = null;

    private void Awake() {
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        UpdateSortingOrder();

        hp = summon.hp;
        attack = summon.attack;
        moveDistance = summon.moveDistance;
        attackDistance = summon.attackDistance;

        animator = GetComponent<Animator>();
        animator.enabled = false;
    }
    private void Start() {
        startPos = transform.position;
        endPos = transform.position;

        if (movableOwner == MovableOwner.Player) {
            GetComponentInChildren<SpriteRenderer>().flipX = true;
        }

        //Summon Effect
        summonsAbility = GetComponent<SummonsAbility>();
        summonsAbility.SummonEffect();
    }
    void Update()
    {   
        //increment timer once per frame
        currentLerpTime += Time.deltaTime;
        if (currentLerpTime > lerpTime) {
            currentLerpTime = lerpTime;
        }
 
        //lerp!
        float perc = currentLerpTime / lerpTime;
        transform.position = Vector3.Lerp(startPos, endPos, perc);
    }

    private void UpdateSortingOrder() {
        switch(transform.position.y) {
            case 1.5f:
                spriteRenderer.sortingOrder = 9;
                break;
            case -0.5f:
                spriteRenderer.sortingOrder = 10;
                break;
            case -2.5f:
                spriteRenderer.sortingOrder = 11;
                break;
            default:
                break;
        }
    }

    public void MoveToStartingTile() {
        currentLerpTime = 0f;
        startPos = transform.position;
        endPos = new Vector3(-7.5f, -0.5f, 0f);
        UpdateSortingOrder();
    }

    public void MoveToSelectedTile(Tile endTilePos) {
        currentLerpTime = 0f;
        startPos = transform.position;
        endPos = endTilePos.transform.position;
        UpdateSortingOrder();
    }

    public void Death() {
        if (gameObject.tag == "Boss") {
            CanvasManager.Instance.endTurnButton.SetActive(false);
            MasterController.Instance.Invoke("LevelComplete", 0.1f);
        }
        
        if (gameObject.tag == "Player") {
            MasterController.Instance.GameOver();
            return;
        }

        // Update tile
        currentTile.isOccupied = false;
        currentTile.tileIsSelectable = false;
        currentTile.occupyingUnit = null;

        //Check for defending unit that dies and update tile color
        if (defendedTile && defendedTile.defendingUnit == this) {
            defendedTile.tileColor.color = defendedTile.defaultTileColor;
            defendedTile.tileIsDefended = false;
            defendedTile.defendingUnit = null;
        }

        //TODO: spawn death effect
        summonsAbility.DeathEffect();
        Destroy(gameObject);
    }

    public void DisableAnimationAfterPlay() {
        animator.enabled = false;
    }

    /// <summary>
    /// Player Command Actions
    /// </summary>
    public void PlayerAttack(Tile tileAttacked) {
        animator.enabled = true;
        animator.SetTrigger("PlayerAttack");
        AudioManager.Instance.Play("Attack");

        // ability effect
        summonsAbility.AttackEffect(tileAttacked);

        if (tileAttacked.occupyingUnit) {
            GameObject info = Instantiate(MasterController.Instance.floatingInfoTextPrefab, tileAttacked.transform.position, Quaternion.identity);
            info.GetComponent<FloatingInfoText>().DamageInfo(attack);
            tileAttacked.occupyingUnit.hp -= attack;
            tileAttacked.occupyingUnit.GetComponentInChildren<FloatingUI>().hp.text = tileAttacked.occupyingUnit.hp.ToString();

            if(tileAttacked.occupyingUnit.hp <= 0) {
                tileAttacked.occupyingUnit.Death();
            }
        } else {
            GameObject info = Instantiate(MasterController.Instance.floatingInfoTextPrefab, tileAttacked.transform.position, Quaternion.identity);
            info.GetComponent<FloatingInfoText>().NoTargetToAttackInfo();
        }
    }

    /// <summary>
    /// Enemy Command Actions
    /// </summary>
    public void EnemyAttack() {
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col
        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x)
                + Mathf.Abs(transform.position.y - tile.transform.position.y) <= attackDistance * GridGenerator.WIDTH
                && tile.transform.position != transform.position
                && tile.isOccupied
                && tile.tileOwner == TileOwner.Player) {
                    commandState = CommandState.Attack;
                    // print(gameObject.name + " enemy attack");
                    animator.enabled = true;
                    animator.SetTrigger("EnemyAttack");
                    AudioManager.Instance.Play("Attack");

                    //attack effect
                    summonsAbility.AttackEffect(tile);

                    //Info, animation, etc
                    GameObject info = Instantiate(MasterController.Instance.floatingInfoTextPrefab, tile.transform.position, Quaternion.identity);
                    info.GetComponent<FloatingInfoText>().DamageInfo(attack);
                    tile.occupyingUnit.hp -= attack;
                    tile.occupyingUnit.GetComponentInChildren<FloatingUI>().hp.text = tile.occupyingUnit.hp.ToString();

                    GameObject actionStateInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, transform.position, Quaternion.identity);
                    actionStateInfo.GetComponent<FloatingInfoText>().EnemyAttackingInfo();

                    if(tile.occupyingUnit.hp <= 0) {
                        tile.occupyingUnit.GetComponentInChildren<FloatingUI>().hp.text = "0";
                        tile.occupyingUnit.Death();
                    }
                    break;
            }
        }

        // No attackable units
        if (commandState == CommandState.None) {
            if (attackDistance <= 0) { 
                print("attack distance not set, check attack distance: " + attackDistance);
                return; 
            }
            EnemyCapture();
        }
    }

    public void EnemyCapture() {
        int captureDistance = 1;
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x)
                + Mathf.Abs(transform.position.y - tile.transform.position.y) <= captureDistance * GridGenerator.WIDTH
                && tile.transform.position != transform.position
                && !tile.isOccupied
                && tile.tileOwner == TileOwner.Player) {
                    if (!tile.tileIsDefended) {
                        GameObject actionStateInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, transform.position, Quaternion.identity);
                        actionStateInfo.GetComponent<FloatingInfoText>().EnemyCapturingInfo();
                    }
                    
                    commandState = CommandState.Capture;
                    tile.CapturePlayerTile(this);
                    // print(gameObject.name + " enemy capture");
                    AudioManager.Instance.Play("Capture_Defend");
                    break;
            }
        }

        // No captureable tiles
        if (commandState == CommandState.None && gameObject.tag == "Moveable") {
            int rand = UnityEngine.Random.Range(0,100);
            if (rand < 50) {
                EnemyDefend();
            } else {
                EnemyMove();
            }
        } else if (commandState == CommandState.None && gameObject.tag == "Boss") {
            EnemyDefend();
        }
    }

    public void EnemyDefend() {
        int defendDistance = 1;
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col
        
        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x)
                + Mathf.Abs(transform.position.y - tile.transform.position.y) <= defendDistance * GridGenerator.WIDTH
                && tile.transform.position != transform.position
                && tile.tileOwner == TileOwner.Enemy
                && tile.transform.position.x < transform.position.x) { //defend left tile only for now
                    commandState = CommandState.Defend;
                    tile.SetTileEnemyDefended(this);
                    // print(gameObject.name + " enemy defend");
                    AudioManager.Instance.Play("Capture_Defend");

                    GameObject actionStateInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, transform.position, Quaternion.identity);
                    actionStateInfo.GetComponent<FloatingInfoText>().EnemyDefendingInfo();
                    break;
            }
        }
    }

    public void EnemyMove() {
        int moveDistance = 1;
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col
        
        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(transform.position.x - tile.transform.position.x)
                + Mathf.Abs(transform.position.y - tile.transform.position.y) <= moveDistance * GridGenerator.WIDTH
                && tile.transform.position != transform.position
                && tile.tileOwner == TileOwner.Enemy
                && !tile.isOccupied) {
                    commandState = CommandState.Move;
                    MoveToSelectedTile(tile);
                    // print(gameObject.name + " enemy move");

                    GameObject actionStateInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, transform.position, Quaternion.identity);
                    actionStateInfo.GetComponent<FloatingInfoText>().EnemyMovingInfo();
                    break;
            }
        }

        // No moveable tiles
        if (commandState == CommandState.None) {
            // print("no moveable tile so defend instead");
            EnemyDefend();
        }
    }
}

public enum MovableOwner {
    Player,
    Enemy
}

public enum CommandState {
    None,
    Attack,
    Capture,
    Defend,
    Move,
    ActionComplete
}
