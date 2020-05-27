using Godot;

namespace CardGame.Server
{
    public class TrainingTrainer : Unit
    {
        public TrainingTrainer()
        {
            Title = "Training Trainer";
            SetCode = SetCodes.Alpha_TrainingTrainer;
            Attack = 1000;
            Defense = 1000;
            GD.PushWarning("Skill Not Implemented");
        }
    }
}