using Godot;
using System;
using Godot.Collections;

public class Connection : Control
{
	
	public Connection() 
	{ 
		CustomMultiplayer = new MultiplayerAPI();
		CustomMultiplayer.SetRootNode(this); 
	}
	
	public override void _Process(float delta)
	{
		if(CustomMultiplayer.HasNetworkPeer()) { CustomMultiplayer.Poll(); }
	}
	
	public override void _Notification(int Notification)
	{
		if(Notification == NotificationEnterTree)
		{
			GetTree().Connect("node_added", this, "OnNodeAdded");
			CustomizeChildren();
		}
	}
	
	public void OnNodeAdded(Godot.Node N) 
	{
		string TreePath = (string)N.GetPath();
		string OurPath = (string)GetPath();
		if(TreePath.Substr(0, OurPath.Length()) != OurPath) { return; }
		string RelativePath = (string)TreePath.Substr(OurPath.Length(), TreePath.Length());
		if(RelativePath.Length() > 0 && RelativePath.BeginsWith("/")) { return; }
		N.CustomMultiplayer = CustomMultiplayer;
	}
	
	public void CustomizeChildren()
	{
		Array<Node> Frontier = new Array<Node>();
		foreach(Node Child in GetChildren()){
			Frontier.Add(Child);
		}

		while(Frontier.Count != 0) {
			Node Child = Frontier[0];
			Frontier.RemoveAt(0);
			Child.CustomMultiplayer = CustomMultiplayer;
			foreach(Node Grandchild in Child.GetChildren()){
				Frontier.Add(Grandchild);
			}
		}
	}
}
