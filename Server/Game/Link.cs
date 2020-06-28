using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Events;
using CardGame.Server.Game.Skills;
using Godot;
using WAT;

namespace CardGame.Server.Game {

	public class Link : Reference
	{
		private readonly Stack<IResolvable> Chain = new Stack<IResolvable>();
		public Players Players;
		private Player TurnPlayer => Players.TurnPlayer;

		public void OnGameEventRecorded(Event gameEvent)
		{
			switch (gameEvent)
			{
				case Activate activation:
					Chain.Push(activation.Skill);
					break;
				case DeclareAttack attackUnit:
					Chain.Push(attackUnit.Attack);
					break;
				case DeclareDirectAttack directAttack:
					Chain.Push(directAttack.DirectAttack);
					break;
			}

			ApplyConstants();
			if (gameEvent is SetFaceDown || gameEvent is EndTurn)
			{
				return;
			}
			ApplyTriggered(gameEvent);
			SetupManual(gameEvent);
		}
		
		private void ApplyConstants()
		{
			var constants = new List<Constant>();
			constants.AddRange(TurnPlayer.Field.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Graveyard.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Field.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Graveyard.Select(c => c.Skill).OfType<Constant>());
			foreach (var constant in constants) { constant.Apply(); }

		}
		
		private void ApplyTriggered(Event gameEvent)
		{
			var automatic = new List<Automatic>();
			automatic.AddRange(TurnPlayer.Field.Select(c => c.Skill).OfType<Automatic>());
			automatic.AddRange(TurnPlayer.Opponent.Field.Select(c => c.Skill).OfType<Automatic>());
			foreach (var skill in automatic)
			{
				skill.Trigger(gameEvent);
				if (skill.Triggered)
				{
					Chain.Push(skill);
				}
			}
		}
		
		private void SetupManual(Event gameEvent)
		{
			var manual = new List<Manual>();
			manual.AddRange(TurnPlayer.Support.Select(c => c.Skill).OfType<Manual>());
			manual.AddRange(TurnPlayer.Opponent.Support.Select(c => c.Skill).OfType<Manual>());
			foreach (var skill in manual)
			{
				skill.SetUp(gameEvent);
			}
		}

		public void Resolve()
		{
			while (Chain.Count != 0)
			{
				var resolvable = Chain.Pop();
				resolvable.Resolve();
				if (resolvable is Skill skill && skill.Targeting)
				{
					// I don't think we're continuing the resolve here in async methods
					return;
				}
			}
			
		}
		
		
	}

}
