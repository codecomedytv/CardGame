using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Game.Cards;

namespace CardGame.Tests.Scripts.Serverside
{
    public class BattleTest: GameFixture
    {
        
    /*
    There are various battle systems we could be using and may change
    When we change the system we will add a specific battle test script
    for each version
    */
    
        private List<SetCodes> DeckList = new List<SetCodes>();

        public override string Title()
        {
            return "Given An Attacking Unit";
        }

        public override void Start()
        {
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug500500);
            DeckList.Add(SetCodes.Debug15001000);
            DeckList.Add(SetCodes.Debug15001000);
            DeckList.Add(SetCodes.Debug15001000);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.Debug10001000);
            DeckList.Add(SetCodes.Debug10001000);
        }

        [Test]
        public void When_Its_Attack_Is_Greater_Than_The_Defending_Units_Defense()
        {
            StartGame(DeckList);

            var debug15001000 = (Unit)Player.Hand[Player.Hand.Count - 1];
            Play.Deploy(Player.Id, debug15001000.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.EndTurn(Player.Id);
            var debug10001000 = (Unit) Opponent.Hand[1];
            Play.Deploy(Opponent.Id, debug10001000.Id);
            Play.PassPlay(Player.Id);
            Play.PassPlay(Opponent.Id);
            Play.EndTurn(Opponent.Id);
            Play.Attack(Player.Id, debug15001000.Id, debug10001000.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            var difference = debug15001000.Attack - debug10001000.Defense;
            var expectedLife = Player.Health - difference;
	
            Assert.Has(debug10001000, Opponent.Graveyard, "Then the defending unit is destroyed");
            Assert.IsEqual(expectedLife, Opponent.Health,
             "Then the defending Players loses life equal to the difference");

        }

        [Test]
        public void When_It_Attacks_Directly()
        {
            StartGame(DeckList);

            var debug15001000 = (Unit)Player.Hand[Player.Hand.Count - 1];
            Play.Deploy(Player.Id, debug15001000.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Play.EndTurn(Player.Id);
            Play.EndTurn(Opponent.Id);
            const int directAttack = -1;
            var expectedLife = Opponent.Health - debug15001000.Attack;
            Play.DirectAttack(Player.Id, debug15001000.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            
            Assert.IsEqual(expectedLife, Opponent.Health, "Then the defending Player loses life equal to its attack");

        }

        [Test]
        public void When_It_Is_CounterAttack_By_A_Unit_With_Attack_Greater_Than_Its_Defense()
        {
            StartGame(DeckList);
            var debug10001000 = (Unit) Player.Hand[1];
            Play.Deploy(Player.Id, debug10001000.Id);
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            Assert.IsTrue(Player.Field.Contains(debug10001000), "Weaking Deployed");
            Play.EndTurn(Player.Id);
			var debug15001000 = (Unit)Opponent.Hand[Opponent.Hand.Count - 1];
			Play.Deploy(Opponent.Id, debug15001000.Id);
			Play.PassPlay(Player.Id);
			Play.PassPlay(Opponent.Id);
			Assert.IsTrue(Opponent.Field.Contains(debug15001000), "Defender Deployed");
			Play.EndTurn(Opponent.Id);
			var difference = debug15001000.Attack - debug10001000.Defense;
			var expectedLife = Player.Health - difference;
			Play.Attack(Player.Id, debug10001000.Id, debug15001000.Id);
			Play.PassPlay(Opponent.Id);
			Play.PassPlay(Player.Id);
				
	        Assert.Has(debug10001000, Player.Graveyard, "Then it is destroyed");
	        Assert.IsEqual(expectedLife, Player.Health,
					"Then the attacking Player loses life equal to the difference");
        }
    }
    
    
}
