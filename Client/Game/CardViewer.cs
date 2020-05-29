using System;
using CardGame.Client.Library.Card;
using Godot;

namespace CardGameSharp.Client.Game
{
    public class CardViewer: TextureRect
    {
        public Label Title;
        public TextureRect Art;
        public Label Effect;
        public Label Battle;

        public override void _Ready()
        {
            Title = GetNode("Title") as Label;
            Art = GetNode("Illustration") as TextureRect;
            Effect = GetNode("Text/Effect") as Label;
            Battle = GetNode("Text/Unit") as Label;
        }

        public override void _Process(float delta)
        {
            if (Input.IsActionPressed("view_card") || Input.IsActionPressed("drag_drop"))
            {
                ViewCard();
            }
        }

        public void ViewCard()
        {
            foreach (Card card in GetTree().GetNodesInGroup("cards"))
            {
                if(card.GetGlobalRect().HasPoint(GetGlobalMousePosition()))
                {
                    Title.Text = card.Title;
                    
                }
                if(card.Illustration != "")
                {
                    Art.Texture = ResourceLoader.Load(card.Illustration) as Texture;
                }

                if (card.CardType == CardTypes.Unit)
                {
                    Battle.Text = String.Format("[ {0} / ATK {1} / DEF {2} ]", "Warrior", card.Attack, card.Defense);
                }
                else
                {
                    Battle.Text = "";
                }

                Effect.Text = card.Effect;
            }
        }
    }
}
