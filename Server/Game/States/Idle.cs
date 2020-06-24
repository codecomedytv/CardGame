using System.Collections.Generic;
using CardGame.Server.Game;
using CardGame.Server.Game.Cards;
using CardGame.Server.Game.Commands;
using Godot;

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
            Link.Register(unit);
            Player.Match.History.Add(new Move(GameEvents.Deploy, Player, unit, Player.Field));
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());
            return Ok;
        }

        public override bool OnAttack(Unit attacker, Unit defender)
        {
            if (!attacker.CanAttack || !Player.Opponent.Field.Contains(defender) || !attacker.ValidAttackTargets.Contains(defender))
            {
                return DisqualifyPlayer;
            }
            
            Battle.Begin(Player, attacker, defender);
            Link.AddResolvable(Battle);
            Player.Match.History.Add(new DeclareAttack(attacker, defender));
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
            Player.Match.History.Add(new DeclareDirectAttack(attacker));
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
            //Player.Match.History.Add(new Move(GameEvents.SetFaceDown, Player, support, Player.Support));
            GD.Print(support.IsReady);
            // This is retriggering cards getting ready!
            Player.SetState(new Idle());
            GD.Print(support.IsReady);
            return Ok;
        }

        public override bool OnActivation(Support card, Card target)
        {
            GD.Print($"Activating Ready Card? {card.IsReady}");
            if (!card.CanBeActivated)
            {
                return DisqualifyPlayer;
            }
            GD.Print(card.IsReady);
            Link.Activate(card.Skill, target);
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());

            return Ok;
        }
        

        public override bool OnEndTurn()
        {
            Player.Match.History.Add(new EndTurn(Player));
            Player.IsTurnPlayer = false;
            Player.Opponent.IsTurnPlayer = true;
            Link.ApplyConstants();
            foreach (var unit in Player.Opponent.Field) { unit.Ready(); };
            foreach (var support in Player.Support) { support.Ready(); }
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