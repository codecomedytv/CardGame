using System.Collections.Generic;
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
    }
}

/*
func test_that_discards_your_target_card() -> void:
	describe("That discards target card from player's hand")

	decklist.append(code.DebugDiscardYourCard)

	start_game(decklist)
	var CardToDiscard: Unit = player[1].hand.get_card_at_index(1)
	var DiscardYourCard: Support = player[1].hand.get_card_at_index(0)
	play.set_facedown(player[1].id, DiscardYourCard.id)
	play.end_turn(player[1].id)
	play.end_turn(player[0].id)
	play.activate_facedown(player[1].id, DiscardYourCard.id, 0, [CardToDiscard.id])
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.has(CardToDiscard, player[1].discard, \
				  "Then that card is in that player's discard")

func test_that_returns_a_unit_to_opponents_hand() -> void:
	describe("That returns target card to the Opponent's hand")

	decklist.append(code.DebugBounceFromField)

	start_game(decklist)
	var BounceEnemyCard: Support = player[1].hand.get_card_at_index(0)
	var CardToBounce: Unit = player[0].hand.get_card_at_index(1)
	play.set_facedown(player[1].id, BounceEnemyCard.id)
	play.end_turn(player[1].id)
	play.deploy(player[0].id, CardToBounce.id)
	play.pass_play(player[1].id)
	play.pass_play(player[0].id)
	play.end_turn(player[0].id)
	var precondition: bool = player[0].field.has(CardToBounce)
	play.activate_facedown(player[1].id, BounceEnemyCard.id, 0, [CardToBounce.id])
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.is_true(precondition, "That was on the Enemies Field")
	asserts.has(CardToBounce, player[0].hand, \
				 "Then that unit is in Enemies Hand")
#
func test_that_sends_a_card_from_the_top_of_players_deck() -> void:
	describe("That sends the top card of players deck to their discard")

	decklist.append(code.DebugMillOneFromDeck)

	start_game(decklist)
	var MillCard: Support = player[1].hand.get_card_at_index(0)
	play.set_facedown(player[1].id, MillCard.id)
	play.end_turn(player[1].id)
	play.end_turn(player[0].id)

	var CardToMill: Card = player[1].deck.back()
	play.activate_facedown(player[1].id, MillCard.id)
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.has(CardToMill, player[1].discard, \
				  "Then that card is in the player's discard")

func test_that_returns_a_card_to_the_players_deck() -> void:
	describe("That returns a card from player's hand to the player's deck")

	decklist.append(code.DebugReturnToDeck)

	start_game(decklist)
	var ReturnToDeckCard: Support = player[1].hand.get_card_at_index(0)
	play.set_facedown(player[1].id, ReturnToDeckCard.id)
	play.end_turn(player[1].id)
	play.end_turn(player[0].id)

	var CardToReturn: Card = player[1].hand.back()
	play.activate_facedown(player[1].id, ReturnToDeckCard.id, 0, [CardToReturn.id])
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.has(CardToReturn, player[1].deck, \
				  "Then that card is in the player's deck")

func test_that_tries_to_destroy_an_indestructible_card() -> void:
	describe("That tries to destroy a card that cannot be destroyed by effects")

	decklist.append(code.DebugDestroyOpponentUnit)
	decklist.append(code.DebugCannotBeDestroyedByEffect)
	start_game(decklist)

	var InvincibleCard: Unit = player[0].hand.get_card_at_index(0)
	var DestroyUnitCard: Support = player[1].hand.get_card_at_index(1)

	play.set_facedown(player[1].id, DestroyUnitCard.id)
	play.end_turn(player[1].id)
	play.deploy(player[0].id, InvincibleCard.id)
	play.pass_play(player[1].id)
	play.pass_play(player[0].id)
	play.end_turn(player[0].id)
	play.activate_facedown(player[1].id, DestroyUnitCard.id, 0, [InvincibleCard.id])
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.has(DestroyUnitCard, player[1].discard, \
				 "Then that skill's card is in the discard")

	asserts.does_not_have(InvincibleCard, player[0].discard, \
						"But the invincible card is not in its owner discard")

func test_that_adds_a_tag_to_player_when_unit_card_is_played() -> void:
	describe("Of a Unit Card that adds the constant Tag.CannotTakeDamage" \
			  + " to its controller")

	decklist.append(code.DebugPlayerCannotTakeDamage)
	start_game(decklist)

	var PreventDamage: Unit = player[0].hand.get_card_at_index(0)
	var Debug500_500: Unit = player[0].hand.get_card_at_index(4)
	var Debug1500_1000: Unit = player[1].hand.get_card_at_index(1)

	play.deploy(player[1].id, Debug1500_1000.id)
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)
	play.end_turn(player[1].id)
	play.deploy(player[0].id, Debug500_500.id)
	play.pass_play(player[1].id)
	play.pass_play(player[0].id)
	play.deploy(player[0].id, PreventDamage.id)
	play.pass_play(player[1].id)
	play.pass_play(player[0].id)
	play.end_turn(player[0].id)
	var life: int = player[0].life
	play.declare_attack(player[1].id, Debug1500_1000.id, Debug500_500.id)
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.is_true(player[0].has_tag(Tags.CannotTakeDamage), \
				"Defending player has Tag.CannotTakeDamage")
	asserts.is_equal(life, player[0].life, "Player's life did not change")
	asserts.has(Debug500_500, player[0].discard, \
				 "But the defending unit was still destroyed")
#
func test_that_targets_a_unit_when_only_one_nontargetable_target_exists() -> void:
	describe("That targets a unit when only one nontargetable target exists")

	decklist.append(code.DebugUntargetableUnit)
	decklist.append(code.DebugDestroyOpponentUnit)
	start_game(decklist)

	var Untargetable: Unit = player[0].hand.get_card_at_index(1)
	var DestroyUnit: Support = player[1].hand.get_card_at_index(0)

	play.set_facedown(player[1].id, DestroyUnit.id)
	play.end_turn(player[1].id)
	play.deploy(player[0].id, Untargetable.id)
	play.pass_play(player[1].id)
	play.pass_play(player[0].id)
	play.end_turn(player[0].id)

	play.activate_facedown(player[1].id, DestroyUnit.id, 0, [Untargetable.id])
	play.pass_play(player[0].id)
	play.pass_play(player[1].id)

	asserts.does_not_have(DestroyUnit, player[1].discard, \
						   "DestroyUnit was not sent to player's discard")
	asserts.does_not_have(Untargetable, player[0].discard, \
						   "Untargetable Card was not destroyed")
						   */