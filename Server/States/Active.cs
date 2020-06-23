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
            Player.Support.ForEach(card => card.SetCanBeActivated());
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
            if (Player.Opponent.State.GetType() == typeof(Passing))
            {
                Link.Resolve();
                var turnPlayer = Player.IsTurnPlayer ? Player : Player.Opponent;
                turnPlayer.SetState(new Idle());
                turnPlayer.Opponent.SetState(new Passive());
            }
            else
            {
                Player.Opponent.SetState(new Active());
                Player.SetState(new Passing());
            }

            return Ok;
        }

        public override string ToString()
        {
            return "Active";
        }
    }
}
