using System.Collections.Generic;
using System.Collections.ObjectModel;
using CardGame.Server.Game.Cards;

namespace CardGame.Server.Game.Commands
{
    public class LoadDeck : Command
    {
        public readonly ISource Source;
        public readonly ReadOnlyCollection<Card> CardsLoaded;

        public LoadDeck()
        {
            
        }
        public LoadDeck(ISource source, ReadOnlyCollection<Card> cardsLoaded)
        {
            Source = source;
            CardsLoaded = cardsLoaded;
        }

        public override void Execute()
        {
        }

        public override void Undo()
        {
        }
    }
}
