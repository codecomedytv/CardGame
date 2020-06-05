namespace CardGame.Server
{
    public class ReturnToDeck: Support
    {
        public ReturnToDeck()
        {
            Title = "Debug.ReturnToDeck";
            SetCode = SetCodes.DebugReturnToDeck;
            AddSkill(new ReturnCard());
        }

        public class ReturnCard : Skill
        {
            public override void _SetUp()
            {
                SetTargets(Controller.Hand);
                CanBeUsed = Controller.Hand.Count > 0;
            }

            protected override void _Resolve()
            {
                Controller.ReturnToDeck(GameState.Target);
            }
        }
        
    }
}