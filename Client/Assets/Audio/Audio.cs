using Godot;

namespace CardGame.Client.Assets.Audio
{
    public class Audio
    {
        private const string LocalPath = "res://Client/Assets/Audio";
        public static readonly AudioStream Draw = GD.Load<AudioStream>($"{LocalPath}/Card_Game_Movement_Deal_Single_01.wav");
    }
}