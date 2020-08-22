using CardGame.Client.Game.Cards;
using CardGame.Client.Library;

namespace CardGame.Tests.Scripts.ClientSide
{
    public class CardFactoryTest: WAT.Test
    {
        public override string Title()
        {
            return "Card Factory Test";
        }

        [Test]
        public void Create3DCard()
        {
             var factory = new CardFactory();
             var card = factory.Create(0, SetCodes.AlphaCounterAttack);
             Assert.IsEqual(nameof(Card), "Card", "Created Card Successfully");
        }

        [Test]
        public void FetchCardInfo()
        {
            var cardInfo = CardLibrary.Cards[SetCodes.AlphaCounterAttack];
            Assert.IsEqual(cardInfo.Title, "Counter Attack", "Fetched Card Info Successfully");
        }
    }
}