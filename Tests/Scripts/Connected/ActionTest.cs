using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Client.Library.Cards;
using CardGame.Client.Players;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Tests.Scripts.Connected
{
    public class ActionTest: ConnectedFixture
    {
        // TODO: Implement Battle Test
        // TODO: Implement Direct Attack Test
        // TODO: Implement Attack Test
        // TODO: Implement EndTurnTest
        public override void Start()
        {
            // Last card added is card 0 in hand
            DeckList.Add(SetCodes.AlphaDungeonGuide, 38);
            DeckList.Add(SetCodes.AlphaQuestReward, 1);
            DeckList.Add(SetCodes.AlphaTrainingTrainer, 1);
        }
        public override void Pre() => AddGame();
        public override void Post() => RemoveGame();
        
        [Test]
        public async void OnUnitInHandDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var toDeploy = (Card) Player.Hand.GetChild(0);
            toDeploy.DoubleClick();
            await ToSignal(UntilSignal(Player, nameof(View.AnimationFinished), 5), YIELD);
            Assert.Has(toDeploy, Player.Units.GetChildren(), $"{toDeploy} was Deployed");
        }
        
        [Test]
        public async void OnSupportInHandDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var toSet = (Card) Player.Hand.GetChild(1);
            toSet.DoubleClick();
            await ToSignal(UntilSignal(Player, nameof(View.AnimationFinished), 5), YIELD);
            Assert.Has(toSet, Player.Support.GetChildren(), $"{toSet} was Set");
        }

        [Test]
        public async void OnEndTurn()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            PlayerMockGame.End();
            PlayerMockGame.Visible = false;
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            Assert.IsEqual(8, Opponent.Hand.GetChildCount(), "Opponent Drew A Card");
        }

        [Test]
        public async void OnUnitOnFieldDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var attacker = (Card) Player.Hand.GetChild(2);
            attacker.DoubleClick();
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            OpponentMockGame.Pass();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);
            PlayerMockGame.Pass();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);
            PlayerMockGame.End();
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            var defending = (Card) Opponent.Hand.GetChild(0);
            defending.DoubleClick();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);
            PlayerMockGame.Pass();
            await ToSignal(UntilTimeout(0.2F), YIELD);
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            OpponentMockGame.Pass();
            await ToSignal(UntilTimeout(0.2F), YIELD);
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            OpponentMockGame.End();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);
            attacker.DoubleClick();
            OppViewFromPlayer.Units.GetChild<Card>(0).DoubleClick(); // Make sure to click the Attacker's copy
            await ToSignal(UntilSignal(OpponentMockGame, nameof(Game.StateSet), 5), YIELD);
            OpponentMockGame.Pass();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);
            PlayerMockGame.Pass();
            await ToSignal(UntilSignal(PlayerMockGame, nameof(Game.StateSet), 5), YIELD);

            Assert.Has(defending, Opponent.Discard.GetChildren(), $"{defending} is in Opponent's Graveyard");


        }
    }
}
