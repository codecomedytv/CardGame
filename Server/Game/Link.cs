using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using CardGame.Server.Game.Skill;
using Godot;

namespace CardGame.Server.Game {

	public class Link : Reference
	{
		private readonly Stack<IResolvable> Chain = new Stack<IResolvable>();
		public Players Players;
		private Player TurnPlayer => Players.TurnPlayer;

		public void OnGameEventRecorded(Command command)
		{
			ApplyConstants(command);
			if (command.Identity == GameEvents.SetFaceDown || command.Identity == GameEvents.EndTurn)
			{
				return;
			}
			ApplyTriggered(command);
			SetupManual(command);
		}

		public void AddResolvable(IResolvable action) => Chain.Push(action);
		
		private void ApplyConstants(Command command)
		{
			foreach (var card in TurnPlayer.Field)
			{
				if (card.Skill.Type != Skill.Skill.Types.Constant)
				{
					continue;
				}
				card.Skill.Resolve();
			}
			
			foreach (var card in TurnPlayer.Opponent.Field)
			{
				if (card.Skill.Type != Skill.Skill.Types.Constant)
				{
					continue;
				}
				card.Skill.Resolve();
			}
			
		}
		
		private void ApplyTriggered(Command command)
		{
			foreach (var card in TurnPlayer.Field)
			{
				if (card.Skill.Type != Skill.Skill.Types.Auto)
				{
					continue;
				}
				card.Skill.SetUp(command);
				if (!card.Skill.CanBeUsed)
				{
					continue;
				}
				card.Skill.Activate();
				Chain.Push(card.Skill);
			}
			
			foreach (var card in TurnPlayer.Opponent.Field)
			{
				if (card.Skill.Type != Skill.Skill.Types.Auto)
				{
					continue;
				}
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
			foreach (var card in TurnPlayer.Support)
			{
				if(card.Skill.Type != Skill.Skill.Types.Manual) {continue;} 
				card.Skill.SetUp(command);
			}

			foreach (var card in TurnPlayer.Opponent.Support)
			{
				if(card.Skill.Type != Skill.Skill.Types.Manual) {continue;}
				card.Skill.SetUp(command);
			}
		}
		
		public void Activate(Skill.Skill skill, Card target)
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
				if(resolvable is Skill.Skill skill && skill.Targeting)
				{
					var result = await ToSignal(skill.Controller, nameof(Player.TargetSelected));
					skill.Target = result[0] as Card;
				}
				resolvable.Resolve();
			}
			
			ApplyConstants(new NullCommand());
		}
		
		
	}

}
