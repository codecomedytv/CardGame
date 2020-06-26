using System.Collections.Generic;
using System.Linq;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skills;
using Godot;

namespace CardGame.Server.Game {

	public class Link : Reference
	{
		private readonly Stack<IResolvable> Chain = new Stack<IResolvable>();
		public Players Players;
		private Player TurnPlayer => Players.TurnPlayer;

		public void OnGameEventRecorded(Command command)
		{
			if (command is Activate activation)
			{
				Activate(activation.Skill, activation.Target);
			}
			ApplyConstants();
			if (command.Identity == GameEvents.SetFaceDown || command.Identity == GameEvents.EndTurn)
			{
				return;
			}
			ApplyTriggered(command);
			SetupManual(command);
		}

		public void AddResolvable(IResolvable action) => Chain.Push(action);
		
		private void ApplyConstants()
		{
			var constants = new List<Constant>();
			constants.AddRange(TurnPlayer.Field.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Graveyard.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Field.Select(c => c.Skill).OfType<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Graveyard.Select(c => c.Skill).OfType<Constant>());
			foreach (var constant in constants) { constant.Apply(); }

		}
		
		private void ApplyTriggered(Command command)
		{
			var automatic = new List<Automatic>();
			automatic.AddRange(TurnPlayer.Field.Select(c => c.Skill).OfType<Automatic>());
			automatic.AddRange(TurnPlayer.Opponent.Field.Select(c => c.Skill).OfType<Automatic>());
			foreach (var skill in automatic)
			{
				skill.Trigger(command);
				if (skill.Triggered)
				{
					Chain.Push(skill);
				}
			}
		}
		
		private void SetupManual(Command command)
		{
			var manual = new List<Manual>();
			manual.AddRange(TurnPlayer.Support.Select(c => c.Skill).OfType<Manual>());
			manual.AddRange(TurnPlayer.Opponent.Support.Select(c => c.Skill).OfType<Manual>());
			foreach (var skill in manual)
			{
				skill.SetUp(command);
			}
		}
		
		private void Activate(Manual skill, Card target)
		{
			skill.Target = target;
			skill.Activate();
			Chain.Push(skill);
		}
		
		public async void Resolve()
		{
			while (Chain.Count != 0)
			{
				var resolvable = Chain.Pop();
				resolvable.Resolve();
				if(resolvable is Skill skill && skill.Targeting)
				{
					await ToSignal(skill, nameof(Skill.Resolved));
				}
			}
			
		}
		
		
	}

}
