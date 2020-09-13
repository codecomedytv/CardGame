using Godot;

namespace CardGame.Client.Assets.Audio
{
    public class Audio
    {
        private const string LocalPath = "res://Client/Assets/Audio";
        public static readonly AudioStream Draw = GD.Load<AudioStream>($"{LocalPath}/Card_Game_Movement_Deal_Single_01.wav");
        public static readonly AudioStream Deploy = GD.Load<AudioStream>($"{LocalPath}/Card_Game_Play_Mech_Arm_01.wav");
        public static readonly AudioStream SetCard =
            GD.Load<AudioStream>($"{LocalPath}/Card_Game_Action_Health_Upgrade_02.wav");
    }
}