using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Network.Messages
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
