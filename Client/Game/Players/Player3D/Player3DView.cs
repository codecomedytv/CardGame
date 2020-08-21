using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game.Players.Player3D
{
    public class Player3DView: Spatial, IPlayerView
    {
        private Spatial _units;
        private Spatial _support;
        private Spatial _hand;
        private Spatial _discard;
        private Spatial _deck;
        private Tween _gfx;
        private AudioStreamPlayer _sfx;

        public override void _Ready()
        {
            _units = (Spatial) GetNode("Units");
            _support = (Spatial) GetNode("Support");
            _hand = (Spatial) GetNode("Hand");
            _discard = (Spatial) GetNode("Discard");
            _deck = (Spatial) GetNode("Deck");
            _gfx = (Tween) GetNode("GFX");
            _sfx = (AudioStreamPlayer) GetNode("SFX");
        }
        
        public void DisplayName(string name)
        {
            throw new System.NotImplementedException();
        }

        public void DisplayHealth(int health)
        {
            throw new System.NotImplementedException();
        }

        public void Draw(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Discard(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Deploy(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Set(ICardView card)
        {
            throw new System.NotImplementedException();
        }

        public void Attack(ICardView attacker, ICardView defender)
        {
            throw new System.NotImplementedException();
        }

        public void AttackDirectly(ICardView attacker)
        {
            throw new System.NotImplementedException();
        }
    }
}