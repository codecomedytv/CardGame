using CardGame.Client.Match;
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

        public override bool OnActivation(Support card, Array<int> targets)
        {
            if (!card.CanBeActivated)
            {
                return DisqualifyPlayer;
            }
            Player.Link.Activate(Player, card, targets);
            Player.SetState(new Acting());
            Player.Opponent.SetState(new Active());

            return Ok;
        }

        public override bool OnPassPlay()
        {
            if (Player.Opponent.State.GetType() == typeof(Passing))
            {
                Player.Link.Resolve();
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
