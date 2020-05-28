using System.Collections.Generic;

namespace CardGame.Tests.Scripts.Serverside
{
    public class JudgeTest: GameFixture
    {
        public List<SetCodes> DeckList = new List<SetCodes>();

        public override void Pre()
        {
	        DeckList.Clear();
	        DeckList.Add(SetCodes.Debug500_500);
	        DeckList.Add(SetCodes.Debug500_500);
	        DeckList.Add(SetCodes.Debug500_500);
			DeckList.Add(SetCodes.Debug1000_1000);
			DeckList.Add(SetCodes.Debug1000_1000);
			DeckList.Add(SetCodes.Debug1000_1000);
			DeckList.Add(SetCodes.Debug1000_1000);
			DeckList.Add(SetCodes.Debug1000_1000);
			DeckList.Add(SetCodes.Debug1000_1000);

        }

        public override string Title()
        {
	        return "A Player is Disqualified";
        }

        [Test]
        public void When_They_Deploy_A_Unit_During_Their_Opponents_Turn()
        {
	
	        StartGame(DeckList);
	        var unit = Players[0].Hand[0].Id;
	        Play.Deploy(Players[0].Id, unit);

	        Assert.IsTrue(Players[0].Disqualified);
        }

        [Test]
        public void When_They_Set_Support_During_Their_Opponents_Turn()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Players[0].Hand[0].Id;
	        Play.SetFaceDown(Players[0].Id, support);

	        Assert.IsTrue(Players[0].Disqualified);
        }

	    [Test]
        public void When_They_End_Their_Turn_During_Their_Opponents_Turn()
        {
	        StartGame(DeckList);
	        Play.EndTurn(Players[0].Id);

	        Assert.IsTrue(Players[0].Disqualified);
        }

        [Test]
        public void When_A_Player_Declares_An_Attack_During_Their_Opponents_Turn()
        {
	        StartGame(DeckList);
	        var unit = Players[1].Hand[0].Id;
	        Play.Deploy(Players[1].Id, unit);
	        // Ending twice so the unit is ready
	        Play.EndTurn(Players[1].Id);
	        Play.EndTurn(Players[0].Id);
	        Play.EndTurn(Players[1].Id);
	        var directAttack = -1;
	        Play.Attack(Players[1].Id, unit, directAttack);

	        Assert.IsTrue(Players[1].Disqualified);

        }
        
    }
}

/*

	
func test_When_a_player_declares_an_attack_during_their_opponents_turn():
	describe("When a player declares an attack during their opponent's turn")
	
	decklist += cards(code.Debug1000_1000, 3)
	start_game(decklist)
	var Debug1000_1000: Unit = player[1].hand.get_card_at_index(0)
	play.deploy(player[1].id, Debug1000_1000.id)
	# We need to end turn twice because the first is to make sure Demon is ready
	play.end_turn(player[1].id)
	play.end_turn(player[0].id)
	play.end_turn(player[1].id)
	var direct_attack: int = -1
	play.declare_attack(player[1].id, Debug1000_1000.id, direct_attack)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	
func test_When_a_player_declares_an_attack_with_an_unready_unit():
	describe("When a player declares an attack with an unready unit")
	
	decklist += cards(code.Debug1000_1000, 3)
	start_game(decklist)
	var Debug1000_1000: Unit = player[1].hand.get_card_at_index(0)
	play.deploy(player[1].id, Debug1000_1000.id)
	var direct_attack: int = -1
	play.declare_attack(player[1].id, Debug1000_1000.id, direct_attack)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	
func test_When_a_player_declares_an_attack_with_a_unit_that_is_not_on_their_field():
	describe("When a player declares an attack with a unit that is not on their field")
	
	decklist += cards(code.Debug1000_1000, 3)
	start_game(decklist)
	var Debug1000_1000: Unit = player[1].hand.get_card_at_index(0)
	var direct_attack: int = -1
	play.declare_attack(player[1].id, Debug1000_1000.id, direct_attack)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	
func test_When_a_player_declares_an_attack_against_a_unit_that_is_not_on_their_opponents_field():
	describe("When a player declares an attack against a unit that is not on their opponent's field")
	
	decklist += cards(code.Debug1000_1000, 3)
	start_game(decklist)
	var Debug1000_1000: Unit = player[1].hand.get_card_at_index(0)
	play.deploy(player[1].id, Debug1000_1000.id)
	play.end_turn(player[1].id)
	play.end_turn(player[0].id)
	var EnemyDebug1000_1000: Unit = player[0].hand.get_card_at_index(0)
	play.declare_attack(player[1].id, Debug1000_1000.id, EnemyDebug1000_1000.id)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	
func test_When_a_player_activates_an_unready_support():
	describe("When a player activates an unready support")
	
	decklist += cards(code.DebugDraw2Cards, 3)
	start_game(decklist)
	var Draw2Cards: Support = player[1].hand.get_card_at_index(0)
	play.set_facedown(player[1].id, Draw2Cards.id)
	play.activate_facedown(player[1].id, Draw2Cards.id)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	
func test_When_a_player_activates_a_support_while_they_are_not_the_active_player():
	describe("When a player activates a support while they are not the active player")
	
	decklist += cards(code.DebugDraw2Cards, 3)
	start_game(decklist)
	var Draw2Cards: Support = player[1].hand.get_card_at_index(0)
	play.set_facedown(player[1].id, Draw2Cards.id)
	play.end_turn(player[1].id)
	# This is test is somewhat flaky because we haven't implemented all priority-related things
	
	play.activate_facedown(player[1].id, Draw2Cards.id)
	
	asserts.is_true(player[1].disqualified, "Then they are disqualified")
	*/