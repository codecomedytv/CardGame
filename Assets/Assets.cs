using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Godot;

namespace CardGame.Assets
{
    public static class Icons
    {
        public static readonly ReadOnlyDictionary<int, Texture> Numbers = SetNumbers();
        
        private static ReadOnlyDictionary<int, Texture> SetNumbers()
        {
            var temp = new Dictionary<int, Texture>();
            for (var i = 0; i < 10; i++)
            {
                temp.Add(i, (Texture) GD.Load($"res://Assets/Numbers/{i}.png"));
            }   
            return new ReadOnlyDictionary<int, Texture>(temp);
        }
    }
    
    
}