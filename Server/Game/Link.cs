using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.States;
using Godot;
using Godot.Collections;

namespace CardGame.Server.Game {

	public class Link : Reference
	{
		public Gamestate Game;
		private List<IResolvable> Chain = new List<IResolvable>();
		private List<Skill> Constants = new List<Skill>();
		private List<Skill> Manual = new List<Skill>();
		private List<Skill> Auto = new List<Skill>();

		public void SetUp(Gamestate game)
		{
			// Will eventually remove this?
			if(Game == null) { Game = game; }
		}

		public void AddResolvable(IResolvable action) => Chain.Add(action);

		public void ApplyConstants(string gameEvent = "") => Constants.ForEach(s => s.Resolve(gameEvent));

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
		
		public void  SetupManual(string gameEvent) => Manual.ForEach(s => s.SetUp(gameEvent));

		public void Broadcast(string gameEvent, List<Godot.Object> arguments)
		{
			ApplyConstants(gameEvent);
			ApplyTriggered(gameEvent);
			SetupManual(gameEvent);
		}
		
		public void Register(Card card)
		{
			
			switch (card.Skill.Type)
			{
				case Skill.Types.Constant:
					Constants.Add(card.Skill);
					break;
				
				case Skill.Types.Auto:
					Auto.Add(card.Skill);
					break;
				
				case Skill.Types.Manual:
					Manual.Add(card.Skill);
					break;
				default:
					return;
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
		
		
		public async void Activate(Player player, Card card, Array<int> targets)
		{
			var activatedSkill = card.Skill;
			if (targets.Count > 0)
			{
				activatedSkill.Target = (Unit)Game.GetCard(targets[0]);
			}
			activatedSkill.Activate();
			if (activatedSkill.Targeting)
			{
				var result = await ToSignal(player, nameof(Player.TargetSelected));
				activatedSkill.Target = result[0] as Card;
			}
			
			Chain.Add(activatedSkill);
		}

		public async void Automatic(Player player, Card card)
		{
			// Needs serious rewriting
			player.State = new Passing();
			var autoSkill = card.Skill;
			autoSkill.Activate();
			if(autoSkill.Targeting)
			{
				var result = await ToSignal(player, nameof(Player.TargetSelected));
				autoSkill.Target = result[0] as Card;
				//autoSkill.Target =  await ToSignal(player, nameof(Player.TargetSelected));
			}
			autoSkill.Resolve();
			player.DeclarePlay(new Resolve());

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
