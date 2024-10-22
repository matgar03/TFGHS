﻿using System;
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
    class SimTreeAgent : AbstractAgent
    {
        Arbolmalo tree;
        private double EXPLORATION_RATE;
		private string score;

		public SimTreeAgent(double exp_rate,string score)
		{
			EXPLORATION_RATE = exp_rate;
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
			tree = new Arbolmalo(poGame, options,score);
			if (!tree.isExplorableNodesEmpty())
			{
				Stopwatch stopwatch = new Stopwatch();
				int i = 0;
				stopwatch.Start();
				while (stopwatch.ElapsedMilliseconds <= Globals.MAX_TIME)
				{
					bool exploting = ((double)stopwatch.ElapsedMilliseconds / (double)Globals.MAX_TIME) > EXPLORATION_RATE;
					tree.Simulation(i, exploting);
					++i;

				}
				stopwatch.Stop();
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
