using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.src.Agent
{
	class Arbolbueno
	{
		private POGame.POGame root;
		private List<Nodobueno> explorableNodes = new List<Nodobueno>();
		private List<Nodobueno> sortedNodes = new List<Nodobueno>();
		private Nodobueno end_turn;
		public Arbolbueno(POGame.POGame root, List<PlayerTask> options)
		{
			this.root = root;
			initTree(options);
		}

		private void initTree(List<PlayerTask> options)
		{
			explorableNodes.Clear();
			sortedNodes.Clear();
			foreach (PlayerTask opt in options){
				var nodo = new Nodobueno(opt, 0.0f, this, null);
				if (opt.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					nodo.addValue(getStateValue(root));
					end_turn = nodo;
				}
				else
				{
					var nextState = root.Simulate(new List<PlayerTask>() { opt })[opt];
					nodo.addValue(getStateValue(nextState));
					nodo.addState(nextState);
					explorableNodes.Add(nodo);
					sortedNodes.Add(nodo);
				}
			}
		}
	
		public bool Simulation()
		{
			if (sortedNodes.Count == 0)
				return false;
			//seleccionamos el nodo a expandir (el mejor valor medio)
			sortedNodes.Sort((x, y) => y.getAverageValue().CompareTo(x.getAverageValue()));
			Nodobueno selectedNode = sortedNodes[0];
			//expandimos el nodo y elegimos el hijo a simular
		
			Nodobueno selChild = ExpandNode(selectedNode);
			if(selChild != null)
			{
				//simulamos el nodo escogido
				float childVal = selChild.simulation();
				//actualizar a los padres
				selChild.BackPropagation(childVal);
			}
			return true;

		}
		public Nodobueno getBestNode()
		{
			//end turn puede ser null si se juega una carta de escoge entre estas opciones
			explorableNodes.Sort((x, y) => y.getAverageValue().CompareTo(x.getAverageValue()));
			if (end_turn != null && (explorableNodes.Count == 0 || end_turn.getAverageValue() > explorableNodes[0].getAverageValue()))
				return end_turn;
			else
				return explorableNodes[0];
		}
		private Nodobueno ExpandNode(Nodobueno node)
		{
			List<Nodobueno> childNodes = new List<Nodobueno>();
			POGame.POGame state = node.getState();
			var options = state.CurrentPlayer.Options();
			foreach (PlayerTask opt in options)
			{
				var nodo = new Nodobueno(opt, 0.0f, this, node);
				if (opt.PlayerTaskType == PlayerTaskType.END_TURN)
				{
					nodo.addValue(getStateValue(state));
					
				}
				else
				{
					var nextState = state.Simulate(new List<PlayerTask>() { opt })[opt];
					nodo.addValue(getStateValue(nextState));
					nodo.addState(nextState);
					sortedNodes.Add(nodo);
					childNodes.Add(nodo);
				}
				
			}
			sortedNodes.Remove(node);
			if (childNodes.Count == 0) return null;

			System.Random r = new Random();
			int indChild = r.Next(0, childNodes.Count);
			
			return childNodes[indChild];
		}

		private int getStateValue(POGame.POGame state)
		{
			return new ScoreUtility { Controller = state.CurrentPlayer }.Rate();
		}
	}
}
