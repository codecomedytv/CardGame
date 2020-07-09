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
        private static Card Viewing;

        public static void View(Card card)
        {
            Viewing = card;
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
            if (Viewing == null || Viewing.CardType == CardTypes.Null)
            {
                return;
            }
            Title.Text = Viewing.Title;
            Art.Texture = ResourceLoader.Load($"res://Assets/CardArt/{Viewing.Art}.png") as Texture;
            Battle.Text = Viewing.CardType == CardTypes.Unit ? $"Warrior / ATK {Viewing.Attack} / DEF {Viewing.Defense}" : "";
            Effect.Text = Viewing.Effect;
        }

        // public void OnCardClicked(Card card)
        // {
        //     if (card.CardType == CardTypes.Null)
        //     {
        //         return;
        //     }
        //     Title.Text = card.Title;
        //     Art.Texture = ResourceLoader.Load($"res://Assets/CardArt/{card.Art}.png") as Texture;
        //     Battle.Text = card.CardType == CardTypes.Unit ? $"Warrior / ATK {card.Attack} / DEF {card.Defense}" : "";
        //     Effect.Text = card.Effect;
        // }
    }
}
