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
		Nodo2 root;
		private double C;
		private string score;
		private double[] weight = null;
		public MCTS2(double C, string score)
		{
			this.C = C;
			this.score = score;
		}

		public MCTS2(double C, string score, double[] pesos)
		{
			this.C = C;
			this.score = score;
			this.weight = pesos;
		}
		public override void FinalizeAgent()
		{
			
		}

		public override void FinalizeGame()
		{
			
		}

		//hay que comprobar si el seleccionado o el expandido son hojas para simplemente evaluar
		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			root = new Nodo2(poGame,C,score,weight);
			double result;
			Stopwatch stopwatch = new Stopwatch();
			stopwatch.Start();
			int it = 0;
			while (stopwatch.ElapsedMilliseconds < Globals.MAX_TIME)
			//for (int i =0;i<1000;++i)
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
				++it;
			}
			//Console.WriteLine($"Iteraciones: {it}");
			stopwatch.Stop();
			return root.bestAverageChild().getTask();
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
