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
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug500_500);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.Debug1500_1000);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.DebugDraw2Cards);
            DeckList.Add(SetCodes.Debug1000_1000);
            DeckList.Add(SetCodes.Debug1000_1000);
        }

        [Test]
        public void When_Its_Attack_Is_Greater_Than_The_Defending_Units_Defense()
        {
            StartGame(DeckList);

            var debug15001000 = (Unit)Players[1].Hand[Players[1].Hand.Count - 1];
            Play.Deploy(Players[1].Id, debug15001000.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            Play.EndTurn(Players[1].Id);
            var debug10001000 = (Unit) Players[0].Hand[1];
            Play.Deploy(Players[0].Id, debug10001000.Id);
            Play.PassPlay(Players[1].Id);
            Play.PassPlay(Players[0].Id);
            Play.EndTurn(Players[0].Id);
            Play.Attack(Players[1].Id, debug15001000.Id, debug10001000.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            var difference = debug15001000.Attack - debug10001000.Defense;
            var expectedLife = Players[1].Health - difference;
	
            Assert.Has(debug10001000, Players[0].Graveyard, "Then the defending unit is destroyed");
            Assert.IsEqual(expectedLife, Players[0].Health,
             "Then the defending Players loses life equal to the difference");

        }

        [Test]
        public void When_It_Attacks_Directly()
        {
            StartGame(DeckList);

            var debug15001000 = (Unit)Players[1].Hand[Players[1].Hand.Count - 1];
            Play.Deploy(Players[1].Id, debug15001000.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            Play.EndTurn(Players[1].Id);
            Play.EndTurn(Players[0].Id);
            const int directAttack = -1;
            var expectedLife = Players[0].Health - debug15001000.Attack;
            Play.Attack(Players[1].Id, debug15001000.Id, directAttack);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            
            Assert.IsEqual(expectedLife, Players[0].Health, "Then the defending Player loses life equal to its attack");

        }

        [Test]
        public void When_It_Is_CounterAttack_By_A_Unit_With_Attack_Greater_Than_Its_Defense()
        {
            StartGame(DeckList);
            var debug10001000 = (Unit) Players[1].Hand[1];
            Play.Deploy(Players[1].Id, debug10001000.Id);
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            Assert.IsTrue(Players[1].Field.Contains(debug10001000), "Weaking Deployed");
            Play.EndTurn(Players[1].Id);
			var debug15001000 = (Unit)Players[0].Hand[Players[0].Hand.Count - 1];
			Play.Deploy(Players[0].Id, debug15001000.Id);
			Play.PassPlay(Players[1].Id);
			Play.PassPlay(Players[0].Id);
			Assert.IsTrue(Players[0].Field.Contains(debug15001000), "Defender Deployed");
			Play.EndTurn(Players[0].Id);
			var difference = debug15001000.Attack - debug10001000.Defense;
			var expectedLife = Players[1].Health - difference;
			Play.Attack(Players[1].Id, debug10001000.Id, debug15001000.Id);
			Play.PassPlay(Players[0].Id);
			Play.PassPlay(Players[1].Id);
				
	        Assert.Has(debug10001000, Players[1].Graveyard, "Then it is destroyed");
	        Assert.IsEqual(expectedLife, Players[1].Health,
					"Then the attacking Player loses life equal to the difference");
        }
    }
    
    
}
