using System;
using System.Collections;
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
		private LinkedList<PlayerTask> buffermoves;
		private LinkedList<PlayerTask> provisional;
		private int bestscore;
		PlayerTask finturn;
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
				List<PlayerTask> options = poGame.CurrentPlayer.Options();

				Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> dic = poGame.Simulate(options);

				foreach (PlayerTask t in options)
				{


					if (t.PlayerTaskType != PlayerTaskType.END_TURN)
					{
						provisional = new LinkedList<PlayerTask>();
						provisional.AddLast(t);
						generarSecuencia(t, dic[t]);
					}
					else
					{
						finturn = t;
						bestscore = this.getScore(dic[t]);
						buffermoves = new LinkedList<PlayerTask>();
						provisional.AddLast(t);
					}

				}

			}
			PlayerTask sol;
			if (buffermoves.Count != 0)
			{
				sol = buffermoves.First.Value;
				buffermoves.RemoveFirst();
			}
			else
				sol = finturn;
			
			return sol;
		}

		private void generarSecuencia(PlayerTask task, SabberStoneCoreAi.POGame.POGame poGame)
		{
			if (bestscore == Int32.MaxValue)
			{
				
			}
			else
			{
				List<PlayerTask> options = poGame.CurrentPlayer.Options();
				Dictionary<PlayerTask, SabberStoneCoreAi.POGame.POGame> dic = poGame.Simulate(options);

				foreach (PlayerTask t in options)
				{
					if (t.PlayerTaskType != PlayerTaskType.END_TURN)
					{
						provisional.AddLast(t);
						generarSecuencia(t, dic[t]);
						provisional.RemoveLast();
					}
					else
					{
						if (bestscore < this.getScore(dic[t]))
						{
							CloneProv();
							buffermoves.AddLast(t);
							bestscore = this.getScore(dic[t]);
						}
					}

				}
				if (options.Count == 0)
				{
					if (bestscore < this.getScore(poGame))
					{
						CloneProv();
						bestscore = this.getScoreNop(poGame);
					}
				}
			}
		}

		/*private void generateTree(ArbolGener<Tuple<SabberStoneCoreAi.POGame.POGame, PlayerTask>> arbol)
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

		}*/

		private int getScore (SabberStoneCoreAi.POGame.POGame poGame)
		{
			 return new ScoreUtility { Controller = poGame.CurrentOpponent }.Rate();

		}

		private int getScoreNop(SabberStoneCoreAi.POGame.POGame poGame)
		{
			return new ScoreUtility { Controller = poGame.CurrentPlayer }.Rate();

		}

		void CloneProv()
		{
			buffermoves = new LinkedList<PlayerTask>();
			foreach (PlayerTask n in provisional){
				buffermoves.AddLast(n);
			}
		}


		public override void InitializeAgent()
		{
			Rnd = new Random(101);
			buffermoves = new LinkedList<PlayerTask>();
			provisional = new LinkedList<PlayerTask>();
		}

		public override void InitializeGame()
		{
		}
	}
}
