using System;
using System.Reflection;
using Godot.Collections;

namespace CardGame.Server
{
        public class Library
        {

            public static Card Create(int setCode)
         {
             switch ((SetCodes)setCode)
             {
                 // This is a naive implementation we should refactor out
                 case SetCodes.Debug1000_1000:
                     return new Atk1KDef1K();
                 case SetCodes.Debug500_500:
                     return new Atk500Def500();
                 case SetCodes.Debug1500_1000:
                     return new Atk15KDef1k();
                 case SetCodes.DebugBounceFromField:
                     return new BounceFromField();
                 case SetCodes.DebugCannotBeDestoyedByEffect:
                     return new CannotBeDestroyedByEffect();
                 case SetCodes.DebugDestroyAttackingUnit:
                     return new DestroyAttackingUnit();
                 case SetCodes.DebugDestroyOpponentUnit:
                     return new DestroyOpponentUnit();
                 case SetCodes.DebugDiscardYourCard:
                     return new DiscardYourCard();
                 case SetCodes.DebugDraw2Cards:
                     return new Draw2Cards();
                 case SetCodes.MillOneFromDeck:
                     return new MillOneFromDeck();
                 case SetCodes.DebugPlayerCannotTakeDamage:
                     return new PlayerCannotTakeDamage();
                 case SetCodes.DebugReturnToDeck:
                     return new ReturnToDeck();
                 case SetCodes.DebugUntargetableUnit:
                     return new Untargetable();
                 case SetCodes.Alpha_CounterAttack:
                     return new CounterAttack();
                 case SetCodes.Alpha_DungeonGuide:
                     return new DungeonGuide();
                 case SetCodes.Alpha_GuardPuppy:
                     return new GuardPuppy();
                 case SetCodes.Alpha_NoviceArcher:
                     return new NoviceArcher();
                 case SetCodes.Alpha_QuestReward:
                     return new QuestReward();
                 case SetCodes.Alpha_TrainingTrainer:
                     return new TrainingTrainer();
                 case SetCodes.Alpha_WrongWay:
                     return new WrongWay();
                 default:
                     throw new CustomAttributeFormatException("setCode Not Found");
             }
         }
    }
}
