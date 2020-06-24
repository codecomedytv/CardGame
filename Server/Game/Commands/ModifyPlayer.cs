namespace CardGame.Server.Game.Commands
{
    public class ModifyPlayer: Command
    {
        public readonly GameEvents Identity;
        public readonly Player Player;
        public readonly ISource Source;
        public readonly string Property;
        public readonly object Old;
        public readonly object New;
        
        public ModifyPlayer(GameEvents identity, ISource source, Player player, string property, object newValue)
        {
            Source = source;
            Player = player;
            Property = property;
            Old = player.Get(property);
            New = newValue;
        }

        public override void Execute() => Player.Set(Property, New);

        public override void Undo() => Player.Set(Property, Old);
    }
}