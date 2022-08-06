using UnityEngine;

public class M_FrostOrb : SummonsAbility
{
    int abilityCountdown = 0;
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {
        if (abilityCountdown == 0) { abilityCountdown++; return;}

        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.attack -= 100;
                if (moveable.attack <= 0) { moveable.attack = 0; }
                moveable.GetComponentInChildren<FloatingUI>().attackValue.text = moveable.attack.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().CustomText("100");
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Debuff All");

        abilityCountdown = 0;
    }
    public override void CaptureEffect(Tile tile) {
        if (abilityCountdown == 0) { abilityCountdown++; return;}

        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.attack -= 100;
                if (moveable.attack <= 0) { moveable.attack = 0; }
                moveable.GetComponentInChildren<FloatingUI>().attackValue.text = moveable.attack.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().CustomText("100");
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Debuff All");

        abilityCountdown = 0;
    }
    public override void DefendEffect(Tile tile) {
        if (abilityCountdown == 0) { abilityCountdown++; return;}

        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.attack -= 100;
                if (moveable.attack <= 0) { moveable.attack = 0; }
                moveable.GetComponentInChildren<FloatingUI>().attackValue.text = moveable.attack.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().CustomText("100");
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Debuff All");
        AudioManager.Instance.Play("Debuff");

        abilityCountdown = 0;
    }
    public override void DeathEffect() {
        
    }
}