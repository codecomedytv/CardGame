namespace CardGame.Server.Game.Events
{
    public class ModifyPlayer: Event
    {
        public readonly Player Player;
        public readonly ISource Source;
        public readonly string Property;
        public readonly object Old;
        public readonly object New;
        
        public ModifyPlayer(GameEvents identity, ISource source, Player player, string property, object oldValue, object newValue)
        {
            Identity = identity;
            Source = source;
            Player = player;
            Property = property;
            Old = oldValue;
            New = newValue;
        }
        
    }
}