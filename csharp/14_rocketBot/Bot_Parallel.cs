using System;
using System.Threading.Tasks;

namespace rocket_bot
{
    public partial class Bot
    {
        public Rocket GetNextMove(Rocket rocket)
        {
            var max = 0.0;
            Tuple<Turn,double> bestMove = null;
            var tasks = new Task<Tuple<Turn,double>>[threadsCount];
            for (var i = 0; i < threadsCount; i++)
            {
                tasks[i] = new Task<Tuple<Turn, double>>(() => SearchBestMove(rocket, new Random(random.Next()),
                    iterationsCount / threadsCount));
                tasks[i].Start();
            }

            foreach (var t in tasks)
            {
                var move = t.Result;
                if (move.Item2 > max)
                {
                    max = move.Item2;
                    bestMove = move;
                }
            }
            
            var newRocket = rocket.Move(bestMove.Item1, level);
            return newRocket;
        }
    }
}