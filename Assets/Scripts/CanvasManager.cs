using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager Instance;

    [field: SerializeField] public TextMeshProUGUI currentPhaseText { get; private set; }
    [field: SerializeField] public float sentenceSpeed { get; set; }
    [field: SerializeField] public GameObject phasePanel { get; set; }
    [field: SerializeField] public GameObject summoningPanel { get; set; }
    [field: SerializeField] public TextMeshProUGUI fireOrbAmount, frostOrbAmount, thunderOrbAmount, windOrbAmount, crystalAmount, magicCrystalAmount;
    public Sprite[] spriteArray;
    [field: SerializeField] public Image selectedOrbsOrCrystal, magicCrystalImage;
    [field: SerializeField] public GameObject cancelSummonButton { get; set; }
    [field: SerializeField] public GameObject endTurnButton { get; set; }
    [field: SerializeField] public GameObject nextLevelButton { get; set; }
    [field: SerializeField] public Image summonChoice1Image, summonChoice2Image, summonChoice3Image;
    [field: SerializeField] TextMeshProUGUI summonChoice1Name, summonChoice2Name, summonChoice3Name;
    [field: SerializeField] public int selectedSummonChoice { get; set; } // 1,2,3
    [field: SerializeField] TextMeshProUGUI summonChoiceDescriptionText;

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        } else {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Phase Panel Section
    /// </summary>
    public void OnEnablePhasePanel() {
        phasePanel.SetActive(true);
        switch(MasterController.Instance.gamePhase) {
            case GamePhase.Player_Summon:
                StartCoroutine(TypeSentence("Player Summon"));
                break;
            case GamePhase.Player_Command:
                StartCoroutine(TypeSentence("Player Command"));
                break;
            case GamePhase.Enemy_Turn:
                StartCoroutine(TypeSentence("Enemy Turn"));
                break;
            case GamePhase.Enemy_Summon:
                StartCoroutine(TypeSentence("Enemy Summon"));
                break;
            case GamePhase.Enemy_Command:
                StartCoroutine(TypeSentence("Enemy Command"));
                break;
            case GamePhase.Player_Turn:
                StartCoroutine(TypeSentence("Player Turn"));
                break;
            case GamePhase.Next_Level:
                StartCoroutine(TypeSentence("Level Complete!"));
                break;
            case GamePhase.Loading_Level:
                StartCoroutine(TypeSentence("Level " + MasterController.Instance.currentSceneNumber + "/15"));
                break;
            default:
                break;
        }
    }

    public void OnDisablePhasePanel() {
        phasePanel.SetActive(false);

        if (MasterController.Instance.gamePhase == GamePhase.Next_Level)
            CanvasManager.Instance.nextLevelButton.SetActive(true);
    }

    
    /// <summary>
    /// Summoning Phase Section
    /// </summary>
    public void OnEnableSummoningPanel() {
        summoningPanel.SetActive(true);
        LeanTweenAnimationController.Instance.LeanSummoningPanelOpen();

        // Update summoning inventory text
        UpdateOrbAndCrystalTextOnPanelOpen();

    }
    public IEnumerator OnDisableSummoningPanel() {
        LeanTweenAnimationController.Instance.LeanSummoningPanelClose();
        yield return new WaitForSeconds(0.5f);
        summoningPanel.SetActive(false);
    }

    /// <summary>
    /// Update summoning inventory text
    /// </summary>
    private void UpdateOrbAndCrystalTextOnPanelOpen() {
        fireOrbAmount.text = "x " + (MasterController.Instance.fireOrb - 1).ToString();
        if (MasterController.Instance.fireOrb <= 0) {
            fireOrbAmount.text = "x 0";
        }
        fireOrbAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[0];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Fire;

        frostOrbAmount.text = "x " + MasterController.Instance.frostOrb.ToString();
        frostOrbAmount.color = Color.white;
        thunderOrbAmount.text = "x " + MasterController.Instance.thunderOrb.ToString();
        thunderOrbAmount.color = Color.white;
        windOrbAmount.text = "x " + MasterController.Instance.windOrb.ToString();
        windOrbAmount.color = Color.white;
        crystalAmount.text = "x " + MasterController.Instance.crystal.ToString();
        crystalAmount.color = Color.white;

        magicCrystalAmount.text = "x " + MasterController.Instance.magicCrystal.ToString();
        magicCrystalAmount.color = Color.white;
        magicCrystalImage.color = Color.black;
        MasterController.Instance.highLevelSummonning = false;

        UpdateSummonChoiceImage();
    }

    /// <summary>
    /// Update summoning inventory text
    /// </summary>
    private void UpdateOrbAndCrystalTextOnSelected() {
        fireOrbAmount.text = "x " + MasterController.Instance.fireOrb.ToString();
        fireOrbAmount.color = Color.white;
        frostOrbAmount.text = "x " + MasterController.Instance.frostOrb.ToString();
        frostOrbAmount.color = Color.white;
        thunderOrbAmount.text = "x " + MasterController.Instance.thunderOrb.ToString();
        thunderOrbAmount.color = Color.white;
        windOrbAmount.text = "x " + MasterController.Instance.windOrb.ToString();
        windOrbAmount.color = Color.white;
        crystalAmount.text = "x " + MasterController.Instance.crystal.ToString();
        crystalAmount.color = Color.white;
        // magicCrystalAmount.text = "x " + MasterController.Instance.magicCrystal.ToString();
        // magicCrystalAmount.color = Color.white;
    }

    public void OnSelectFireOrb() {
        UpdateOrbAndCrystalTextOnSelected();
        int postOrbCount = MasterController.Instance.fireOrb - 1;
        if (postOrbCount <= 0) postOrbCount = 0;
        fireOrbAmount.text = "x " + postOrbCount.ToString();
        fireOrbAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[0];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Fire;

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }
    public void OnSelectFrostOrb() {
        UpdateOrbAndCrystalTextOnSelected();
        int postOrbCount = MasterController.Instance.frostOrb - 1;
        if (postOrbCount - 1 <= 0) postOrbCount = 0;
        frostOrbAmount.text = "x " + postOrbCount.ToString();
        frostOrbAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[1];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Frost;

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }
    public void OnSelectThunderOrb() {
        UpdateOrbAndCrystalTextOnSelected();
        int postOrbCount = MasterController.Instance.thunderOrb - 1;
        if (postOrbCount <= 0) postOrbCount = 0;
        thunderOrbAmount.text = "x " + postOrbCount.ToString();
        thunderOrbAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[2];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Thunder;

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }
    public void OnSelectWindOrb() {
        UpdateOrbAndCrystalTextOnSelected();
        int postOrbCount = MasterController.Instance.windOrb - 1;
        if (postOrbCount <= 0) postOrbCount = 0;
        windOrbAmount.text = "x " + postOrbCount.ToString();
        windOrbAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[3];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Wind;

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }
    public void OnSelectCrystal() {
        UpdateOrbAndCrystalTextOnSelected();
        int postCrystalCount = MasterController.Instance.crystal - 1;
        if (postCrystalCount <= 0) postCrystalCount = 0;
        crystalAmount.text = "x " + postCrystalCount.ToString();
        crystalAmount.color = Color.red;
        selectedOrbsOrCrystal.sprite = spriteArray[4];
        MasterController.Instance.selectedOrbOrCrystal = OrbsAndCrystal.Crystal;

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }
    public void OnToggleMagicCrystal() {
        MasterController.Instance.highLevelSummonning = !MasterController.Instance.highLevelSummonning;

        if (MasterController.Instance.highLevelSummonning) {
            int postCrystalCount = MasterController.Instance.magicCrystal - 1;
            if (postCrystalCount <= 0) postCrystalCount = 0;
            magicCrystalAmount.text = "x " + postCrystalCount.ToString();
            magicCrystalAmount.color = Color.red;
            magicCrystalImage.color = Color.white;
        } else {
            magicCrystalAmount.text = "x " + (MasterController.Instance.magicCrystal).ToString();
            magicCrystalAmount.color = Color.white;
            magicCrystalImage.color = Color.black;
        }

        UpdateSummonChoiceImage();
        
        AudioManager.Instance.Play("Click");
    }

    /// <summary>
    /// Update summoning choice image
    /// </summary>
    public void UpdateSummonChoiceImage() {

        selectedSummonChoice = 1;
        summonChoice1Name.color = Color.white;
        summonChoice2Name.color = Color.white;
        summonChoice3Name.color = Color.white;
        Color newColor;
        ColorUtility.TryParseHtmlString("#FCFF58", out newColor);
        summonChoice1Name.color = newColor;

        if(MasterController.Instance.highLevelSummonning) {
            switch(MasterController.Instance.selectedOrbOrCrystal) {
                case OrbsAndCrystal.Fire:
                    summonChoice1Image.sprite = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].sprite;
                    summonChoice2Image.sprite = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].sprite;
                    summonChoice3Image.sprite = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].sprite;
                    // print(MasterController.Instance.possibleHighLevelFireSummon.choice1 + " - " 
                    //     + MasterController.Instance.possibleHighLevelFireSummon.choice2 + " - " 
                    //     + MasterController.Instance.possibleHighLevelFireSummon.choice3);
                    summonChoice1Name.text = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].name;
                    summonChoice2Name.text = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].name;
                    summonChoice3Name.text = MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].name;

                    summonChoiceDescriptionText.text = 
                        "HP: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].hp + "\n" +
                        "Attack: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].attack + "\n" +
                        "Move Distance: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].moveDistance + "\n" +
                        "Summon Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].summonEffect + "\n" +
                        "Attack Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].attackEffect + "\n" +
                        "Capture Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].captureEffect + "\n" +
                        "Defend Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].defendEffect + "\n" +
                        "Death Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].deathEffect;
                    break;
                case OrbsAndCrystal.Frost:
                    summonChoice1Image.sprite = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].sprite;
                    summonChoice2Image.sprite = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].sprite;
                    summonChoice3Image.sprite = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].sprite;
                    // print(MasterController.Instance.possibleHighLevelFrostSummon.choice1 + " - " 
                    //     + MasterController.Instance.possibleHighLevelFrostSummon.choice2 + " - " 
                    //     + MasterController.Instance.possibleHighLevelFrostSummon.choice3);
                    summonChoice1Name.text = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].name;
                    summonChoice2Name.text = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].name;
                    summonChoice3Name.text = MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].name;

                    summonChoiceDescriptionText.text = 
                        "HP: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].hp + "\n" +
                        "Attack: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].attack + "\n" +
                        "Move Distance: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].moveDistance + "\n" +
                        "Summon Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].summonEffect + "\n" +
                        "Attack Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].attackEffect + "\n" +
                        "Capture Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].captureEffect + "\n" +
                        "Defend Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].defendEffect + "\n" +
                        "Death Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].deathEffect;
                    break;
                case OrbsAndCrystal.Thunder:
                    summonChoice1Image.sprite = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].sprite;
                    summonChoice2Image.sprite = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].sprite;
                    summonChoice3Image.sprite = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].sprite;
                    // print(MasterController.Instance.possibleHighLevelThunderSummon.choice1 + " - " 
                    //     + MasterController.Instance.possibleHighLevelThunderSummon.choice2 + " - " 
                    //     + MasterController.Instance.possibleHighLevelThunderSummon.choice3);
                    summonChoice1Name.text = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].name;
                    summonChoice2Name.text = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].name;
                    summonChoice3Name.text = MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].name;

                    summonChoiceDescriptionText.text = 
                        "HP: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].hp + "\n" +
                        "Attack: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].attack + "\n" +
                        "Move Distance: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].moveDistance + "\n" +
                        "Summon Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].summonEffect + "\n" +
                        "Attack Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].attackEffect + "\n" +
                        "Capture Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].captureEffect + "\n" +
                        "Defend Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].defendEffect + "\n" +
                        "Death Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].deathEffect;
                    break;
                case OrbsAndCrystal.Wind:
                    summonChoice1Image.sprite = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].sprite;
                    summonChoice2Image.sprite = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].sprite;
                    summonChoice3Image.sprite = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].sprite;
                    // print(MasterController.Instance.possibleHighLevelWindSummon.choice1 + " - " 
                    //     + MasterController.Instance.possibleHighLevelWindSummon.choice2 + " - " 
                    //     + MasterController.Instance.possibleHighLevelWindSummon.choice3);
                    summonChoice1Name.text = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].name;
                    summonChoice2Name.text = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].name;
                    summonChoice3Name.text = MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].name;

                    summonChoiceDescriptionText.text = 
                        "HP: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].hp + "\n" +
                        "Attack: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].attack + "\n" +
                        "Move Distance: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].moveDistance + "\n" +
                        "Summon Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].summonEffect + "\n" +
                        "Attack Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].attackEffect + "\n" +
                        "Capture Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].captureEffect + "\n" +
                        "Defend Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].defendEffect + "\n" +
                        "Death Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].deathEffect;
                    break;
                case OrbsAndCrystal.Crystal:
                    summonChoice1Image.sprite = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].sprite;
                    summonChoice2Image.sprite = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].sprite;
                    summonChoice3Image.sprite = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].sprite;
                    // print(MasterController.Instance.possibleHighLevelCrystalSummon.choice1 + " - " 
                    //     + MasterController.Instance.possibleHighLevelCrystalSummon.choice2 + " - " 
                    //     + MasterController.Instance.possibleHighLevelCrystalSummon.choice3);
                    summonChoice1Name.text = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].name;
                    summonChoice2Name.text = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].name;
                    summonChoice3Name.text = MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].name;

                    summonChoiceDescriptionText.text = 
                        "HP: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].hp + "\n" +
                        "Attack: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].attack + "\n" +
                        "Move Distance: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].moveDistance + "\n" +
                        "Summon Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].summonEffect + "\n" +
                        "Attack Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].attackEffect + "\n" +
                        "Capture Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].captureEffect + "\n" +
                        "Defend Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].defendEffect + "\n" +
                        "Death Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].deathEffect;
                    break;
                default:
                    break;
            }
            return;
        }

        switch(MasterController.Instance.selectedOrbOrCrystal) {
            case OrbsAndCrystal.Fire:
                summonChoice1Image.sprite = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].sprite;
                summonChoice2Image.sprite = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].sprite;
                summonChoice3Image.sprite = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].sprite;
                // print(MasterController.Instance.possibleFireSummon.choice1 + " - " 
                //     + MasterController.Instance.possibleFireSummon.choice2 + " - " 
                //     + MasterController.Instance.possibleFireSummon.choice3);
                summonChoice1Name.text = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].name;
                summonChoice2Name.text = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].name;
                summonChoice3Name.text = MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].name;

                summonChoiceDescriptionText.text = 
                    "HP: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].hp + "\n" +
                    "Attack: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].attack + "\n" +
                    "Move Distance: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].moveDistance + "\n" +
                    "Summon Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].summonEffect + "\n" +
                    "Attack Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].attackEffect + "\n" +
                    "Capture Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].captureEffect + "\n" +
                    "Defend Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].defendEffect + "\n" +
                    "Death Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].deathEffect;
                break;
            case OrbsAndCrystal.Frost:
                summonChoice1Image.sprite = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].sprite;
                summonChoice2Image.sprite = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].sprite;
                summonChoice3Image.sprite = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].sprite;
                // print(MasterController.Instance.possibleFrostSummon.choice1 + " - " 
                //     + MasterController.Instance.possibleFrostSummon.choice2 + " - " 
                //     + MasterController.Instance.possibleFrostSummon.choice3);
                summonChoice1Name.text = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].name;
                summonChoice2Name.text = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].name;
                summonChoice3Name.text = MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].name;

                summonChoiceDescriptionText.text = 
                    "HP: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].hp + "\n" +
                    "Attack: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].attack + "\n" +
                    "Move Distance: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].moveDistance + "\n" +
                    "Summon Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].summonEffect + "\n" +
                    "Attack Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].attackEffect + "\n" +
                    "Capture Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].captureEffect + "\n" +
                    "Defend Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].defendEffect + "\n" +
                    "Death Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].deathEffect;
                break;
            case OrbsAndCrystal.Thunder:
                summonChoice1Image.sprite = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].sprite;
                summonChoice2Image.sprite = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].sprite;
                summonChoice3Image.sprite = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].sprite;
                // print(MasterController.Instance.possibleThunderSummon.choice1 + " - " 
                //     + MasterController.Instance.possibleThunderSummon.choice2 + " - " 
                //     + MasterController.Instance.possibleThunderSummon.choice3);
                summonChoice1Name.text = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].name;
                summonChoice2Name.text = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].name;
                summonChoice3Name.text = MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].name;

                summonChoiceDescriptionText.text = 
                    "HP: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].hp + "\n" +
                    "Attack: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].attack + "\n" +
                    "Move Distance: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].moveDistance + "\n" +
                    "Summon Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].summonEffect + "\n" +
                    "Attack Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].attackEffect + "\n" +
                    "Capture Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].captureEffect + "\n" +
                    "Defend Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].defendEffect + "\n" +
                    "Death Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].deathEffect;
                break;
            case OrbsAndCrystal.Wind:
                summonChoice1Image.sprite = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].sprite;
                summonChoice2Image.sprite = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].sprite;
                summonChoice3Image.sprite = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].sprite;
                // print(MasterController.Instance.possibleWindSummon.choice1 + " - " 
                //     + MasterController.Instance.possibleWindSummon.choice2 + " - " 
                //     + MasterController.Instance.possibleWindSummon.choice3);
                summonChoice1Name.text = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].name;
                summonChoice2Name.text = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].name;
                summonChoice3Name.text = MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].name;

                summonChoiceDescriptionText.text = 
                    "HP: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].hp + "\n" +
                    "Attack: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].attack + "\n" +
                    "Move Distance: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].moveDistance + "\n" +
                    "Summon Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].summonEffect + "\n" +
                    "Attack Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].attackEffect + "\n" +
                    "Capture Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].captureEffect + "\n" +
                    "Defend Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].defendEffect + "\n" +
                    "Death Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].deathEffect;
                break;
            case OrbsAndCrystal.Crystal:
                summonChoice1Image.sprite = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].sprite;
                summonChoice2Image.sprite = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].sprite;
                summonChoice3Image.sprite = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].sprite;
                // print(MasterController.Instance.possibleCrystalSummon.choice1 + " - " 
                //     + MasterController.Instance.possibleCrystalSummon.choice2 + " - " 
                //     + MasterController.Instance.possibleCrystalSummon.choice3);
                summonChoice1Name.text = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].name;
                summonChoice2Name.text = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].name;
                summonChoice3Name.text = MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].name;

                summonChoiceDescriptionText.text = 
                    "HP: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].hp + "\n" +
                    "Attack: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].attack + "\n" +
                    "Move Distance: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].moveDistance + "\n" +
                    "Summon Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].summonEffect + "\n" +
                    "Attack Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].attackEffect + "\n" +
                    "Capture Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].captureEffect + "\n" +
                    "Defend Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].defendEffect + "\n" +
                    "Death Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].deathEffect;
                break;
            default:
                break;
        }
    }

    public void UpdateSelectedSummonDetails(int _selectedChoice) {
        selectedSummonChoice = _selectedChoice;
        summonChoice1Name.color = Color.white;
        summonChoice2Name.color = Color.white;
        summonChoice3Name.color = Color.white;
        Color newColor;
        ColorUtility.TryParseHtmlString("#FCFF58", out newColor);

        if(MasterController.Instance.highLevelSummonning) {
            switch(MasterController.Instance.selectedOrbOrCrystal) {
                case OrbsAndCrystal.Fire:
                    switch(_selectedChoice) {
                        case 1:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice1].deathEffect;
                                
                            summonChoice1Name.color = newColor;
                            break;
                        case 2:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice2].deathEffect;
                            
                            summonChoice2Name.color = newColor;
                            break;
                        case 3:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFireSummons[MasterController.Instance.possibleHighLevelFireSummon.choice3].deathEffect;
                            
                            summonChoice3Name.color = newColor;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Frost:
                    switch(_selectedChoice) {
                        case 1:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice1].deathEffect;

                            summonChoice1Name.color = newColor;
                            break;
                        case 2:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice2].deathEffect;

                            summonChoice2Name.color = newColor;
                            break;
                        case 3:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelFrostSummons[MasterController.Instance.possibleHighLevelFrostSummon.choice3].deathEffect;

                            summonChoice3Name.color = newColor;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Thunder:
                    switch(_selectedChoice) {
                        case 1:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice1].deathEffect;

                            summonChoice1Name.color = newColor;
                            break;
                        case 2:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice2].deathEffect;

                            summonChoice2Name.color = newColor;
                            break;
                        case 3:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelThunderSummons[MasterController.Instance.possibleHighLevelThunderSummon.choice3].deathEffect;

                            summonChoice3Name.color = newColor;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Wind:
                    switch(_selectedChoice) {
                        case 1:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice1].deathEffect;

                            summonChoice1Name.color = newColor;
                            break;
                        case 2:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice2].deathEffect;

                            summonChoice2Name.color = newColor;
                            break;
                        case 3:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelWindSummons[MasterController.Instance.possibleHighLevelWindSummon.choice3].deathEffect;

                            summonChoice3Name.color = newColor;
                            break;
                        default:
                            break;
                    }
                    break;
                case OrbsAndCrystal.Crystal:
                    switch(_selectedChoice) {
                        case 1:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice1].deathEffect;

                            summonChoice1Name.color = newColor;
                            break;
                        case 2:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice2].deathEffect;

                            summonChoice2Name.color = newColor;
                            break;
                        case 3:
                            summonChoiceDescriptionText.text = 
                                "HP: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].hp + "\n" +
                                "Attack: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].attack + "\n" +
                                "Move Distance: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].moveDistance + "\n" +
                                "Summon Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].summonEffect + "\n" +
                                "Attack Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].attackEffect + "\n" +
                                "Capture Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].captureEffect + "\n" +
                                "Defend Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].defendEffect + "\n" +
                                "Death Effect: " + MasterController.Instance.highLevelCrystalSummons[MasterController.Instance.possibleHighLevelCrystalSummon.choice3].deathEffect;

                            summonChoice3Name.color = newColor;
                            break;
                        default:
                            break;
                    }
                    break;
                default:
                    break;
            }
            return;
        }

        switch(MasterController.Instance.selectedOrbOrCrystal) {
            case OrbsAndCrystal.Fire:
                switch(_selectedChoice) {
                    case 1:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].hp + "\n" +
                            "Attack: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice1].deathEffect;

                        summonChoice1Name.color = newColor;
                        break;
                    case 2:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].hp + "\n" +
                            "Attack: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice2].deathEffect;

                        summonChoice2Name.color = newColor;
                        break;
                    case 3:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].hp + "\n" +
                            "Attack: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.fireSummons[MasterController.Instance.possibleFireSummon.choice3].deathEffect;

                        summonChoice3Name.color = newColor;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Frost:
                switch(_selectedChoice) {
                    case 1:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].hp + "\n" +
                            "Attack: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice1].deathEffect;

                        summonChoice1Name.color = newColor;
                        break;
                    case 2:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].hp + "\n" +
                            "Attack: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice2].deathEffect;

                        summonChoice2Name.color = newColor;
                        break;
                    case 3:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].hp + "\n" +
                            "Attack: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.frostSummons[MasterController.Instance.possibleFrostSummon.choice3].deathEffect;

                        summonChoice3Name.color = newColor;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Thunder:
                switch(_selectedChoice) {
                    case 1:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].hp + "\n" +
                            "Attack: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice1].deathEffect;

                        summonChoice1Name.color = newColor;
                        break;
                    case 2:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].hp + "\n" +
                            "Attack: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice2].deathEffect;

                        summonChoice2Name.color = newColor;
                        break;
                    case 3:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].hp + "\n" +
                            "Attack: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.thunderSummons[MasterController.Instance.possibleThunderSummon.choice3].deathEffect;

                        summonChoice3Name.color = newColor;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Wind:
                switch(_selectedChoice) {
                    case 1:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].hp + "\n" +
                            "Attack: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice1].deathEffect;

                        summonChoice1Name.color = newColor;
                        break;
                    case 2:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].hp + "\n" +
                            "Attack: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice2].deathEffect;

                        summonChoice2Name.color = newColor;
                        break;
                    case 3:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].hp + "\n" +
                            "Attack: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.windSummons[MasterController.Instance.possibleWindSummon.choice3].deathEffect;

                        summonChoice3Name.color = newColor;
                        break;
                    default:
                        break;
                }
                break;
            case OrbsAndCrystal.Crystal:
                switch(_selectedChoice) {
                    case 1:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].hp + "\n" +
                            "Attack: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice1].deathEffect;

                        summonChoice1Name.color = newColor;
                        break;
                    case 2:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].hp + "\n" +
                            "Attack: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice2].deathEffect;

                        summonChoice2Name.color = newColor;
                        break;
                    case 3:
                        summonChoiceDescriptionText.text = 
                            "HP: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].hp + "\n" +
                            "Attack: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].attack + "\n" +
                            "Move Distance: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].moveDistance + "\n" +
                            "Summon Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].summonEffect + "\n" +
                            "Attack Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].attackEffect + "\n" +
                            "Capture Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].captureEffect + "\n" +
                            "Defend Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].defendEffect + "\n" +
                            "Death Effect: " + MasterController.Instance.crystalSummons[MasterController.Instance.possibleCrystalSummon.choice3].deathEffect;

                        summonChoice3Name.color = newColor;
                        break;
                    default:
                        break;
                }
                break;
            default:
                break;
        }

        AudioManager.Instance.Play("Click");
    }

    /// <summary>
    /// Miscellaneous
    /// </summary>
    public IEnumerator TypeSentence(string sentence) {
        currentPhaseText.text = "";
        foreach (char letter in sentence.ToCharArray()) {
            currentPhaseText.text += letter;
            yield return new WaitForSecondsRealtime(sentenceSpeed);
        }
    }
}
