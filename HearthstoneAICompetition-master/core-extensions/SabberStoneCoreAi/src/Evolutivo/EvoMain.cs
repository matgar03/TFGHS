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
        private static bool cambio = true; //si se cambia el número de partidas el score guardado no es válido y hay que volver a calcularlo
        private static int numPartidas = 100;
        private static int popSize = 200;
        private static List<Individuo> population;
        private static List<float> scores;
        private static int num_gen = 30;
        private static int num_Padres = 60;
        private static int num_hijos = 90;
        private static int num_al = 50;
        private static double Mut_prop = 0.2;

        private static float fitness(Individuo bot)
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

        private static void save_Population()
        {
            StreamWriter sr = File.CreateText("save.txt");
            for (int i = 0; i < popSize; ++i)
            {
                writeResul(sr, i);
            }
        }

        private static void load_Population()
        {
            population = new List<Individuo>();
            string fileName = "save.txt";
            if (File.Exists(fileName))
            {
                string[] lines = File.ReadAllLines(fileName);
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
            }
            for (int i = population.Count; i < popSize; ++i)
            {
                population.Add(new Individuo());
                population[population.Count - 1].indScore = fitness(population[population.Count - 1]);
            }
        }

        private static void normalizeScores()
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
        private static (Individuo, Individuo) selection()
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
            return (padre, madre);
        }

        private static void simulate(List<Individuo> pob)
        {
            for (int i = 0; i < pob.Count; ++i)
            {
                pob[i].indScore = fitness(pob[i]);
            }
        }
        private static void writeResul(StreamWriter sr, int index)
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
            population.Sort(((x, y) => y.indScore.CompareTo(x.indScore)));
            StreamWriter er = File.CreateText("evol_0.txt");
            StreamWriter bog = File.CreateText("bestofGen_0.txt");

            for (int i = 0; i < num_gen; ++i)
            {
                normalizeScores();

                List<Individuo> hijos = new List<Individuo>();
                for (int k = 0; k < num_hijos / 2; ++k)
                {
                    Individuo p1, p2;
                    (p1, p2) = selection();
                    //Primero se crea dos cruzando los padres y luego se muta
                    Individuo h1;
                    Individuo h2;
                    (h1, h2) = Cruce.combinacion(p1, p2);
                    double prob1 = Globals.r.NextDouble();
                    double prob2 = Globals.r.NextDouble();
                    if (prob1 < Mut_prop) h1 = Mutacion.totalAcotada(h1, Mut_prop / 2);
                    if (prob2 < Mut_prop) h2 = Mutacion.totalAcotada(h2, Mut_prop / 2);
                    hijos.Add(h1);
                    hijos.Add(h2);

                }
                simulate(hijos);

                List<Individuo> aleat = new List<Individuo>();
                for (int k = 0; k < num_al; ++k)
                {
                    aleat.Add(new Individuo());
                }
                simulate(aleat);

                List<Individuo> newPop = new List<Individuo>(popSize);
                population.RemoveRange(num_Padres, popSize - num_Padres);
                newPop.AddRange(population);
                newPop.AddRange(hijos);
                newPop.AddRange(aleat);
                population = newPop;
                population.Sort(((x, y) => y.indScore.CompareTo(x.indScore)));


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
