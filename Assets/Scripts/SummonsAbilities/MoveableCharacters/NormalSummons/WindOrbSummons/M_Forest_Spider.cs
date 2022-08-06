using UnityEngine;

public class M_Forest_Spider : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        
        self.attack += 100;
        self.GetComponentInChildren<FloatingUI>().attackValue.text = self.attack.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Buff");
    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}