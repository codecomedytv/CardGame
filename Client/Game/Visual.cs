using System;
using System.Collections.Generic;
using CardGame.Client.Library.Card;
using CardGameSharp.Client.Game;
using Godot;
using Godot.Collections;
using Array = Godot.Collections.Array;
using Object = Godot.Object;

namespace CardGame.Client.Match
{
	public class Visual : Control
	{
		public Label Life;
		public HBoxContainer Hand;
		public HBoxContainer Units;
		public HBoxContainer Support;
		public Control Discard;
		public Label Deck;
		public Label Damage;
		public Gfx Animate;
		public int Who;
		public AudioStreamPlayer Sfx;
		public History History;
		public Dictionary Cards;

		public override void _Ready()
		{
			Life = GetNode("View/Life") as Label;
			Hand = GetNode("Hand") as HBoxContainer;
			Units = GetNode("Units") as HBoxContainer;
			Support = GetNode("Support") as HBoxContainer;
			Discard = GetNode("Discard") as Control;
			Deck = (Label) GetNode("Deck");
			Damage = GetNode("Damage") as Label;
		}
		
		public float Delay(object Delay = null)
		{
			if (Delay is float timeDelay)
			{
				return Animate.AddDelay(timeDelay, Who);
			}
			else
			{
				return Animate.TotalDelay(Who);
			}
		}

		public void AutoTarget(Card card)
		{
			foreach (var target in card.ValidTargets)
			{
				QueueCallback((Card)Cards[target], Delay(), "ShowAsValid", true);
			}
		}

		public void QueueProperty(Object obj, string property, object start, object end, float duration,
			float Delay = 0.0F)
		{
			Animate.InterpolateProperty(obj, property, start, end, duration, Tween.TransitionType.Linear,
				Tween.EaseType.In, Delay);
		}

		public void QueueCallback(Object obj, float delay, string callback, object args1 = null, object args2 = null, object args3 = null, object args4 = null, object args5 = null)
		{
			Animate.InterpolateCallback(obj, delay, callback, args1, args2, args3, args4, args5);
		}

		[Signal]
		public delegate void ButtonAction();

		public void SetState(Player.States state)
		{
			switch (state)
			{
				case Player.States.Idle:
					EmitSignal(nameof(ButtonAction), "");
					break;
				case Player.States.Active:
					EmitSignal(nameof(ButtonAction), "Pass");
					break;
				case Player.States.Passive:
					break;
				case Player.States.Acting:
					break;
				case Player.States.Passing:
					break;
				case Player.States.Targeting:
					break;
				default:
					EmitSignal(nameof(ButtonAction), "Wait");
					break;
			}

			var active = GetNode("Active") as Label;
			if (active != null) active.Text = state.ToString();
		}

		public void ShowAttack(Card attacker, object defender)
		{
			attacker.Combat.Show();
			if (defender is Card card)
			{
				card.Combat.Show();
				History.Attack(Who, attacker, card);
			}
			else
			{
				GD.Print("Visual.CS: Todo Add History");
				History.DirectAttack(Who, attacker);
			}
		}

		public void Setup(Gfx animate, AudioStreamPlayer sfx, History history, int who)
		{
			Animate = animate;
			Sfx = sfx;
			History = history;
			Who = who;
		}

