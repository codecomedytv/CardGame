using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class Atk1KDef1K : Unit
    {
        public Atk1KDef1K(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.1000ATK.1000DEF";
            SetCode = SetCodes.Debug1000_1000;
            Attack = 1000;
            Defense = 1000;
        }
    }
}