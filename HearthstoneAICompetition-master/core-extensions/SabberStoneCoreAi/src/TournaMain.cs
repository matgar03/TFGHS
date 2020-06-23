using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.POGame;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Meta;
using SabberStoneCoreAi.Competition.Agents;
using SabberStoneCoreAi.src.Agent;
using SabberStoneCoreAi.src;
using System.IO;

namespace SabberStoneCoreAi.src
{
	internal class TournaMain
	{
		private static void Main()
		{
			double[] pesos1 = {0.8844121293557864, 0.5722002329175362, 0.3038759251608867, 0.3258540142913601, 0.49661011923877996, 0.5702691169317202,
					0.14050075930566563, 0.5138821958163204, 0.6154015355815187, 0.12601818057057362, 0.6913895680063356, 0.029961458886955613, 0.2187224352819484 };
			Globals.MAX_TIME = 1000;
			Console.WriteLine("Setup gameConfig");
			//seleccionamos el mazo con el que se va a jugar
			var gameConfig = new GameConfig()
			{
				StartPlayer = 1,
				Player1HeroClass = CardClass.SHAMAN,
				Player2HeroClass = CardClass.SHAMAN,
				Player1Deck = Decks.MidrangeJadeShaman,
				Player2Deck = Decks.MidrangeJadeShaman,
				Shuffle = true,
				Logging = false
			};
			//configuramos los participantes del torneo
			Console.WriteLine("Setup POGameHandler");
			AbstractAgent[] agents = new AbstractAgent[3];
			/*
			agents[0] = new GreedyAgent();
			agents[1] = new SimTreeAgent(0.5, "midrange");
			agents[2] = new SimTreeAgent(0.75, "utility");
			agents[3] = new MCTS1(2.4, "utility");
			agents[4] = new MCTS1(2.9, "midrange");
			agents[5] = new MCTS2(1.5, "utility");
			agents[6] = new MCTS2(3.0, "utility");
			*/
			agents[0] = new MCTS2(3.0, "utility");
			agents[1] = new MCTS2(1.9, "evo",pesos1);
			agents[2] = new MCTS2(2.4, "evo",pesos1);
			string[] nombres = new string[3];
			/*
			nombres[0] = "Greedy agent";
			nombres[1] = "Simtree agent 0.5 midrange";
			nombres[2] = "Simtree agent 0.75 utility";
			nombres[3] = "MCTS1 agent 2.4 utility";
			nombres[4] = "MCTS1 agent 2.9 midrange";
			nombres[5] = "MCTS2 agent 1.5 utility";
			nombres[6] = "MCTS2 agent 3.0 utility";
			*/
			nombres[0] = "MCTS2 agent 3.0 utility";
			nombres[1] = "MCTS2 agent 1.9 evo";
			nombres[2] = "MCTS2 agent 2.4 evo";
			for (int i = 0; i < agents.Length; ++i)
			{
				for (int j = i+1 ; j < agents.Length; ++j)
				{
					var gameHandler1 = new POGameHandler(gameConfig, agents[i], agents[j], repeatDraws: true);
					var gameHandler2 = new POGameHandler(gameConfig, agents[j], agents[i], repeatDraws: true);

					Console.WriteLine("Simulate Games");
					gameHandler1.PlayGames(nr_of_games: 50, addResultToGameStats: true, debug: false);
					gameHandler2.PlayGames(nr_of_games: 50, addResultToGameStats: true, debug: false);

					GameStats gameStats1 = gameHandler1.getGameStats();
					GameStats gameStats2 = gameHandler2.getGameStats();

					using (System.IO.StreamWriter file = File.AppendText("Resultados_torneo_1000.txt"))
					{
						file.WriteLine($"{nombres[i]} vs {nombres[j]}");
					}
					gameStats1.writeResults("Resultados_torneo_1000.txt");
					using (System.IO.StreamWriter file = File.AppendText("Resultados_torneo_1000.txt"))
					{
						file.WriteLine($"{nombres[j]} vs {nombres[i]}");
					}
					gameStats2.writeResults("Resultados_torneo_1000.txt");
				}
			}
		}

	}
}
