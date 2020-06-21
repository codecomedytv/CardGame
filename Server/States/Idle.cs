using System.Collections.Generic;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
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

        public override bool OnDeploy(Unit unit)
        {
            if (!unit.CanBeDeployed)
            {
                return DisqualifyPlayer;
            }
            Player.DeclarePlay(new Move(Player, Player, unit, Player.Field));
            Player.Link.Register(unit);
            Player.Link.Broadcast("deploy", new List<Godot.Object>{unit});
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());
            return Ok;
        }

        public override bool OnAttack(Unit attacker, object defender, bool isDirectAttack)
        {
            if (!attacker.CanAttack)
            {
                return DisqualifyPlayer;
            }

            if (isDirectAttack && Player.Opponent.Field.Count != 0)
            {
                return DisqualifyPlayer;
            }

            if (!isDirectAttack && !Player.Opponent.Field.Contains((Unit)defender))
            {
                return DisqualifyPlayer;
            }
            
            if (defender is Card defendingUnit && !attacker.ValidAttackTargets.Contains(defendingUnit))
            {
                return DisqualifyPlayer;
            }

            Player.Battle.Begin(Player, attacker, defender, isDirectAttack);
            Player.Link.AddResolvable(Player.Battle);
            Player.Link.Broadcast("attack", new List<Object>());
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());
            return Ok;
        }

        public override bool OnSetFaceDown(Support support)
        {
            if (!support.CanBeSet)
            {
                return DisqualifyPlayer;
            }
            
            Player.Hand.Remove(support);
            Player.Support.Add(support);
            support.Zone = Player.Support;
            Player.Link.ApplyConstants();
            Player.Link.Register(support);
            Player.DeclarePlay(new Move(Player, Player, support, Player.Support));

            // Returning a new Idle State Retriggers the OnEnter System
            Player.SetState(new Idle());

            return Ok;
        }

        public override bool OnActivation(Support card, Card target)
        {
            if (!card.CanBeActivated)
            {
                return DisqualifyPlayer;
            }
            Player.Link.Activate(card.Skill, target);
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());

            return Ok;
        }
        

        public override bool OnEndTurn()
        {
            Player.EndTurn();
            Player.IsTurnPlayer = false;
            Player.Opponent.IsTurnPlayer = true;
            Player.Opponent.Field.ForEach(unit => Player.Opponent.DeclarePlay(new Modify(Player.Opponent, unit, nameof(Card.Ready), true)));
            Player.Support.ForEach(support => Player.DeclarePlay(new Modify(Player, support, nameof(Card.Ready), true)));
            Player.Link.ApplyConstants();
            Player.SetState(new Passive());
            Player.Opponent.SetState(new Idle());
            return Ok;
        }
        
        public override string ToString()
        {
            return "Idle";
        }
    }
}