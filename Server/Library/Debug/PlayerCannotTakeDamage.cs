using CardGame.Server.Game;
using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class PlayerCannotTakeDamage : Unit
    {
        public PlayerCannotTakeDamage()
        {
            Title = "Debug.PlayerCannotTakeDamage";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugPlayerCannotTakeDamage;
            // TODO: Re-Implement Tagged Decorators
        }
    }
}
