using System.Collections.Generic;
using CardGame.Server.Room.Cards;
using CardGame.Server.States;
using Godot;
using Godot.Collections;

namespace CardGame.Server.Room {

	public class Link : Reference
	{
		private Stack<IResolvable> Chain = new Stack<IResolvable>();
		private List<Skill> Constants = new List<Skill>();
		private List<Skill> Manual = new List<Skill>();
		private List<Skill> Auto = new List<Skill>();

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

		public void Unregister(Card card)
		{
			
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
