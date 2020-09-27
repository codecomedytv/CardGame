using Godot;

namespace CardGame.Client.Screens
{
    public class MatchFinder: IScreen<Node>
    {
        public Node View { get; private set; } = new Node();
        
        public void Display()
        {
            throw new System.NotImplementedException();
        }

        public void StopDisplaying()
        {
            throw new System.NotImplementedException();
        }
    }
}