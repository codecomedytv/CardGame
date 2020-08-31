using Godot;

namespace CardGame.Client.Game
{
	public class MessageReceiver: Object
	{
		[Signal] public delegate void ExecutedEvents();
		[Signal] public delegate void QueueEvents();

		public void Execute(int stateAfterExecution)
		{
			EmitSignal(nameof(ExecutedEvents), stateAfterExecution);
		}

		public void Queue(string signal, params object[] args)
		{
			EmitSignal(nameof(QueueEvents), signal, args);
		}
	}
}
