using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCoreAi.Agent;
using System.IO;

namespace SabberStoneCoreAi.src.Evolutivo
{
    class Individuo
    {

        private float indScore = 0f;

		private static readonly int numGenes = EvoParameters.numGenes;

		private double[] pesos;


        //individuo dado una lista de pesos

        public Individuo(double[] pesos)
        {
			this.pesos = new double[numGenes];
			Array.Copy(pesos, this.pesos, numGenes);
			indScore = Fitness.Calcular(this);

        }
        //individuo aleatorio
        public Individuo()
        {

			pesos = new double[numGenes];
			for(int i = 0; i < numGenes; ++i)
			{
				pesos[i] = Globals.r.NextDouble();
			}
			indScore = Fitness.Calcular(this);
        }

        


        public double[] getAttributes()
        {
			double[] attributes = new double[numGenes];
			Array.Copy(pesos, attributes, numGenes);
			return attributes;
        }

		public void saveIndividual(StreamWriter sr)
		{
			for (int i = 0; i < numGenes; ++i)
			{
				sr.WriteLine(pesos[i]);
				sr.Flush();
			}
			sr.WriteLine(indScore);
			sr.Flush();
		}

		public double getScore() => indScore;
		public static int getNumGenes() => numGenes;
    }
}
