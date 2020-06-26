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
			constants.AddRange(TurnPlayer.Field.Select(c => c.Skill).Where(s => s is Constant).Cast<Constant>());
			constants.AddRange(TurnPlayer.Graveyard.Select(c => c.Skill).Where(s => s is Constant).Cast<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Field.Select(c => c.Skill).Where(s => s is Constant).Cast<Constant>());
			constants.AddRange(TurnPlayer.Opponent.Graveyard.Select(c => c.Skill).Where(s => s is Constant).Cast<Constant>());
			foreach (var constant in constants) { constant.Apply(); }

		}
		
		private void ApplyTriggered(Command command)
		{
			foreach (var card in TurnPlayer.Field.Where(c => c.Skill.Type == Skill.Types.Auto))
			{
				card.Skill.SetUp(command);
				if (!card.Skill.CanBeUsed)
				{
					continue;
				}
				card.Skill.Activate();
				Chain.Push(card.Skill);
			}
			
			foreach (var card in TurnPlayer.Opponent.Field.Where(c => c.Skill.Type == Skill.Types.Auto))
			{
				card.Skill.SetUp(command);
				if (!card.Skill.CanBeUsed)
				{
					continue;
				}
				card.Skill.Activate();
				Chain.Push(card.Skill);
			}
		}
		
		private void SetupManual(Command command)
		{
			foreach (var card in TurnPlayer.Support.Where(c => c.Skill.Type == Skill.Types.Manual))
			{
				card.Skill.SetUp(command);
			}

			foreach (var card in TurnPlayer.Opponent.Support.Where(c => c.Skill.Type == Skill.Types.Manual))
			{
				card.Skill.SetUp(command);
			}
		}
		
		public void Activate(Skill skill, Card target)
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
				if(resolvable is Skill skill && skill.Targeting)
				{
					var result = await ToSignal(skill.Controller, nameof(Player.TargetSelected));
					skill.Target = result[0] as Card;
				}
				resolvable.Resolve();
			}
			
			ApplyConstants();
		}
		
		
	}

}
