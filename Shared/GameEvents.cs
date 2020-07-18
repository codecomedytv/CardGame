namespace CardGame
{
    public enum GameEvents
    {
        NoOp,
        LoadDeck,
        Draw,
        Deploy,
        SetFaceDown,
        Discard,
        Bounce, 
        Mill,
        DestroyByEffect,
        DestroyByBattle,
        BattleDamage,
        EffectDamage,
        DeclareAttack,
        DeclareDirectAttack,
        EndTurn,
        GameOver,
        TopDeck,
        Activate,
    }
}