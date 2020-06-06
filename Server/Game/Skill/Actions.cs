using System.Collections.Generic;
using Godot.Collections;

namespace CardGame.Server
{
    public partial class Skill
    {
        protected void AddTagToController(Tag tag)
        {
            var decorator = new Decorator(tag);
            decorator.AddTagToPlayer(Controller);
        }

        protected void AddTagToGroup(List<Card> cards, Tag tag, string destroyCondition, string taggedExit, bool includeCard = true)
        {
            var decorator = new Decorator(tag);
            if (destroyCondition != "")
            {
                Card.Connect(destroyCondition, decorator, nameof(decorator.OnZoneExit),
                    new Array {Card.Zone, Card, "destroy"});
            }

            foreach (var card in cards)
            {
                if (Card == card && !includeCard)
                {
                    // I have no idea what this is doing?
                    // Excluding card from its own tags
                    continue;
                }
                decorator.AddTagTo(card);
                if (taggedExit != "")
                {
                    card.Connect(taggedExit, decorator,
                        nameof(decorator.OnZoneExit), new Array {card.Zone, card, "untag"});
                }
            }
        }

        protected void SetTargets(List<Card> cards)
        {
            Controller.DeclarePlay(new SetTargets(Card, cards));
        }

        protected void AutoTarget()
        {
            GameState.Paused = true;
            Controller.DeclarePlay(new AutoTarget(Card));
        }
    }
}