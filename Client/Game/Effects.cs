using System.Threading.Tasks;
using Godot;

namespace CardGame.Client.Game
{
    public class Effects: Node
    {
        private readonly Tween Tween = new Tween();
        private readonly AudioStreamPlayer Audio = new AudioStreamPlayer();

        public override void _Ready()
        {
            //Tween.PlaybackProcessMode = Tween.TweenProcessMode.Idle;
            //Tween.PlaybackSpeed = 2;
            AddChild(Tween);
            AddChild(Audio);
            
            // Muting for a moment
            Audio.VolumeDb = 0; 
        }

        public float GetRunTime() => Tween.GetRuntime();

        public void Play(AudioStream audio, float delay = 0.01F)
        {
            Tween.InterpolateCallback(this, delay, "_playAudio", audio);
        }

        public void _playAudio(AudioStream stream)
        {
            Audio.Stream = stream;
            Audio.Play();
        }

        public SignalAwaiter Start()
        {
            Tween.Start();
            var awaiter = Tween.ToSignal(Tween, "tween_all_completed");
            return awaiter;
        }
        
        public void RemoveAll() => Tween.RemoveAll();

        public void InterpolateProperty(Object source, string property, object initialVal, object finalVal, float duration, 
            Tween.TransitionType transType = Tween.TransitionType.Quint, Tween.EaseType easeType = Tween.EaseType.In, float delay = 0.0F)
        {
            Tween.InterpolateProperty(source, property, initialVal, finalVal, duration, transType, easeType, delay);
        }
        public void InterpolateCallback(Object obj, float duration, string callback, object arg1 = null, object arg2 = null, object arg3 = null, object arg4 = null, 
            object arg5 = null)
        {
            Tween.InterpolateCallback(obj, duration, callback, arg1, arg2, arg3, arg4, arg5);
        }

        public void InterpolateProperty(object structArgs)
        {
            // Takes struct args for Property
        }

        public void InterpolateCallback(object structArgs)
        {
            // Takes struct args for callback
        }
    }

    public struct PropertyEffect
    {
        public Object Source;
        public object Initial;
        public object Final;
        public float Duration;
        public float Delay;
        public Tween.TransitionType TransType;
        public Tween.EaseType EaseType;
    }

    public struct Callback
    {
        public Object Source;
        public object Initial;
        public float Duration;
        public object[] Args;
    }
}