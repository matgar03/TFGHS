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

namespace SabberStoneCoreAi.Agent
{
	class Nodomalo
	{
		private PlayerTask task;
		private float value;
		private int visits;
		private Arbolmalo tree;

		private string score;
		public Nodomalo (PlayerTask task, float initialValue, Arbolmalo tree, string score)
		{
			this.task = task;
			this.value = initialValue;
			this.tree = tree;
			this.visits = 0;

			this.score = score;
		}

		public PlayerTask getTask() { return task; }
		public float getAverageValue() { return value / visits; }
		public float getValue() { return value; }
		public int getVisits() { return visits; }

		public void simulation(ResultSim resultSim)
		{
			var state = resultSim.state;
			var nextState = state.Simulate(new List<PlayerTask>() { resultSim.task })[resultSim.task];
			System.Random r = new Random();
			if(nextState == null)
			{
				addValue(getStateValue(state));
				return;
			}
			var options = nextState.CurrentPlayer.Options();
			int selectedOpt = r.Next(0, options.Count-1);
			PlayerTask nextTask = options[selectedOpt];
			if (nextTask.PlayerTaskType != PlayerTaskType.END_TURN)
			{
				simulation(new ResultSim(nextState, nextTask, getStateValue(nextState)));
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
			if (score.Equals("utility"))
				return new ScoreUtility { Controller = state.CurrentPlayer }.Rate();
			else if (score.Equals("midrange"))
				return new MidRangeScore { Controller = state.CurrentPlayer }.Rate();
			else return -1;
		}
	}


}
