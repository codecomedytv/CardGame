namespace CardGame.Server.Game.Skill
{
    public interface IResolvable
    {
        void Resolve(string gameEvent = "");
    }
}