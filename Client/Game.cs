using Godot;
using System;

namespace CardGame.Client {

	public class Game : Node 
	{
		public override void _Ready()
		{
			string id = CustomMultiplayer.GetNetworkUniqueId().ToString();
			GD.Print(id + ": Created Room " + Name);
		}
		
	}

}
