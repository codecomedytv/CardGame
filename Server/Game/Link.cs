using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Skill;
using Godot;

namespace CardGame.Server.Game {

	public class Link : Reference
	{
		private Stack<IResolvable> Chain = new Stack<IResolvable>();
		private List<Skill.Skill> Constants = new List<Skill.Skill>();
		private List<Skill.Skill> Manual = new List<Skill.Skill>();
		private List<Skill.Skill> Auto = new List<Skill.Skill>();

		public void AddResolvable(IResolvable action) => Chain.Push(action);

		public void ApplyConstants(string gameEvent = "") => Constants.ForEach(s => s.Resolve(gameEvent));

		public void ApplyTriggered(string gameEvent)
		{
			foreach (var skill in Auto)
			{
				skill.SetUp(gameEvent);
				if (!skill.CanBeUsed) continue;
				skill.Activate();
				Chain.Push(skill);
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
				case Skill.Skill.Types.Constant:
					Constants.Add(card.Skill);
					break;
				
				case Skill.Skill.Types.Auto:
					Auto.Add(card.Skill);
					break;
				
				case Skill.Skill.Types.Manual:
					Manual.Add(card.Skill);
					break;
				default:
					return;
			}
			
		}
		
		public void Unregister(Skill.Skill skill)
		{
			switch (skill.Type)
			{
				case Skill.Skill.Types.Constant:
					Constants.Remove(skill);
					return;
				case Skill.Skill.Types.Auto:
					Auto.Remove(skill);
					return;
				case Skill.Skill.Types.Manual:
					Manual.Remove(skill);
					return;
				default:
					return;
			}
		}

		public void Unregister(Card card)
		{
			
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
			
			ApplyConstants();
		}
		
		
	}

}
