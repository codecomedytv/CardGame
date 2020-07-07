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

        public override void _Ready()
        {
            Title = GetNode("Title") as Label;
            Art = GetNode("Illustration") as TextureRect;
            Effect = GetNode("Text/Effect") as Label;
            Battle = GetNode("Text/Unit") as Label;
        }
        
        public void OnCardClicked(Card card)
        {
            if (card.CardType == CardTypes.Null)
            {
                return;
            }
            Title.Text = card.Title;
            Art.Texture = ResourceLoader.Load(card.Art) as Texture;
            Battle.Text = card.CardType == CardTypes.Unit ? $"Warrior / ATK {card.Attack} / DEF {card.Defense}" : "";
            Effect.Text = card.Effect;
        }
    }
}
