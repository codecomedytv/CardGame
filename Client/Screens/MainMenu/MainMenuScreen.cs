using System;
using Godot;

namespace CardGame.Client.Screens
{
    public class MainMenuScreen: IScreen<MainMenuView>
    {
        public event Action FindMatchPressed;
        public event Action DeckEditorPressed;
        public event Action UserProfilePressed;
        public event Action QuitPressed;

        public MainMenuView View { get; }

        public MainMenuScreen()
        {
            View = new MainMenuView {Visible = false};
        }
        
        public void Display()
        {
            View.Visible = true;
        }

        public void StopDisplaying()
        {
            View.Visible = true;
        }

        private void OnFindMatchPressed()
        {
            FindMatchPressed?.Invoke();
        }

        private void OnDeckEditorPressed()
        {
            DeckEditorPressed?.Invoke();
        }

        private void OnUserProfilePressed()
        {
            UserProfilePressed?.Invoke();
        }

        private void OnQuitPressed()
        {
            QuitPressed?.Invoke();
        }
    }
}