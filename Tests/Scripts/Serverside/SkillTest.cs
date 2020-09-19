using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Client;
using CardGame.Server;
using CardGame.Server.Game.Tags;
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
            DeckList.Add(SetCodes.Debug10001000);
            DeckList.Add(SetCodes.Debug10001000);
            DeckList.Add(SetCodes.Debug10001000);
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug15001000);
            DeckList.Add(SetCodes.Debug15001000);
            DeckList.Add(SetCodes.Debug15001000);
 
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
        
        [Test]
        public void That_Tries_To_Destroy_An_Indestructible_Card()
        {

	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.DebugCannotBeDestoyedByEffect);
	        StartGame(DeckList);
	        var invincibleCard = Opponent.Hand[0];
	        var destroyUnitCard = Player.Hand[1];
	        Play.SetFaceDown(Player.Id, destroyUnitCard.Id);
	        Play.EndTurn(Player.Id);
	        Play.Deploy(Opponent.Id, invincibleCard.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);
	        Play.Activate(Player.Id, destroyUnitCard.Id, invincibleCard.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.Has(destroyUnitCard, Player.Graveyard, "Then that skill's card is in the discard");

	        Assert.DoesNotHave(invincibleCard, Opponent.Graveyard,
		        "But the invincible card is not in its owner discard");
        }
        
        [Test]
        public void That_Adds_A_Tag_To_Players_When_Unit_Card_Is_Played()
        {
	        
	        // Battle is not implemented currently so this fails
	        DeckList.Add(SetCodes.DebugPlayerCannotTakeDamage);
	        StartGame(DeckList);

	        var preventDamage = Opponent.Hand[0];
	        var debug500500 = Opponent.Hand[4];
	        var debug15001000 = Player.Hand[1];

	        Play.Deploy(Player.Id, debug15001000.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);
	        Play.EndTurn(Player.Id);
	        Play.Deploy(Opponent.Id, debug500500.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.Deploy(Opponent.Id, preventDamage.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);
	        var life = Opponent.Health;
	        Play.Attack(Player.Id, debug15001000.Id, debug500500.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.IsTrue(Opponent.HasTag(TagIds.CannotTakeBattleDamage), "Defending Player has Tag.CannotTakeDamage");
	        Assert.IsEqual(life, Opponent.Health, "Player's life did not change");
	        Assert.Has(debug500500, Opponent.Graveyard, "But the defending unit was still destroyed");
        }
        
        [Test]
        public void That_Targets_A_Unit_When_Only_One_Nontargetable_Target_Exists()
        {
	        // This requires restructing our targeting based systems.
	        DeckList.Add(SetCodes.DebugCannotBeTargeted);
	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        StartGame(DeckList);

	        var cannotBeTargeted  = Opponent.Hand[1];
	        
	        var destroyUnit  = Player.Hand[0];

	        Play.SetFaceDown(Player.Id, destroyUnit.Id);
	        Play.EndTurn(Player.Id);
	        Play.Deploy(Opponent.Id, cannotBeTargeted.Id);
	        Play.PassPlay(Player.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.EndTurn(Opponent.Id);

	        Play.Activate(Player.Id, destroyUnit.Id, cannotBeTargeted.Id);
	        Play.PassPlay(Opponent.Id);
	        Play.PassPlay(Player.Id);

	        Assert.DoesNotHave(destroyUnit, Player.Graveyard, "DestroyUnit was not sent to Player's discard");
	        Assert.DoesNotHave(cannotBeTargeted, Opponent.Graveyard, "Untargetable Card was not destroyed");
        }		


        
        

    }
}
