using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;

namespace SabberStoneCoreAi.src.Agent
{
	class MCTS2 : AbstractAgent
	{

		int nsimulations = 1000;
		Nodo2 root;
		public override void FinalizeAgent()
		{
			
		}

		public override void FinalizeGame()
		{
			
		}

		//hay que comprobar si el seleccionado o el expandido son hojas para simplemente evaluar
		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			root = new Nodo2(poGame);
			int result;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			while (stopwatch.ElapsedMilliseconds < Globals.MAX_TIME)
			{
				Nodo2 selNode = Selection(root);
				//la raiz tiene null la task por eso lo comprobamos, como siempre tiene hijos nunca debería entrar en el if
				if (selNode != root && selNode.getTask().PlayerTaskType == PlayerTaskType.END_TURN)
				{
					result = Nodo2.GetStateValue(selNode.getState());
					Nodo2.BackPropagation(selNode, result);
				}
				else
				{
					Nodo2 expNode = selNode.expand();
					if (expNode.getTask().PlayerTaskType == PlayerTaskType.END_TURN)
					{
						result = Nodo2.GetStateValue(expNode.getState());
					}
					else
					{
						result = Nodo2.Simulate(expNode.getState());
					}
					Nodo2.BackPropagation(expNode, result);
				}
				
			}
			stopwatch.Stop();
			return root.bestChild().getTask();
		}

		public override void InitializeAgent()
		{
			
		}

		public override void InitializeGame()
		{
			
		}

		private Nodo2 Selection(Nodo2 node)
		{
			//si no han sido expandidos todos sus hijos este es el nodo seleccionado
			//también puede llegar a darse el caso de llegar a una hoja del árbol, en ese caso, devolvemos ese nodo
			//y lo tratamos fuera
			if (!node.isExpanded() || (node != root && node.getTask().PlayerTaskType == PlayerTaskType.END_TURN))
				return node;
			//si han sido expandidos entonces seleccionamos el mejor hijo y delegamos la selección a él
			else 
			{
				return Selection(node.bestChild());
			}
		}

	}
}
