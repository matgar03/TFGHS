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
    static class Mutacion
    {

        public static Individuo TotalAcotada(Individuo ind, double prob_mut)
        {
            Individuo res;
            double[] punt = ind.getAttributes();
            List<double> res_list = new List<double>();
            for (int i = 0; i < punt.Length; ++i)
            {
                double cambio = Globals.r.NextDouble();
                if (cambio <= prob_mut)
                {
                    double variacion = (Globals.r.NextDouble() - 0.5) / 5;
                    double nuevo = Math.Clamp(punt[i] + variacion, 0, 1);
                    punt[i] = nuevo;
                }
                else res_list.Add(punt[i]);

            }
            res = new Individuo(punt);
            return res;
        }

        public static Individuo AleatoriaNoCota(Individuo ind)
        {
            Individuo res;
            double[] punt = ind.getAttributes();
            
            int pos = Globals.r.Next(punt.Length);
            punt[pos] = Globals.r.NextDouble();

            res = new Individuo(punt);
            return res;
        }

		public static Individuo Scramble(Individuo ind, int longScramble)
		{
			Individuo res;
			double[] punt = ind.getAttributes();
			int[] chosen = new int[longScramble];
			for(int i = 0; i < longScramble; ++i)
			{
				chosen[i] = Globals.r.Next(punt.Length);
			}
			int[] newLoc = new int[longScramble];
			Array.Copy(chosen, newLoc, longScramble);
			double[] new_punt = new double[punt.Length];
			Array.Copy(punt, new_punt, punt.Length);
			Shuffle(Globals.r, newLoc);
			for (int i = 0; i < longScramble; ++i)
			{
				new_punt[chosen[i]] = punt[newLoc[i]];
			}
			res = new Individuo(new_punt);
			return res;

		}

		private static void Shuffle(Random rng, int[] array)
		{
			int n = array.Length;
			while (n > 1)
			{
				int k = rng.Next(n--);
				int temp = array[n];
				array[n] = array[k];
				array[k] = temp;
			}
		}

	}
}
