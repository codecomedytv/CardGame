using System.Collections.Generic;
using CardGame.Server;
using Godot;
using static CardGame.Tests.MockMessenger;

namespace CardGame.Tests.Scripts.Serverside
{
    public class PlayerStates: GameFixture
    {
        List<SetCodes> Decklist = new List<SetCodes>();

        public override void Pre()
        {
            Decklist.Add(SetCodes.Debug1000_1000);
            Decklist.Add(SetCodes.Debug1000_1000);
            Decklist.Add(SetCodes.Debug1000_1000);
            Decklist.Add(SetCodes.DebugDraw2Cards);
            Decklist.Add(SetCodes.DebugDraw2Cards);
            Decklist.Add(SetCodes.DebugDraw2Cards);
            Decklist.Add(SetCodes.Debug500_500);
            Decklist.Add(SetCodes.Debug500_500);
            Decklist.Add(SetCodes.Debug500_500);
        }

        [Test]
        public void Idle_Player_Becomes_Passive_When_Ending_Their_Turn()
        {
            StartGame(Decklist);
            var oldState = Players[1].State;
            Play.EndTurn(Players[1].Id);
            var newState = Players[1].State;
            
            Assert.IsEqual(oldState, Player.States.Idle, "Player Was Idle");
            Assert.IsEqual(newState, Player.States.Passive, "Player Is Passive");
        }

        [Test]
        public void Idle_Player_Becomes_Acting_When_Taking_Action()
        {
            StartGame(Decklist);
            var oldState = Players[1].State;
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            var newState = Players[1].State;
            
            Assert.IsEqual(oldState, Player.States.Idle, "Player Was Idle");
            Assert.IsEqual(newState, Player.States.Acting, "Player is Acting");
        }
        

        
    }
}