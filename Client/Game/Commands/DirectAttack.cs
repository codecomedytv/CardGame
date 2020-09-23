using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class DirectAttack: Command
    {
        private readonly Player Player;
        private readonly Card Attacker;

        public DirectAttack(Player player, Card attacker)
        {
            Player = player;
            Attacker = attacker;
        }
        protected override void SetUp(Effects gfx)
        {
            var destination = Attacker.Controller.IsUser? new Vector3(2.5F, 9F, Attacker.Translation.z): new Vector3(2.5F, -2.95F, 1);

            gfx.InterpolateProperty(Attacker, nameof(Translation), Attacker.Translation, destination, 0.1F);
            gfx.InterpolateProperty(Attacker, nameof(Translation), destination, Attacker.Translation, 0.1F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            gfx.InterpolateCallback(Attacker, 0.2F, nameof(Card.StopAttack));
            gfx.InterpolateCallback(Player.View, 0.2F, nameof(PlayerView.StopDefending));
        }
    }
}