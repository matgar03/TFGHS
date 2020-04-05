using System;
using System.Collections.Generic;
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

		public override PlayerTask GetMove(POGame.POGame poGame)
		{
			root = new Nodo2(poGame, null, null);
			for (int i = 0; i < nsimulations; ++i)
			{
				Nodo2 selNode = Selection(root);
				//el nodo devuelto debería ser expandible
				Nodo2 expNode = selNode.expand();
				int result = Nodo2.Simulate(expNode.getState());
				Nodo2.BackPropagation(expNode, result);
			}
			return null;
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
			if (!node.isExpanded())
				return node;
			//si han sido expandidos entonces seleccionamos el mejor hijo y delegamos la selección a él
			else
			{
				return Selection(node.bestChild());
			}
		}

	}
}
