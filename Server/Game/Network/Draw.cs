using CardGame.Server.Room.Cards;

namespace CardGame.Server.Room.Network.Messages
{
	public class Draw: Message
	{

		public Draw(Card card)
		{
			Player[Command] = (int)GameEvents.Draw;
			Player[Id] = card.Id;
			Player[SetCode] = (int) card.SetCode;
			Opponent[Command] = (int) GameEvents.OpponentDraw;
		}
	}
}
