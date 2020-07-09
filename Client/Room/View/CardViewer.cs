using CardGame.Client.Cards;
using Godot;

namespace CardGame.Client.Room.View
{
    public class CardViewer: TextureRect
    {
        private Label Title;
        private TextureRect Art;
        private Label Effect;
        private Label Battle;
        private static Card _viewing;

        public static void View(Card card)
        {
            _viewing = card;
        }
        public override void _Ready()
        {
            Title = GetNode("Title") as Label;
            Art = GetNode("Illustration") as TextureRect;
            Effect = GetNode("Text/Effect") as Label;
            Battle = GetNode("Text/Unit") as Label;
        }

        public override void _Process(float delta)
        {
            if (_viewing == null || _viewing.CardType == CardTypes.Null)
            {
                return;
            }
            Title.Text = _viewing.Title;
            Art.Texture = ResourceLoader.Load($"res://Assets/CardArt/{_viewing.Art}.png") as Texture;
            Battle.Text = _viewing.CardType == CardTypes.Unit ? $"Warrior / ATK {_viewing.Attack} / DEF {_viewing.Defense}" : "";
            Effect.Text = _viewing.Effect;
        }
    }
}
