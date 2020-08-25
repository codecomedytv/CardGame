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
