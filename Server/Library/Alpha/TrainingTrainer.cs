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
            Power = 1000;
        }
    }
}