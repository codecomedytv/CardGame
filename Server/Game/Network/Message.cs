using Godot.Collections;

namespace CardGame.Server.Game.Network
{
    public class Message
    {
        protected const string Command = "command";
        protected const string Id = "id";
        protected const string SetCode = "setCode";
        public Dictionary<string, int> Player = new Dictionary<string, int> {{Command, (int) GameEvents.NoOp}};
        public Dictionary<string, int> Opponent = new Dictionary<string, int>{{Command, (int) GameEvents.NoOp}};

        public Message()
        {
            
        }
    }
}