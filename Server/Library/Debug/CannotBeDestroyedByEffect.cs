using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Tags;

namespace CardGame.Server
{
    public class CannotBeDestroyedByEffect : Unit
    {
        public CannotBeDestroyedByEffect(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.CannotBeDestroyedByEffect";
            Power = 1000;
            SetCode = SetCodes.DebugCannotBeDestoyedByEffect;
            Tags.Add(new Tag(TagIds.CannotBeDestroyedByEffect));
        }
    }
}