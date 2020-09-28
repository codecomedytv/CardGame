using Godot;

namespace CardGame.Client.Screens
{
    public interface IScreen
    {
        void Display();
        void StopDisplaying();
    }
    
    public interface IScreen<out T>: IScreen where T: Node
    {
        T View { get; }
    }
}