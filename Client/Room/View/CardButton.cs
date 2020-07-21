using Godot;
using System;
using System.Security.Principal;

public class CardButton : TextureButton
{
    public int Id;
    private string Art;

    public void Create(int id, string art)
    {
        Id = id;
        Art = art;
    }
    public override void _Ready()
    {
        GetNode<Sprite>("Art").Texture = ResourceLoader.Load($"res://Assets/CardArt/{Art}.jpg") as Texture;

    }
    
}
