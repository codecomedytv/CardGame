using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Room.Commands
{
    public class GameOver: Command
    {
        public readonly Player User;
        public readonly bool Won;
        private readonly Label WinLoseNote;
        
        public GameOver(Player user, bool won, Label winLoseNote)
        {
            User = user;
            Won = won;
            WinLoseNote = winLoseNote;
        }
        protected override async Task<object[]> Execute()
        {
            WinLoseNote.Text = Won ? "Victory!" : "Game Over!";
            User.State = States.GameOver;
            QueueCallback(WinLoseNote, 0, "set_visible", true);
            QueueCallback(Sfx, 0, Won ? nameof(SoundFx.Won) : nameof(SoundFx.Lost));
            // Stinger Here
            return await Start();
        }
    }
}