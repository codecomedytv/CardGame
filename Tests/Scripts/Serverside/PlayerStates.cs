using System.Collections.Generic;
using CardGame.Server;
using Godot;

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
        public void IdlePlayerBecomesPassiveWhenEndingTheirTurn()
        {
            StartGame(Decklist);
            var oldState = Players[1].State;
            Play.EndTurn(Players[1].Id);
            var newState = Players[1].State;

            Assert.IsTrue(true);
            Watch(Play, nameof(BaseMessenger.PlayerSeated));
            Assert.SignalWasEmitted(Play, nameof(BaseMessenger.PlayerSeated));
            Assert.IsEqual(oldState, Player.States.Idle);
            Assert.IsEqual(newState, Player.States.Passive);
        }
        
    }
}