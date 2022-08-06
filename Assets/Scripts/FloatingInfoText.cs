using UnityEngine;

public class FloatingInfoText : MonoBehaviour
{
    public TextMesh infoText;

    void Start()
    {
        Destroy(gameObject, 0.75f);
    }

    public void DamageInfo(int value) {
        infoText.text = value.ToString();
    }

    public void NoTargetToAttackInfo() {
        infoText.text = "No target...";
    }

    public void EnemyAttackingInfo() {
        infoText.text = "Attacking";
    }

    public void EnemyCapturingInfo() {
        infoText.text = "Capturing";
    }

    public void EnemyDefendingInfo() {
        infoText.text = "Defending";
    }

    public void EnemyMovingInfo() {
        infoText.text = "Moving";
    }

    public void CustomText(string text) {
        infoText.text = text;
    }
}
