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

        public static Individuo totalAcotada(Individuo ind, double prob_mut)
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
                    res_list.Add(punt[i] + variacion);
                }
                else res_list.Add(punt[i]);

            }
            res = new Individuo(res_list);
            return res;
        }

        public static Individuo aleatoriaNoCota(Individuo ind)
        {
            Individuo res;
            double[] punt = ind.getAttributes();
            List<double> res_list = new List<double>();
            int pos = Globals.r.Next(punt.Length);
            punt[pos] = Globals.r.NextDouble();

            for (int i = 0; i < punt.Length; ++i)
            {
                res_list.Add(punt[i]);

            }
            res = new Individuo(res_list);
            return res;
        }


    }
}
