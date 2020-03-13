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

namespace SabberStoneCoreAi
{
	internal class Program
	{

		private static void Main()
		{
			Globals.C = 0.95f;
			for(int i = 0; i < 21; ++i)
			{
				Globals.C += 0.05f;
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
				AbstractAgent player1 = new MCTSbueno();
				AbstractAgent player2 = new GreedyAgent();
				var gameHandler = new POGameHandler(gameConfig, player1, player2, repeatDraws: true);

				Console.WriteLine("Simulate Games");
				//gameHandler.PlayGame();
				gameHandler.PlayGames(nr_of_games: 20, addResultToGameStats: true, debug: false);
				GameStats gameStats = gameHandler.getGameStats();

				gameStats.printResults();
				gameStats.writeResults("resultados.txt");

			}
			Console.WriteLine("Test successful");
			Console.ReadLine();
		}
	}
}
