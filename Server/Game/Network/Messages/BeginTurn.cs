namespace CardGame.Server.Game.Network.Messages
{
    public class BeginTurn: Message
    {
        public BeginTurn() => Player[Command] = (int) GameEvents.BeginTurn;
    }
}
