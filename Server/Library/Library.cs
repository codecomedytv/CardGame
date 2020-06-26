using System;
using System.Reflection;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server
{
        public class Library
        {

            public static Card Create(SetCodes setCode, Player owner)
         {
             switch ((SetCodes)setCode)
             {
                 // This is a naive implementation we should refactor out
                 case SetCodes.Debug1000_1000:
                     return new Atk1KDef1K(owner);
                 case SetCodes.Debug500_500:
                     return new Atk500Def500(owner);
                 case SetCodes.Debug1500_1000:
                     return new Atk15KDef1k(owner);
                 case SetCodes.DebugBounceFromField:
                     return new BounceFromField(owner);
                 case SetCodes.DebugCannotBeDestoyedByEffect:
                     return new CannotBeDestroyedByEffect(owner);
                 case SetCodes.DebugDestroyAttackingUnit:
                     return new DestroyAttackingUnit(owner);
                 case SetCodes.DebugDestroyOpponentUnit:
                     return new DestroyOpponentUnit(owner);
                 case SetCodes.DebugDiscardYourCard:
                     return new DiscardYourCard(owner);
                 case SetCodes.DebugDraw2Cards:
                     return new Draw2Cards(owner);
                 case SetCodes.MillOneFromDeck:
                     return new MillOneFromDeck(owner);
                 case SetCodes.DebugPlayerCannotTakeDamage:
                     return new PlayerCannotTakeDamage(owner);
                 case SetCodes.DebugReturnToDeck:
                     return new ReturnCardToDeck(owner);
                 case SetCodes.DebugUntargetableUnit:
                     return new Untargetable(owner);
                 case SetCodes.Alpha_CounterAttack:
                     return new CounterAttack(owner);
                 case SetCodes.Alpha_DungeonGuide:
                     return new DungeonGuide(owner);
                 case SetCodes.Alpha_GuardPuppy:
                     return new GuardPuppy(owner);
                 case SetCodes.Alpha_NoviceArcher:
                     return new NoviceArcher(owner);
                 case SetCodes.Alpha_QuestReward:
                     return new QuestReward(owner);
                 case SetCodes.Alpha_TrainingTrainer:
                     return new TrainingTrainer(owner);
                 case SetCodes.Alpha_WrongWay:
                     return new WrongWay(owner);
                 default:
                     throw new CustomAttributeFormatException("setCode Not Found");
             }
         }
    }
}
