using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class Atk15KDef1k : Unit
    {
        public Atk15KDef1k(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.1500ATK.1000DEF";
            SetCode = SetCodes.Debug15001000;
            Power = 1500;
        }
    }
}