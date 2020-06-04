using CardGame.Client.Library.Card;
using CardGame.Client.Match;
using Godot;
using Godot.Collections;

namespace CardGameSharp.Client.Game
{
	public class GameInput : Control
	{
		[Signal]
		public delegate void CommandRequested();

		public int NetworkId = 0;
		public Player Player;
		public Opponent Opponent;
		public Messenger Messenger;
		public Dictionary Cards;

		public void SetUp(Player player, Opponent opponent, int networkId, Messenger messenger, Dictionary cards)
		{
			Player = player;
			Opponent = opponent;
			NetworkId = networkId;
			Messenger = messenger;
			Cards = cards;
		}

		public void OnDeploy(Card card)
		{
			if (!card.CanBeDeployed)
			{
				return;
			}

			Messenger.Deploy(card.Id);
		}

		public void OnSetFaceDown(Card card)
		{
			if (!card.CanBeSet)
			{
				return;
			}

			Messenger.SetFaceDown(card.Id);
		}

		public void OnActivation(Card card, Array targets)
		{
			//	if not player.active or card.card_type != CARD_TYPE.SUPPORT  or not card.ready or not card.legal or not card in player.support:
			//		return
			GD.Print("Declaring Activation");
			Messenger.Activate(card, targets);
		}

		public void OnTarget(Card card)
		{
			Messenger.Target(card.Id);
		}

		public void OnAttack(Card attacker, object defender)
		{

			if (!attacker.CanAttack)
			{
				return;
			}

			if (defender is Card card)
			{
				card.RemoveAura();
				Messenger.Attack(attacker.Id, card.Id);
			}
			else
			{
				Messenger.Attack(attacker.Id, (int) defender);
			}
		}

		public void OnPassPriority()
		{
			Messenger.PassPriority();
		}

		public void EndTurn()
		{
			Messenger.EndTurn();
		}

		public void Command()
		{
			EmitSignal(nameof(CommandRequested));
		}


	}
}
