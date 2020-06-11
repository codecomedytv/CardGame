using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.Game.Commands
{
    public class Draw : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Draw(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }

        public void Execute()
        {
            Player.Move(Player.Deck, Card, Player.Hand);
        }

        public void Undo()
        {
            Player.Move(Player.Hand, Card, Player.Deck);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Draw;
            message.Player["args"] = new Array {Card.Serialize()};
            message.Opponent["command"] = GameEvents.OpponentDraw;
            message.Opponent["args"] = new Array {1};
            return message;
        }
       
    }
}