using System.Linq;
using CardGame.Client.Assets.Audio;
using CardGame.Client.Game.Players;
using CardGame.Client.Game.Zones;
using CardGame.Client.Game.Zones.ModelControllers;
using Godot;

namespace CardGame.Client.Game
{
    public class OpponentSetFaceDown: Command
    {
        private readonly Opponent Opponent;

        public OpponentSetFaceDown(Opponent opponent)
        {
            Opponent = opponent;
        }
        protected override void SetUp(Effects gfx)
        {
            var card = Opponent.HandModel.First();
            var origin = card.Translation;
            var destination = Opponent.Support.NextSlot() + new Vector3(0, 0, 0.05F);

            Opponent.HandModel.Remove(card);
            Opponent.Support.Add(card);

            gfx.Play(Audio.SetCard);
            gfx.InterpolateProperty(card, nameof(card.Translation), origin, destination, 0.3F);
            gfx.InterpolateProperty(card, nameof(card.RotationDegrees), new Vector3(-25, 0, 0), new Vector3(0, 0, 0),
                0.1F);
            gfx.InterpolateCallback(new HelperNode(card.Controller.HandModel), 0.2F, nameof(HelperNode.Sort));
        }

        public class HelperNode: Node
        {
            private IZoneModelController Zone;
            public HelperNode(IZoneModelController zone)
            {
                Zone = zone;
            }

            public void Sort()
            {
                Zone.Sort();
                QueueFree();
            }
        }
    }
}