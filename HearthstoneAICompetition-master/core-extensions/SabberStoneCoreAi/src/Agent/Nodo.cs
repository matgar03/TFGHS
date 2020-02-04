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
	class Nodo
	{
		private PlayerTask task;
		private float value;
		private int visits;
		private Arbol tree;
		public Nodo (PlayerTask task, float initialValue, Arbol tree)
		{
			this.task = task;
			this.value = initialValue;
			this.tree = tree;
			this.visits = 0;
		}

		public PlayerTask getTask() { return task; }
		public float getAverageValue() { return value / visits; }
		public float getValue() { return value; }
		public int getVisits() { return visits; }

		public void simulation(ResultSim resultSim)
		{
			System.Random r = new Random();
			var state = resultSim.state;
			var options = state.CurrentPlayer.Options();
			int selectedOpt = r.Next(0, options.Count-1);
			var task = options[selectedOpt];
			var nextState = state.Simulate(new List<PlayerTask>() { task })[task];
			if (task.PlayerTaskType != PlayerTaskType.END_TURN)
			{
				simulation(new ResultSim(nextState, task, getStateValue(nextState)));
			}
			else
			{
				addValue(getStateValue(nextState));
			}
		}
		public void addValue(float value)
		{
			++visits;
			this.value += value;
		}

		private int getStateValue(POGame.POGame state)
		{
			return new AggroScore { Controller = state.CurrentOpponent }.Rate();
		}
	}


}
