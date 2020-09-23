using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Cards
{
    public class CardState
    {
        private readonly Card Card;
        public CardStates State;

        public CardState(Card card)
        {
            Card = card;
        }

        public bool IsInActive = false;

        public bool CanBeDeployed()
        {
            return State == CardStates.CanBeDeployed && Card.Controller.PlayerState.State == States.Idle;
        }

        public bool CanBeSet()
        {
            return State == CardStates.CanBeSet && Card.Controller.PlayerState.State == States.Idle;
        }

        public bool CanBeActivated()
        {
            return State == CardStates.CanBeActivated && !Card.Controller.PlayerState.IsInActive;
        }

        public bool CanAttack()
        {
            return State == CardStates.CanAttack && Card.ValidAttackTargets.Count > 0 &&
                   Card.Controller.PlayerState.State == States.Idle;
        }

        public bool CanAttackDirectly()
        {
            return State == CardStates.CanAttackDirectly && Card.Controller.PlayerState.State == States.Idle;
        }

        public bool CanBePlayed()
        {
            return CanBeSet() || CanBeActivated() || CanBeDeployed();
        }
    }
}