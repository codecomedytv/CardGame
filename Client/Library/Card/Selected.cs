using System;
using Godot;
using Array = Godot.Collections.Array;

namespace CardGame.Client.Library.Card
{
    public class Selected: TextureRect
    {
        private const float MaxFrame = 25;
        public bool Playing = true;
        public float Frame = 0;
        public Array Frames = new Array();

        public override void _Ready()
        {
            var d = new Directory();
            const string path = "res://Assets/spritesheets/S-PNg.1";
            var err = d.Open(path);
            if (err != Error.Ok)
            {
                GD.PushWarning(err.ToString());
            }

            d.ListDirBegin(true);
            var filename = d.GetNext();
            while (filename != "")
            {
                if(filename.EndsWith(".png"))
                {
                    Frames.Add(ResourceLoader.Load(String.Format("{0}/{1}", path, filename)) as Texture);
                }

                filename = d.GetNext();
            }
            d.ListDirEnd();
        }

        public override void _Process(float delta)
        {
            if (Playing)
            {
                NextFrame();
            }
        }

        public void NextFrame()
        {
            Frame += 0.5F;
            if (Math.Abs(Frame - MaxFrame) < 0)
            {
                Frame = 0;
            }

            Texture = Frames[(int)Frame] as Texture;
        }
    }
}

