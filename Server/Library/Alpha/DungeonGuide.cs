using CardGame.Server.Room.Cards;

namespace CardGame.Server
{
    public class DungeonGuide: Unit
    {
        public DungeonGuide()
        {
            Title = "Dungeon Guide";
            SetCode = SetCodes.Alpha_DungeonGuide;
            Attack = 1000;
            Defense = 1500;
        }
    }
}