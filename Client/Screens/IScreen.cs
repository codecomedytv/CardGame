using System;
using Godot;

namespace CardGame.Client.Screens
{
    public interface IScreen<out T> where T: Node
    {
        T View { get; }

        void Display();
        void StopDisplaying();
    }
}