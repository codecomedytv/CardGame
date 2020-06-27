using System;
using System.Reflection;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server
{
        public static class Library
        {

            public static Card Create(SetCodes setCode, Player owner)
            {
                return (SetCodes) setCode switch
                {
                    // This is a naive implementation we should refactor out
                    SetCodes.Debug10001000 => new Atk1KDef1K(owner),
                    SetCodes.Debug500500 => new Atk500Def500(owner),
                    SetCodes.Debug15001000 => new Atk15KDef1k(owner),
                    SetCodes.DebugBounceFromField => new BounceFromField(owner),
                    SetCodes.DebugCannotBeDestoyedByEffect => new CannotBeDestroyedByEffect(owner),
                    SetCodes.DebugDestroyAttackingUnit => new DestroyAttackingUnit(owner),
                    SetCodes.DebugDestroyOpponentUnit => new DestroyOpponentUnit(owner),
                    SetCodes.DebugDiscardYourCard => new DiscardYourCard(owner),
                    SetCodes.DebugDraw2Cards => new Draw2Cards(owner),
                    SetCodes.MillOneFromDeck => new MillOneFromDeck(owner),
                    SetCodes.DebugPlayerCannotTakeDamage => new PlayerCannotTakeDamage(owner),
                    SetCodes.DebugReturnToDeck => new ReturnCardToDeck(owner),
                    SetCodes.DebugCannotBeTargeted => new Untargetable(owner),
                    SetCodes.AlphaCounterAttack => new CounterAttack(owner),
                    SetCodes.AlphaDungeonGuide => new DungeonGuide(owner),
                    SetCodes.AlphaGuardPuppy => new GuardPuppy(owner),
                    SetCodes.AlphaNoviceArcher => new NoviceArcher(owner),
                    SetCodes.AlphaQuestReward => new QuestReward(owner),
                    SetCodes.AlphaTrainingTrainer => new TrainingTrainer(owner),
                    SetCodes.AlphaWrongWay => new WrongWay(owner),
                    _ => throw new CustomAttributeFormatException("setCode Not Found")
                };
            }
    }
}
