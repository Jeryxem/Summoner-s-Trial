using UnityEngine;

public class M_Player_Default : SummonsAbility
{
    public static M_Player_Default Instance;
    Moveable self;

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
        self = GetComponent<Moveable>();
    }

    public void MoveToDefaultPosition() {
        self.MoveToStartingTile();
    }

    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {

    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}
