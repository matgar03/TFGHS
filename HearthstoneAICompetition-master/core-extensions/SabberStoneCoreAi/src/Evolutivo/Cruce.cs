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
    static class Cruce
    {
        const int NUMERO_VAR = 16;
        public static (Individuo, Individuo) cruceCorte(Individuo parent1, Individuo parent2, int numCruces)
        {
            Individuo h1, h2;
            if (numCruces > NUMERO_VAR) numCruces = NUMERO_VAR;
            bool[] cruzar = new bool[16]; //El valor por defecto es false
            for (int i = 0; i < NUMERO_VAR; ++i)
            {
                int ind;
                do
                {
                    ind = Globals.r.Next(NUMERO_VAR);
                } while (cruzar[ind]);
            }
            double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
            List<double> list_h1 = new List<double>(NUMERO_VAR);
            List<double> list_h2 = new List<double>(NUMERO_VAR);
            bool cambio = false;
            for (int i = 0; i < NUMERO_VAR; ++i)
            {
                if (!cambio)
                {
                    list_h1[i] = uno[i];
                    list_h2[i] = otro[i];
                }
                else
                {
                    list_h1[i] = otro[i];
                    list_h2[i] = uno[i];
                }
                cambio ^= cruzar[i];

            }
            h1 = new Individuo(list_h1);
            h2 = new Individuo(list_h2);
            return (h1, h2);
        }

        public static (Individuo, Individuo) cruceUniforme(Individuo parent1, Individuo parent2)
        {
            Individuo h1, h2;

            double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
            List<double> list_h1 = new List<double>(NUMERO_VAR);
            List<double> list_h2 = new List<double>(NUMERO_VAR);
            for (int i = 0; i < NUMERO_VAR; ++i)
            {
                int cual = Globals.r.Next(2);
                if (cual == 0)
                {
                    list_h1[i] = uno[i];
                    list_h2[i] = otro[i];
                }
                else
                {
                    list_h1[i] = otro[i];
                    list_h2[i] = uno[i];
                }
            }
            h1 = new Individuo(list_h1);
            h2 = new Individuo(list_h2);
            return (h1, h2);
        }

        public static (Individuo, Individuo) cruceCombinacion(Individuo parent1, Individuo parent2)
        {
            Individuo h1, h2;

            double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
            List<double> list_h1 = new List<double>(NUMERO_VAR);
            List<double> list_h2 = new List<double>(NUMERO_VAR);
            for (int i = 0; i < NUMERO_VAR; ++i)
            {
                double par = Globals.r.NextDouble();
                list_h1[i] = par * uno[i] + (1 - par) * otro[i];
                list_h2[i] = (1 - par) * uno[i] + par * otro[i];

            }
            h1 = new Individuo(list_h1);
            h2 = new Individuo(list_h2);
            return (h1, h2);
        }


    }
}