using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using Godot;

namespace CardGame.Client.Game.Commands
{
    public class OpponentLoadDeck: Command
    {
        private readonly Opponent Opponent;
        private readonly CardFactory CardFactory;

        public OpponentLoadDeck(Opponent opponent, CardFactory cardFactory)
        {
            Opponent = opponent;
            CardFactory = cardFactory;
            
            // We don't technically need to execute this immediately but we do it for the sake of consistency..
            // ..with the player load deck command
            _LoadDeck();
        }

        protected override void SetUp(Effects gfx) { }

        private void _LoadDeck()
        {
            for (var i = 0; i < 40; i++)
            {
                var card = CardFactory.Create(0, SetCodes.NullCard);
                card.OwningPlayer = Opponent;
                card.Controller = Opponent;
                Opponent.Deck.Add(card);
                card.Translation = Opponent.View.Deck.GlobalTransform.origin;
                card.Translation = new Vector3(card.Translation.x, card.Translation.y, card.ZoneIndex * 0.01F);
            }
        }
    }
}