using Godot;
using System;
using Godot.Collections;

public class Connection : Control
{
	
	protected Connection() 
	{ 
		CustomMultiplayer = new MultiplayerAPI();
		CustomMultiplayer.SetRootNode(this); 
	}
	
	public override void _Process(float delta)
	{
		if(CustomMultiplayer.HasNetworkPeer()) { CustomMultiplayer.Poll(); }
	}
	
	public override void _Notification(int notification)
	{
		if(notification == NotificationEnterTree)
		{
			GetTree().Connect("node_added", this, "OnNodeAdded");
			CustomizeChildren();
		}
	}
	
	public void OnNodeAdded(Godot.Node N) 
	{
		var treePath = (string)N.GetPath();
		var ourPath = (string)GetPath();
		var substr = treePath.Substr(0, ourPath.Length());
		if(substr != ourPath) 
		{
				return; 
		}
		var relativePath = (string)treePath.Substr(ourPath.Length(), treePath.Length());
		if(relativePath.Length() > 0 && !relativePath.BeginsWith("/")) 
		{  
				return; 
		}
		N.CustomMultiplayer = CustomMultiplayer;
	}
	
	public void CustomizeChildren()
	{
		var frontier = new Array<Node>();
		foreach(Node child in GetChildren()){
			frontier.Add(child);
		}

		while(frontier.Count != 0) {
			var child = frontier[0];
			frontier.RemoveAt(0);
			child.CustomMultiplayer = CustomMultiplayer;
			foreach(Node grandchild in child.GetChildren()){
				frontier.Add(grandchild);
			}
		}
	}
}
