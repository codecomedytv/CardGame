using Godot;
using Godot.Collections;

namespace CardGame.Client.DeckEditor
{
    // ReSharper disable once ClassNeverInstantiated.Global (Instanced As A Scene By Godot)
    public class Editor: Screen
    {
        [Signal]
        public delegate void BackPressed();

        public override void _Ready()
        {
            GetNode<Button>("Back").Connect("pressed", this, nameof(OnBackPressed));
        }

        private void OnBackPressed() => EmitSignal(nameof(BackPressed));
    }
}