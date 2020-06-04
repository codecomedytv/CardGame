using System.Collections.Generic;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class Idle: State
    {
        
        public override State OnDeploy(Unit unit)
        {
            Player.Deploy(unit);
            Player.Link.Register(unit);
            Player.Link.ApplyConstants("deploy");
            Player.Link.ApplyTriggered("deploy");
            Player.Opponent.SetActivatables("deploy");
            Player.Link.Broadcast("deploy", new List<Godot.Object>{unit});
            return new Acting();
        }

        public override State OnAttack()
        {
            return new Acting();
        }

        public override State OnActivation(Support card, int skillIndex, Array<int> targets)
        {
            Player.Link.Activate(Player, card, skillIndex, targets);
            return new Acting();
        }

        public override State OnPassPlay()
        {
            return new Disqualified();
        }

        public override State OnEndTurn()
        {
            Player.EndTurn();
            Player.IsTurnPlayer = false;
            Player.Opponent.IsTurnPlayer = true;
            foreach (var card in Player.Opponent.Field)
            {
                // This should be an entry action of the Idle State.
                Player.Opponent.ReadyCard(card);
            }

            foreach (var card in Player.Support)
            {
                // This should be an entry action of the Idle State(???)
                Player.ReadyCard(card);
            }
            Player.Link.ApplyConstants();
            return new Passive();
        }
        
        public override string ToString()
        {
            return "Idle";
        }
    }
}