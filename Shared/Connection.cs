using Godot;
using Godot.Collections;

namespace CardGame
{
	public class Connection : Control
	{
	
		protected const string NodeAdded = "node_added";
		
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
			if (notification != NotificationEnterTree) return;
			GetTree().Connect(NodeAdded, this, nameof(OnNodeAdded));
			CustomizeChildren();
		}

		protected void OnNodeAdded(Node node)
		{
			var treePath = (string) node.GetPath();
			var ourPath = (string) GetPath();
			var substr = treePath.Substr(0, ourPath.Length());
			if(substr != ourPath) { return; }
			var relativePath = treePath.Substr(ourPath.Length(), treePath.Length());
			if(relativePath.Length() > 0 && !relativePath.BeginsWith("/")) { return; }
			node.CustomMultiplayer = CustomMultiplayer;
		}

		protected void CustomizeChildren()
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
}
