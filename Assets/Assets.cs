using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mime;
using Godot;

namespace CardGame.Assets
{
    public static class Icons
    {
        public static class Numbers
        {
            public static readonly Texture Zero = (Texture) GD.Load("res://Assets/Numbers/0.png");
            public static readonly Texture One = (Texture) GD.Load("res://Assets/Numbers/1.png");
            public static readonly Texture Two = (Texture) GD.Load("res://Assets/Numbers/2.png");
            public static readonly Texture Three = (Texture) GD.Load("res://Assets/Numbers/3.png");
            public static readonly Texture Four = (Texture) GD.Load("res://Assets/Numbers/4.png");
            public static readonly Texture Five = (Texture) GD.Load("res://Assets/Numbers/5.png");
            public static readonly Texture Six = (Texture) GD.Load("res://Assets/Numbers/6.png");
            public static readonly Texture Seven = (Texture) GD.Load("res://Assets/Numbers/7.png");
            public static readonly Texture Eight = (Texture) GD.Load("res://Assets/Numbers/8.png");
            public static readonly Texture Nine = (Texture) GD.Load("res://Assets/Numbers/9.png");
            public static readonly IReadOnlyDictionary<string, Texture> IconList;

            static Numbers()
            {
                var temp = new Dictionary<string, Texture>
                {
                    {"0", Zero},
                    {"1", One},
                    {"2", Two},
                    {"3", Three},
                    {"4", Four},
                    {"5", Five},
                    {"6", Six},
                    {"7", Seven},
                    {"8", Eight},
                    {"9", Nine}
                };
                IconList = new ReadOnlyDictionary<string, Texture>(temp);
            }
            
        }
    }
}