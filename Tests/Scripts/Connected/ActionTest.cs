using System;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Client.Library.Cards;
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
            //Assert.Fail("Unfinished Test");
        }
    }
}
