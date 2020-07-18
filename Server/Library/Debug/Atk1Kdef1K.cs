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
            SetCode = SetCodes.Debug10001000;
            Power = 1000;
        }
    }
}