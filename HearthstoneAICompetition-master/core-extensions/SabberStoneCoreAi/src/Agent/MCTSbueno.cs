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
    class MCTSbueno : AbstractAgent
    {
        Arbolbueno tree;
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
            int visits = 2500;
            for (int i = 0; i < visits; ++i)
            {
				if (!tree.Simulation())
					break;
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
