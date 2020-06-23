using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class Atk15KDef1k : Unit
    {
        public Atk15KDef1k()
        {
            Title = "Debug.1500ATK.1000DEF";
            SetCode = SetCodes.Debug1500_1000;
            Attack = 1500;
            Defense = 1000;
        }
    }
}