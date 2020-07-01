using System;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Client.Library.Cards;
using CardGame.Client.Player;
using Godot;

namespace CardGame.Tests.Scripts.Connected
{
    public class ActionTest: ConnectedFixture
    {
        public override void Start()
        {
            DeckList.Add(SetCodes.AlphaDungeonGuide, 39);
            DeckList.Add(SetCodes.AlphaQuestReward, 1);
        }
        public override void Pre() => AddGame();
        public override void Post() => RemoveGame();
        
        [Test]
        public async void OnUnitInHandDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var toDeploy = (Card) Player.Hand.GetChild(1);
            toDeploy.DoubleClick();
            await ToSignal(UntilSignal(Player, nameof(View.AnimationFinished), 5), YIELD);
            Assert.Has(toDeploy, Player.Units.GetChildren(), $"{toDeploy} was Deployed");
        }
        
        [Test]
        public async void OnCardInHandDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var toSet = (Card) Player.Hand.GetChild(0);
            toSet.DoubleClick();
            await ToSignal(UntilSignal(Player, nameof(View.AnimationFinished), 5), YIELD);
            Assert.Has(toSet, Player.Support.GetChildren(), $"{toSet} was Deployed");
        }
    }
}
