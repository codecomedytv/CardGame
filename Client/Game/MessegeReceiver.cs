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

		public void Execute(int stateAfterExecution)
		{
			GD.Print("Message Receiver Executing");
		}

		public void Queue(string signal, params object[] args)
		{
			EmitSignal(signal, args);
		}
	}
}
