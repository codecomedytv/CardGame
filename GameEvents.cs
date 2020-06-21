﻿namespace CardGame
{
    public enum GameEvents
    {
        NoOp,
        LoadDeck,
        Draw,
        OpponentLoadDeck,
        OpponentDraw,
        SetState,
        SetDeployable,
        SetSettable,
        SetActivatable,
        MoveCard,
        SetProperty,
        CreateCard,
        DeleteCard,
        EndTurn
    }
}