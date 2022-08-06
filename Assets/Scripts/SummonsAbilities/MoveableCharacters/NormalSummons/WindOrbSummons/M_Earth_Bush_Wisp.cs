using UnityEngine;

public class M_Earth_Bush_Wisp : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner == self.movableOwner && moveable != self) {
                moveable.hp += 200;
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();
                moveable.attack += 200;
                moveable.GetComponentInChildren<FloatingUI>().attackValue.text = moveable.attack.ToString();

                Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                GameObject buffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                buffValueInfo.GetComponent<FloatingInfoText>().CustomText("200");

                break;
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Buff");
        AudioManager.Instance.Play("Heal_Buff");
    }
}