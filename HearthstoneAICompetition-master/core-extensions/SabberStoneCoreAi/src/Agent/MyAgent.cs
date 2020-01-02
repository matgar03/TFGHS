using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.POGame;
using SabberStoneCore.Tasks.PlayerTasks;
using SabberStoneCoreAi.src;
using SabberStoneCoreAi.Score;

namespace SabberStoneCoreAi.Agent
{
	class MyAgent : AbstractAgent
	{
		private Random Rnd = new Random();
		private int np = 3;
		private Stack<PlayerTask> buffermoves;
		private int bestscore;
		public override void FinalizeAgent()
		{
		}

		public override void FinalizeGame()
		{
		}

		public override PlayerTask GetMove(SabberStoneCoreAi.POGame.POGame poGame)
		{
			if (buffermoves.Count == 0)
			{
				ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>> arbol =
					new ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>>(new Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>(poGame, null));

				List<PlayerTask> options = poGame.CurrentPlayer.Options();

				Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> dic = poGame.Simulate(options);
				foreach (PlayerTask t in options)
				{

					arbol.AddHijo(new Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>(dic[t], t));

					if (t.PlayerTaskType != PlayerTaskType.END_TURN)
						generateTree(arbol.getHijo(arbol.getHijoCount()));
					else
					{
						bestscore = this.getScore(dic[t]);
						buffermoves = new Stack<PlayerTask>();
						buffermoves.Push(t);
					}

				}
			}
			return buffermoves.Pop();
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
					generateTree(arbol.getHijo(arbol.getHijoCount()));
				else
				{
					if (bestscore < getScore(dic[t]))
					{
						bestscore = getScore(dic[t]);
						buffermoves = new Stack<PlayerTask>();
						buffermoves.Push(t);
						buffermoves.Push(arbol.getNodo().Item2);
						ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>> papa = arbol.getPadre();
						while(papa.getPadre()!= null)
						{
							buffermoves.Push(papa.getNodo().Item2);
							papa = papa.getPadre();
						}
					}
				}

			}

		}

		private int getScore (SabberStoneCoreAi.POGame.POGame poGame)
		{
			 return new ScoreAux { Controller = poGame.CurrentOpponent }.Rate();

		}




		public override void InitializeAgent()
		{
			Rnd = new Random(101);
			buffermoves = new Stack<PlayerTask>();
		}

		public override void InitializeGame()
		{
		}
	}
}
