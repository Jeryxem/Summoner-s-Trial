using UnityEngine;

public class SummoningIndicatorAnimation : MonoBehaviour
{
    private void OnEnable() {
        LeanTweenAnimationController.Instance.LeanSummoningIndicatorAnimation(this.gameObject);
    }
}
