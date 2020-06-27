using System.Diagnostics.PerformanceData;
using System.Dynamic;
using CardGame.Client.Library.Alpha;
using CardGame.Client.Library.Cards;
using Godot;
using Godot.Collections;

namespace CardGame.Client.Library
{
    public class Library
    {
        public static Card Fetch(int id, SetCodes setCode)
        {
            
            var card = setCode switch
            {
                SetCodes.AlphaDungeonGuide => Create(id, new DungeonGuide()),
                SetCodes.AlphaQuestReward => Create(id, new QuestReward()),
                SetCodes.AlphaCounterAttack => Create(id, new CounterAttack()),
                SetCodes.AlphaWrongWay => Create(id, new WrongWay()),
                SetCodes.AlphaGuardPuppy => Create(id, new GuardPuppy()),
                SetCodes.AlphaTrainingTrainer => Create(id, new TrainingTrainer()),
                SetCodes.AlphaNoviceArcher => Create(id, new NoviceArcher()),
                _ => Create(id, new NullCard())
            };
                
            return card;
        }

        private static Card Create(int id, BaseCard data)
        {
            var cardScene = (PackedScene) ResourceLoader.Load("res://Client/Library/Card/Card.tscn");
            var card = (Card) cardScene.Instance();
            card.Id = id;
            card.SetData(data);
            return card;
        }
    }
}
