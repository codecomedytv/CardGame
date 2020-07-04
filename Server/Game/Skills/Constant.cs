namespace CardGame.Server.Game.Skills
{
    public class Constant: Skill
    {
        public void Apply()
        {
            if (!AreaOfEffects.Contains(Card.Zone))
            {
                return;
            }
            
            _Apply();
        }

        protected virtual void _Apply()
        {
            
        }

    }
}