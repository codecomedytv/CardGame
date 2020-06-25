using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
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
            
            var support = Player.Hand[0].Id;
            Play.SetFaceDown(Player.Id, support);
            Play.EndTurn(Player.Id);
            var unitA = Opponent.Hand[1];
            var unitB = Opponent.Hand[6];
            Play.Deploy(Opponent.Id, unitA.Id);
            Play.PassPlay(Player.Id);
            Play.PassPlay(Opponent.Id);
            Play.Deploy(Opponent.Id, unitB.Id);
            Play.PassPlay(Player.Id);
            Play.PassPlay(Opponent.Id);
            Play.EndTurn(Opponent.Id);
            Play.Activate(Player.Id, support, unitA.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            
            Assert.Has(unitA, Opponent.Graveyard, "Then That Unit Is Destroyed");

        }

        [Test]
        public void That_Discards_Your_Target_Card()
        {
	        DeckList.Add(SetCodes.DebugDiscardYourCard);
	        StartGame(DeckList);
	        var discarding = Player.Hand[1];
	        var discarder = Player.Hand[0].Id;
	        Play.SetFaceDown(Player.Id, discarder);
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);
	        Play.Activate(Player.Id, discarder, discarding.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);
	        
	        Assert.Has(discarding, Player.Graveyard, "Then that card is in that Player's Discard");
        }

        [Test]
        public void That_Returns_A_Unit_To_Opponents_Hand()
        {
	        DeckList.Add(SetCodes.DebugBounceFromField);
	        StartGame(DeckList);
	        var bouncer = Player.Hand[0].Id;
	        var bounce = Opponent.Hand[1];
	        Play.SetFaceDown(Player.Id, bouncer);
	        Play.EndTurn(Player.Id);
	        Play.Deploy(Opponent.Id, bounce.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);
	        var bounceIsOnField = Opponent.Field.Contains(bounce);
	        Play.Activate(Player.Id, bouncer, bounce.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.IsTrue(bounceIsOnField, "Bounced Card Was On Field");
	        Assert.Has(bounce, Opponent.Hand, "Bounce is in Owners Hand");

        }

        [Test]
        public void That_Mills_A_Card()
        {

	        DeckList.Add(SetCodes.MillOneFromDeck);

	        StartGame(DeckList);
	        var millCard = Player.Hand[0].Id;
	        Play.SetFaceDown(Player.Id, millCard);
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);
	        var cardToMill = Player.Deck[Player.Deck.Count - 1];
	        Play.Activate(Player.Id, millCard);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.Has(cardToMill, Player.Graveyard, "Then that card is in the Player's discard");
        }

        [Test]
        public void That_Returns_A_Card_To_The_Players_Deck()
        {

	        DeckList.Add(SetCodes.DebugReturnToDeck);

	        StartGame(DeckList);
	        var returnToDeckCard = Player.Hand[0].Id;
	        Play.SetFaceDown(Player.Id, returnToDeckCard);
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);

	        var cardToReturn = Player.Hand[0];
	        Play.Activate(Player.Id, returnToDeckCard, cardToReturn.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.Has(cardToReturn, Player.Deck, "Then that card is in the Player's Deck");
        }
        
        

    }
}

/*

#





#
	
*/