using System;
using System.Collections.Generic;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public delegate Tween Command();


    public class CommandQueue: Godot.Object
    {
        [Signal]
        public delegate void SetState();
        private readonly Queue<Command> Commands = new Queue<Command>();
        
        public void SubscribeTo(MessageReceiver messageReceiver)
        {
            messageReceiver.Connect(nameof(MessageReceiver.ExecutedEvents), this, nameof(Execute));
        }
        
        public void Add(Command command)
        {
            Commands.Enqueue(command);
        }

        // Setting State Should be a Queued Action In Future
        private async void Execute(int stateAfterExecution)
        {
            // Investigate Await ForEach?
            while(Commands.Count > 0)
            {
                var command = Commands.Dequeue();
                var executor = command();
                executor.Start();
                await ToSignal(executor, "tween_all_completed");
                executor.RemoveAll();
            }
            
            EmitSignal(nameof(SetState), stateAfterExecution);
        }
    }
}