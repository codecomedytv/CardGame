using System.Collections.Generic;
using CardGame.Server;
using Godot;

namespace CardGame.Tests
{
    public class MockMessenger: Node, IMessenger
    {
        [Signal]
        public delegate void Deployed();

        [Signal]
        public delegate void Attacked();

        [Signal]
        public delegate void Activated();

        [Signal]
        public delegate void FaceDownSet();

        [Signal]
        public delegate void Targeted();

        [Signal]
        public delegate void EndedTurn();

        [Signal]
        public delegate void PassedPriority();

        [Signal]
        public delegate void PlayerSeated();

        public MockMessenger()
        {
            Name = "Messenger";
        }
        
        public void OnPlayExecuted(Player player, object gameEvent)
        {
            // Warning: Dummy Method
        }

        public void Update(List<Player> players)
        {
            // Warning: Dummy Method 
        }

        public void DisqualifyPlayer(int player, int reason)
        {
            // Warning: Dummy Method
        }

        public void Deploy(int player, int card)
        {
            EmitSignal(nameof(Deployed), player, card);
        }

        public void Attack(int player, int attacker, int defender)
        {
            EmitSignal(nameof(Attacked), player, attacker, defender);
        }

        public void Activate(int player, int card, int skillIndex, List<int> targets)
        {
            EmitSignal(nameof(Activated), player, card, skillIndex, targets);
        }

        public void SetFaceDown(int player, int card)
        {
            EmitSignal(nameof(FaceDownSet), player, card);
        }

        public void Target(int player, int target)
        {
            EmitSignal(nameof(Targeted), target);
        }

        public void PassPlay(int player)
        {
            EmitSignal(nameof(PassedPriority), player);
        }

        public void EndTurn(int player)
        {
            EmitSignal(nameof(EndedTurn), player);
        }

        public void SetReady(int player)
        {
            EmitSignal(nameof(PlayerSeated), player);
        }
    }
}