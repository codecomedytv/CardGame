using System.Collections.Generic;
using CardGame.Server;
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