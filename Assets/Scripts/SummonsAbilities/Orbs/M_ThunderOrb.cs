using UnityEngine;

public class M_ThunderOrb : SummonsAbility
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
                moveable.hp -= 200;
                if (moveable.hp <= 0) { moveable.Death(); }
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().DamageInfo(200);

                break;
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Sneak Attack");

        abilityCountdown = 0;
        AudioManager.Instance.Play("Attack");
    }
    public override void CaptureEffect(Tile tile) {
        if (abilityCountdown == 0) { abilityCountdown++; return;}

        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.hp -= 200;
                if (moveable.hp <= 0) { moveable.Death(); }
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().DamageInfo(200);

                break;
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Sneak Attack");

        abilityCountdown = 0;
        AudioManager.Instance.Play("Attack");
    }
    public override void DefendEffect(Tile tile) {
        if (abilityCountdown == 0) { abilityCountdown++; return;}

        Moveable self = GetComponent<Moveable>();
        Moveable[] unitsInField = FindObjectsOfType<Moveable>();
        
        foreach (Moveable moveable in unitsInField) {
            if (moveable.movableOwner != self.movableOwner && moveable != self) {
                moveable.hp -= 200;
                if (moveable.hp <= 0) { moveable.Death(); }
                moveable.GetComponentInChildren<FloatingUI>().hp.text = moveable.hp.ToString();

                GameObject deBuffValueInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, moveable.transform.position, Quaternion.identity);
                deBuffValueInfo.GetComponent<FloatingInfoText>().DamageInfo(200);

                break;
            }
        }

        Vector3 pos = new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z);

        GameObject effectInfo = Instantiate(MasterController.Instance.floatingInfoTextPrefab, pos, Quaternion.identity);
        effectInfo.GetComponent<FloatingInfoText>().CustomText("Sneak Attack");

        abilityCountdown = 0;
        AudioManager.Instance.Play("Attack");
    }
    public override void DeathEffect() {
        
    }
}