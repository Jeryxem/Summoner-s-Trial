using UnityEngine;

public class M_Forest_Spora : SummonsAbility
{
    public override void SummonEffect() {
        Moveable self = GetComponent<Moveable>();
        self.moveDistance = 0;
    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner == self.movableOwner && moveable != self) {
                moveable.hp += 600;
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                GameObject healValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                healValueInfo.GetComponent<FloatingInfoText>().CustomText("600");
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Heal All");
    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.hp -= 200;
                if (moveable.hp <= 0) { moveable.Death(); }
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                GameObject damageValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                damageValueInfo.GetComponent<FloatingInfoText>().DamageInfo(200);
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Damage All");
        AudioManager.Instance.Play("Attack");
    }
}