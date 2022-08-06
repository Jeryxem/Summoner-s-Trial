using UnityEngine;

public class M_Fire_Ogre : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.attack -= 100;
        if (self.attack <= 0) { self.attack = 0; }
        self.GetComponentInChildren<FloatingUI>().attackValue.text = self.attack.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void CaptureEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.attack -= 100;
        if (self.attack <= 0) { self.attack = 0; }
        self.GetComponentInChildren<FloatingUI>().attackValue.text = self.attack.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void DefendEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.attack -= 100;
        if (self.attack <= 0) { self.attack = 0; }
        self.GetComponentInChildren<FloatingUI>().attackValue.text = self.attack.ToString();

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Weaken");
    }
    public override void DeathEffect() {
        
    }
}