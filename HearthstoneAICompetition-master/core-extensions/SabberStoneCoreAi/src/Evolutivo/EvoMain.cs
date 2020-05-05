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
    class EvoMain
    {
        private static bool cambio = false; //si se cambia el número de partidas el score guardado no es válido y hay que volver a calcularlo
        private static int numPartidas = 100;
        private static int popSize = 200;
        private static List<Individuo> population;
        private static List<float> scores;
        private static int num_gen = 2;

        static float fitness(Individuo bot)
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

        static void init()
        {
            population = new List<Individuo>();
            scores = new List<float>();
            //Creacion de la poblacion inicial (random)
            for (int i = 0; i < popSize; ++i)
            {
                population.Add(new Individuo());
            }
        }

        static void save_Population()
        {
            StreamWriter sr = File.CreateText("save.txt");
            for (int i = 0; i < popSize; ++i)
            {
                writeResul(sr, i);
            }
        }

        static void load_Population()
        {
            population = new List<Individuo>();
            string[] lines = File.ReadAllLines("save.txt");
            int cont = 0;
            List<double> pesos = new List<double>();
            foreach (string line in lines)
            {
                if (cont == 16)
                {
                    cont = 0;
                    population.Add(new Individuo(pesos));
                    pesos = new List<double>();
                    if (!cambio)
                        population[population.Count - 1].indScore = float.Parse(line);
                    else
                        population[population.Count - 1].indScore = fitness(population[population.Count - 1]);
                }
                else
                {
                    pesos.Add(Convert.ToDouble(line));
                    ++cont;
                }
            }
            for (int i = population.Count; i < popSize; ++i)
            {
                population.Add(new Individuo());
                population[population.Count - 1].indScore = fitness(population[population.Count - 1]);
            }
        }

        static void normalizeScores()
        {
            scores = new List<float>();
            for (int i = 0; i < popSize; ++i)
            {
                scores.Add(population[i].indScore);
            }
            //Acumulamos
            for (int i = 1; i < popSize; ++i)
            {
                scores[i] += scores[i - 1];
            }
            //Dividimos
            for (int i = 0; i < popSize; ++i)
            {
                scores[i] /= scores[popSize - 1];
            }
        }
        //El padre y madre seleccionados pueden ser los mismos
        static Tuple<Individuo, Individuo> selection()
        {
            double r1 = Globals.r.NextDouble();
            double r2 = Globals.r.NextDouble();
            Individuo padre = null, madre = null;
            if (r2 < r1)
            {
                double aux = r1;
                r1 = r2;
                r2 = aux;
            }
            for (int i = 0; i < popSize; ++i)
            {

                if (r1 < scores[i] && padre == null)
                {
                    padre = population[i];
                }

                if (r2 < scores[i])
                {
                    madre = population[i];
                    break;
                }

            }
            return new Tuple<Individuo, Individuo>(padre, madre);
        }

        static void simulate(List<Individuo> pob)
        {
            for (int i = 0; i < popSize; ++i)
            {
                pob[i].indScore = fitness(pob[i]);
            }
        }
        static void writeResul(StreamWriter sr, int index)
        {
            double[] stats = population[index].getAttributes();
            for (int i = 0; i < stats.Length; ++i)
            {
                sr.WriteLine(stats[i]);
            }
            sr.WriteLine(population[index].indScore);
            sr.Flush();
        }
        private static void Main()
        {
            load_Population();
            StreamWriter er = File.CreateText("evol_1.txt");
            StreamWriter bog = File.CreateText("bestofGen_1.txt");

            for (int i = 0; i < num_gen; ++i)
            {
                normalizeScores();
                List<Individuo> new_pop = new List<Individuo>();
                for (int k = 0; k < popSize; ++k)
                {
                    Tuple<Individuo, Individuo> parents = selection();
                    //Primero se crea un individuo cruzando los padres y luego se muta
                    new_pop.Add(new Individuo(new Individuo(parents.Item1, parents.Item2)));
                }
                simulate(new_pop);
                List<Individuo> sort_pop = new List<Individuo>(popSize * 2);
                sort_pop.AddRange(population);
                sort_pop.AddRange(new_pop);
                sort_pop.Sort((x, y) => y.indScore.CompareTo(x.indScore));
                sort_pop.RemoveRange(popSize, popSize);
                population = sort_pop;
                er.Write($"Generacion {i + 1} : ");
                er.WriteLine(population[0].indScore);
                bog.WriteLine($"Generacion {i + 1} : ");
                writeResul(bog, 0);
                bog.WriteLine("-------------------");
                bog.Flush();
                er.Flush();
            }
            save_Population();





        }
    }
}
