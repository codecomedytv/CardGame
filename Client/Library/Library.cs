using System.Diagnostics.PerformanceData;
using CardGame.Client.Library.Alpha;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Library
{
    public class Library
    {
        static public Object Fetch(int id, SetCodes setCode)
        {
            var cardScene = ResourceLoader.Load("Card/object.tscn") as PackedScene;
            var card = (Card.Card) cardScene.Instance();
            card.Id = id;
            switch (setCode)
            {
                case SetCodes.Alpha_DungeonGuide:
                    card.SetData(new DungeonGuide());
                    break;
                case SetCodes.Alpha_QuestReward:
                    card.SetData(new QuestReward());
                    break;
                case SetCodes.Alpha_CounterAttack:
                    card.SetData(new CounterAttack());
                    break;
                case SetCodes.Alpha_WrongWay:
                    card.SetData(new WrongWay());
                    break;
                case SetCodes.Alpha_GuardPuppy:
                    card.SetData(new GuardPuppy());
                    break;
                case SetCodes.Alpha_TrainingTrainer:
                    card.SetData(new TrainingTrainer());
                    break;
                case SetCodes.Alpha_NoviceArcher:
                    card.SetData(new NoviceArcher());
                    break;
                default:
                    card.Blank = true;
                    break;
            }

            return card;

        }
    }
}
