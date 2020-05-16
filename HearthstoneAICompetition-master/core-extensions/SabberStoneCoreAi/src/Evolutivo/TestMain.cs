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
    class TestMain
    {

        private static void Main()
        {
            Individuo p1 = new Individuo();
            Individuo p2 = new Individuo();
            Individuo h1, h2;
            //Probar el cruce por corte
            Cruce.Corte(p1, p2, 1);
            Cruce.Corte(p1, p2, 5);
            Cruce.Corte(p1, p2, 10);
            Cruce.Corte(p1, p2, 34);
            //Probar el cruce uniforme
            Cruce.Uniforme(p1, p2);
            //Probar el cruce combinacion
            (h1, h2) = Cruce.Combinacion(p1, p2);
            Mutacion.TotalAcotada(p1, 0.5);
            Mutacion.AleatoriaNoCota(p1);
        }
    }
}
