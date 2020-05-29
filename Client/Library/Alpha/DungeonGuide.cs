using CardGame.Client.Library.Card;

namespace CardGame.Client.Library.Alpha
{
    public class DungeonGuide: Unit
    {
        public DungeonGuide()
        {
            Title = "Dungeon Guide";
            SetCode = SetCodes.Alpha_DungeonGuide;
            Attack = 2000;
            Defense = 1500;
            Illustration = "res://Assets/CardArt/Adventurer.png";
        }
    }
}