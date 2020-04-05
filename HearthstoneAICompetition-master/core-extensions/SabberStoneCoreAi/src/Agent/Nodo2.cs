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

		private float value;
		private int visits;

		public PlayerTask getTask() { return task; }

		public POGame.POGame getState() { return state; }
		public float getAverageValue() { return value / visits; }
		public float getValue() { return value; }
		public int getVisits() { return visits; }
		public Nodo2(POGame.POGame state, Nodo2 padre, PlayerTask task)
		{
			this.state = state;
			this.padre = padre;
			this.task = task;
			//hay que hacer la distinción para no pasar al turno del otro jugador
			if (task.PlayerTaskType != PlayerTaskType.END_TURN)
				posiblesHijos = state.CurrentPlayer.Options();
			else posiblesHijos = new List<PlayerTask>();
		}

		public bool isExpanded()
		{
			return posiblesHijos.Count == 0;
		}

		public Nodo2 expand()
		{
			//seleccionamos aleatoriamente que hijo expandir
			int i = Globals.r.Next(0, posiblesHijos.Count);
			//simulamos el estado después de aplicarlo
			POGame.POGame nextState = state.Simulate(new List<PlayerTask>() { posiblesHijos[i] })[posiblesHijos[i]];
			//creamos el hijo expandido
			Nodo2 expNode = new Nodo2(nextState, this, posiblesHijos[i]);
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
		public float ucb()
		{
			return (float)(getAverageValue() + Globals.C * Math.Sqrt(Math.Log(padre.getVisits()) / visits));
		}
		private void addValue(float value)
		{
			++visits;
			this.value += value;
		}
		private static int GetStateValue(POGame.POGame state)
		{
			//utilizamos al oponente porque evaluamos después de pasar turno
			return new ScoreUtility { Controller = state.CurrentOpponent }.Rate();
		}
		public static int Simulate(POGame.POGame state)
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

		public static void BackPropagation(Nodo2 node, int result)
		{
			node.addValue(result);
			if (node.padre != null)
				BackPropagation(node.padre, result);
		}
	}
}
