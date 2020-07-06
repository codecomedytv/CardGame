warning: LF will be replaced by CRLF in project.godot.
The file will have its original line endings in your working directory
[1mdiff --git a/CardGame.csproj b/CardGame.csproj[m
[1mindex 21299d0..60a3192 100644[m
[1m--- a/CardGame.csproj[m
[1m+++ b/CardGame.csproj[m
[36m@@ -87,6 +87,7 @@[m
     <Compile Include="Client\Library\Cards\Card.cs" />[m
     <Compile Include="Client\Library\CheckOut.cs" />[m
     <Compile Include="Client\Room\Player.cs" />[m
[32m+[m[32m    <Compile Include="Client\Room\View\PlayMat.cs" />[m
     <Compile Include="Client\Room\Zone.cs" />[m
     <Compile Include="Client\Room\ZoneIds.cs" />[m
     <Compile Include="Connection.cs" />[m
[1mdiff --git a/Client/ClientConn.cs b/Client/ClientConn.cs[m
[1mindex a1633fa..e193326 100644[m
[1m--- a/Client/ClientConn.cs[m
[1m+++ b/Client/ClientConn.cs[m
[36m@@ -1,4 +1,5 @@[m
 using CardGame.Client.Room;[m
[32m+[m[32musing CardGame.Client.Room.View;[m
 using Godot;[m
 using Godot.Collections;[m
 [m
[36m@@ -42,7 +43,7 @@[m [mnamespace CardGame.Client {[m
 [m
 		[Puppet][m
 		public void CreateRoom(string gameId, int seatPosition){[m
[31m-			//var room = new Game();[m
[32m+[m			[32mvar room = new Game();[m
 			Game.Name = gameId;[m
 			AddChild(Game);[m
 			Game.SetUp();[m
[1mdiff --git a/Client/Room/Game.cs b/Client/Room/Game.cs[m
[1mindex 3577590..5a779c2 100644[m
[1m--- a/Client/Room/Game.cs[m
[1m+++ b/Client/Room/Game.cs[m
[36m@@ -14,13 +14,11 @@[m [mnamespace CardGame.Client.Room {[m
 		[Signal][m
 		public delegate void StateSet();[m
 		[m
[31m-		private readonly PackedScene PlayMat = (PackedScene) ResourceLoader.Load("res://Client/Room/Game.tscn");[m
 		private readonly CommandQueue CommandQueue;[m
 		private readonly CardCatalog CardCatalog;[m
 		private readonly Messenger Messenger;[m
 		private readonly Input Input;[m
 		private readonly Tween Gfx;[m
[31m-[m
 		protected readonly Player Opponent;[m
 		protected readonly Player Player;[m
 		[m
[1mdiff --git a/Client/Room/View/PlayMat.cs b/Client/Room/View/PlayMat.cs[m
[1mindex f156075..c61b2f6 100644[m
[1m--- a/Client/Room/View/PlayMat.cs[m
[1m+++ b/Client/Room/View/PlayMat.cs[m
[36m@@ -1,7 +1,16 @@[m
[31m-﻿namespace CardGame.Client.Room.View[m
[32m+[m[32m﻿using Godot;[m
[32m+[m
[32m+[m[32mnamespace CardGame.Client.Room.View[m
 {[m
[31m-    public class PlayMat[m
[32m+[m[32m    public class PlayMat: Control[m
     {[m
[31m-        [m
[32m+[m[32m        public Control Player;[m
[32m+[m[32m        public Control Opponent;[m
[32m+[m
[32m+[m[32m        public override void _Ready()[m
[32m+[m[32m        {[m
[32m+[m[32m            Player = GetNode<Control>("Player");[m
[32m+[m[32m            Opponent = GetNode<Control>("Opponent");[m
[32m+[m[32m        }[m
     }[m
 }[m
\ No newline at end of file[m
