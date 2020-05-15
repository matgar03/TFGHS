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
    class Population
    {
        private List<Individuo> listaIndividuos;
        private readonly int elitismo = 20;
        private readonly int tam = 200;

        private readonly int aleatoriosNuevos = 60;

        public Population()
        {
            listaIndividuos = new List<Individuo>();

            for (int i = 0; i < tam; ++i)
            {
                listaIndividuos.Add(new Individuo());
            }

        }

        public Population(string path, bool cambio)
        {
            listaIndividuos = new List<Individuo>();
            if (File.Exists(path))
            {
                string[] lines = File.ReadAllLines(path);
                int cont = 0;
                double[] pesos = new double[Individuo.getNumGenes()];
                foreach (string line in lines)
                {
                    if (cont == Individuo.getNumGenes())
                    {
                        cont = 0;
                        listaIndividuos.Add(new Individuo(pesos));
                        pesos = new double[Individuo.getNumGenes()];

					}
                    else
                    {
                        pesos[cont]=(Convert.ToDouble(line));
                        ++cont;
                    }
                }
            }
            for (int i = tam; i < tam; ++i)
            {
                listaIndividuos.Add(new Individuo());
            }
        }

       

        public void saveNIndviduos(int n, StreamWriter sr)
        {
            int tope = Math.Min(n, tam);
            for (int i = 0; i < tope; ++i)
            {
                listaIndividuos[i].saveIndividual(sr);
            }
        }

		public int getTam() => tam;



		public Individuo getIndividualAt(int i) => listaIndividuos[i];

		public Individuo getBestofSubpopulation(int tamTorneo)
		{
			List<int> chosenIndex = new List<int>();
			for(int i = 0; i < tamTorneo; ++i)
			{
				chosenIndex.Add(Globals.r.Next(tam));
			}
			double maximo = 0;
			int elegido = chosenIndex[0];
			foreach( int index in chosenIndex)
			{	
				if(listaIndividuos[index].getScore()  > maximo)
				{
					maximo = listaIndividuos[index].getScore();
					elegido = index;
				}

			}

			return listaIndividuos[elegido];
		}

		public void crearNuevaGeneracion(List<Individuo> hijos, int elitismo, int aleat)
		{

			listaIndividuos.Sort(((x, y) => y.getScore().CompareTo(x.getScore())));
			listaIndividuos.RemoveRange(elitismo, tam-elitismo);
			listaIndividuos.AddRange(hijos);

			for (int i = 0; i < aleat; ++i)
			{
				listaIndividuos.Add(new Individuo());
			}

		}

		public Individuo getBestIndividual()
		{
			listaIndividuos.Sort(((x, y) => y.getScore().CompareTo(x.getScore())));
			return listaIndividuos[0];
		}




	}
}
