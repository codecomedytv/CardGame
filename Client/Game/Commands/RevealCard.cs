using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;

namespace CardGame.Client.Game.Commands
{
    public class RevealCard: Command
    {
        private readonly Player Opponent;
        private readonly CardFactory CardFactory;
        private readonly int Id;
        private readonly SetCodes SetCode;
        public RevealCard(Player opponent, CardFactory cardFactory, int id, SetCodes setCode)
        {
            Opponent = opponent;
            CardFactory = cardFactory;
            Id = id;
            SetCode = setCode;
            
            // Required to be Immediate so other commands can use it
            _RevealCard();
        }

        protected override void SetUp(Effects gfx) { }

        private void _RevealCard()
        {
            var card = CardFactory.Create(Id, SetCode);
            card.OwningPlayer = Opponent; 
            card.Controller = Opponent;
        }
    }
}