using UnityEngine;

[CreateAssetMenu(fileName = "New Summon", menuName = "Summon")]
public class Summon : ScriptableObject
{
    public Sprite sprite;
    public new string name;
    public string description;
    public GameObject prefab;

    public int hp;
    public int attack;
    public int moveDistance;
    public int attackDistance;
    public string summonEffect;
    public string attackEffect;
    public string captureEffect;
    public string defendEffect;
    public string deathEffect;
}
