using SabberStoneCore.Config;
using SabberStoneCore.Enums;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.Agent.ExampleAgents;
using SabberStoneCoreAi.Meta;
using SabberStoneCoreAi.POGame;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace SabberStoneCoreAi.src.Evolutivo
{
    static class Fitness
    {
        private static int numPartidas = EvoParameters.numPartidas;
        public static float Calcular(Individuo bot)
        {
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
            Globals.WEIGHT = bot.getAttributes();
            AbstractAgent player1 = new EvoGreedy();
            AbstractAgent player2 = new GreedyAgent();
            var ida = new POGameHandler(gameConfig, player1, player2, repeatDraws: true);
            ida.PlayGames(nr_of_games: numPartidas, addResultToGameStats: true, debug: false);
            var vuelta = new POGameHandler(gameConfig, player2, player1, repeatDraws: true);
            vuelta.PlayGames(nr_of_games: numPartidas, addResultToGameStats: true, debug: false);
            GameStats stats_ida = ida.getGameStats();
            GameStats stats_vuelta = vuelta.getGameStats();
            float sol = stats_ida.PlayerA_Wins + stats_vuelta.PlayerB_Wins;
            sol /= (numPartidas * 2);
            return sol;
        }

    }
}
