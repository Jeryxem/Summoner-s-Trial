using UnityEngine;

public abstract class SummonsAbility : MonoBehaviour
{
    public abstract  void SummonEffect();
    public abstract  void AttackEffect(Tile tile);
    public abstract  void CaptureEffect(Tile tile);
    public abstract  void DefendEffect(Tile tile);
    public abstract  void DeathEffect();
}
