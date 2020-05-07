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
            //Probar el cruce por corte
            Cruce.corte(p1, p2, 1);
            Cruce.corte(p1, p2, 5);
            Cruce.corte(p1, p2, 10);
            Cruce.corte(p1, p2, 34);
            //Probar el cruce uniforme
            Cruce.uniforme(p1, p2);
            //Probar el cruce combinacion
            Cruce.combinacion(p1, p2);
        }
    }
}
