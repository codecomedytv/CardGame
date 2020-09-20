using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
using Godot;

namespace CardGame.Client.Game
{
    public class Deploy: Command
    {
        private readonly Card Card;
        
        public Deploy(Card card)
        {
            Card = card;
        }
        protected override void SetUp(Effects gfx)
        {
            if (Card.Controller is Opponent)
            {
                var fakeCard = Card.Controller.HandModel.First();
                Card.Controller.HandModel.Remove(fakeCard);
                Card.Controller.HandModel.Add(Card);
                fakeCard.Free();
            }

            var origin = Card.Translation;
            //var destination = Card.Controller.UnitsModel.NextSlot() + new Vector3(0, 0, 0.05F);

            Card.Controller.HandModel.Remove(Card);
            Card.Controller.UnitsModel.Add(Card);
            var destination = Card.Translation + new Vector3(0, 0, 0.05F);
            Card.Translation = origin;

            gfx.Play(Audio.Deploy);
            gfx.InterpolateProperty(Card, nameof(Card.Translation), origin, destination, 0.3F);
            gfx.InterpolateProperty(Card, nameof(Card.RotationDegrees), new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
            gfx.InterpolateCallback(new Sorter(Card.Controller.HandModel), 0.2F, nameof(Sorter.Sort));
        }
        
    }
}
