using CardGame.Client.Library.Cards;

namespace CardGame.Client.Library.Alpha
{
    public class DungeonGuide: Unit
    {
        public DungeonGuide()
        {
            Title = "Dungeon Guide";
            SetCode = SetCodes.AlphaDungeonGuide;
            Attack = 2000;
            Defense = 1500;
            Illustration = "res://Assets/CardArt/Adventurer.png";
        }
    }
}