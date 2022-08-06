using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeanTweenAnimationController : MonoBehaviour
{
    public static LeanTweenAnimationController Instance;

    [field: SerializeField] public CanvasRenderer phasePanel { get; set; }
    [field: SerializeField] public CanvasRenderer summoningPanel { get; set; }

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            LeanSummoningPanelOpen();
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            LeanSummoningPanelClose();
        }
    }

    public void LeanPhasePanel() {
        phasePanel.gameObject.transform.localPosition = new Vector2(0, -Screen.height -100f);
        phasePanel.gameObject.LeanMoveLocalY(0, 0.25f).setEaseOutExpo().delay = 0.1f;
        phasePanel.gameObject.LeanMoveLocalY(-Screen.height -100f, 0.25f).setEaseOutExpo().delay = 1.25f;
    }

    public void LeanSummoningPanelOpen() {
        summoningPanel.gameObject.transform.localPosition = new Vector2(Screen.width + 1000f, 0);
        summoningPanel.gameObject.LeanMoveLocalX(0, 0.25f).setEaseOutExpo().delay = 0.1f;
    }
    public void LeanSummoningPanelClose() {
        summoningPanel.gameObject.LeanMoveLocalX(Screen.width + 1000f, 0.25f).setEaseOutExpo().delay = 0.1f;
    }

    public void LeanSummoningIndicatorAnimation(GameObject go) {
        //Color hex code - #FDFF9C, rgb - (255,255,155) - yellow
        //Color hex code - #B9FFFF, rgb - (185,255,255) - blue
        
        Color newColor;
        ColorUtility.TryParseHtmlString("#B9FFFF", out newColor);
        go.LeanColor(newColor, .75f).setLoopPingPong();
    }

    public void LeanActionIndicatorAnimation(GameObject go) {
        Vector3 target = new Vector3(0.25f, 0.25f, 1f);
        go.LeanScale(target, .25f).setLoopPingPong();
    }

    public void LeanCancelAnimation(GameObject go) {
        go.LeanCancel();
    }
}
