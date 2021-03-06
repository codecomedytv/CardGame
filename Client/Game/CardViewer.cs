using CardGame.Client.Game.Cards;
using Godot;

namespace CardGame.Client.Game
{
    public class CardViewer : Control
    {
        private TextureRect Illustration;
        private Label Title;
        private Label CardTypes;
        private Label Effect;
        private Label Power;

        public void OnCardCreated(Card card)
        {
            card.MouseOvered += OnCardFocused;
        }
        public override void _Ready()
        {
            Title = GetNode<Label>("Title");
            Illustration = GetNode<TextureRect>("Face");
            CardTypes = GetNode<Label>("Types");
            Effect = GetNode<Label>("Effect");
            Power = GetNode<Label>("Power");
        }
        
        public void OnCardFocused(Card card)
        {
            // Null Check Too? Or should just leave as last card focused?
            Title.Text = card.Title;
            Illustration.Texture = card.Art;
            // CardTypes.Text = card.Faction; (This doesn't exist right now)
            Effect.Text = card.Effect;
            if (card.CardType == CardType.Unit)
            {
                Power.Text = card.Power.ToString();
                Power.Visible = true;
            }
            else
            {
                Power.Visible = false;
            }

            Title.Visible = true;
            Title.Uppercase = true;
        }


    }
}
