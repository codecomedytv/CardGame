using CardGame.Server.Room;
using CardGame.Server.Room.Cards;

namespace CardGame.Server
{
    public class CannotBeDestroyedByEffect : Unit
    {
        public CannotBeDestroyedByEffect()
        {
            Title = "Debug.CannotBeDestroyedByEffect";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugCannotBeDestoyedByEffect;
        }
    }
}