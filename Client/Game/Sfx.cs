using Godot;

namespace CardGame.Client.Match
{
	public class Sfx : AudioStreamPlayer
	{
		public readonly AudioStream Deploy = ResourceLoader.Load("res://Assets/sounds/Rune.wav") as AudioStream;

		public readonly AudioStream Draw =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Movement_Deal_Single_01.wav") as AudioStream;

		public readonly AudioStream SetFaceDown =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Play_Swirl_Wind_01.wav") as AudioStream;

		public readonly AudioStream Battle =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Action_Stomp_01.wav") as AudioStream;

		public readonly AudioStream Victory =
			ResourceLoader.Load("res://Assets/sounds/BRPG_Victory_Stinger.wav") as AudioStream;

		public readonly AudioStream Defeat =
			ResourceLoader.Load("res://Assets/sounds/BRPG_Defeat_Stinger.wav") as AudioStream;

		public void Play(AudioStream audio)
		{
			Stream = audio;
			Play();
		}
		

	}
}