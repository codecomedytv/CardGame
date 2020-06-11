namespace CardGame.Server.Game.Network.Messages
{
    public class SetState: Message
    {
        public SetState(string state)
        {
            Player[Command] = (int) GameEvents.SetState;
            Player["state"] = 0;
            
        }
    }
}
