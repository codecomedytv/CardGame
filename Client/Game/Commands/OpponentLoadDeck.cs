using System;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentLoadDeck: Command
    {
        private Opponent Opponent;
        private CardFactory CardFactory;
        private Action<Card> AddCardToTree;

        public OpponentLoadDeck(Opponent opponent, CardFactory cardFactory, Action<Card> addCardToTree)
        {
            Opponent = opponent;
            CardFactory = cardFactory;
            AddCardToTree = addCardToTree;
        }
        
        protected override void SetUp(Tween gfx)
        {
            for (var i = 0; i < 40; i++)
            {
                    var card = CardFactory.Create(0, SetCodes.NullCard);
                    AddCardToTree(card); 
                    card.OwningPlayer = Opponent;
                    card.Controller = Opponent;
                    Opponent.Deck.Add(card);
            }
        }
    }
}