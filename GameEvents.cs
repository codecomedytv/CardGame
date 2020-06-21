namespace CardGame
{
    public enum GameEvents
    {
        NoOp,
        LoadDeck,
        Draw,
        OpponentLoadDeck,
        OpponentDraw,
        Deploy,
        OpponentDeploy,
        ReadyCard,
        UnreadyCard,
        SetState,
        SetDeployable,
        SetSettable,
        SetActivatable,
        SetAsAttacker,
        MoveCard,
        SetProperty,
        CreateCard,
        DeleteCard,
        EndTurn
    }
}