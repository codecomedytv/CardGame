using Godot.Collections;

namespace CardGame.Server.Commands
{
    public class  Discard : GameEvent, ICommand
    {
        public readonly ISource Source;
        public readonly Player Player;
        public readonly Card Card;

        public Discard(ISource source, Player player, Card card)
        {
            Source = source;
            Player = player;
            Card = card;
        }

        public void Execute()
        {
            Player.Move(Player.Hand, Card, Player.Graveyard);
        }

        public void Undo()
        {
            Player.Move(Player.Graveyard, Card, Player.Hand);
        }

        public override Message GetMessage()
        {
            var message = new Message();
            message.Player["command"] = GameEvents.Discard;
            message.Player["args"] = new Array{Card.Id};
            message.Opponent["command"] = GameEvents.OpponentDiscard;
            message.Opponent["args"] = new Array{Card.Serialize()};
            return message;
        }
    }
}