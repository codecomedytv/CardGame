using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using CardGame.Server.States;
using Godot.Collections;
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
			foreach (var skill in Auto)
			{
				skill.SetUp(gameEvent);
				if (skill.CanBeUsed)
				{
					Automatic(skill.Controller, skill.Card);
				}
			}
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
		
		
		public async void Activate(Player player, Card card, int skillIndex, Array<int> targets)
		{
			var activatedSkill = card.Skills[skillIndex];
			if (!activatedSkill.CanBeUsed)
			{
				return;
			}
			if (targets.Count > 0)
			{
				Game.Target = (Unit)Game.GetCard(targets[0]);
			}
			activatedSkill.Activate();
			if (Game.Paused)
			{
				await ToSignal(Game, nameof(Gamestate.UnPaused));
			}

			var cards = new List<Card>();
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
			GD.Print("Hello?");
			player.State = new Passing();
			var autoSkill = card.Skills[skillIndex];
			autoSkill.Activate();
			if(Game.Paused)
			{
				GD.Print("Game Paused");
				EmitSignal(nameof(Updated));
				await ToSignal(Game, nameof(Gamestate.UnPaused));
			}
			// GD.Print(Game.Target);
			autoSkill.Resolve();
			Game.GetTurnPlayer().DeclarePlay(new Resolve());
			Game.GetTurnPlayer().SetValidAttackTargets();
			EmitSignal(nameof(Updated));

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
		}
		
		
	}

}
