using Godot.Collections;

namespace CardGame.Server.Game.Network.Messages
{
    public class Message
    {
        public readonly Dictionary<string, int> Player;
        public readonly Dictionary<string, int> Opponent;

        public Message()
        {
            
        }
    }
}