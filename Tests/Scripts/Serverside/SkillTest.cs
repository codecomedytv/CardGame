using System.Collections.Generic;
using System.Linq;
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
            Play.Activate(Players[1].Id, support, 0, new Array<int>{unitA.Id});
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
	        Play.Activate(Players[1].Id, discarder, 0, new Array<int> {discarding.Id} );
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
	        Play.Activate(Players[1].Id, bouncer, 0, new Array<int>{ bounce.Id });
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
	        Play.Activate(Players[1].Id, millCard, 0, new Array<int>());
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
	        Play.Activate(Players[1].Id, returnToDeckCard, 0, new Array<int> {cardToReturn.Id});
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);

	        Assert.Has(cardToReturn, Players[1].Deck, "Then that card is in the Player's Deck");
        }

        [Test]
        public void That_Tries_To_Destroy_An_Indestructible_Card()
        {

	        DeckList.Add(SetCodes.DebugDestroyOpponentUnit);
	        DeckList.Add(SetCodes.DebugCannotBeDestoyedByEffect);
	        StartGame(DeckList);
	        var invincibleCard = Players[0].Hand[0];
	        var destroyUnitCard = Players[1].Hand[1];
	        Play.SetFaceDown(Players[1].Id, destroyUnitCard.Id);
	        Play.EndTurn(Players[1].Id);
	        Play.Deploy(Players[0].Id, invincibleCard.Id);
	        Play.PassPlay(Players[1].Id);
	        Play.PassPlay(Players[0].Id);
	        Play.EndTurn(Players[0].Id);
	        Play.Activate(Players[1].Id, destroyUnitCard.Id, 0, new Array<int>{invincibleCard.Id});
	        Play.PassPlay(Players[0].Id);
	        Play.PassPlay(Players[1].Id);

	        Assert.Has(destroyUnitCard, Players[1].Graveyard, "Then that skill's card is in the discard");

	        Assert.DoesNotHave(invincibleCard, Players[0].Graveyard,
		        "But the invincible card is not in its owner discard");
        }
    }
}

/*

#






public void test_that_adds_a_tag_to_Players_when_unit_card_is_played() -> void:
	describe("Of a Unit Card that adds the constant Tag.CannotTakeDamage" \
			  + " to its controller")

	DeckList.Add(SetCodes.DebugPlayerCannotTakeDamage)
	StartGame(DeckList)

	var PreventDamage: Unit = Players[0].hand.get_card_at_index(0)
	var Debug500_500: Unit = Players[0].hand.get_card_at_index(4)
	var Debug1500_1000: Unit = Players[1].hand.get_card_at_index(1)

	Play.Deploy(Players[1].Id, Debug1500_1000.Id)
	Play.PassPlay(Players[0].Id)
	Play.PassPlay(Players[1].Id)
	Play.EndTurn(Players[1].Id)
	Play.Deploy(Players[0].Id, Debug500_500.Id)
	Play.PassPlay(Players[1].Id)
	Play.PassPlay(Players[0].Id)
	Play.Deploy(Players[0].Id, PreventDamage.Id)
	Play.PassPlay(Players[1].Id)
	Play.PassPlay(Players[0].Id)
	Play.EndTurn(Players[0].Id)
	var life: int = Players[0].life
	Play.declare_attack(Players[1].Id, Debug1500_1000.Id, Debug500_500.Id)
	Play.PassPlay(Players[0].Id)
	Play.PassPlay(Players[1].Id)

	Assert.IsTrue(Players[0].has_tag(Tags.CannotTakeDamage), \
				"Defending Players has Tag.CannotTakeDamage")
	Assert.IsEqual(life, Players[0].life, "Player's life did not change")
	Assert.has(Debug500_500, Players[0].discard, \
				 "But the defending unit was still destroyed")
#
public void test_that_targets_a_unit_when_only_one_nontargetable_target_exists() -> void:
	describe("That targets a unit when only one nontargetable target exists")

	DeckList.Add(SetCodes.DebugUntargetableUnit)
	DeckList.Add(SetCodes.DebugDestroyOpponentUnit)
	StartGame(DeckList)

	var Untargetable: Unit = Players[0].hand.get_card_at_index(1)
	var DestroyUnit: Support = Players[1].hand.get_card_at_index(0)

	Play.SetFaceDown(Players[1].Id, DestroyUnit.Id)
	Play.EndTurn(Players[1].Id)
	Play.Deploy(Players[0].Id, Untargetable.Id)
	Play.PassPlay(Players[1].Id)
	Play.PassPlay(Players[0].Id)
	Play.EndTurn(Players[0].Id)

	Play.Activate(Players[1].Id, DestroyUnit.Id, 0, [Untargetable.Id])
	Play.PassPlay(Players[0].Id)
	Play.PassPlay(Players[1].Id)

	Assert.does_not_have(DestroyUnit, Players[1].discard, \
						   "DestroyUnit was not sent to Players's discard")
	Assert.does_not_have(Untargetable, Players[0].discard, \
						   "Untargetable Card was not destroyed")
						   */