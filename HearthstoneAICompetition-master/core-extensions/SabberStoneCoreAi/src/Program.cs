#region copyright
// SabberStone, Hearthstone Simulator in C# .NET Core
// Copyright (C) 2017-2019 SabberStone Team, darkfriend77 & rnilva
//
// SabberStone is free software: you can redistribute it and/or modify
// it under the terms of the GNU Affero General Public License as
// published by the Free Software Foundation, either version 3 of the
// License.
// SabberStone is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU Affero General Public License for more details.
#endregion
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

namespace SabberStoneCoreAi
{
    internal class Program
    {

        private static void Main()
        {
			
			//double[] pesos2 = {0.7243816575319507, 0.45470054687573397, 0.8004099650018286, 0.16344587459929505, 0.600089147728018, 0.8319472484059762,
			//		 0.11447438262562208, 0.5321763012524791, .6199096492389915, 0.7243816575319507, 0.605734885977482, 0.13754218984893232, 0.1277260279552838};
			//double[] pesos3 = {0.7067835339450543, 0.5479739324090757,0.7428480567874419, 0.16325367686068742, 0.6002174999258411,  .7774106137937544,
			//	0.123383518404457, 0.4165603232209667, 0.6506833410056059, 0.5872092414996068, 0.5908417466806593, 0.2126874368218829, 0.17426894323089565};
			/*
			Globals.C = 0.90f;
            for (int i = 0; i < 21; ++i)
            {
                Globals.C += 0.1f;
                Console.WriteLine($"Iteración {i}");
                Console.WriteLine("Setup gameConfig");
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

                Console.WriteLine("Setup POGameHandler");
                AbstractAgent player1 = new MCTS2(Globals.C, "evo", pesos3);
                AbstractAgent player2 = new GreedyAgent();
                var gameHandler1 = new POGameHandler(gameConfig, player1, player2, repeatDraws: true);

                Console.WriteLine("Simulate Games");
                //gameHandler.PlayGame();
                gameHandler1.PlayGames(nr_of_games: 20, addResultToGameStats: true, debug: false);
                GameStats gameStats1 = gameHandler1.getGameStats();

                gameStats1.printResults();
                //gameStats1.writeResults("resultados_c_mcts2_local.txt");


				var gameHandler2 = new POGameHandler(gameConfig, player2, player1, repeatDraws: true);

				Console.WriteLine("Simulate Games");
				//gameHandler.PlayGame();
				gameHandler2.PlayGames(nr_of_games: 20, addResultToGameStats: true, debug: false);
				GameStats gameStats2 = gameHandler2.getGameStats();

				gameStats2.printResults();
				//gameStats2.writeResults("resultados_c_mcts2_visitante.txt");
			}
			*/
			
			
            // esto era para probar el nuevo MCTS
            Console.WriteLine("Setup gameConfig");
			Globals.MAX_TIME = 10;
			for (int i = 0; i < 1; ++i) { 
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
				
				double[] pesos = {0.8844121293557864, 0.5722002329175362, 0.3038759251608867, 0.3258540142913601, 0.49661011923877996, 0.5702691169317202,
					0.14050075930566563, 0.5138821958163204, 0.6154015355815187, 0.12601818057057362, 0.6913895680063356, 0.029961458886955613, 0.2187224352819484 };
				Console.WriteLine("Setup POGameHandler");
				AbstractAgent player1 = new MCTS2(1.5, "evo",pesos);
				AbstractAgent player2 = new GreedyAgent();

				var gameHandler1 = new POGameHandler(gameConfig, player1, player2, repeatDraws: true);
				Console.WriteLine("Simulate Games");
				gameHandler1.PlayGames(nr_of_games: 5, addResultToGameStats: true, debug: false);
				GameStats gameStats1 = gameHandler1.getGameStats();

				var gameHandler2 = new POGameHandler(gameConfig, player2, player1, repeatDraws: true);
				Console.WriteLine("Simulate Games");
				gameHandler2.PlayGames(nr_of_games: 5, addResultToGameStats: true, debug: false);
				GameStats gameStats2 = gameHandler2.getGameStats();

				gameStats1.printResults();
				//gameStats1.writeResults("resultados_mcts2_local24.txt");

				gameStats2.printResults();
				//gameStats2.writeResults("resultados_mcts2_visitante24.txt");
				Globals.MAX_TIME *= 10;
			}
			
		}
    }
}
