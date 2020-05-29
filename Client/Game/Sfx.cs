using Godot;

namespace CardGameSharp.Client.Game
{
	public class Sfx : AudioStreamPlayer
	{
		private AudioStream DeploySfx = ResourceLoader.Load("res://Assets/sounds/Rune.wav") as AudioStream;

		private AudioStream DrawSfx =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Movement_Deal_Single_01.wav") as AudioStream;

		private AudioStream SetFaceDownSfx =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Play_Swirl_Wind_01.wav") as AudioStream;

		private AudioStream BattleSfx =
			ResourceLoader.Load("res://Assets/sounds/Card_Game_Action_Stomp_01.wav") as AudioStream;

		private AudioStream VictorySfx =
			ResourceLoader.Load("res://Assets/sounds/BRPG_Victory_Stinger.wav") as AudioStream;

		private AudioStream DefeatSfx =
			ResourceLoader.Load("res://Assets/sounds/BRPG_Defeat_Stinger.wav") as AudioStream;

		public void Deploy()
		{
			Stream = DeploySfx;
			Play();
		}

		public void SetSupport()
		{
			Stream = SetFaceDownSfx;
			Play();
		}

		public void DrawCard()
		{
			Stream = DrawSfx;
			Play();
		}

		public void BattleUnit()
		{
			Stream = BattleSfx;
			Play();
		}

		public void Victory()
		{
			Stream = VictorySfx;
			Play();
		}

		public void Defeat()
		{
			Stream = DefeatSfx;
			Play();
		}

	}
}