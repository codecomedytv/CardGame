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
            await OpponentState;
            Assert.IsEqual(8, Opponent.Hand.GetChildCount(), "Opponent Drew A Card");
        }

        [Test]
        public async void OnUnitOnFieldDoubleClicked()
        {
            await ToSignal(UntilTimeout(0.2), YIELD);
            var attacker = (Card) Player.Hand.GetChild(2);
            attacker.DoubleClick();
            await PlayerState;
            OpponentMockGame.Pass();
            await PlayerState;
            PlayerMockGame.Pass();
            await PlayerState;
            PlayerMockGame.End();
            await OpponentState;
            var defending = (Card) Opponent.Hand.GetChild(0);
            defending.DoubleClick();
            await PlayerState;
            PlayerMockGame.Pass();
            await ToSignal(UntilTimeout(0.2F), YIELD);
            await OpponentState;
            OpponentMockGame.Pass();
            await ToSignal(UntilTimeout(0.2F), YIELD);
            await OpponentState;
            OpponentMockGame.End();
            await PlayerState;
            attacker.DoubleClick();
            OppViewFromPlayer.Units.GetChild<Card>(0).DoubleClick(); // Make sure to click the Attacker's copy
            await OpponentState;
            OpponentMockGame.Pass();
            await PlayerState;
            PlayerMockGame.Pass();
            await PlayerState;

            Assert.Has(defending, Opponent.Discard.GetChildren(), $"{defending} is in Opponent's Graveyard");


        }
    }
}
