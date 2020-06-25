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
        

        public override bool OnActivation(Support card, Card target)
        {
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
        
        
        public override string ToString()
        {
            return "Idle";
        }
    }
}