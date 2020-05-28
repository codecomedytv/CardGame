using Godot;
using System;
using System.Collections.Generic;
using Object = Godot.Object;

namespace CardGame.Server {

	public class Link : Reference
	{
		public Gamestate Game;
		private List<IResolvable> Chain = new List<IResolvable>();
		private List<Skill> Constants = new List<Skill>();
		private List<Skill> Manual = new List<Skill>();
		private List<Skill> Auto = new List<Skill>();
		
		[Signal]
		public delegate void Updated();
		
		public void SetUp(Gamestate game)
		{
			if(Game == null) { Game = game; }
		}
		
		public void AddResolvable(IResolvable action)
		{
			// In the original source, we checked if this had the action resolvable?
			// We may be able to use an IResolvable interface here instead
			Chain.Add(action);
		}
		
		public void ApplyConstants(string gameEvent = "")
		{
			foreach (var skill in Constants)
			{
				skill.Resolve(gameEvent);
			}
		}
		
		public void ApplyTriggered(string gameEvent)
		{
			throw new NotImplementedException();
		}
		
		public void  SetupManual(string gameEvent)
		{
			foreach (var skill in Manual)
			{
				skill.SetUp(gameEvent);
			}
		}
		
		public void Broadcast(string gameEvent, List<Godot.Object> arguments)
		{
			ApplyConstants();
			ApplyTriggered(gameEvent);
			SetupManual(gameEvent);
		}
		
		public void Register(Card card)
		{
			foreach (var skill in card.Skills)
			{
				switch (skill.Type)
				{
					case Skill.Types.Constant:
						Constants.Add(skill);
						break;
					
					case Skill.Types.Auto:
						Auto.Add(skill);
						break;
					
					case Skill.Types.Manual:
						Manual.Add(skill);
						break;
					default:
						return;
				}
			}
		}
		
		public void Unregister(Skill skill)
		{
			switch (skill.Type)
			{
				case Skill.Types.Constant:
					Constants.Remove(skill);
					return;
				case Skill.Types.Auto:
					Auto.Remove(skill);
					return;
				case Skill.Types.Manual:
					Manual.Remove(skill);
					return;
				default:
					return;
			}
		}
		
		public void OnPriorityPassed(int playerId)
		{

			var player = Game.Player(playerId);
			player.State = Player.States.Passing;
			if(player.Opponent.State == Player.States.Passing)
			{
				Resolve();
			}
			else
			{
				player.Opponent.State = Player.States.Active;
			}
			Update();
			

		}
		
		public async void Activate(Player player, Card card, int skillIndex = 0, List<int> targets = null)
		{
			var activatedSkill = card.Skills[skillIndex];
			if (!activatedSkill.CanBeUsed)
			{
				return;
			}

			if (targets != null)
			{
				Game.Target = Game.GetCard(targets[0]);
			}
			activatedSkill.Activate();
			if (Game.Paused)
			{
				await ToSignal(Game, nameof(Gamestate.UnPaused));
			}

			List<Card> cards = new List<Card>();
			foreach (var skill in Manual)
			{
				if (skill == activatedSkill)
				{
					if (Game.Target != null)
					{
						cards.Add(Game.Target);
					}
					player.Activate(skill.Card, skillIndex, cards);
					Chain.Add(skill);
					return;
				}
			}
		}

		public async void Automatic(Player player, Card card, int skillIndex = 0)
		{
			player.State = Player.States.Passing;
			var autoSkill = card.Skills[skillIndex];
			autoSkill.Activate();
			if(Game.Paused)
			{
				Update();
				await ToSignal(Game, nameof(Gamestate.UnPaused));
			}
			autoSkill.Resolve();
			Game.GetTurnPlayer().DeclarePlay(new Resolve());
			Game.GetTurnPlayer().SetValidAttackTargets();
			Update();
		}
		
		public void Resolve()
		{
			while (Chain.Count != 0)
			{
				var skill = Chain[Chain.Count - 1];
				Chain.RemoveAt(Chain.Count - 1);
				skill.Resolve();
			}
			
			ApplyConstants();
			Game.TurnPlayer.State = Player.States.Idle;
			Game.TurnPlayer.Opponent.State = Player.States.Passive;
		}
		
		public void Update()
		{
			EmitSignal(nameof(Updated));
		}
		
	}

}
