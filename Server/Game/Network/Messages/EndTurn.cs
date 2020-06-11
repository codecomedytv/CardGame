namespace CardGame.Server.Game.Network.Messages
{
    public class EndTurn: Message
    {
        public EndTurn() => Player[Command] = (int)GameEvents.EndTurn;
    }
}