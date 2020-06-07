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
		public Godot.Collections.Dictionary<int, Card> Cards;

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
				QueueCallback((Card)Cards[(int)target], Delay(), "ShowAsValid", true);
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

		public void SetState(string state)
		{
			switch (state)
			{
				case "Idle":
					EmitSignal(nameof(ButtonAction), "");
					break;
				case "Active":
					EmitSignal(nameof(ButtonAction), "Pass");
					break;
				case "Passive":
					break;
				case "Acting":
					break;
				case "Passing":
					break;
				case "Targeting":
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
				History.AddLine($"Your {attacker} attacked Enemy's {defender}");
			}
			else
			{
				History.AddLine($"Your {attacker} attacked directly" );
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
			QueueCallback(History, Delay() + 0.3F, "AddLine", $"{card} was returned to your hand");
			QueueCallback(card.GetParent(), Delay(0.3), "remove_child", card);
			QueueCallback(Hand, Delay(), "add_child", card);
			QueueCallback(Sfx, Delay(), "add_child", card);
			QueueCallback(Sfx, Delay(), "Deploy"); // Guess we didn't have a dedicated bounce sfx
		}

		public void Resolve(Array<Card> linked)
		{
			foreach (var card in linked)
			{
				QueueCallback(card.GetParent(), Delay(0.3), "remove_child", card);
				QueueCallback(Discard, Delay(), "add_child", card);
				// Should add a check for unit based effects (for some reason?)
				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F, Delay());
			}
		}

		public void Activate(Card card, List<Card> link, Array<Card> targets)
		{
			link.Add(card);
			QueueCallback(card.Link, Delay(), "set_text", link.Count.ToString());
			QueueCallback(card.Link, Delay(0.1F), "set_visible", true);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Deploy");
			QueueCallback(card.Back, Delay(0.1F), "hide");
			QueueCallback(History, Delay(0.1F), "AddLine", $"You activated {card}");
			if (targets.Count != 0)
			{
				QueueCallback(History, Delay(0.1), "AddLine", $"Targeting: {targets}");
			}
		}

		public void AttackUnit(Card attacker, Card defender)
		{
			var targetPoisition = AttackTargetPosition(defender, Who);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPoisition, 0.1F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPoisition, attacker.RectGlobalPosition, .1F, Delay(0.1));
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(defender, Delay(), "RemoveAura");
			QueueCallback(History, Delay(), "Attack", Who, attacker, defender);
			QueueCallback(Sfx, Delay(0.3), "BattleUnit");
		}

		public Vector2 AttackTargetPosition(Card defender, int player)
		{
			var yModifier = new Vector2(0, defender.RectScale.y);
			return defender.RectGlobalPosition + yModifier;
			
		}

		public void AttackDirectly(Card attacker)
		{
			var targetPosition = DirectAttackTargetPosition(attacker, Who);
			QueueProperty(attacker, "RectGlobalPosition", attacker.RectGlobalPosition, targetPosition, 0.3F, Delay());
			QueueProperty(attacker, "RectGlobalPosition", targetPosition, attacker.RectGlobalPosition, 0.3F, Delay(0.3F));
			Animate.AddDelay(0.3F, (int)Gfx.Who.Opponent);
			QueueCallback(attacker.Combat, Delay(), "hide");
			QueueCallback(History, Delay(0.1), "DirectAttack", Who, attacker);
			QueueCallback(Sfx, Delay(0.3F), "BattleUnit");
		}

		public Vector2 DirectAttackTargetPosition(Card attacker, int player)
		{
			var yModifier = new Vector2(0, 70);
			return attacker.RectGlobalPosition - yModifier;
		}

		public void ReadyCards(Array args)
		{
			foreach (var id in args)
			{
				QueueCallback((Card)Cards[(int)id], Delay(), "Ready");
			}
		}

		public void UnreadyCards(Array args)
		{
			foreach(var id in args)
			{
				QueueCallback((Card)Cards[(int)id], Delay(), "Exhaust");
			}
		}

		public void Deploy(Card card)
		{
			
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Units), 0.3F, Delay());
			QueueCallback(History, Delay() + 0.3F, "AddLine", $"You deployed {card}");
			QueueCallback(card.GetParent(), Delay(0.3F), "remove_child", card);
			QueueCallback(Units, Delay(), "add_child", card);
			QueueCallback(card, Delay(), "FlipFaceUp");
			QueueCallback(Sfx, Delay(), "Deploy");
			
		}

		public void SetFaceDown(Card card)
		{
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, FuturePosition(Support), 0.3F, Delay());
			QueueCallback(History, Delay() + 0.3F, "AddLine", $"You set {card} FaceDown");
			QueueCallback(card.GetParent(), Delay(0.3), "remove_child", card);
			QueueCallback(Support, Delay(), "add_child", card);
			GD.Print("setting ", card.ToString());
			QueueCallback(card, Delay(), "FlipFaceDown");
			QueueCallback(Sfx, Delay(), "SetFaceDown");
		}
		
		public void LoseLife(int damageTaken)
		{
			Damage.Text = "-" + damageTaken.ToString();
			Life.Text = (Life.Text.ToInt() - (int) damageTaken).ToString();
			var visible = Damage.Modulate + new Color(0, 0, 0, 255);
			var invisible = Damage.Modulate - new Color(0, 0, 0, 255);
			QueueCallback(Damage, Delay(), "set_self_modulate", visible);
			QueueCallback(History, Delay(0.1), "AddLine", $"You took {damageTaken} damage");
			QueueCallback(Damage, Delay(0.5), "set_self_modulate", invisible);
		}
		
		public void DestroyUnit(Card card)
		{
			QueueCallback(card.GetParent(), Delay(0.3), "remove_child", card);
			QueueCallback(Discard, Delay(), "add_child", card);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, Discard.RectGlobalPosition, 0.3F,
				Delay());
			QueueCallback(History, Delay() + 0.1F, "AddLine", $"Your {card} was destroyed");
		}
		
		public void LoadDeck(int deckSize)
		{
			QueueCallback(Deck, Delay(0.3F), "set_text", deckSize.ToString());
		}

		public void Draw(Card card, Player playerData)
		{
			var positions = NextHandPositions(1);
			Hand.AddChild((Card)card);
			card.RectGlobalPosition = Deck.RectGlobalPosition;
			card.TurnInvisible();
			var pos = positions[0];
			positions.RemoveAt(0);
			QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, pos, 0.2F, Delay(0.2F));
			QueueCallback(card, Delay(0.0), "TurnVisible");
			var deckSize = (playerData.DeckSize - 1).ToString();
			QueueCallback(Deck, Delay(), "set_text", deckSize);
			QueueCallback(Sfx, Delay(), "DrawCard");
			QueueCallback(this, Delay(0.2), "Sort", Hand);
			QueueCallback(History, Delay(), "AddLine", $"You drew {card}");
			
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
			QueueCallback(History, Delay(), "AddLine", "Your turn has begun");
		}

		public void EndTurn()
		{
			QueueCallback(History, Delay() ,"AddLine", "You ended your turn");
		}


		public void Draw(Array args, Opponent playerData)
		{
			var count = 0;
			count = (int) (Who == (int)Gfx.Who.Player ? args.Count : args[0]);
			Array drawn = new Array();
			var positions = NextHandPositions(count);
			for(var i = 0; i < count; i++)
			{
				var card = Library.Library.Placeholder();
				drawn.Add(card);
				Hand.AddChild((Card)card);
				card.RectGlobalPosition = Deck.RectGlobalPosition;
				card.TurnInvisible();
				var pos = positions[0];
				positions.RemoveAt(0);
				QueueProperty(card, "RectGlobalPosition", card.RectGlobalPosition, pos, 0.2F, Delay(0.2F));
				QueueCallback(card, Delay(0.0), "TurnVisible");
			

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
