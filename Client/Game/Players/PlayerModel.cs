using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Players
{
    public class PlayerModel: IPlayerModel
    {
        public void Draw(CardModel card)
        {
            GD.Print("Player Model Drew Card");
        }
    }
}