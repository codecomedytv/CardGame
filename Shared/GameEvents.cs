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
        // We may merge Commands with Events in future but..
        // .. we need to consider more specific events (no "SentToZone")
        
        // Some unhandled events are not listed for the time being
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
        DirectAttack,
        SetState
    }
}