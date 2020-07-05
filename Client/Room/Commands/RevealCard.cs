using System.Threading.Tasks;
using CardGame.Client.Library.Cards;
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
                    var old = Player.Hand.GetChild(0);
                    Player.Hand.RemoveChild(old);
                    Player.Hand.AddChild(Card);
                    Player.Sort(Player.Hand);
                    break;
                }
                case ZoneIds.Support:
                {
                    foreach (Card oldCard in Player.Support.GetChildren())
                    {
                        if (oldCard.IsFaceUp)
                        {
                            continue;
                        }
                        var index = oldCard.GetPositionInParent();
                        Player.Support.RemoveChild(oldCard);
                        Player.Support.AddChild(Card);
                        Player.Support.MoveChild(Card, index);
                        Player.Sort(Player.Support);
                        oldCard.Free();
                        break;
                    }

                    break;
                }
            }

            Gfx.Start();
            return await ToSignal(Gfx, "tween_all_completed");
        }
    }
}
