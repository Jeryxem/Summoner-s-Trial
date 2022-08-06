using UnityEngine;

public class M_Forest_Darkluff : SummonsAbility
{
    public override void SummonEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        
        self.hp += (100 * unitsInField.Length);
        self.GetComponentInChildren<FloatingUI>().hp.text = self.hp.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Hp Up");
        AudioManager.Instance.Play("Heal_Buff");
    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}