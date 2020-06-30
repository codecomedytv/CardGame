using CardGame.Client.Room;
using Godot;

namespace CardGame.Tests
{
    public class MockGame: Game
    {
        // !!!WARNING!!! ///
        // Since Game is a Scene, we will need to change this somehow and preferably without instancing the scene
        // beforehand? 
        // Mock that extends room for some internal accessors to either declare commands, retrieve card ids
        // or check on GameState. Even if this is flaky, we can at the very least centralize all of the changes here 

        public void Success()
        {
            GD.Print("Success");
        }
    }
}