using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src;
using SabberStoneCoreAi.Score;
using SabberStoneCoreAi.src.Agent;

namespace SabberStoneCoreAi.Agent
{
    class MCTSmalo : AbstractAgent
    {
        Arbolbueno tree;
        private const int VISITSxNODE = 100;
        private const double EXPLORATION_RATE = 0.7;
        public override void FinalizeAgent()
        {
        }

        public override void FinalizeGame()
        {
        }

        public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
        {
            var options = poGame.CurrentPlayer.Options();
            tree = new Arbolbueno(poGame, options);
            //el end_turn no lo vamos a visitar
            int visits = VISITSxNODE * (options.Count - 1);
            for (int i = 0; i < visits; ++i)
            {
                bool exploting = ((double)i / (double)visits) > EXPLORATION_RATE;
                tree.Simulation(i, exploting);
            }
            return tree.getBestNode().getTask();
        }



        public override void InitializeAgent()
        {

        }

        public override void InitializeGame()
        {

        }
    }
}
