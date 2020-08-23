using System;
using System.Collections.Generic;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public delegate void Declaration(Func<Tween> command);

    public class CommandQueue: Godot.Object
    {
        private IList<Func<Tween>> Commands = new List<Func<Tween>>();
        private readonly Declaration Declare;
        
        public CommandQueue()
        {
            Declare = OnCommandDeclared;
        }

        public void SubscribeTo(IPlayer player)
        {
            //player.ConnectDeclaration(this, nameof(OnCommandDeclared));
            player.Connect(Declare);
        }
        private void OnCommandDeclared(Func<Tween> command)
        {
            GD.Print("Command Added");
            Commands.Add(command);
        }

        // Setting State Should be a Queued Action In Future
        private async void Execute(int stateAfterExecution)
        {
            foreach (var command in Commands)
            {
                var executor = command();
                executor.Start();
                await ToSignal(executor, "tween_all_completed");
            }
        }
    }
}