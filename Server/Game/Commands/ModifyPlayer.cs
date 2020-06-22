using System.Collections;

namespace CardGame.Server.Room.Commands
{
    public class ModifyPlayer: GameEvent, ICommand
    {
        public readonly Player Player;
        public readonly ISource Source;
        public readonly string Property;
        public readonly object Old;
        public readonly object New;
        
        public ModifyPlayer(ISource source, Player player, string property, object newValue)
        {
            Source = source;
            Player = player;
            Property = property;
            Old = player.Get(property);
            New = newValue;
        }

        public void Execute() => Player.Set(Property, New);

        public void Undo() => Player.Set(Property, Old);
    }
}