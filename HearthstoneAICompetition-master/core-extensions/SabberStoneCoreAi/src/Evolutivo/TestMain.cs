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
            p1= Mutacion.Scramble(p1,EvoParameters.tamScramble);
        }
    }
}
