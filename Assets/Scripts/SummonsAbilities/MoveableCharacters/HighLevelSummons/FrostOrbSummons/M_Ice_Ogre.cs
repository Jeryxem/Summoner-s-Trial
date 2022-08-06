using UnityEngine;

public class M_Ice_Ogre : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.hp -= 100;
        if (self.hp <= 0) { self.Death(); }
        self.GetComponentInChildren<FloatingUI>().hp.text = self.hp.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void CaptureEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.hp -= 100;
        if (self.hp <= 0) { self.Death(); }
        self.GetComponentInChildren<FloatingUI>().hp.text = self.hp.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void DefendEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.hp -= 100;
        if (self.hp <= 0) { self.Death(); }
        self.GetComponentInChildren<FloatingUI>().hp.text = self.hp.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void DeathEffect() {
        
    }
}