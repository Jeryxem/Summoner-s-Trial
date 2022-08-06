public class M_Clockwork_Spider_Mini : SummonsAbility
{
    public override void SummonEffect() {

    }
    public override void AttackEffect(Tile tile) {

    }
    public override void CaptureEffect(Tile tile) {
        Moveable self = GetComponent<Moveable>();
        self.MoveToSelectedTile(tile);
    }
    public override void DefendEffect(Tile tile) {

    }
    public override void DeathEffect() {
        
    }
}