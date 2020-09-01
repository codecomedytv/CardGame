using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class DirectAttack: xCommand
    {
        private readonly IPlayer Player;
        private readonly Card Attacker;

        public DirectAttack(IPlayer player, Card attacker)
        {
            Player = player;
            Attacker = attacker;
        }
        protected override void SetUp(Tween gfx)
        {
            var destination = Attacker.Controller is Opponent? new Vector3(2.5F, -2.95F, 1) :  new Vector3(2.5F, 9F, Attacker.Translation.z);

            gfx.InterpolateProperty(Attacker, nameof(Translation), Attacker.Translation, destination, 0.1F);
            gfx.InterpolateProperty(Attacker, nameof(Translation), destination, Attacker.Translation, 0.1F,
                Tween.TransitionType.Linear, Tween.EaseType.In, 0.3F);
            gfx.InterpolateCallback(Attacker.AttackingIcon, 0.2F, "set_visible", false);
            gfx.InterpolateCallback((Object) Player, 0.3F, nameof(Player.StopDefending));
        }
    }
}