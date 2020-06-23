using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.src.Agent
{
	class Nodo2
	{
		Nodo2 padre;
		PlayerTask task;
		POGame.POGame state;
		//las PlayerTask de hijos y posibles hijos tienen que ser disjuntas
		//al principio la primera es vacía y la segunda contiene todas las acciones y cuando se expanden todos los hijos será al revés
		List<Nodo2> hijos;
		List<PlayerTask> posiblesHijos;

		private double value;
		private int visits;

		private double C;
		private static string score;
		private static double[] weight = null;

		public PlayerTask getTask() { return task; }

		public POGame.POGame getState() { return state; }
		public double getAverageValue() { return value / visits; }
		public double getValue() { return value; }
		public int getVisits() { return visits; }

		public Nodo2(POGame.POGame state, double C, string score1, double[] pesos)
		{
			this.state = state;
			this.padre = null;
			this.task = null;
			this.hijos = new List<Nodo2>();
			posiblesHijos = state.CurrentPlayer.Options();

			this.C = C;
			score = score1;
			weight = pesos;
		}
		public Nodo2(POGame.POGame state, Nodo2 padre, PlayerTask task, double C, string score1, double[] pesos)
		{
			this.state = state;
			this.padre = padre;
			this.task = task;
			this.hijos = new List<Nodo2>();
			//hay que hacer la distinción para no pasar al turno del otro jugador
			if (task.PlayerTaskType != PlayerTaskType.END_TURN)
				posiblesHijos = state.CurrentPlayer.Options();
			else posiblesHijos = new List<PlayerTask>();

			this.C = C;
			score = score1;
			weight = pesos;
		}

		public bool isExpanded()
		{
			return posiblesHijos.Count == 0;
		}
		//esta función siempre es llamada desde un nodo no hoja
		public Nodo2 expand()
		{
			//seleccionamos aleatoriamente que hijo expandir
			int i = Globals.r.Next(0, posiblesHijos.Count);
			//simulamos el estado después de aplicarlo
			POGame.POGame nextState = state.Simulate(new List<PlayerTask>() { posiblesHijos[i] })[posiblesHijos[i]];
			//creamos el hijo expandido
			Nodo2 expNode = new Nodo2(nextState, this, posiblesHijos[i],C,score,weight);
			//lo añadimos al árbol
			hijos.Add(expNode);
			//lo eliminamos de los posibles hijos porque ya está en hijos
			posiblesHijos.RemoveAt(i);
			return expNode;
		}
		public Nodo2 bestChild()
		{
			hijos.Sort((x, y) => y.ucb().CompareTo(x.ucb()));
			return hijos[0];
		}

		public Nodo2 bestAverageChild()
		{
			hijos.Sort((x, y) => y.getAverageValue().CompareTo(x.getAverageValue()));
			return hijos[0];
		}
		public float ucb()
		{
			return (float)(getAverageValue() + C * Math.Sqrt(Math.Log(padre.getVisits()) / visits));
		}
		private void addValue(double value)
		{
			++visits;
			this.value += value;
		}
		public static double GetStateValue(POGame.POGame state)
		{
			if (score.Equals("utility"))
				return new ScoreUtility { Controller = state.CurrentOpponent }.Rate();
			else if (score.Equals("midrange"))
				return new MidRangeScore { Controller = state.CurrentOpponent }.Rate();
			else if (score.Equals("evo"))
				return new EvoScore { Controller = state.CurrentOpponent }.Rate(weight);
			else return -1;
		}

		//igual que en el otro MCTS
		public static double Simulate(POGame.POGame state)
		{
			List<PlayerTask> options = state.CurrentPlayer.Options();
			int selectedOpt = Globals.r.Next(0, options.Count);
			PlayerTask task = options[selectedOpt];
			POGame.POGame nextState = state.Simulate(new List<PlayerTask>() { task })[task];
			if (task.PlayerTaskType == PlayerTaskType.END_TURN)
			{
				return GetStateValue(nextState);
			}
			else return Simulate(nextState);
		}

		//igual que en el otro MCTS
		public static void BackPropagation(Nodo2 node, double result)
		{
			node.addValue(result);
			if (node.padre != null)
				BackPropagation(node.padre, result);
		}
	}
}
