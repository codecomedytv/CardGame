using CardGame.Server.Room;
using CardGame.Server.Room.Cards;

namespace CardGame.Server
{
    public class Untargetable: Unit
    {
        public Untargetable()
        {
            Title = "Debug.Untargetable";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugUntargetableUnit;
        }
    }
}