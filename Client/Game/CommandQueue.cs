﻿using System;
using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Players;
using CardGame.Tests;
using Godot;

namespace CardGame.Client.Game
{
    public delegate void Declaration(Func<Tween> command);

    public class CommandQueue: Godot.Object
    {
        private readonly Queue<Func<Tween>> Commands = new Queue<Func<Tween>>();
        private readonly Declaration Declare;
        
        public CommandQueue()
        {
            Declare = OnCommandDeclared;
        }

        public void SubscribeTo(IPlayer player)
        {
            player.Connect(Declare);
        }

        public void SubscribeTo(MessageReceiver messageReceiver)
        {
            messageReceiver.Connect(nameof(MessageReceiver.ExecutedEvents), this, nameof(Execute));
        }
        private void OnCommandDeclared(Func<Tween> command)
        {
            Commands.Enqueue(command);
        }

        // Setting State Should be a Queued Action In Future
        private async void Execute(int stateAfterExecution)
        {
            while(Commands.Count > 0)
            {
                var command = Commands.Dequeue();
                var executor = command();
                executor.Start();
                await ToSignal(executor, "tween_all_completed");
                executor.RemoveAll();
            }
        }
    }
}