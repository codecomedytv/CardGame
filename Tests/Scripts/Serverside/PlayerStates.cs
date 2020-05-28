using System.Collections.Generic;
using CardGame.Server;
using Godot;
using Godot.Collections;
using static CardGame.Tests.MockMessenger;

namespace CardGame.Tests.Scripts.Serverside
{
    public class PlayerStates: GameFixture
    {
        List<SetCodes> Decklist = new List<SetCodes>();

        public override void Start()
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

        [Test]
        public void NonTurn_Player_Becomes_Passive_When_Both_Players_Pass()
        {
            StartGame(Decklist);
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            var oldState = Players[0].State;
            Play.PassPlay(Players[0].Id);
            Play.PassPlay(Players[1].Id);
            var newState = Players[0].State;
            
            Assert.IsEqual(oldState, Player.States.Active, "Player was Active");
            Assert.IsEqual(newState, Player.States.Passive, "Player is Passive");
        }

        [Test]
        public void NonTurn_Player_Becomes_Idle_When_TurnPlayer_Ends_Their_Turn()
        {
            StartGame(Decklist);
            var oldState = Players[0].State;
            Play.EndTurn(Players[1].Id);
            var newState = Players[0].State;
            
            Assert.IsEqual(oldState, Player.States.Passive, "Player was Passive");
            Assert.IsEqual(newState, Player.States.Idle, "Player is Idle");
        }

        [Test]
        public void Passive_Opponent_Becomes_Active_When_IdlePlayer_Takes_Action()
        {
            StartGame(Decklist);
            var oldState = Players[0].State;
            var unit = Players[1].Hand[0].Id;
            Play.Deploy(Players[1].Id, unit);
            var newState = Players[0].State;
            
            Assert.IsEqual(oldState, Player.States.Passive, "Player was Passive");
            Assert.IsEqual(newState, Player.States.Active, "Player is Active");
        }

        [Test]
        public void ActivePlayer_Becomes_Active_When_They_Take_An_Action()
        {
            StartGame(Decklist);
            var support = Players[1].Hand[3].Id;
            Play.SetFaceDown(Players[1].Id, support);
            Play.EndTurn(Players[1].Id);
            var unit = Players[0].Hand[0].Id;
            Play.Deploy(Players[0].Id, unit);
            var oldState = Players[1].State;
            Play.Activate(Players[1].Id, support, 0, new Array<int>());
            var newState = Players[1].State;
            
            Assert.IsEqual(oldState, Player.States.Active, "Player was Active");
            Assert.IsEqual(newState, Player.States.Acting, "Player is Acting");

        }
        
   
        
    }
}