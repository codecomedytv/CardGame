using Godot;

namespace CardGame.Client.Game
{
	public class MessageReceiver: Object
	{
		// Responsible for interpreting and redistributing messages from the server
		
		[Signal] public delegate void ExecutedEvents();
		[Signal] public delegate void Draw();
		[Signal] public delegate void LoadDeck();
		[Signal] public delegate void UpdateCard();
		[Signal] public delegate void Deploy();
		[Signal] public delegate void RevealCard();
		[Signal] public delegate void SetFaceDown();
		[Signal] public delegate void Activate();
		[Signal] public delegate void SendCardToZone();
		[Signal] public delegate void ResolveCard();
		[Signal] public delegate void OpponentAttackUnit();
		[Signal] public delegate void BattleUnit();
		[Signal] public delegate void OpponentAttackDirectly();

		[Signal] public delegate void DirectAttack();

		[Signal] public delegate void LoseLife();

		public void Execute(int stateAfterExecution)
		{
			EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
		}

		public void Queue(string signal, params object[] args)
		{
			EmitSignal(signal, args);
		}
	}
}
