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
        
        public static (Individuo, Individuo) Corte(Individuo parent1, Individuo parent2, int numCruces)
        {
            Individuo h1, h2;
			int NUMERO_VAR = Individuo.getNumGenes();
            if (numCruces > NUMERO_VAR) numCruces = NUMERO_VAR;
            bool[] cruzar = new bool[NUMERO_VAR]; //El valor por defecto es false
            for (int i = 0; i < numCruces; ++i)
            {
                int ind;
                do
                {
                    ind = Globals.r.Next(NUMERO_VAR);
                } while (cruzar[ind]);
				cruzar[ind] = true;
            }
            double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
            double[] list_h1 = new double[NUMERO_VAR];
            double[] list_h2 = new double[NUMERO_VAR];
            bool cambio = false;
            for (int i = 0; i < NUMERO_VAR; ++i)
            {
                if (!cambio)
                {
                    list_h1[i]= uno[i];
                    list_h2[i]= otro[i];
                }
                else
                {
                    list_h1[i]= otro[i];
                    list_h2[i]= uno[i];
                }
                cambio ^= cruzar[i];

            }
            h1 = new Individuo(list_h1);
            h2 = new Individuo(list_h2);
            return (h1, h2);
        }

        public static (Individuo, Individuo) Uniforme(Individuo parent1, Individuo parent2)
        {
            Individuo h1, h2;


			int NUMERO_VAR = Individuo.getNumGenes();

			double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
			double[] list_h1 = new double[NUMERO_VAR];
			double[] list_h2 = new double[NUMERO_VAR];
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

        public static (Individuo, Individuo) Combinacion(Individuo parent1, Individuo parent2)
        {
            Individuo h1, h2;

			int NUMERO_VAR = Individuo.getNumGenes();

			double[] uno = parent1.getAttributes();
            double[] otro = parent2.getAttributes();
			double[] list_h1 = new double[NUMERO_VAR];
			double[] list_h2 = new double[NUMERO_VAR];
			for (int i = 0; i < NUMERO_VAR; ++i)
            {
                double par = Globals.r.NextDouble();
                list_h1[i] = (par * uno[i] + (1 - par) * otro[i]);
                list_h2[i] = ((1 - par) * uno[i] + par * otro[i]);

            }
            h1 = new Individuo(list_h1);
            h2 = new Individuo(list_h2);
            return (h1, h2);
        }


    }
}
