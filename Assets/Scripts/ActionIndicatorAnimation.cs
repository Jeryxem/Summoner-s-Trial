using UnityEngine;

public class ActionIndicatorAnimation : MonoBehaviour
{
    private void OnEnable() {
        LeanTweenAnimationController.Instance.LeanActionIndicatorAnimation(this.gameObject);
    }

    private void OnDisable() {
        LeanTweenAnimationController.Instance.LeanCancelAnimation(this.gameObject);
        this.gameObject.transform.localScale = new Vector3(.5f, .5f, 1f);
    }
}
