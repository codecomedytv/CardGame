using CardGame.Server.Room.Cards;
using Godot;

namespace CardGame.Server.Room.Network.Messages
{
    public class SetAsDeployable: Message
    {
        public SetAsDeployable(Card unit)
        {
            Player[Command] = (int) GameEvents.SetDeployable;
            Player[Id] = unit.Id;
        }
    }
}
