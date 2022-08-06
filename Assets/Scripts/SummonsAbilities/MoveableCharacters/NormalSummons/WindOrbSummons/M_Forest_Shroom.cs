using UnityEngine;


public class M_Forest_Shroom : SummonsAbility
{
    public override void SummonEffect() {
        Invoke("DelayEffect", 1.55f);
    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }

    private void DelayEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();

        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner) {
                if (moveable.gameObject.tag == "Player" || moveable.gameObject.tag == "Boss") {
                    moveable.hp -= 100 * unitsInField.Length;
                    if (moveable.hp <= 0) { moveable.Death(); }
                    moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                    Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                    GameObject damageValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                    damageValueInfo.GetComponent<FloatingInfoText>().DamageInfo(100 * unitsInField.Length);
                }
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Sneak Attack");
        AudioManager.Instance.Play("Attack");
    }
}