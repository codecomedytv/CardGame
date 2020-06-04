using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class Idle: State
    {
        public override void OnEnter(Player player)
        {
            Player = player;
            // We're only doing this for cards in hand but it might worth iterating through all cards?
            // Otherwise users may be able to deploy cards illegal from graveyard or deck etc
            Player.Hand.ForEach(card => card.SetCanBeDeployed());
            Player.Hand.ForEach(card => card.SetCanBeSet());
            Player.Field.ForEach(card => card.SetCanAttack());
            Player.Support.ForEach(card => card.SetCanBeActivated());
        }

        public override State OnDeploy(Unit unit)
        {
            if (!unit.CanBeDeployed)
            {
                Player.Disqualify();
                return new Disqualified();
            }
            Player.Deploy(unit);
            Player.Link.Register(unit);
            Player.Link.Broadcast("deploy", new List<Godot.Object>{unit});
            return new Acting();
        }

        public override State OnAttack(Unit attacker, object defender, bool isDirectAttack)
        {
            if (!attacker.CanAttack)
            {
                GD.Print(1);
                Player.Disqualify();
                return new Disqualified();
            }

            if (isDirectAttack && Player.Opponent.Field.Count != 0)
            {
                Player.Disqualify();
                return new Disqualified();
            }

            else if (!isDirectAttack && !Player.Opponent.Field.Contains((Card)defender))
            {
                Player.Disqualify();
                return new Disqualified();
            }
            
            else if (defender is Card defendingUnit && !attacker.ValidAttackTargets.Contains(defendingUnit))
            {
                Player.Disqualify();
                return new Disqualified();
            }

            if(!isDirectAttack){ Player.Opponent.ShowAttack(Player, attacker, (Unit)defender); }
            Player.Battle.Begin(Player, attacker, defender, isDirectAttack);
            Player.Link.AddResolvable(Player.Battle);
            Player.Link.Broadcast("attack", new List<Object>());
            return new Acting();
        }

        public override State OnSetFaceDown(Support support)
        {
            if (!support.CanBeSet)
            {
                Player.Disqualify();
                return new Disqualified();
            }
            
            Player.Hand.Remove(support);
            Player.Support.Add(support);
            support.Zone = Player.Support;
            support.EmitSignal(nameof(Card.Exit));
            Player.Link.ApplyConstants();
            Player.Link.Register(support);
            Player.DeclarePlay(new SetSupport(support));

            // Returning a new Idle State Retriggers the OnEnter System
            return new Idle();
        }

        public override State OnActivation(Support card, Array<int> targets)
        {
            if (!card.CanBeActivated)
            {
                Player.Disqualify();
                return new Disqualified();
            }
            Player.Link.Activate(Player, card, targets);
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
            Player.Opponent.Field.ForEach(unit => Player.Opponent.ReadyCard(unit));
            Player.Support.ForEach(support => Player.ReadyCard(support));
            Player.Link.ApplyConstants();
            return new Passive();
        }
        
        public override string ToString()
        {
            return "Idle";
        }
    }
}