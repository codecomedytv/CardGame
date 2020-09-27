using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class Login: IScreen<Node>
    {
        public event Action LoggedIn;
        public Node View { get; } = new Node();
        
        public void Display()
        {
            throw new NotImplementedException();
        }

        public void StopDisplaying()
        {
            throw new NotImplementedException();
        }

        private void OnLoggedIn()
        {
            LoggedIn?.Invoke();
        }

        
    }
}