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
	        var unit = Opponent.Hand[0].Id;
	        Play.Deploy(Opponent.Id, unit);
	        Assert.IsTrue(Opponent.IsDisqualified);
        }

        [Test]
        public void When_They_Try_To_Deploy_A_Unit_When_Their_Field_Is_Full()
        {
	        StartGame(DeckList);
	        
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);
	        var ids = Player.Hand.Select(card => card.Id).ToList();
			foreach (var card in ids)
			{
				Play.Deploy(Player.Id, card);
				Play.PassPlay(Opponent.Id);
				Play.PassPlay(Player.Id);
			}

			
			Play.Deploy(Player.Id, Player.Hand[0].Id);
			Assert.IsEqual(Player.Field.Count, 7);
			Assert.IsTrue(Player.IsDisqualified);
        }
        
        [Test]
        public void When_They_Try_To_Deploy_A_Unit_Not_In_Their_Hand()
        {
	        StartGame(DeckList);
	        var card = Player.Deck[0].Id;
	        Play.Deploy(Player.Id, card);
	        Assert.IsTrue(Player.IsDisqualified);
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
	        Play.EndTurn(Player.Id);
	        GD.Print(Player.IsTurnPlayer);
	        var ids = Opponent.Hand.Select(card => card.Id).ToList();
	        Assert.IsEqual(ids.Count, 8);
	        foreach (var id in ids)
	        {
		        Play.SetFaceDown(Opponent.Id, id);
	        }

	        Assert.IsTrue(Opponent.IsDisqualified);
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
	        Play.SetFaceDown(Player.Id, Player.Deck[0].Id);
	        
	        Assert.IsTrue(Player.IsDisqualified);
        }

        [Test]
        public void When_They_Set_Support_During_Their_Opponents_Turn()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Opponent.Hand[0].Id;
	        Play.SetFaceDown(Opponent.Id, support);

	        Assert.IsTrue(Opponent.IsDisqualified);
        }

	    [Test]
        public void When_They_End_Their_Turn_During_Their_Opponents_Turn()
        {
	        StartGame(DeckList);
	        Play.EndTurn(Opponent.Id);

	        Assert.IsTrue(Opponent.IsDisqualified);
        }

        [Test]
        public void When_A_Player_Declares_An_Attack_During_Their_Opponents_Turn()
        {
	        StartGame(DeckList);
	        var unit = Player.Hand[0].Id;
	        Play.Deploy(Player.Id, unit);
	        // Ending twice so the unit is ready
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);
	        Play.EndTurn(Player.Id);
	        Play.DirectAttack(Player.Id, unit);

	        Assert.IsTrue(Player.IsDisqualified);

        }

        [Test]
        public void When_They_Declare_An_Attack_With_An_Unready_Unit()
        {
	        StartGame(DeckList);
	        var unit = Player.Hand[0].Id;
	        Play.Deploy(Player.Id, unit);
	        Play.DirectAttack(Player.Id, unit);

	        Assert.IsTrue(Player.IsDisqualified);
        }

        public void When_They_Declare_An_Attack_With_A_Unit_Not_On_Their_Field()
        {
	        StartGame(DeckList);
	        var unit = Player.Hand[0].Id;
	        Play.DirectAttack(Player.Id, unit);

	        Assert.IsTrue(Player.IsDisqualified);
        }

        public void When_They_Declare_An_Attack_Against_A_Unit_Not_On_Their_Opponents_Field()
        {
	        StartGame(DeckList);
	        var unit = Player.Hand[0].Id;
	        Play.Deploy(Player.Id, unit);
	        Play.EndTurn(Player.Id);
	        Play.EndTurn(Opponent.Id);
	        var attackTarget = Opponent.Hand[0].Id;
	        Play.Attack(Player.Id, unit, attackTarget);

	        Assert.IsTrue(Player.IsDisqualified);
        }

        [Test]
        public void When_They_Activate_An_Unready_Support()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var s = Player.Hand[0];
	        var support = Player.Hand[0].Id;
	        Assert.IsFalse(s.IsReady, "Card is not ready in hand");
	        Play.SetFaceDown(Player.Id, support);
	        //Play.PassPlay(Opponent.Id);
	        //Play.PassPlay(Player.Id);
	        Assert.IsFalse(s.IsReady, "Card is not ready when set");
	        Play.Activate(Player.Id, support);
	        Assert.IsFalse(s.IsReady, "Card is not ready when activated");
	        Assert.IsTrue(Player.IsDisqualified, "Player is disqualified");
        }

        [Test]
        public void When_They_Activate_A_Support_While_InActive()
        {
	        DeckList.Add(SetCodes.DebugDraw2Cards);
	        StartGame(DeckList);
	        var support = Player.Hand[0].Id;
	        Play.SetFaceDown(Player.Id, support);
	        Play.EndTurn(Player.Id);
	        Play.Activate(Player.Id, support);

	        Assert.IsTrue(Player.IsDisqualified);
        }
    }
}
