using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseClickEventController : MonoBehaviour
{
    public static MouseClickEventController Instance;
    private Camera mainCamera;

    private Ray ray;
    private RaycastHit2D[] hits2D;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        mainCamera = Camera.main;
    }

    private void Update() {
        if (Input.GetMouseButtonUp(0)) {
            ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            hits2D = Physics2D.GetRayIntersectionAll(ray);
            for (int i = 0; i < hits2D.Length; i++) {
                if (hits2D[i].collider != null) {
                    AudioManager.Instance.Play("Click");
                    if(hits2D[i].collider.tag == "AttackAction")
                        AttackAction(hits2D, i);
                    if(hits2D[i].collider.tag == "CaptureAction")
                        CaptureAction(hits2D, i);
                    if(hits2D[i].collider.tag == "DefendAction")
                        DefendAction(hits2D, i);
                    if(hits2D[i].collider.tag == "MoveAction")
                        MoveAction(hits2D, i);
                    if(hits2D[i].collider.tag == "ActionIndicator")
                        OpenActionPanel(hits2D, i);
                }
            }
        }   
    }

    public void AttackAction(RaycastHit2D[] rayHits2d, int index) {
        Moveable selectedMoveableUnit = rayHits2d[index].collider.gameObject.GetComponentInParent<Moveable>();
        selectedMoveableUnit.commandState = CommandState.Attack;

        FloatingUI floatingUI = rayHits2d[index].collider.gameObject.GetComponentInParent<FloatingUI>();
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col
        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(selectedMoveableUnit.transform.position.x - tile.transform.position.x)
                + Mathf.Abs(selectedMoveableUnit.transform.position.y - tile.transform.position.y) <= selectedMoveableUnit.attackDistance * GridGenerator.WIDTH
                && tile.transform.position != selectedMoveableUnit.transform.position) {
                tile.SetTileSelectable();
                floatingUI.actionUIPanel.SetActive(false);
            }
        }
    }

    public void CaptureAction(RaycastHit2D[] rayHits2d, int index) {
        Moveable selectedMoveableUnit = rayHits2d[index].collider.gameObject.GetComponentInParent<Moveable>();
        selectedMoveableUnit.commandState = CommandState.Capture;

        FloatingUI floatingUI = rayHits2d[index].collider.gameObject.GetComponentInParent<FloatingUI>();
        int captureDistance = 1;
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(selectedMoveableUnit.transform.position.x - tile.transform.position.x)
                + Mathf.Abs(selectedMoveableUnit.transform.position.y - tile.transform.position.y) <= captureDistance * GridGenerator.WIDTH
                && tile.transform.position != selectedMoveableUnit.transform.position
                && tile.tileOwner == TileOwner.Enemy
                && !tile.isOccupied) {
                tile.SetTileSelectable();
                floatingUI.actionUIPanel.SetActive(false);
            }
        }
    }

    public void DefendAction(RaycastHit2D[] rayHits2d, int index) {
        Moveable selectedMoveableUnit = rayHits2d[index].collider.gameObject.GetComponentInParent<Moveable>();
        selectedMoveableUnit.commandState = CommandState.Defend;

        FloatingUI floatingUI = rayHits2d[index].collider.gameObject.GetComponentInParent<FloatingUI>();
        int defendDistance = 1;
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(selectedMoveableUnit.transform.position.x - tile.transform.position.x)
                + Mathf.Abs(selectedMoveableUnit.transform.position.y - tile.transform.position.y) <= defendDistance * GridGenerator.WIDTH
                && tile.transform.position != selectedMoveableUnit.transform.position
                && tile.tileOwner == TileOwner.Player) {
                tile.SetTileSelectable();
                floatingUI.actionUIPanel.SetActive(false);
            }
        }
    }

    public void MoveAction(RaycastHit2D[] rayHits2d, int index) {
        Moveable selectedMoveableUnit = rayHits2d[index].collider.gameObject.GetComponentInParent<Moveable>();
        selectedMoveableUnit.commandState = CommandState.Move;

        FloatingUI floatingUI = rayHits2d[index].collider.gameObject.GetComponentInParent<FloatingUI>();
        // GridGenerator.WIDTH = 3
        // GridGenerator.HEIGHT = 2
        // Use higher value for abs for calculation when grid has uneven row/col
        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (Mathf.Abs(selectedMoveableUnit.transform.position.x - tile.transform.position.x)
                + Mathf.Abs(selectedMoveableUnit.transform.position.y - tile.transform.position.y) <= selectedMoveableUnit.moveDistance * GridGenerator.WIDTH
                && tile.transform.position != selectedMoveableUnit.transform.position
                && tile.tileOwner == TileOwner.Player
                && !tile.isOccupied) {
                tile.SetTileSelectable();
                floatingUI.actionUIPanel.SetActive(false);
            }
        }
    }

    public void OpenActionPanel(RaycastHit2D[] rayHits2d, int index) {
        FloatingUI floatingUI = rayHits2d[index].collider.gameObject.GetComponentInParent<FloatingUI>();
        floatingUI.actionUIPanel.SetActive(true);
        floatingUI.actionIndicator.SetActive(false);

        Moveable[] playerSummonsOnField = FindObjectsOfType<Moveable>();
        foreach(Moveable moveable in playerSummonsOnField) {
            if (moveable.movableOwner == MovableOwner.Player) {
                moveable.GetComponentInChildren<FloatingUI>().actionIndicator.SetActive(false);
            }
        }
    }
}
