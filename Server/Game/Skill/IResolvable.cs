namespace CardGame.Server
{
    public interface IResolvable
    {
        void Resolve(string gameEvent = "");
    }
}