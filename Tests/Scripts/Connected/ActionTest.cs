using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Client.Cards;
using CardGame.Client.Room;
using Godot;

namespace CardGame.Tests.Scripts.Connected
{
    public class ActionTest: ConnectedFixture
    {
        // TODO: Implement Direct Attack Test
        // TODO: Implement Attack Test
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
            await PlayerState;
            var toDeploy = (Card) Player.Hand[0];
            toDeploy.View.DoubleClick();
            //await ToSignal(UntilSignal(Player, nameof(Player.AnimationFinished), 5), YIELD);
            await PlayerState;
            Assert.Has(toDeploy, Player.Units, $"{toDeploy} was Deployed");
        }
        
        [Test]
        public async void OnSupportInHandDoubleClicked()
        {
            await PlayerState;
            var toSet = (Card) Player.Hand[1];
            toSet.View.DoubleClick();
            //await ToSignal(UntilSignal(Player, nameof(Player.AnimationFinished), 5), YIELD);
            await PlayerState;
            Assert.Has(toSet, Player.Support, $"{toSet} was Set");
        }

        [Test]
        public async void OnEndTurn()
        {
            await PlayerState;
            PlayerMockGame.End();
            PlayerMockGame.Visible = false;
            await OpponentState;
            Assert.IsEqual(8, Opponent.Hand.Count, "Opponent Drew A Card");
        }

        [Test]
        public async void OnUnitOnFieldDoubleClicked()
        {
            await PlayerState;
            var attacker = (Card) Player.Hand[2];
            attacker.View.DoubleClick();
            await OpponentState;
            OpponentMockGame.Pass();
            await PlayerState;
            PlayerMockGame.Pass();
            await PlayerState;
            PlayerMockGame.End();
            await OpponentState;
            var defending = (Card) Opponent.Hand[0];
            defending.View.DoubleClick();
            await PlayerState;
            PlayerMockGame.Pass();
            await OpponentState;
            OpponentMockGame.Pass();
            await OpponentState;
            OpponentMockGame.End();
            await PlayerState;
            attacker.View.DoubleClick();
            OppPlayerFromPlayer.Units[0].View.DoubleClick(); // Make sure to click the Attacker's copy
            await OpponentState;
            OpponentMockGame.Pass();
            await PlayerState;
            PlayerMockGame.Pass();
            await PlayerState;
        
            Assert.Has(defending, Opponent.Discard, $"{defending} is in Opponent's Graveyard");

        }
    }
}
