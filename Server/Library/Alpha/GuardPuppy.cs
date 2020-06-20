using CardGame.Server.Game;
using CardGame.Server.Game.Cards;

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