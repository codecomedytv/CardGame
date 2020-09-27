using System;
using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Screens
{
    [UsedImplicitly]
    public sealed class ScreenManager: Node
    {
        private readonly Login Login;
        private readonly MainMenu MainMenu;
        private readonly DeckEditor DeckEditor;
        private readonly MatchFinder MatchFinder;
        private readonly UserProfile UserProfile;
        private IScreen<Node> PreviousScreen;
        private IScreen<Node> CurrentScreen;

        public ScreenManager()
        {
            Login = new Login();
            MainMenu = new MainMenu();
            DeckEditor = new DeckEditor();
            MatchFinder = new MatchFinder();
            UserProfile = new UserProfile();
        }

        public override void _Ready()
        {
            // We can likely afford to keep all screens in the tree at once..
            // ..we would just turn them off/on when necessary
            AddScreen(Login);
            AddScreen(MainMenu);
            AddScreen(DeckEditor);
            AddScreen(MatchFinder);
            AddScreen(UserProfile);
            
            Login.LoggedIn += OnUserLoggedIn;
            MainMenu.FindMatchPressed += OnFindMatchPressed;
            MainMenu.DeckEditorPressed += OnDeckEditorPressed;
            MainMenu.UserProfilePressed += OnUserProfilePressed;
            MainMenu.QuitPressed += OnQuitPressed;
            DeckEditor.BackPressed += OnBackPressed;
             
        }

        private void OnUserLoggedIn()
        {
            PreviousScreen = Login;
            CurrentScreen = MainMenu;
        }

        private void OnFindMatchPressed()
        {
            PreviousScreen = MainMenu;
            CurrentScreen = MatchFinder;
        }

        private void OnDeckEditorPressed()
        {
            PreviousScreen = MainMenu;
            CurrentScreen = DeckEditor;
        }

        private void OnUserProfilePressed()
        {
            PreviousScreen = MainMenu;
            CurrentScreen = UserProfile;
        }

        private void OnBackPressed()
        {
            PreviousScreen = CurrentScreen;
            CurrentScreen = MainMenu;
        }

        private void OnQuitPressed()
        {
            GetTree().Quit();
        }

        private void AddScreen<T>(IScreen<T> screen) where T: Node
        {
           AddChild(screen.View);
        }
    }
    

}