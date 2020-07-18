using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Tags;

namespace CardGame.Server
{
    public class Untargetable: Unit
    {
        public Untargetable(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Debug.Untargetable";
            Power = 1000;
            SetCode = SetCodes.DebugCannotBeTargeted;
            Tags.Add(new Tag(TagIds.CannotBeTargeted));
        }
    }
}
