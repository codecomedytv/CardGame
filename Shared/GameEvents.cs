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

    public enum Commands
    {
        // We may merge this with events in future
        // Have to considered things like "bounce" etc && SendCardToZone differences
        Draw,
        LoadDeck,
        UpdateCard,
        Deploy,
        RevealCard,
        SetFaceDown,
        Activate,
        SendCardToZone,
        BattleUnit,
        OpponentAttackUnit,
        OpponentAttackDirectly,
        LoseLife,
        
        // Unhandled
        ResolveCard,
        Trigger,
        GameOver,
        DirectAttack,
        BounceCard,
        TargetRequested,
        SetState
    }
}