		public void Bounce(Card card)
		{
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Hand), 0.3F, Delay());
			QueueCallback(History, Delay() + 0.3F, "Bounce", Who, card);
			QueueCallback(card.GetParent(), Delay(0.3), "RemoveChild", card);
			QueueCallback(Hand, Delay(), "AddChild", card);
			QueueCallback(Sfx, Delay(), "AddChild", card);
			QueueCallback(Sfx, Delay(), "Deploy"); // Guess we didn't have a dedicated bounce sfx
			if (Who != (int) Gfx.Who.Opponent) return;
			QueueCallback(card, Delay(), "FlipFaceDown");
			var fake = Library.Library.Placeholder();
			QueueCallback(Hand, Delay(), "RemoveChild", card);
			QueueCallback(Hand, Delay(), "AddChild", fake);
			QueueCallback(card, Delay(), "QueueFree");
		}

		public void Resolve(Array<Card> linked)
		{
			foreach (var card in linked)
			{
				QueueCallback(card.GetParent(), Delay(0.3), "RemoveChild", card);
				QueueCallback(Discard, Delay(), "AddChild", card);
				// Should add a check for unit based effects (for some reason?)
				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F, Delay());
			}
		}

		public void Activate(Card card, List<Card> link, Array<Card> targets)
		{
			if (Who == (int) Gfx.Who.Opponent)
			{
				Support.GetChild(0).Free();
				Support.AddChild(card);
				card.Back.Visible = true;
			}

			link.Add(card);
			QueueCallback(card.Link, Delay(), "set_text", link.Count.ToString());
			QueueCallback(card.Link, Delay(0.1F), "SetVisible", true);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Deploy");
			QueueCallback(card.Back, Delay(0.1F), "Hide");
			QueueCallback(History, Delay(0.1F), "Activate", Who, card);
			if (targets.Count != 0)
			{
				QueueCallback(History, Delay(0.1), "Target", Who, targets);
			}
		}

		public void AttackUnit(Card attacker, Card defender)
		{
			var targetPoisition = AttackTargetPosition(defender, Who);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPoisition, 0.1F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPoisition, attacker.RectGlobalPosition, .1F, Delay(0.1));
			QueueCallback(attacker.Combat, Delay(), "Hide");
			QueueCallback(attacker.Combat, Delay(), "Hide");
			QueueCallback(defender, Delay(), "RemoveAura");
			if (Who == (int)Gfx.Who.Player)
			{
				QueueCallback(History, Delay(), "Attack", Who, attacker, defender);
			}
			QueueCallback(Sfx, Delay(0.3), "BattleUnit");
		}

		public Vector2 AttackTargetPosition(Card defender, int player)
		{
			var yModifier = new Vector2(0, defender.RectScale.y);
			return player == (int) Gfx.Who.Player
				? defender.RectGlobalPosition + yModifier
				: defender.RectGlobalPosition - yModifier;
		}

		public void AttackDirectly(Card attacker)
		{
			var targetPosition = DirectAttackTargetPosition(attacker, Who);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPosition, 0.3F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPosition, attacker.RectGlobalPosition, 0.3F, Delay(0.3F));
			if (Who == (int) Gfx.Who.Player)
			{
				Animate.AddDelay(0.3F, (int)Gfx.Who.Opponent);
			}
			QueueCallback(attacker.Combat, Delay(), "Hide");
			QueueCallback(History, Delay(0.1), "DirectAttack", Who, attacker);
			QueueCallback(Sfx, Delay(0.3F), "BattleUnit");
		}

		public Vector2 DirectAttackTargetPosition(Card attacker, int player)
		{
			var yModifier = new Vector2(0, 70);
			return player == (int) Gfx.Who.Player
				? attacker.RectGlobalPosition - yModifier
				: attacker.RectGlobalPosition + yModifier;
		}

		public void ReadyCards(Array<int> args)
		{
			foreach (var id in args)
			{
				QueueCallback((Card)Cards[id], Delay(), "Ready");
			}
		}

		public void UnreadyCards(Array<int> args)
		{
			foreach(var id in args)
			{
				QueueCallback((Card)Cards[id], Delay(), "Exhaust");
			}
		}

		public void Deploy(Array args)
		{
			if (args[0] is Dictionary arg)
			{
				var card = Who == (int) Gfx.Who.Player ? (Card)Cards[args[0]] : (Card)Cards[arg["Id"]];
				if (Who == (int) Gfx.Who.Opponent)
				{
					Hand.RemoveChild(Hand.GetChild(0));
					Hand.AddChild(card);
					Sort(Hand);
					card.FlipFaceDown();
				}
			
				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay());
				QueueCallback(History, Delay() + 0.3F, "Deploy", Who, card);
				QueueCallback(card.GetParent(), Delay(0.3F), "RemoveChild", card);
				QueueCallback(Units, Delay(), "AddChild", card);
				QueueCallback(card, Delay(), "FlipFaceUp");
			}

			QueueCallback(Sfx, Delay(), "Deploy");
			
		}

		public void SetFaceDown(Array args)
		{
			if (args[0] is Dictionary arg)
			{
				var card = Who == (int) Gfx.Who.Player ? (Card)Cards[args[0]] : (Card)Cards[arg["Id"]];
				if (Who == (int) Gfx.Who.Opponent)
				{
					Hand.RemoveChild(Hand.GetChild(0));
					Hand.AddChild(card);
					Sort(Hand);
				}

				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Support), 0.3F, Delay());
				QueueCallback(History, Delay() + 0.3F, "SetFaceDown", Who, card);
				QueueCallback(card.GetParent(), Delay(0.3), "RemoveChild", card);
				QueueCallback(Support, Delay(), "AddChild", card);
				QueueCallback(card, Delay(), "FlipFaceDown");
			}

			QueueCallback(Sfx, Delay(), "SetFaceDown");
		}
		
		public void LoseLife(Array<int> args)
		{
			var damageTaken = args[0];
			Damage.Text = "-" + args[0].ToString();
			Life.Text = (Life.Text.ToInt() - (int) damageTaken).ToString();
			var visible = Damage.Modulate + new Color(0, 0, 0, 255);
			var invisible = Damage.Modulate - new Color(0, 0, 0, 255);
			QueueCallback(Damage, Delay(), "SetSelfModulate", visible);
			QueueCallback(History, Delay(0.1), "LoseLife", Who, damageTaken);
			QueueCallback(Damage, Delay(0.5), "SetSelfModulate", invisible);
		}
		
		public void DestroyUnit(Array args)
		{
			var card = (Card) Cards[args[0]];
			QueueCallback(card.GetParent(), Delay(0.3), "RemoveChild", card);
			QueueCallback(Discard, Delay(), "AddChild", card);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F,
				Delay());
			QueueCallback(History, Delay() + 0.1F, "Destroyunit", Who, card);
		}
		
		public void LoadDeck(Array args)
		{
			var deckSize = args[0].ToString();
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize);
		}

		public void Draw(Array args, Player playerData)
		{
			var count = 0;
			count = (int) (Who == (int)Gfx.Who.Player ? args.Count : args[0]);
			Array drawn = new Array();
			var positions = NextHandPositions(count);
			for(var i = 0; i < count; i++)
			{
				if (args[i] is Dictionary arg)
				{
					var card = (Card) (Who == (int)Gfx.Who.Player ? Cards[arg["Id"]] : Library.Library.Placeholder());
					drawn.Add(card);
					Hand.AddChild((Card)card);
					card.RectGlobalPosition = Deck.RectGlobalPosition;
					card.TurnInvisible();
					var pos = positions[0];
					positions.RemoveAt(0);
					QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, pos, 0.2F, Delay(0.2F));
					QueueCallback(card, Delay(0.0), "TurnVisible");
				}

				var deckSize = (playerData.DeckSize + count - i - 1).ToString();
				QueueCallback(Deck, Delay(), "set_text", deckSize);
				QueueCallback(Sfx, Delay(), "DrawCard");
			}

			QueueCallback(this, Delay(0.2), "Sort", Hand);
			if(Who == (int)Gfx.Who.Player)
			{
				QueueCallback(History, Delay(), "PlayerDraw", drawn);
			}
			else
			{
				QueueCallback(History, Delay(), "OpponentDraw", count);
			}
		}
		
		public Array NextHandPositions(int count)
		{
			var blanks = new Array();
			var positions = new Array();
			for(var i = 0; i < count; i++)
			{
				var blank = Library.Library.Placeholder();
				Hand.AddChild(blank);
				blanks.Add(blank);
			}

			Sort(Hand);
			foreach(Card blank in blanks)
			{
				positions.Add(blank.RectGlobalPosition);
			}
			foreach(Card blank in blanks)
			{
				Hand.RemoveChild(blank);
				blank.Free();
			}
			blanks.Clear();
			return positions;
		}


		public Vector2 FuturePosition(Container zone)
		{
			var blank = Library.Library.Placeholder();
			zone.AddChild(blank);
			Sort(zone);
			var nextPosition = blank.RectGlobalPosition;
			zone.RemoveChild(blank);
			return nextPosition;
		}

		public void Sort(Container zone)
		{
			zone.Notification(Container.NotificationSortChildren);
		}

		public void BeginTurn()
		{
			QueueCallback(History, Delay(), "BeginTurn");
		}

		public void EndTurn()
		{
			QueueCallback(History, Delay() ,"EndTurn");
		}


		public void Draw(Array args, Opponent playerData)
		{
			var count = 0;
			count = (int) (Who == (int)Gfx.Who.Player ? args.Count : args[0]);
			Array drawn = new Array();
			var positions = NextHandPositions(count);
			for(var i = 0; i < count; i++)
			{
				if (args[i] is Dictionary arg)
				{
					var card = (Card) (Who == (int)Gfx.Who.Player ? Cards[arg["Id"]] : Library.Library.Placeholder());
					drawn.Add(card);
					Hand.AddChild((Card)card);
					card.RectGlobalPosition = Deck.RectGlobalPosition;
					card.TurnInvisible();
					var pos = positions[0];
					positions.RemoveAt(0);
					QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, pos, 0.2F, Delay(0.2F));
					QueueCallback(card, Delay(0.0), "TurnVisible");
				}

				var deckSize = (playerData.DeckSize + count - i - 1).ToString();
				QueueCallback(Deck, Delay(), "set_text", deckSize);
				QueueCallback(Sfx, Delay(), "DrawCard");
			}

			QueueCallback(this, Delay(0.2), "Sort", Hand);
			if(Who == (int)Gfx.Who.Player)
			{
				QueueCallback(History, Delay(), "PlayerDraw", drawn);
			}
			else
			{
				QueueCallback(History, Delay(), "OpponentDraw", count);
			}
		}
	}
}
