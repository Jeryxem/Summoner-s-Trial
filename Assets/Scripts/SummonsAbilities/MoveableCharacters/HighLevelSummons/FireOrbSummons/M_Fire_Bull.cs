using UnityEngine;

public class M_Fire_Bull : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {
        if (tile.defendingUnit == null) { return; }

        Moveable self = GetComponent<Moveable>();
        int captureDamage = self.attack * 2;

        tile.defendingUnit.hp -= captureDamage;
        tile.defendingUnit.GetComponentInChildren<FloatingUI>().hp.text = tile.defendingUnit.hp.ToString();
        if (tile.defendingUnit.hp <= 0) { tile.defendingUnit.Death(); }

        Vector3 pos = new Vector3(tile.defendingUnit.transform.position.x, tile.defendingUnit.transform.position.y + 0.5f, tile.defendingUnit.transform.position.z);

        GameObject damageValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        damageValueInfo.GetComponent<FloatingInfoText>().DamageInfo(captureDamage);

        AudioManager.Instance.Play("Attack");
    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}