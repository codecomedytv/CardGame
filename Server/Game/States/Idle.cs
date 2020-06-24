using System.Collections.Generic;
using System.Linq;
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
            foreach(var card in Player.Hand) {card.SetCanBeDeployed();}
            foreach(var card in Player.Hand) {card.SetCanBeSet();}
            foreach(var card in Player.Field) {card.SetCanAttack();}
            foreach(var card in Player.Support) {card.SetCanBeActivated();}
        }

        public override bool OnDeploy(Unit unit)
        {
            if (!unit.CanBeDeployed)
            {
                return DisqualifyPlayer;
            }
            Player.DeclarePlay(new Move(Player, unit, Player.Field));
            Link.Register(unit);
            Link.Broadcast("deploy", new List<Godot.Object>{unit});
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());
            return Ok;
        }

        public override bool OnAttack(Unit attacker, Unit defender)
        {
            if (!attacker.CanAttack)
            {
                return DisqualifyPlayer;
            }

            if (!Player.Opponent.Field.Contains(defender))
            {
                return DisqualifyPlayer;
            }
            
            if (defender is Card defendingUnit && !attacker.ValidAttackTargets.Contains(defendingUnit))
            {
                return DisqualifyPlayer;
            }

            Battle.Begin(Player, attacker, defender);
            Link.AddResolvable(Battle);
            Link.Broadcast("attack", new List<Object>());
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());
            return Ok;
        }

        public override bool OnDirectAttack(Unit attacker)
        {
            if (!attacker.CanAttack || Player.Opponent.Field.Count != 0)
            {
                return DisqualifyPlayer;
            }
            Battle.BeginDirectAttack(Player, attacker);
            Link.AddResolvable(Battle);
            Link.Broadcast("attack", new List<Object>());
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
            Link.ApplyConstants();
            Link.Register(support);
            Player.DeclarePlay(new Move(Player, support, Player.Support));

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
            Link.Activate(card.Skill, target);
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());

            return Ok;
        }
        

        public override bool OnEndTurn()
        {
            Player.DeclarePlay(new EndTurn(Player));
            Player.IsTurnPlayer = false;
            Player.Opponent.IsTurnPlayer = true;
            Link.ApplyConstants();
            foreach(var unit in Player.Opponent.Field) {Player.Opponent.DeclarePlay(new ModifyCard(Player.Opponent, unit, nameof(Card.Ready), true)); };
            foreach (var support in Player.Support) { Player.DeclarePlay(new ModifyCard(Player, support, nameof(Card.Ready), true)); }
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