using CardGame.Server.Room;
using CardGame.Server.Room.Cards;

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
