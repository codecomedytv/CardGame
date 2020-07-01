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
            DeckList.Add(SetCodes.AlphaDungeonGuide, 40);
        }

        public override void Pre()
        {
            AddGame();
        }

        [Test]
        public async void OnCardDeployed()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            Assert.IsNotNull(PlayerMockGame);
            Assert.IsNotNull(OpponentMockGame);
            var toDeploy = (Card) Player.Hand.GetChild(0);
            toDeploy.DoubleClick();
            await ToSignal(UntilSignal(Player, nameof(View.AnimationFinished), 5), YIELD);
            //await ToSignal(UntilTimeout(1), YIELD);
            Assert.Has(toDeploy, Player.Units.GetChildren(), $"{toDeploy} was Deployed");
        }
    }
}
