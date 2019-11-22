using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src;

namespace SabberStoneCoreAi.Agent
{
	class MyAgent : AbstractAgent
	{
		private Random Rnd = new Random();

		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame()
		{
		}

		public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			ArbolGener < Tuple <SabberStoneCoreAi.POGame.POGame, PlayerTask>> arbol =
				new ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>>(new Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>(poGame,null));

			List<PlayerTask> options = poGame.CurrentPlayer.Options();

			Dictionary< PlayerTask,SabberStoneCoreAi.POGame.POGame> dic= poGame.Simulate(options);
			foreach (PlayerTask t in options){

				arbol.AddHijo(new Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>(dic[t], t));

				if (t.PlayerTaskType != PlayerTaskType.END_TURN)
					generateTree(arbol.getHijo(arbol.getHijoCount() - 1));

			}
	
			return null;
		}

		private void generateTree(ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>> arbol)
		{
			Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask> tupla = arbol.getNodo();
			List<PlayerTask> options = tupla.Item1.CurrentPlayer.Options();
			Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> dic = tupla.Item1.Simulate(options);
			foreach (PlayerTask t in options)
			{
				arbol.AddHijo(new Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>(dic[t], t));
				if (t.PlayerTaskType != PlayerTaskType.END_TURN)
					generateTree(arbol.getHijo(arbol.getHijoCount()- 1));

			}

		}




		public override void InitializeAgent()
		{
			Rnd = new Random(101);
		}

		public override void InitializeGame()
		{
		}
	}
}
