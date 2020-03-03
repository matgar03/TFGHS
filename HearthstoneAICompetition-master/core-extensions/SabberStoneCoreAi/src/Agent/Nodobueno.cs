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
	class Nodobueno
	{
		private PlayerTask task;
		//estado después de ejecutar la task
		private POGame.POGame state;
		private float value;
		private int visits;
		private Arbolbueno tree;
		private Nodobueno padre;
		public Nodobueno (PlayerTask task, float initialValue, Arbolbueno tree,Nodobueno padre)
		{
			this.task = task;
			this.value = initialValue;
			this.tree = tree;
			this.padre = padre;
			this.visits = 0;
		}

		public PlayerTask getTask() { return task; }

		public POGame.POGame getState() { return state; }
		public float getAverageValue() { return value / visits; }
		public float getValue() { return value; }
		public int getVisits() { return visits; }

		
		public float simulation(ResultSim resultSim)
		{

			var state = resultSim.state;
			var nextState = state.Simulate(new List<PlayerTask>() { resultSim.task })[resultSim.task];
			System.Random r = new Random();
			if(nextState == null)
			{
				addValue(resultSim.value);
				return resultSim.value;
			}
			var options = nextState.CurrentPlayer.Options();
			int selectedOpt = r.Next(0, options.Count-1);
			PlayerTask nextTask = options[selectedOpt];
			if (nextTask.PlayerTaskType != PlayerTaskType.END_TURN)
			{
				return simulation(new ResultSim(nextState, nextTask, getStateValue(nextState)));
			}
			else
			{
				float value = getStateValue(nextState);
				addValue(value);
				return value;

			}
		}
		
		public float simulation()
		{
			System.Random r = new Random();
			var options = state.CurrentPlayer.Options();
			int selectedOpt = r.Next(0, options.Count);
			if(options[selectedOpt].PlayerTaskType == PlayerTaskType.END_TURN)
			{
				float value = getStateValue(state);
				addValue(value);
				return value;
			}
			return simulation(new ResultSim(state, options[selectedOpt], getStateValue(state)));
		}
		public void addValue(float value)
		{
			++visits;
			this.value += value;
		}
		public void BackPropagation(float valor)
		{
			if (padre != null)
			{
				padre.addValue(valor);
				padre.BackPropagation(valor);

			}
		}
		private int getStateValue(POGame.POGame state)
		{
			return new ScoreUtility { Controller = state.CurrentPlayer}.Rate();
		}

		public void addState(POGame.POGame state)
		{
			this.state = state;
		}
	}


}