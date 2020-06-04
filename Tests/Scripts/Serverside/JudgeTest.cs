using System.Collections.Generic;
using System.Linq;
using CardGame.Server;
using Godot;
using Godot.Collections;

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

	        Assert.IsTrue(Players[0].IsDisqualified);
        }

        [Test]
        public void When_They_Try_To_Deploy_A_Unit_When_Their_Field_Is_Full()
        {
	        StartGame(DeckList);
			var ids = Players[1].Hand.Select(card => card.Id).ToList();
			foreach (var card in ids)
			{
				Play.Deploy(Players[1].Id, card);
				Play.PassPlay(Players[0].Id);
				Play.PassPlay(Players[1].Id);
			}

			Play.EndTurn(Players[1].Id);
			Play.EndTurn(Players[0].Id);
			Play.Deploy(Players[1].Id, Players[1].Hand[0].Id);

			Assert.IsEqual(Players[1].Field.Count, 7);
			Assert.IsTrue(Players[1].IsDisqualified);
        }
        
        [Test]
        public void When_They_Try_To_Deploy_A_Unit_Not_In_Their_Hand()
        {
	        StartGame(DeckList);
	        var card = Players[1].Deck[0].Id;
	        Play.Deploy(Players[1].Id, card);

	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        [Test]
        public void When_They_Try_To_Set_A_Card_To_A_Full_Field()
        {
	        DeckList.Clear();
	        for (var i = 0; i < 10; i++)
	        {
		        DeckList.Add(SetCodes.DebugDraw2Cards);
	        }
	        StartGame(DeckList);
	        Play.EndTurn(Players[1].Id);
	        var ids = Players[0].Hand.Select(card => card.Id).ToList();
	        Assert.IsEqual(ids.Count, 8);
	        foreach (var id in ids)
	        {
		        Play.SetFaceDown(Players[0].Id, id);
	        }

	        Assert.IsTrue(Players[0].IsDisqualified);
        }

        [Test]
        public void When_They_Try_To_Set_A_Card_Not_In_Their_Hand()
        {
	        DeckList.Clear();
	        for (var i = 0; i < 10; i++)
	        {
		        DeckList.Add(SetCodes.DebugDraw2Cards);
	        }
	        StartGame(DeckList);
	        Play.SetFaceDown(Players[1].Id, Players[1].Deck[0].Id);
	        
	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        [Test]
        public void When_They_Set_Support_During_Their_Opponents_Turn()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Players[0].Hand[0].Id;
	        Play.SetFaceDown(Players[0].Id, support);

	        Assert.IsTrue(Players[0].IsDisqualified);
        }

	    [Test]
        public void When_They_End_Their_Turn_During_Their_Opponents_Turn()
        {
	        StartGame(DeckList);
	        GD.Print(Players[0].IsTurnPlayer);
	        Play.EndTurn(Players[0].Id);

	        Assert.IsTrue(Players[0].IsDisqualified);
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
	        const int directAttack = -1;
	        Play.Attack(Players[1].Id, unit, directAttack);

	        Assert.IsTrue(Players[1].IsDisqualified);

        }

        [Test]
        public void When_They_Declare_An_Attack_With_An_Unready_Unit()
        {
	        StartGame(DeckList);
	        var unit = Players[1].Hand[0].Id;
	        Play.Deploy(Players[1].Id, unit);
	        const int directAttack = -1;
	        Play.Attack(Players[1].Id, unit, directAttack);

	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        public void When_They_Declare_An_Attack_With_A_Unit_Not_On_Their_Field()
        {
	        StartGame(DeckList);
	        var unit = Players[1].Hand[0].Id;
	        const int directAttack = -1;
	        Play.Attack(Players[1].Id, unit, directAttack);

	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        public void When_They_Declare_An_Attack_Against_A_Unit_Not_On_Their_Opponents_Field()
        {
	        StartGame(DeckList);
	        var unit = Players[1].Hand[0].Id;
	        Play.Deploy(Players[1].Id, unit);
	        Play.EndTurn(Players[1].Id);
	        Play.EndTurn(Players[0].Id);
	        var attackTarget = Players[0].Hand[0].Id;
	        Play.Attack(Players[1].Id, unit, attackTarget);

	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        [Test]
        public void When_They_Activate_An_Unready_Support()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Players[1].Hand[0].Id;
	        Play.SetFaceDown(Players[1].Id, support);
	        Play.Activate(Players[1].Id, support, 0, new Array<int>());

	        Assert.IsTrue(Players[1].IsDisqualified);
        }

        [Test]
        public void When_They_Activate_A_Support_While_InActive()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Players[1].Hand[0].Id;
	        Play.SetFaceDown(Players[1].Id, support);
	        Play.EndTurn(Players[1].Id);
	        Play.Activate(Players[1].Id, support, 0, new Array<int>());

	        Assert.IsTrue(Players[1].IsDisqualified);
        }
    }
}