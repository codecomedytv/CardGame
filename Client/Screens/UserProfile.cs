using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class UserProfile: IScreen<Node>
    {
        public event Action BackPressed;
        public Node View { get; private set; } = new Node();
        
        public void Display()
        {
            throw new NotImplementedException();
        }

        public void StopDisplaying()
        {
            throw new NotImplementedException();
        }

        private void OnBackPressed()
        {
            BackPressed?.Invoke();
        }

        
    }
}