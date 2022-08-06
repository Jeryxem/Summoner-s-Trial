using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingUI : MonoBehaviour
{
    private Moveable moveable;
    public TextMesh hp, attackValue;
    public GameObject actionUIPanel;
    public GameObject actionIndicator;

    private void Awake() {
        moveable = GetComponentInParent<Moveable>();

        hp.text = moveable.summon.hp.ToString();
        attackValue.text = moveable.summon.attack.ToString();
    }
}
