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
            var decorator = new Decorator(Tag.CannotBeDestroyedByEffect);
            decorator.AddTagTo(this);
        }
    }
}