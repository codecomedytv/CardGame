using CardGame.Server.Game.Cards;

namespace CardGame.Server
{
    public class DungeonGuide: Unit
    {
        public DungeonGuide(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Dungeon Guide";
            SetCode = SetCodes.Alpha_DungeonGuide;
            Attack = 1000;
            Defense = 1500;
        }
    }
}