using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.src.Agent
{
	class Arbolmalo
	{
		private POGame.POGame root;
		private List<Nodomalo> explorableNodes = new List<Nodomalo>();
		private List<Nodomalo> sortedNodes = new List<Nodomalo>();
		private Nodomalo end_turn;
		public Arbolmalo(POGame.POGame root, List<PlayerTask> options)
		{
			this.root = root;
			initTree(options);
		}

		private void initTree(List<PlayerTask> options)
		{
			explorableNodes.Clear();
			sortedNodes.Clear();
			foreach (PlayerTask opt in options){
				var nodo = new Nodomalo(opt, 0.0f, this);
				if (opt.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					nodo.addValue(getStateValue(root));
					end_turn = nodo;
				}
				else
				{
					explorableNodes.Add(nodo);
					sortedNodes.Add(nodo);
				}
			}
		}

		public void Simulation(int i, bool exploting)
		{
			Nodomalo node = null;
			if (exploting)
			{
				// Estamos escogiendo uno de los 7 mejores aleatoriamente
				sortedNodes.Sort((x, y) => y.getAverageValue().CompareTo(x.getAverageValue()));
				System.Random r = new Random();
				int count = Math.Min(sortedNodes.Count, 7);
				int selectedOpt = r.Next(0, count - 1);
				node = sortedNodes[selectedOpt];
			}
			else
			{
				//Estamos explorando todos los nodos por igual
					node = explorableNodes[i % explorableNodes.Count];
			}
			var resultsim = new ResultSim(root, node.getTask(), getStateValue(root));
			node.simulation(resultsim);
		}
		public Nodomalo getBestNode()
		{
			//end turn puede ser null si se juega una carta de escoge entre estas opciones
			sortedNodes.Sort((x, y) => y.getAverageValue().CompareTo(x.getAverageValue()));
			if (end_turn != null && (sortedNodes.Count == 0 || end_turn.getAverageValue() > sortedNodes[0].getAverageValue()))
				return end_turn;
			else
				return sortedNodes[0];
		}
		private int getStateValue(POGame.POGame state)
		{
			return new ScoreUtility { Controller = state.CurrentPlayer }.Rate();
		}
		public bool isExplorableNodesEmpty()
		{
			return explorableNodes.Count == 0;
		}
	}
}
