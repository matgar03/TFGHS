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
using System.Diagnostics;

namespace SabberStoneCoreAi.Agent
{
    class MCTS1 : AbstractAgent
    {
        Arbolbueno tree;
		private double C;
		private string score;
		public MCTS1(double C,string score)
		{
			this.C = C;
			this.score = score;
		}
        public override void FinalizeAgent()
        {
        }

        public override void FinalizeGame()
        {
        }

        public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
        {
            var options = poGame.CurrentPlayer.Options();
            tree = new Arbolbueno(poGame, options,C,score);
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while(stopwatch.ElapsedMilliseconds < Globals.MAX_TIME)
            {
				if (!tree.Simulation())
					break;
            }
			stopwatch.Stop();
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
