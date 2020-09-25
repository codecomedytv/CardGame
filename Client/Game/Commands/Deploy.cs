using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Cards;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
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
            if (!Card.Controller.IsUser)
            {
                var fakeCard = Card.Controller.Hand.First();
                Card.Controller.Hand.Remove(fakeCard);
                Card.Controller.Hand.Add(Card);
                fakeCard.Free();
            }

            var origin = Card.Translation;
            //var destination = Card.Controller.UnitsModel.NextSlot() + new Vector3(0, 0, 0.05F);

            Card.Controller.Hand.Remove(Card);
            Card.Controller.Units.Add(Card);
            var destination = Card.Controller.View.Units.GetNode<Sprite3D>($"CardSlot{Card.ZoneIndex-1}").GlobalTransform.origin + new Vector3(0, 0, 0.05F);
            Card.Translation = origin;

            gfx.Play(Audio.Deploy);
            gfx.InterpolateProperty(Card, "translation", origin, destination, 0.3F);
            gfx.InterpolateProperty(Card, "rotation_degrees", new Vector3(-25, 180, 0), new Vector3(0, 180, 0), 0.1F);
            gfx.InterpolateCallback(new Sorter(Card.Controller.Hand), 0.2F, nameof(Sorter.Sort));
        }
        
    }
}
