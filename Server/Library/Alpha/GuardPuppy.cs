using CardGame.Server.Room;
using CardGame.Server.Room.Cards;

namespace CardGame.Server
{
    public class GuardPuppy: Unit
    {
        public GuardPuppy()
        {
            Title = "Guard Puppy";
            SetCode = SetCodes.Alpha_GuardPuppy;
            Attack = 500;
            Defense = 500;
            // TODO: Re-implement Tag Decorators
        }
    }
}