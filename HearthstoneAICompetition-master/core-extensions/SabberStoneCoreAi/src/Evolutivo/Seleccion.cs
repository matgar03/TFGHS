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
	class Seleccion
	{

		//por testear
		private static (double, int)[] obtenerPuntuacionIndividuo(Population pop)
		{
			(double, int)[] scoreAndIndividual = new (double, int)[pop.getTam()];
			for (int i = 0; i < pop.getTam(); ++i)
			{
				scoreAndIndividual[i] = (pop.getIndividualAt(i).getScore(), i);
			}
			return scoreAndIndividual;

		}

		//por testear
		public static Individuo Ruleta(Population pop)
		{
			(double, int)[] scoreAndIndividual = obtenerPuntuacionIndividuo(pop);


			double total = 0;

			for(int i = 0; i < pop.getTam(); ++i)
				total += scoreAndIndividual[i].Item1;

			double acum = 0;
			for (int i = 0; i < pop.getTam(); ++i)
			{

				 acum+= scoreAndIndividual[i].Item1/total;
				scoreAndIndividual[i].Item1 = acum;

			}

			double escogido = Globals.r.NextDouble();
			for(int i = 0; i < pop.getTam(); ++i)
			{
				if(scoreAndIndividual[i].Item1 >= escogido)
				{
					return pop.getIndividualAt(scoreAndIndividual[i].Item2);
				}
				
			}
			return pop.getIndividualAt(pop.getTam()-1);
		}

		//por testear
		public static Individuo Rango(Population pop)
		{
			(double, int)[] scoreAndIndividual = obtenerPuntuacionIndividuo(pop);
			Array.Sort(scoreAndIndividual);
			double sumDeRangos = (pop.getTam() * (pop.getTam() - 1)) / 2;

			double acum = 0;
			for (int i = 0; i < pop.getTam(); ++i)
			{

				acum += (i+1)/ sumDeRangos;
				scoreAndIndividual[i].Item1 = acum;

			}

			double escogido = Globals.r.NextDouble();
			for (int i = 0; i < pop.getTam(); ++i)
			{
				if (scoreAndIndividual[i].Item1 >= escogido)
				{
					return pop.getIndividualAt(scoreAndIndividual[i].Item2);
				}

			}


			return pop.getIndividualAt(scoreAndIndividual[pop.getTam()-1].Item2);

		}

		//por testear
		public static Individuo Torneo(Population pop, int tamTorneo)
		{
			return pop.getBestofSubpopulation(tamTorneo);
		}

		
	}
}
