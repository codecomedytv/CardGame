using CardGame.Server.Game.Skills;

namespace CardGame.Server.Game.Tags
{
    public enum TagIds
    {
        CannotBeAttacked,
        CannotBeDestroyedByBattle,
        CannotBeDestroyedByEffect,
        CannotTakeBattleDamage,
        CannotTakeEffectDamage,
    }

    public class Tag
    {
        public readonly TagIds TagId;

        public Tag(TagIds tagId)
        {
            TagId = tagId;
        }
    }

}