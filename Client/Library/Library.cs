using System.Diagnostics.PerformanceData;
using System.Dynamic;
using CardGame.Client.Library.Alpha;
using CardGame.Client.Library.Card;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Library
{
    public class Library
    {
        public static Card.Card Fetch(int id, SetCodes setCode)
        {
            
            var card = setCode switch
            {
                SetCodes.Alpha_DungeonGuide => Create(id, new DungeonGuide()),
                SetCodes.Alpha_QuestReward => Create(id, new QuestReward()),
                SetCodes.Alpha_CounterAttack => Create(id, new CounterAttack()),
                SetCodes.Alpha_WrongWay => Create(id, new WrongWay()),
                SetCodes.Alpha_GuardPuppy => Create(id, new GuardPuppy()),
                SetCodes.Alpha_TrainingTrainer => Create(id, new TrainingTrainer()),
                SetCodes.Alpha_NoviceArcher => Create(id, new NoviceArcher()),
                _ => Create(id, new NullCard())
            };
                
            return card;
        }

        private static Card.Card Create(int id, BaseCard data)
        {
            var cardScene = (PackedScene) ResourceLoader.Load("res://Client/Library/Card/Card.tscn");
            var card = (Card.Card) cardScene.Instance();
            card.Id = id;
            card.SetData(data);
            return card;
        }
    }
}
