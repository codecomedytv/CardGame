using System.IO.Ports;
using CardGame.Client.Library;
using CardGame.Client.Library.Cards;

namespace CardGame.Client.Player
{
    public class Controller
    {
        public readonly Model Model;
        public readonly View View;

        public Controller(View view)
        {
            Model = new Model();
            View = view;
        }

        public void Execute() => View.Execute();

        public void Draw(Card card)
        {
            Model.DeckCount -= 1;
            Model.Hand.Add(card);
            View.Draw(card, Model.DeckCount.ToString());
        }

        public void Deploy(Card card)
        {
            Model.Hand.Remove(card);
            Model.Units.Add(card);
            View.Deploy(card);
        }
    }
    
    

    
}