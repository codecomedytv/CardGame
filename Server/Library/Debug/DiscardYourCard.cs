namespace CardGame.Server
{
    public class DiscardYourCard : Support
    {
        public DiscardYourCard()
        {
            Title = "Debug.DiscardYourCard";
            SetCode = SetCodes.DebugDiscardYourCard;
            AddSkill(new DiscardCard());
        }

        public class DiscardCard : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Controller.Hand);
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.Discard(GameState.Target);
            }
        }
    }
}