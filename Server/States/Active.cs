using CardGameSharp.Client.Game;
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
            return new Passing();
        }

        public override string ToString()
        {
            return "Active";
        }
    }
}
