namespace CardGame.Server
{
    public class MillOneFromDeck : Support
    {
        public MillOneFromDeck()
        {
            Title = "Debug.MillOneFromCard";
            SetCode = SetCodes.MillOneFromDeck;
            AddSkill(new MillCard());
        }
        
        public class  MillCard: Skill
        {
            protected override void _Resolve()
            {
                Controller.MillFromDeck();
            }
        }
    }
}