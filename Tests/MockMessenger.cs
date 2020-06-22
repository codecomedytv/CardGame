using System.Collections.Generic;
using CardGame.Server;
using CardGame.Server.Room;
using CardGame.Server.Room.Network.Messenger;
using Godot;
using static Godot.Collections.Array;

namespace CardGame.Tests
{
    public class MockMessenger: BaseMessenger
    {
        
        public MockMessenger()
        {
            Name = "Messenger";
        }
        
        public override void OnPlayExecuted(Player player, GameEvent @event)
        {
            // Warning: Dummy Method
        }

        public override void Update(IEnumerable<Player> enumerable)
        {
            // Warning: Dummy Method 
        }

        public override void DisqualifyPlayer(int player, int reason)
        {
            // Warning: Dummy Method
        }

        public override void Deploy(int player, int card)
        {
            EmitSignal(nameof(Deployed), player, card);
        }

        public override void Attack(int player, int attacker, int defender)
        {
            EmitSignal(nameof(Attacked), player, attacker, defender);
        }

        public override void DirectAttack(int player, int attacker)
        {
            EmitSignal(nameof(AttackedDirectly), player, attacker);
        }

        public override void Activate(int player, int card, int targetId = 0)
        {
            EmitSignal(nameof(Activated), player, card, targetId);
        }

        public override void SetFaceDown(int player, int card)
        {
            EmitSignal(nameof(FaceDownSet), player, card);
        }

        public override void Target(int player, int target)
        {
            EmitSignal(nameof(Targeted), player, target);
        }

        public override void PassPlay(int player)
        {
            EmitSignal(nameof(PassedPriority), player);
        }

        public override void EndTurn(int player)
        {
            EmitSignal(nameof(EndedTurn), player);
        }

        public override void SetReady(int player)
        {
            EmitSignal("PlayerSeated", player);
        }
    }
}