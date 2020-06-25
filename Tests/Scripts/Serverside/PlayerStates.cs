using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.States;
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
            var oldState = Player.State;
            Play.EndTurn(Player.Id);
            var newState = Player.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Idle), "Player Was Idle");
            Assert.IsTrue(newState.GetType() == typeof(Passive), "Player Is Passive");
        }

        [Test]
        public void Idle_Player_Becomes_Acting_When_Taking_Action()
        {
            StartGame(Decklist);
            var oldState = Player.State;
            var unit = Player.Hand[0].Id;
            Play.Deploy(Player.Id, unit);
            var newState = Player.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Idle), "Player Was Idle");
            Assert.IsTrue(newState.GetType() == typeof(Acting), "Player is Acting");
        }

        [Test]
        public void Active_Player_Becomes_Passing_When_Passing()
        {
            StartGame(Decklist);
            var unit = Player.Hand[0].Id;
            Play.Deploy(Player.Id, unit);
            var oldState = Opponent.State;
            Play.PassPlay(Opponent.Id);
            var newState = Opponent.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Active), "Player was Active");
            Assert.IsTrue(newState.GetType() == typeof(Passing), "Player is Passing");
        }

        [Test]
        public void Turn_Player_Becomes_Idle_When_Both_Players_Pass()
        {
            StartGame(Decklist);
            var unit = Player.Hand[0].Id;
            Play.Deploy(Player.Id, unit);
            var oldState = Player.State;
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            var newState = Player.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Acting), "Player was Acting");
            Assert.IsTrue(newState.GetType() == typeof(Idle), "Player is Idle");
        }

        [Test]
        public void NonTurn_Player_Becomes_Passive_When_Both_Players_Pass()
        {
            StartGame(Decklist);
            var unit = Player.Hand[0].Id;
            Play.Deploy(Player.Id, unit);
            var oldState = Opponent.State;
            Play.PassPlay(Opponent.Id);
            Play.PassPlay(Player.Id);
            var newState = Opponent.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Active), "Player was Active");
            Assert.IsTrue(newState.GetType() == typeof(Passive), "Player is Passive");
        }

        [Test]
        public void NonTurn_Player_Becomes_Idle_When_TurnPlayer_Ends_Their_Turn()
        {
            StartGame(Decklist);
            var oldState = Opponent.State;
            Play.EndTurn(Player.Id);
            var newState = Opponent.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Passive), "Player was Passive");
            Assert.IsTrue(newState.GetType() == typeof(Idle), "Player is Idle");
        }

        [Test]
        public void Passive_Opponent_Becomes_Active_When_IdlePlayer_Takes_Action()
        {
            StartGame(Decklist);
            var oldState = Opponent.State;
            var unit = Player.Hand[0].Id;
            Play.Deploy(Player.Id, unit);
            var newState = Opponent.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Passive), "Player was Passive");
            Assert.IsTrue(newState.GetType() == typeof(Active), "Player is Active");
        }

        [Test]
        public void ActivePlayer_Becomes_Active_When_They_Take_An_Action()
        {
            StartGame(Decklist);
            var support = Player.Hand[3].Id;
            Play.SetFaceDown(Player.Id, support);
            Play.EndTurn(Player.Id);
            var unit = Opponent.Hand[0].Id;
            Play.Deploy(Opponent.Id, unit);
            var oldState = Player.State;
            Play.Activate(Player.Id, support);
            var newState = Player.State;
            
            Assert.IsTrue(oldState.GetType() == typeof(Active), "Player was Active");
            Assert.IsTrue(newState.GetType() == typeof(Acting), "Player is Acting");

        }
        
   
        
    }
}