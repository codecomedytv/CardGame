using System;
using Godot;

namespace CardGame.Client.Assets.Audio
{
    public static class Audio
    {
        public static readonly AudioStream Draw = Load("Card_Game_Movement_Deal_Single_01.wav");
        public static readonly AudioStream Deploy = Load("Card_Game_Play_Mech_Arm_01.wav");
        public static readonly AudioStream SetCard = Load("Card_Game_Action_Health_Upgrade_02.wav");
        public static readonly AudioStream Activate = Load("Card_Game_Play_Slam_Water_Rise_01.wav");
        public static readonly AudioStream LoseLife = Load("Card_Game_Play_Slam_Wind_01.wav");
        public static readonly AudioStream BackgroundMusic = Load("car race - loop.ogg");

        private static AudioStream Load(string path)
        {
            return GD.Load<AudioStream>($"res://Client/Assets/Audio/{path}");
        }
    }
}