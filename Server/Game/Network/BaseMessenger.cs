using System;
using System.Collections.Generic;
using CardGame.Server.Game.Events;
using Godot;

namespace CardGame.Server.Game.Network {
	
	public class BaseMessenger : Node
	{
		[Signal]
		public delegate void Deployed();

		[Signal]
		public delegate void Attacked();

		[Signal]
		public delegate void AttackedDirectly();

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
		public delegate void PlayerSeated(int player);

		public BaseMessenger()
		{
			Name = "Messenger";
		}

		public virtual void OnPlayExecuted(Player player, GameEvent @event)
		{
			throw new NotImplementedException();
		}

		public virtual void Update(IEnumerable<Player> enumerable)
		{
			throw new NotImplementedException();
		}

		public virtual void DisqualifyPlayer(int player, int reason)
		{
			throw new NotImplementedException();
		}

		public virtual void Deploy(int player, int card)
		{
			throw new NotImplementedException();
		}

		public virtual void Attack(int player, int attacker, int defender)
		{
			throw new NotImplementedException();
		}

		public virtual void DirectAttack(int player, int attacker)
		{
			throw new NotImplementedException();
		}

		public virtual void Activate(int player, int card, int targetId = 0)
		{
			throw new NotImplementedException();
		}

		public virtual void SetFaceDown(int player, int card)
		{
			throw new NotImplementedException();
		}

		public virtual void Target(int player, int target)
		{
			throw new NotImplementedException();
		}

		public virtual void PassPlay(int player)
		{
			throw new NotImplementedException();
		}

		public virtual void EndTurn(int player)
		{
			throw new NotImplementedException();
		}

		public virtual void SetReady(int player)
		{
			throw new NotImplementedException();
		}

		public virtual void OnPlayExecuted(GameEvent gameEvent)
		{
			// TODO: Implement This
		}
		
		
	}
	
}
