using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class CannotBeDestroyedByEffect : Unit
    {
        public CannotBeDestroyedByEffect(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.CannotBeDestroyedByEffect";
            Attack = 1000;
            Defense = 1000;
            SetCode = SetCodes.DebugCannotBeDestoyedByEffect;
        }
    }
}