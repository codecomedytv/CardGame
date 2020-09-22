using System.Collections.Generic;
using System.Linq;
using CardGame.Client.Game.Cards;
using CardGame.Server.Game;

namespace CardGame.Client.Game.Commands
{
    public class UpdateCard: Command
    {
        private readonly Catalog Cards;
        private readonly Card Card;
        private readonly CardStates State;
        private readonly IEnumerable<int> AttackTargetIds;
        private readonly IEnumerable<int> SkillTargetIds;
        public UpdateCard(Catalog cards, Card card, CardStates state, IEnumerable<int> attackTargets, IEnumerable<int> targets)
        {
            Cards = cards;
            Card = card;
            State = state;
            AttackTargetIds = attackTargets;
            SkillTargetIds = targets;
            
            // Required to be an immediate update for other things to work (I think?)
            _UpdateCard();
        }

        protected override void SetUp(Effects gfx) { }

        private void _UpdateCard()
        {
            // Targets might be better suited if we create client-side skill objects?
            Card.Update(State, 
                SkillTargetIds.Select(targetId => Cards[targetId]).ToList(), 
                AttackTargetIds.Select(targetId => Cards[targetId]).ToList());
        }
    }
}