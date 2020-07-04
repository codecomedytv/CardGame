using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class Atk500Def500 : Unit
    {
        public Atk500Def500(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.500ATK.500DEF";
            SetCode = SetCodes.Debug500500;
            Attack = 500;
            Defense = 500;
        }
    }
}