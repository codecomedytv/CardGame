using System.Threading.Tasks;
using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class RevealCard : Command
    {
        public readonly Player Player;
        public readonly Card Card;
        public readonly ZoneIds ZoneId;

        public RevealCard(Player player, Card card, ZoneIds zoneId)
        {
            Player = player;
            Card = card;
            ZoneId = zoneId;
        }

        protected override async Task<object[]> Execute()
        {
            Card.Player = Player;
            switch (ZoneId)
            {
                case ZoneIds.Hand:
                {
                    var old = Player.Hand[0];
                    Player.Hand.Remove(old);
                    Player.Hand.Add(Card);
                    Player.Hand.Sort();
                    break;
                }
                case ZoneIds.Support:
                {
                    foreach (Card oldCard in Player.Support)
                    {
                        if (oldCard.View.IsFaceUp)
                        {
                            continue;
                        }
                        var index = oldCard.GetPositionInParent();
                        Player.Support.Remove(oldCard);
                        Player.Support.Add(Card);
                        Player.Support.Move(Card, index);
                        Player.Support.Sort();
                        oldCard.Free();
                        break;
                    }

                    break;
                }
            }

            return await Start();

        }
    }
}
