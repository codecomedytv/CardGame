using Godot;

namespace CardGame.Client.Room.View
{
    public class Player: Control
    {
        public Label Damage { get; private set; }
        public Container Deck { get; private set; }
        public Container Discard { get; private set; }
        public Container Hand { get; private set; }
        public Container Units { get; private set; }
        public Container Support { get; private set; }
        public AnimatedSprite PlayingState { get; private set; }

        public override void _Ready()
        {
            Damage = GetNode<Label>("Damage");
            Deck = GetNode<Container>("Deck");
            Discard = GetNode<Container>("Discard");
            Hand = GetNode<Container>("Hand");
            Units = GetNode<Container>("Units");
            Support = GetNode<Container>("Support");
            PlayingState = GetNode<AnimatedSprite>("View/PlayingState");

        }
    }
}