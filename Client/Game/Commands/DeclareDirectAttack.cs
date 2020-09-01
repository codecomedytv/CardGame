using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class DeclareDirectAttack: Command
    {
        private readonly Player Player;
        private readonly Card Attacker;

        public DeclareDirectAttack(Player player, Card attacker)
        {
            Player = player;
            Attacker = attacker;
        }
        protected override void SetUp(Tween gfx)
        {
            gfx.InterpolateCallback(Attacker, 0.1F, nameof(Card.Attack));
            gfx.InterpolateCallback(Player, 0.1F, nameof(Player.Defend));
        }
    }
}