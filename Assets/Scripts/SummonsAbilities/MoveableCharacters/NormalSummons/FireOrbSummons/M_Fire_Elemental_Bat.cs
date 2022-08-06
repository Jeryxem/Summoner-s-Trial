using UnityEngine;
public class M_Fire_Elemental_Bat : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        if (tile.occupyingUnit == null) { return; }

        Moveable self = GetComponent<Moveable>();
        self.hp += self.attack / 2;
        self.GetComponentInChildren<FloatingUI>().hp.text = self.hp.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Drain");
    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}