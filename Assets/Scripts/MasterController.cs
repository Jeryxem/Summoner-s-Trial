using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using System;

public class MasterController : MonoBehaviour
{
    public static MasterController Instance;

    public GamePhase gamePhase;
    public int currentSceneNumber = 1; // 0 = maine menu

    [Header("Summons List")]
    public List<Summon> fireSummons = new List<Summon>();
    public List<Summon> frostSummons = new List<Summon>();
    public List<Summon> thunderSummons = new List<Summon>();
    public List<Summon> windSummons = new List<Summon>();
    public List<Summon> crystalSummons = new List<Summon>();
    public List<Summon> highLevelFireSummons = new List<Summon>();
    public List<Summon> highLevelFrostSummons = new List<Summon>();
    public List<Summon> highLevelThunderSummons = new List<Summon>();
    public List<Summon> highLevelWindSummons = new List<Summon>();
    public List<Summon> highLevelCrystalSummons = new List<Summon>();
    public PossibleSummon possibleFireSummon;
    public PossibleSummon possibleFrostSummon;
    public PossibleSummon possibleThunderSummon;
    public PossibleSummon possibleWindSummon;
    public PossibleSummon possibleCrystalSummon;
    public PossibleSummon possibleHighLevelFireSummon;
    public PossibleSummon possibleHighLevelFrostSummon;
    public PossibleSummon possibleHighLevelThunderSummon;
    public PossibleSummon possibleHighLevelWindSummon;
    public PossibleSummon possibleHighLevelCrystalSummon;

    public struct PossibleSummon {
        public int choice1, choice2, choice3;

        public PossibleSummon(int c1, int c2, int c3) {
            choice1 = c1;
            choice2 = c2;
            choice3 = c3;
        }
    }

    [Header("Summoning Inventory")]
    public int fireOrb;
    public int frostOrb;
    public int thunderOrb;
    public int windOrb;
    public int crystal;
    public int magicCrystal;
    public OrbsAndCrystal selectedOrbOrCrystal;
    public bool highLevelSummonning;

    [Header("Misc/Reusable Prefabs")]
    public GameObject floatingUIPrefab;
    public GameObject floatingInfoTextPrefab;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    private void Update() {
        // if (Input.GetKeyDown(KeyCode.Alpha1))
        // {
        //     if (CanvasManager.Instance.phasePanel.activeSelf) return;
        //     NextPhase();
        // }
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     UpdateCurrentTurnSummonable(14);
        // }
    }

    private void Start() {
        NextPhase();
    }

