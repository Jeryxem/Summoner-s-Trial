using UnityEngine;

public class M_Forest_Golem : SummonsAbility
{
    public override void SummonEffect() {
        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();

        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner == self.movableOwner && moveable != self) {
                if (moveable.gameObject.tag == "Player" || moveable.gameObject.tag == "Boss") {
                    moveable.hp -= 500;
                    if (moveable.hp <= 0) { moveable.hp = 1; }
                    moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();
                    
                    Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                    GameObject healValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                    healValueInfo.GetComponent<FloatingInfoText>().CustomText("500");
                }
            }
        }
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
                if (moveable.gameObject.tag == "Player" || moveable.gameObject.tag == "Boss") {
                    moveable.hp += 500;
                    moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();
                    
                    Vector3 moveablePos = new Vector3(moveable.transform.position.x, moveable.transform.position.y + 0.5f, moveable.transform.position.z);

                    GameObject healValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveablePos, Quaternion.identity);
                    healValueInfo.GetComponent<FloatingInfoText>().CustomText("500");
                }
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Heal");
    }
}