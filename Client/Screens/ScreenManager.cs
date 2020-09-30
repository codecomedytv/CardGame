using Godot;
using JetBrains.Annotations;

namespace CardGame.Client.Screens
{
    [UsedImplicitly]
    public sealed class ScreenManager: Node
    {
        private readonly LoginScreen Login;
        private readonly MainMenuScreen MainMenu;
        private readonly DeckEditor DeckEditor;
        private readonly MatchFinder MatchFinder;
        private readonly UserProfile UserProfile;
        private IScreen PreviousScreen;
        private IScreen CurrentScreen;

        public ScreenManager()
        {
            Login = new LoginScreen();
            MainMenu = new MainMenuScreen();
            DeckEditor = new DeckEditor();
            MatchFinder = new MatchFinder();
            UserProfile = new UserProfile();
        }

        public override void _Ready()
        {
            // We can likely afford to keep all screens in the tree at once..
            // ..we would just turn them off/on when necessary
            AddScreen<LoginView>(Login);
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

        private void OnUserLoggedIn() => ChangeScreen(Login, MainMenu);
        private void OnFindMatchPressed() => ChangeScreen(MainMenu, MatchFinder);
        private void OnDeckEditorPressed() => ChangeScreen(MainMenu, DeckEditor);
        private void OnUserProfilePressed() => ChangeScreen(MainMenu, UserProfile);
        private void OnBackPressed() => ChangeScreen(CurrentScreen, MainMenu);
        private void OnQuitPressed() => GetTree().Quit();
        private void AddScreen<T>(IScreen<T> screen) where T: Node => AddChild(screen.View);

        private void ChangeScreen(IScreen previousScreen, IScreen nextScreen) //where T: Node
        {
            GD.Print("change");
            PreviousScreen = previousScreen;
            CurrentScreen = nextScreen;
            PreviousScreen.StopDisplaying();
            CurrentScreen.Display();
        }
    }
    

}