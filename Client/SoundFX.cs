using Godot;

namespace CardGame.Client
{
    public class SoundFx: AudioStreamPlayer
    {
        // Some of these aren't triggered by events because they're local actions. We're likely just hearing the opponent
        // counterpart when running 2 games
        
        private const string Audio = "res://Assets/Sounds";
        private readonly AudioStream DrawFx = (AudioStream) GD.Load($"{Audio}/Card_Game_Movement_Deal_Single_01.wav");
        private readonly AudioStream DeployFx = (AudioStream) GD.Load($"{Audio}/Card_Game_Play_Mech_Arm_03.wav");
        private readonly AudioStream ActivateFx = (AudioStream) GD.Load($"{Audio}/Card_Game_Action_Blow_Wind_02.wav");
        private readonly AudioStream SetFaceDownFx = (AudioStream) GD.Load($"{Audio}/Card_Game_Items_Potion_Dud_01.wav");
        private readonly AudioStream LostFx = (AudioStream) GD.Load($"{Audio}/Stinger3.ogg");
        private readonly AudioStream WonFx = (AudioStream) GD.Load($"{Audio}/BRPG_Victory_Stinger.wav");

        public SoundFx()
        {
            VolumeDb = -20;
        }
        public void Draw()
        {
            Stream = DrawFx;
            Play();
        }

        public void Deploy()
        {
            Stream = DeployFx;
            Play();
        }

        public void Activate()
        {
            Stream = ActivateFx;
            Play();
        }

        public void SetFaceDown()
        {
            Stream = SetFaceDownFx;
            Play();
        }

        public void Won()
        {
            VolumeDb = -5;
            Stream = WonFx;
            Play();
        }

        public void Lost()
        {
            VolumeDb = -5;
            Stream = LostFx;
            Play();
        }
        
    }
}