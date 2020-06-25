using CardGame.Client.Match;
using CardGame.Server.Game.Cards;
using Godot.Collections;

namespace CardGame.Server.States
{
    public class Active: State
    {
        public override void OnEnter(Player player)
        {
            Player = player;
            foreach(var card in Player.Support) {card.SetCanBeActivated();}
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

        public override bool OnPassPlay()
        {
            return Ok;
        }

        public override string ToString()
        {
            return "Active";
        }
    }
}
