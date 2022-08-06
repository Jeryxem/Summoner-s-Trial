using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager Instance;
    public CurrentLevelBoss currentLevelBoss = CurrentLevelBoss.CrystalBoss;
    public MonsterList monsterList = MonsterList.Slimes;
    public Summon crystalBoss, fireOrbBoss, frostOrbBoss, thunderOrbBoss, windOrbBoss, magicCrystalBoss;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Start() {
        SpawnCurrentLevelBoss();
    }

    public void SpawnCurrentLevelBoss() {
        GameObject bossPrefab = null;
        switch (currentLevelBoss) {
            case CurrentLevelBoss.CrystalBoss:
                bossPrefab = crystalBoss.prefab;
                break;
            case CurrentLevelBoss.FireOrbBoss:
                bossPrefab = fireOrbBoss.prefab;
                break;
            case CurrentLevelBoss.FrostOrbBoss:
                bossPrefab = frostOrbBoss.prefab;
                break;
            case CurrentLevelBoss.ThunderOrbBoss:
                bossPrefab = thunderOrbBoss.prefab;
                break;
            case CurrentLevelBoss.WindOrbBoss:
                bossPrefab = windOrbBoss.prefab;
                break;
            case CurrentLevelBoss.MagicCrystalBoss:
                bossPrefab = magicCrystalBoss.prefab;
                break;
            default:
                break;
        }

        GameObject bossSpawn = Instantiate(bossPrefab, new Vector3(7.5f, -0.5f, 0), Quaternion.identity);
        bossSpawn.GetComponent<Moveable>().movableOwner = MovableOwner.Enemy;
    }
}

public enum CurrentLevelBoss {
    CrystalBoss,
    FireOrbBoss,
    FrostOrbBoss,
    ThunderOrbBoss,
    WindOrbBoss,
    MagicCrystalBoss
}

public enum MonsterList {
    Wisp,
    Goblins,
    Slimes,
    FireBuffing,
    FireBuffing2,
    FrostDebuff,
    FrostDebuff2,
    ThunderOrbSlimes,
    ThunderOrbMix,
    WindOrbEarthHealing,
    WindOrbForestMix,
    HighLevelFire,
    HighLevelFrost,
    HighLevelThunder,
    HighLevelWind
}
