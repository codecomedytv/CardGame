using CardGame.Server.Game.Cards;
using Godot;

namespace CardGame.Server
{
    public class TrainingTrainer : Unit
    {
        public TrainingTrainer(Player owner)
        {
            Owner = owner;
            Controller = owner;
            Title = "Training Trainer";
            SetCode = SetCodes.AlphaTrainingTrainer;
            Attack = 1000;
            Defense = 1000;
        }
    }
}