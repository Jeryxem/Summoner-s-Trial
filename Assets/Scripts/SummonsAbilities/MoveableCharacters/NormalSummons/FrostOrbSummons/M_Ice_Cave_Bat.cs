using UnityEngine;

public class M_Ice_Cave_Bat : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        if (tile.occupyingUnit == null) { return; }
        
        tile.occupyingUnit.attack -= 200;
        if (tile.occupyingUnit.attack <= 0) { tile.occupyingUnit.attack = 0; }
        tile.occupyingUnit.GetComponentInChildren<FloatingUI>().attackValue.text = tile.occupyingUnit.attack.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Debuff");
        AudioManager.Instance.Play("Debuff");
    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}