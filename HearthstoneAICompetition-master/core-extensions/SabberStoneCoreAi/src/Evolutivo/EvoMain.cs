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
		
		private static void Main()
		{

			//Se genera la poblacion inicial y ya se evalúa
			Population pop = new Population();

			StreamWriter er = File.CreateText("evol_1.txt");
			StreamWriter bog = File.CreateText("bestofGen_1.txt");
			StreamWriter save = File.CreateText("save_1.txt");



			for (int i = 0; i < EvoParameters.numGeneraciones; ++i)
			{

				List<Individuo> hijos = new List<Individuo>();
				for (int k = 0; k < EvoParameters.numHijos / 2; ++k)
				{
					Individuo p1, p2;
					//Se selecionan los dos padres mediante Ruleta,Rango o Torneo
					(p1, p2) = (Seleccion.Torneo(pop,EvoParameters.tamTorneo),Seleccion.Torneo(pop, EvoParameters.tamTorneo));

					//Se cruzan mediante Corte, Uniforme o Combinacion
					(Individuo h1,Individuo h2) = Cruce.Combinacion(p1, p2);

					//Se mutan mediante TotalAcotada o AleatoriaNoCota
					double prob1 = Globals.r.NextDouble();
					double prob2 = Globals.r.NextDouble();
					if (prob1 < EvoParameters.probabilidadMutacion) h1 = Mutacion.TotalAcotada(h1, EvoParameters.probabilidadMutacion / 2);
					if (prob2 < EvoParameters.probabilidadMutacion) h2 = Mutacion.TotalAcotada(h2, EvoParameters.probabilidadMutacion / 2);
					hijos.Add(h1);
					hijos.Add(h2);

				}


				pop.crearNuevaGeneracion(hijos, EvoParameters.elitismo, EvoParameters.aleatoriosInsertados);


				Individuo best = pop.getBestIndividual();

				er.Write($"Generacion {i + 1} : ");
				er.WriteLine(best.getScore());
				er.Flush();

				bog.WriteLine($"Generacion {i + 1} : ");
				
				pop.saveNBestIndividuos(5,bog);
				
				bog.WriteLine("-------------------");
				bog.Flush();
			}

			
			pop.saveNBestIndividuos(pop.getTam(),save);





		}

		
	}
}
