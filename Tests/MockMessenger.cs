using System.Collections.Generic;
using CardGame.Server;
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

        public override void Update(Dictionary<int, Player>.ValueCollection players)
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

        public override void Activate(int player, int card, Godot.Collections.Array<int> targets)
        {
            EmitSignal(nameof(Activated), player, card, targets);
        }

        public override void SetFaceDown(int player, int card)
        {
            EmitSignal(nameof(FaceDownSet), player, card);
        }

        public override void Target(int player, int target)
        {
            EmitSignal(nameof(Targeted), target);
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