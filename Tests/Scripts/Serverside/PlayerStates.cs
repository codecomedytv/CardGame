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

        [Test]
        public void Active_Player_Becomes_Passing_When_Passing()
        {
            StartGame(Decklist);
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            var oldState = Players[0].State;
            Play.PassPlay(Players[0].Id);
            var newState = Players[0].State;
            
            Assert.IsEqual(oldState, Player.States.Active, "Player was Active");
            Assert.IsEqual(newState, Player.States.Passing, "Player is Passing");
        }

        [Test]
        public void Turn_Player_Becomes_Idle_When_Both_Players_Pass()
        {
            StartGame(Decklist);
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            var oldState = Players[1].State;
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            var newState = Players[1].State;
            
            Assert.IsEqual(oldState, Player.States.Acting, "Player was Acting");
            Assert.IsEqual(newState, Player.States.Idle, "Player is Idle");
        }
        
        
        
    }
}