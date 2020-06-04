using System.Collections.Generic;
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
            support.Legal = false; // Need To Update This
            Player.Link.ApplyConstants();
            Player.Link.Register(support);
            Player.DeclarePlay(new SetSupport(support));

            // Returning a new Idle State Retriggers the OnEnter System
            return new Idle();
        }

        public override State OnActivation(Support card, Array<int> targets)
        {
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