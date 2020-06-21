using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
using CardGame.Server.Game;
using Godot;
using Godot.Collections;

namespace CardGame.Tests.Scripts.Serverside
{
    public class SkillTest: GameFixture
    {
        private List<SetCodes> DeckList = new List<SetCodes>();

        public override string Title()
        {
            return "When using a Skill";
        }

        public override void Pre()
        {
            DeckList.Clear();
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
 
        }

        [Test]
        public void That_Destroys_Target_Opponent_Unit()
        {
            DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
            StartGame(DeckList);
            
            var support = Players[1].Hand[0].Id;
            Play.SetFaceDown(Players[1].Id, support);
            Play.EndTurn(Players[1].Id);
            var unitA = Players[0].Hand[1];
            var unitB = Players[0].Hand[6];
            Play.Deploy(Players[0].Id, unitA.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.Deploy(Players[0].Id, unitB.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.EndTurn(Players[0].Id);
            Play.Activate(Players[1].Id, support, unitA.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            
            Assert.Has(unitA, Players[0].Graveyard, "Then That Unit Is Destroyed");

        }

        [Test]
        public void That_Discards_Your_Target_Card()
        {
	        DeckList.Add(SetCodes.DebugDiscardYourCard);
	        StartGame(DeckList);
	        var discarding = Players[1].Hand[1];
	        var discarder = Players[1].Hand[0].Id;
	        Play.SetFaceDown(Players[1].Id, discarder);
	        Play.EndTurn(Players[1].Id);
	        Play.EndTurn(Players[0].Id);
	        Play.Activate(Players[1].Id, discarder, discarding.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);
	        
	        Assert.Has(discarding, Players[1].Graveyard, "Then that card is in that Player's Discard");
        }

        [Test]
        public void That_Returns_A_Unit_To_Opponents_Hand()
        {
	        DeckList.Add(SetCodes.DebugBounceFromField);
	        StartGame(DeckList);
	        var bouncer = Players[1].Hand[0].Id;
	        var bounce = Players[0].Hand[1];
	        Play.SetFaceDown(Players[1].Id, bouncer);
	        Play.EndTurn(Players[1].Id);
	        Play.Deploy(Players[0].Id, bounce.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.EndTurn(Players[0].Id);
	        var bounceIsOnField = Players[0].Field.Contains(bounce);
	        Play.Activate(Players[1].Id, bouncer, bounce.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);

	        Assert.IsTrue(bounceIsOnField, "Bounced Card Was On Field");
	        Assert.Has(bounce, Players[0].Hand, "Bounce is in Owners Hand");

        }

        [Test]
        public void That_Mills_A_Card()
        {

	        DeckList.Add(SetCodes.MillOneFromDeck);

	        StartGame(DeckList);
	        var millCard = Players[1].Hand[0].Id;
	        Play.SetFaceDown(Players[1].Id, millCard);
	        Play.EndTurn(Players[1].Id);
	        Play.EndTurn(Players[0].Id);
	        var cardToMill = Players[1].Deck[Players[1].Deck.Count - 1];
	        Play.Activate(Players[1].Id, millCard);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);

	        Assert.Has(cardToMill, Players[1].Graveyard, "Then that card is in the Player's discard");
        }

        [Test]
        public void That_Returns_A_Card_To_The_Players_Deck()
        {

	        DeckList.Add(SetCodes.DebugReturnToDeck);

	        StartGame(DeckList);
	        var returnToDeckCard = Players[1].Hand[0].Id;
	        Play.SetFaceDown(Players[1].Id, returnToDeckCard);
	        Play.EndTurn(Players[1].Id);
	        Play.EndTurn(Players[0].Id);

	        var cardToReturn = Players[1].Hand[0];
	        Play.Activate(Players[1].Id, returnToDeckCard, cardToReturn.Id);
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);

	        Assert.Has(cardToReturn, Players[1].Deck, "Then that card is in the Player's Deck");
        }
        
        

    }
}

/*

#





#
	
*/