    public void NextPhase() {
        if (CanvasManager.Instance.phasePanel.activeSelf) { CanvasManager.Instance.phasePanel.SetActive(false); }

        gamePhase = gamePhase.Next();
        
        CanvasManager.Instance.OnEnablePhasePanel();
        LeanTweenAnimationController.Instance.LeanPhasePanel();
        CanvasManager.Instance.Invoke("OnDisablePhasePanel", 1.5f);

        switch(gamePhase) {
            case GamePhase.Player_Summon:
                //Normal
                possibleFireSummon = UpdateCurrentTurnSummonable(fireSummons.Count);
                possibleFrostSummon = UpdateCurrentTurnSummonable(frostSummons.Count);
                possibleThunderSummon = UpdateCurrentTurnSummonable(thunderSummons.Count);
                possibleWindSummon = UpdateCurrentTurnSummonable(windSummons.Count);
                possibleCrystalSummon = UpdateCurrentTurnSummonable(crystalSummons.Count);
                //HighLevel
                possibleHighLevelFireSummon = UpdateCurrentTurnSummonable(highLevelFireSummons.Count);
                possibleHighLevelFrostSummon = UpdateCurrentTurnSummonable(highLevelFrostSummons.Count);
                possibleHighLevelThunderSummon = UpdateCurrentTurnSummonable(highLevelThunderSummons.Count);
                possibleHighLevelWindSummon = UpdateCurrentTurnSummonable(highLevelWindSummons.Count);
                possibleHighLevelCrystalSummon = UpdateCurrentTurnSummonable(highLevelCrystalSummons.Count);
                CanvasManager.Instance.Invoke("OnEnableSummoningPanel", 2f);
                break;
            case GamePhase.Player_Command:
                Invoke("PlayerCommand", 1.5f);
                break;
            case GamePhase.Enemy_Turn:
                // Remove all enemy defended tile and set default
                foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                    if (tile.tileIsDefended && tile.defendingUnit.movableOwner == MovableOwner.Enemy){
                        tile.SetTileDefault();
                    }
                }
                Invoke("NextPhase", 2f);
                break;
            case GamePhase.Enemy_Summon:
                Invoke("EnemySummon", 2f);
                break;
            case GamePhase.Enemy_Command:
                StartCoroutine(DelayBeforeEnemyCommandTurn());
                break;
            case GamePhase.Player_Turn:
                // Remove all enemy defended tile and set default
                foreach (Tile tile in GridGenerator.Instance.generatedTiles) {
                    if (tile.tileIsDefended && tile.defendingUnit.movableOwner == MovableOwner.Player) {
                        tile.SetTileDefault();
                    }
                }
                gamePhase = GamePhase.Loading_Level;
                Invoke("NextPhase", 2f);
                break;
            case GamePhase.Next_Level:
                break;
            case GamePhase.Loading_Level:
                break;
            default:
                break;
        }
    }

    private PossibleSummon UpdateCurrentTurnSummonable(int _max) {
        List<int> uniqueNumArray = new List<int>();
        int min = 0, max = _max;

        int rand = UnityEngine.Random.Range(min, max);
        uniqueNumArray.Add(rand);

        int rand2 = UnityEngine.Random.Range(min, max);
        while (uniqueNumArray.Contains(rand2)) {
            rand2 = UnityEngine.Random.Range(min, max);
        }
        uniqueNumArray.Add(rand2);
        // if (rand == rand2)
        //     rand = min + (rand - min + UnityEngine.Random.Range(min, max - min)) % (max - min);

        int rand3 = UnityEngine.Random.Range(min, max);
        while (uniqueNumArray.Contains(rand3)) {
            rand3 = UnityEngine.Random.Range(min, max);
        }

        PossibleSummon possibleSummon = new PossibleSummon(rand, rand2, rand3);

        return possibleSummon;
    }

    public void PlayerSummon() {
        if ((highLevelSummonning && magicCrystal <= 0) ||
            (selectedOrbOrCrystal == OrbsAndCrystal.Fire && fireOrb <= 0) || 
            (selectedOrbOrCrystal == OrbsAndCrystal.Frost && frostOrb <= 0) || 
            (selectedOrbOrCrystal == OrbsAndCrystal.Thunder && thunderOrb <= 0) || 
            (selectedOrbOrCrystal == OrbsAndCrystal.Wind && windOrb <= 0) || 
            (selectedOrbOrCrystal == OrbsAndCrystal.Crystal && crystal <= 0)) { return; } // no magic crystal, prevent summon

        StartCoroutine(CanvasManager.Instance.OnDisableSummoningPanel());
        CanvasManager.Instance.cancelSummonButton.SetActive(true);

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            if (tile.tileOwner == TileOwner.Player && !tile.isOccupied) {
                tile.summonIndicator.SetActive(true);
            }
        }
        AudioManager.Instance.Play("Click");
    }
    public void CancelSummon() {
        CanvasManager.Instance.OnEnableSummoningPanel();
        CanvasManager.Instance.cancelSummonButton.SetActive(false);

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            tile.summonIndicator.SetActive(false);
        }
    }

    public void SkipSummon() {
        StartCoroutine(CanvasManager.Instance.OnDisableSummoningPanel());
        Invoke("NextPhase", 0.1f);
        AudioManager.Instance.Play("Click");
    }

    public void SummonMonster(Tile selectedTile) {
        GameObject summonPrefab = null;

        switch(selectedOrbOrCrystal) {
            case OrbsAndCrystal.Fire:
                fireOrb--;
                switch(CanvasManager.Instance.selectedSummonChoice) {
                    case 1:
                        summonPrefab = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].prefab;
                        break;
                    case 2:
                        summonPrefab = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].prefab;
                        break;
                    case 3:
                        summonPrefab = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].prefab;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Frost:
                frostOrb--;
                switch(CanvasManager.Instance.selectedSummonChoice) {
                    case 1:
                        summonPrefab = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].prefab;
                        break;
                    case 2:
                        summonPrefab = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].prefab;
                        break;
                    case 3:
                        summonPrefab = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].prefab;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Thunder:
                thunderOrb--;
                switch(CanvasManager.Instance.selectedSummonChoice) {
                    case 1:
                        summonPrefab = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].prefab;
                        break;
                    case 2:
                        summonPrefab = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].prefab;
                        break;
                    case 3:
                        summonPrefab = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].prefab;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Wind:
                windOrb--;
                switch(CanvasManager.Instance.selectedSummonChoice) {
                    case 1:
                        summonPrefab = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].prefab;
                        break;
                    case 2:
                        summonPrefab = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].prefab;
                        break;
                    case 3:
                        summonPrefab = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].prefab;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Crystal:
                crystal--;
                switch(CanvasManager.Instance.selectedSummonChoice) {
                    case 1:
                        summonPrefab = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].prefab;
                        break;
                    case 2:
                        summonPrefab = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].prefab;
                        break;
                    case 3:
                        summonPrefab = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].prefab;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        if(highLevelSummonning) {
            magicCrystal--;
            switch(selectedOrbOrCrystal) {
                case OrbsAndCrystal.Fire:
                    switch(CanvasManager.Instance.selectedSummonChoice) {
                        case 1:
                            summonPrefab = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].prefab;
                            break;
                        case 2:
                            summonPrefab = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].prefab;
                            break;
                        case 3:
                            summonPrefab = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].prefab;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Frost:
                    switch(CanvasManager.Instance.selectedSummonChoice) {
                        case 1:
                            summonPrefab = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].prefab;
                            break;
                        case 2:
                            summonPrefab = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].prefab;
                            break;
                        case 3:
                            summonPrefab = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].prefab;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Thunder:
                    switch(CanvasManager.Instance.selectedSummonChoice) {
                        case 1:
                            summonPrefab = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].prefab;
                            break;
                        case 2:
                            summonPrefab = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].prefab;
                            break;
                        case 3:
                            summonPrefab = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].prefab;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Wind:
                    switch(CanvasManager.Instance.selectedSummonChoice) {
                        case 1:
                            summonPrefab = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].prefab;
                            break;
                        case 2:
                            summonPrefab = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].prefab;
                            break;
                        case 3:
                            summonPrefab = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].prefab;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Crystal:
                    switch(CanvasManager.Instance.selectedSummonChoice) {
                        case 1:
                            summonPrefab = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].prefab;
                            break;
                        case 2:
                            summonPrefab = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].prefab;
                            break;
                        case 3:
                            summonPrefab = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].prefab;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
        }

        foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
            tile.summonIndicator.SetActive(false);
        }

        //TODO - spawn summon monster on selected tile and set ownership
        GameObject playerSummon = Instantiate(summonPrefab, selectedTile.transform.position, Quaternion.identity);
        playerSummon.GetComponent<Moveable>().movableOwner = MovableOwner.Player;

        // Next phase after summon        
        Invoke("NextPhase", 0.1f);
        
        AudioManager.Instance.Play("Summon");
    }

    public void PlayerCommand() {
        Moveable[] playerSummonsOnField = FindObjectsOfType<Moveable>();
        foreach(Moveable moveable in playerSummonsOnField) {
            moveable.commandState = CommandState.None;
            if (moveable.movableOwner == MovableOwner.Player) {
                moveable.GetComponentInChildren<FloatingUI>().actionIndicator.SetActive(true);
            }
        }
        CanvasManager.Instance.endTurnButton.SetActive(true);
    }

    public void EndPlayerTurn() {
        CanvasManager.Instance.endTurnButton.SetActive(false);
        Moveable[] playerSummonsOnField = FindObjectsOfType<Moveable>();
        foreach(Moveable moveable in playerSummonsOnField) {
            if (moveable.movableOwner == MovableOwner.Player) {
                FloatingUI unitFloatingUI = moveable.GetComponentInChildren<FloatingUI>();
                unitFloatingUI.actionIndicator.SetActive(false);
                unitFloatingUI.actionUIPanel.SetActive(false);
            }
        }
        NextPhase();
        
        AudioManager.Instance.Play("Click");
    }

    public void EnemySummon() {
        GameObject summonPrefab = null;
        int randTile = UnityEngine.Random.Range(0,100);
        int randMonster = UnityEngine.Random.Range(0,100);
        int summonTileCol = 0; //0 = None Selected, 1,2,3, = Tile Closest To Player, 4 = front, 5 = middle, 6 = back

        switch (LevelManager.Instance.currentLevelBoss) {
            case CurrentLevelBoss.CrystalBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.Wisp:
                        //tile
                        if (randTile < 50) {
                            summonTileCol = 4; //front
                        } else {
                            summonTileCol = 5; //middle
                        }
                        //monster
                        if (randMonster < 50) {
                            summonPrefab = crystalSummons[1].prefab; // dark wisp
                        } else {
                            summonPrefab = crystalSummons[6].prefab; // light wisp
                        }
                        break;
                    case MonsterList.Goblins:
                        if (randMonster < 20) {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = crystalSummons[2].prefab; // goblin archer
                        } else if (randMonster >= 20 && randMonster < 80) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = crystalSummons[3].prefab; // goblin grunt
                        } else {
                            summonTileCol = 6;
                            summonPrefab = crystalSummons[4].prefab; // goblin mage
                        }
                        break;
                    case MonsterList.Slimes:
                        //tile
                        if (randTile < 50) {
                            summonTileCol = 1; //closet to player
                        } else {
                            summonTileCol = 4; //front
                        }
                        //monster
                        if (randMonster < 50) {
                            summonPrefab = crystalSummons[0].prefab; // dark slime
                        } else {
                            summonPrefab = crystalSummons[5].prefab; // light slime
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CurrentLevelBoss.FireOrbBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.FireBuffing:
                        if (randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = fireSummons[0].prefab; // fire bat
                        } else if (randMonster >= 40 && randMonster < 60) {
                            summonTileCol = 6;
                            summonPrefab = fireSummons[2].prefab; // fire minivolcano
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = fireSummons[5].prefab; // fire wisp
                        }
                        break;
                    case MonsterList.FireBuffing2:
                        if (randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = fireSummons[1].prefab; // fire imp
                        } else if (randMonster >= 40 && randMonster < 60) {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = fireSummons[5].prefab; // fire wisp
                        } else {
                            summonTileCol = 6;
                            summonPrefab = fireSummons[3].prefab; // fire maiden
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CurrentLevelBoss.FrostOrbBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.FrostDebuff:
                        if (randMonster < 50) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = frostSummons[1].prefab; // ice bat
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = frostSummons[4].prefab; // ice wisp
                        }
                        break;
                    case MonsterList.FrostDebuff2:
                        if (randMonster < 50) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = frostSummons[0].prefab; // ice avian
                        } else if (randMonster >= 50 && randMonster < 75) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = frostSummons[2].prefab; // ice slime
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = frostSummons[3].prefab; // ice turtle
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CurrentLevelBoss.ThunderOrbBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.ThunderOrbSlimes:
                        if (randMonster < 50) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = thunderSummons[1].prefab; // clockwerk slime
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = thunderSummons[4].prefab; // cyber slime
                        }
                        break;
                    case MonsterList.ThunderOrbMix:
                        if (randMonster < 20) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = thunderSummons[0].prefab; // clockwerk mini
                        } else if (randMonster >= 20 && randMonster < 60) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = thunderSummons[2].prefab; // clockwerk spider mini
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 5; //middle
                            } else {
                                summonTileCol = 6; //back
                            }
                            summonPrefab = thunderSummons[3].prefab; // cyber hawk
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CurrentLevelBoss.WindOrbBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.WindOrbEarthHealing:
                        if (randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = windSummons[0].prefab; // earth wisp
                        } else if (randMonster >= 40 && randMonster < 70) {
                            if (randTile < 50) {
                                summonTileCol = 5; //middle
                            } else {
                                summonTileCol = 6; //back
                            }
                            summonPrefab = windSummons[1].prefab; // earth turtle
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = windSummons[4].prefab; // forest spider
                        }
                        break;
                    case MonsterList.WindOrbForestMix:
                        if (randMonster < 70) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = windSummons[2].prefab; // forest darkluff
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closest to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = windSummons[3].prefab; // forest shroom
                        }
                        break;
                    default:
                        break;
                }
                break;
            case CurrentLevelBoss.MagicCrystalBoss:
                switch (LevelManager.Instance.monsterList) {
                    case MonsterList.HighLevelFire:
                        if (randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelFireSummons[1].prefab; // fire elemental angel
                        } else if (randMonster >= 40 && randMonster < 80) {
                            summonTileCol = 5; //middle
                            summonPrefab = highLevelFireSummons[2].prefab; // flame knight
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = highLevelFireSummons[3].prefab; // fire lion
                        }
                        break;
                    case MonsterList.HighLevelFrost:
                        if (randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelFrostSummons[1].prefab; // ice lion
                        } else if (randMonster >= 40 && randMonster < 80) {
                            summonTileCol = 5; //middle
                            summonPrefab = highLevelFrostSummons[3].prefab; // ice snowman
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelFrostSummons[4].prefab; // ice yeti
                        }
                        break;
                    case MonsterList.HighLevelThunder:
                        if (randMonster < 20) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelThunderSummons[0].prefab; // clockwerk iron maiden
                        } else if (randMonster >= 20 && randMonster < 60) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelThunderSummons[1].prefab; // clockwerk knight
                        } else {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelThunderSummons[2].prefab; // cyber ogre
                        }
                        break;
                    case MonsterList.HighLevelWind:
                        if (randMonster < 20) {
                            if (randTile < 50) {
                                summonTileCol = 4; //front
                            } else {
                                summonTileCol = 5; //middle
                            }
                            summonPrefab = highLevelWindSummons[0].prefab; // earth lion
                        } else if (randMonster >= 20 && randMonster < 40) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelWindSummons[1].prefab; // earth mandrake
                        } else if (randMonster >= 40 && randMonster < 80) {
                            if (randTile < 50) {
                                summonTileCol = 1; //closet to player
                            } else {
                                summonTileCol = 4; //front
                            }
                            summonPrefab = highLevelWindSummons[2].prefab; // forest golem
                        }else {
                            summonTileCol = 1; //closet to player
                            summonPrefab = highLevelWindSummons[3].prefab; // forest spora
                        }
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        Tile selectedTile = null;

        if (summonTileCol == 4) { //front
            foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                if (tile.tileOwner == TileOwner.Enemy && !tile.isOccupied && tile.col == 4) {
                    selectedTile = tile;
                    break;
                }
            }
            if (selectedTile == null) { summonTileCol = 0; } //no available tile
        } else if (summonTileCol == 5) { //middle
            foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                if (tile.tileOwner == TileOwner.Enemy && !tile.isOccupied && tile.col == 5) {
                    selectedTile = tile;
                    break;
                }
            }
            if (selectedTile == null) { summonTileCol = 0; } //no available tile
        } else if (summonTileCol == 6) { //back
            foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                if (tile.tileOwner == TileOwner.Enemy && !tile.isOccupied && tile.col == 6) {
                    selectedTile = tile;
                    break;
                }
            }
            if (selectedTile == null) { summonTileCol = 0; } //no available tile
        } else { //closest to player or front
            foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                if (tile.tileOwner == TileOwner.Enemy && !tile.isOccupied) {
                    selectedTile = tile;
                    break;
                }
            }
            if (selectedTile == null) { summonTileCol = 0; } //no available tile
        }

        if (summonTileCol == 0) {
            foreach(Tile tile in GridGenerator.Instance.generatedTiles) {
                if (tile.tileOwner == TileOwner.Enemy && !tile.isOccupied) {
                    selectedTile = tile;
                    break;
                }
            }
        }

        // If no spawnable tiles selectable, skip summon
        if (selectedTile) {
            // Spawn summon monster on selected tile and set ownership
            GameObject enemySummon = Instantiate(summonPrefab, selectedTile.transform.position, Quaternion.identity);
            enemySummon.GetComponent<Moveable>().movableOwner = MovableOwner.Enemy;
            AudioManager.Instance.Play("Summon");
        }

        // Next phase after summon        
        Invoke("NextPhase", 0.1f);
    }

    public IEnumerator EnemyCommand() {
        var delayPerCommand = new WaitForSeconds(2f);

        Moveable[] enemySummonsOnField = FindObjectsOfType<Moveable>();
        foreach(Moveable moveable in enemySummonsOnField) {
            if (moveable.movableOwner == MovableOwner.Enemy && moveable != null) {
                moveable.commandState = CommandState.None;
                moveable.EnemyAttack();
                yield return delayPerCommand;
            }
        }
        Invoke("NextPhase", 0.1f);
    }

    public IEnumerator DelayBeforeEnemyCommandTurn() {
        yield return new WaitForSeconds(2f);
        
        StartCoroutine(EnemyCommand());
    }

    public void LevelComplete() {
        // Destroy all enemy
        Moveable[] enemySummonsOnField = FindObjectsOfType<Moveable>();
        foreach(Moveable moveable in enemySummonsOnField) {
            if (moveable.movableOwner == MovableOwner.Enemy && moveable.gameObject.tag != "Boss") {
                moveable.Death();
            }
            if (moveable.movableOwner == MovableOwner.Player) {
                moveable.GetComponentInChildren<FloatingUI>().actionIndicator.SetActive(false);
            }
        }

        // Enable next phase and show panel
        MasterController.Instance.gamePhase = GamePhase.Next_Level;
        CanvasManager.Instance.OnEnablePhasePanel();
        LeanTweenAnimationController.Instance.LeanPhasePanel();
        CanvasManager.Instance.Invoke("OnDisablePhasePanel", 1.5f);
        
        print("boss dead, prepare next level");
    }

    public void NextLevel() {
        if (SceneManager.GetActiveScene().buildIndex == 15) {
            LoadCreditScene();
        } else {
            //TODO: Go to next level and set game phase to load level
            M_Player_Default.Instance.MoveToDefaultPosition();
            CanvasManager.Instance.endTurnButton.SetActive(false);
            CanvasManager.Instance.nextLevelButton.SetActive(false);
            currentSceneNumber++;
            SceneManager.LoadScene(currentSceneNumber);

            // after loading new scene
            Invoke("LoadingLevel", .1f);
            
            AudioManager.Instance.Play("Click");
        }
    }

    public void LoadingLevel() {
        gamePhase = GamePhase.Loading_Level;
        GridGenerator.Instance.GenerateNewTileMap();
        //Update level manager
        LevelData levelData = FindObjectOfType<LevelData>();
        LevelManager.Instance.currentLevelBoss = levelData.currentLevelBoss;
        LevelManager.Instance.monsterList = levelData.monsterList;
        LevelManager.Instance.SpawnCurrentLevelBoss();

        //Panel animation
        CanvasManager.Instance.OnEnablePhasePanel();
        LeanTweenAnimationController.Instance.LeanPhasePanel();
        CanvasManager.Instance.Invoke("OnDisablePhasePanel", 4f);
        Invoke("NextPhase", 2f);
    }

    public void RestartGame() {
        //Load Main Menu
        SceneManager.LoadScene(0);

        //Destroy all instance
        Destroy(MainCamera.Instance.gameObject);
        Destroy(GridGenerator.Instance.gameObject);
        Destroy(M_Player_Default.Instance.gameObject);
        Destroy(CanvasManager.Instance.gameObject);
        Destroy(LeanTweenAnimationController.Instance.gameObject);
        Destroy(MouseClickEventController.Instance.gameObject);
        Destroy(LevelManager.Instance.gameObject);
        //destroy this gameobject last
        Destroy(MasterController.Instance.gameObject);
        
        AudioManager.Instance.Play("Click");
    }

    public void LoadCreditScene() {
        SceneManager.LoadScene(16);
            
        //Destroy all instance
        Destroy(MainCamera.Instance.gameObject);
        Destroy(GridGenerator.Instance.gameObject);
        Destroy(M_Player_Default.Instance.gameObject);
        Destroy(CanvasManager.Instance.gameObject);
        Destroy(LeanTweenAnimationController.Instance.gameObject);
        Destroy(MouseClickEventController.Instance.gameObject);
        Destroy(LevelManager.Instance.gameObject);
        //destroy this gameobject last
        Destroy(MasterController.Instance.gameObject);

        AudioManager.Instance.Play("Click");
    }

    public void GameOver() {
        CanvasManager.Instance.phasePanel.SetActive(true);
        StartCoroutine(CanvasManager.Instance.TypeSentence("You Died, Game Over!"));
        LeanTweenAnimationController.Instance.LeanPhasePanel();

        Invoke("Pause", 1f);

        Invoke("RestartGame", 2f);
    }

    public void Pause() {
        Time.timeScale = 0.5f;
        Invoke("Resume", 1f);
    }

    public void Resume() {
        Time.timeScale = 1;
    }
}

public enum GamePhase {
    Loading_Level,
    Player_Summon,
    Player_Command,
    Enemy_Turn,
    Enemy_Summon,
    Enemy_Command,
    Player_Turn,
    Next_Level
}

public enum OrbsAndCrystal {
    Fire, 
    Frost, 
    Thunder, 
    Wind, 
    Crystal
}

public static class Extensions
{
    public static T Next<T>(this T src) where T : struct
    {
        if (!typeof(T).IsEnum) throw new ArgumentException(String.Format("Argument {0} is not an Enum", typeof(T).FullName));

        T[] Arr = (T[])Enum.GetValues(src.GetType());
        int j = (Array.IndexOf<T>(Arr, src) + 1) % Arr.Length;
        return Arr[j];         
    }
}
