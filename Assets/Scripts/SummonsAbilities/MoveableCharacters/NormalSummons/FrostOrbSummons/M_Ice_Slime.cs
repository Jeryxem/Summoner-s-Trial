using UnityEngine;

public class M_Ice_Slime : SummonsAbility
{
    public override void SummonEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.attack -= 100;
                if (moveable.attack <= 0) { moveable.attack = 0; }
                moveable.GetComponentInChildren<FloatingUI>().attackValue.text = moveable.attack.ToString();

                Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().CustomText("100");
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Debuff All");
        AudioManager.Instance.Play("Debuff");
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