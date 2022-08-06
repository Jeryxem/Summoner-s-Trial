using UnityEngine;

public class M_Clockwork_Iron_Maiden : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        if (tile.occupyingUnit == null) { return; }
        
        if (tile.occupyingUnit.hp <= 800) { 
            tile.occupyingUnit.hp = 0; 
            
            Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

            GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
            effectInfo.GetComponent<FloatingInfoText>().CustomText("Insta Kill");
        }
    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}