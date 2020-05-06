using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCoreAi.Agent;

namespace SabberStoneCoreAi.src.Evolutivo
{
    class Individuo
    {

        public float indScore { get; set; } = 0;

        const double mutProp = 0.1;
        const double ini_mut = 0.2;
        double vida;
        double numMano;
        double numDeck;
        double clearOpBoard;

        double minionLife;
        double minionAtq;

        double m_prov;
        double m_ven;
        double m_ultAli;
        double m_insp;
        double m_escDiv;
        double m_lifesteal;
        double m_cargar;
        double m_stealth;
        double m_battlecry;
        double m_windFury;


        //individuo dado una lista de pesos

        public Individuo(List<double> pesos)
        {
            vida = pesos[0];
            numMano = pesos[1];
            numDeck = pesos[2];
            clearOpBoard = pesos[3];

            minionLife = pesos[4];
            minionAtq = pesos[5];

            m_prov = pesos[6];
            m_ven = pesos[7];
            m_ultAli = pesos[8];
            m_insp = pesos[9];
            m_escDiv = pesos[10];
            m_lifesteal = pesos[11];
            m_cargar = pesos[12];
            m_stealth = pesos[13];
            m_battlecry = pesos[14];
            m_windFury = pesos[15];
        }
        //individuo aleatorio
        public Individuo()
        {
            vida = Globals.r.NextDouble();
            numMano = Globals.r.NextDouble();
            numDeck = Globals.r.NextDouble();
            clearOpBoard = Globals.r.NextDouble();

            minionLife = Globals.r.NextDouble();
            minionAtq = Globals.r.NextDouble();

            m_prov = Globals.r.NextDouble();
            m_ven = Globals.r.NextDouble();
            m_ultAli = Globals.r.NextDouble();
            m_insp = Globals.r.NextDouble();
            m_escDiv = Globals.r.NextDouble();
            m_lifesteal = Globals.r.NextDouble();
            m_cargar = Globals.r.NextDouble();
            m_stealth = Globals.r.NextDouble();
            m_battlecry = Globals.r.NextDouble();
            m_windFury = Globals.r.NextDouble();
        }

        //individuo con dos padres
        public Individuo(Individuo p1, Individuo p2)
        {
            vida = Globals.r.Next(2) == 0 ? p1.vida : p2.vida;
            numMano = Globals.r.Next(2) == 0 ? p1.numMano : p2.numMano;
            numDeck = Globals.r.Next(2) == 0 ? p1.numDeck : p2.numDeck;
            clearOpBoard = Globals.r.Next(2) == 0 ? p1.clearOpBoard : p2.clearOpBoard;

            minionLife = Globals.r.Next(2) == 0 ? p1.minionLife : p2.minionLife;
            minionAtq = Globals.r.Next(2) == 0 ? p1.minionAtq : p2.minionAtq;

            m_prov = Globals.r.Next(2) == 0 ? p1.m_prov : p2.m_prov;
            m_ven = Globals.r.Next(2) == 0 ? p1.m_ven : p2.m_ven;
            m_ultAli = Globals.r.Next(2) == 0 ? p1.m_ultAli : p2.m_ultAli;
            m_insp = Globals.r.Next(2) == 0 ? p1.m_insp : p2.m_insp;
            m_escDiv = Globals.r.Next(2) == 0 ? p1.m_escDiv : p2.m_escDiv;
            m_lifesteal = Globals.r.Next(2) == 0 ? p1.m_lifesteal : p2.m_lifesteal;
            m_cargar = Globals.r.Next(2) == 0 ? p1.m_cargar : p2.m_cargar;
            m_stealth = Globals.r.Next(2) == 0 ? p1.m_stealth : p2.m_stealth;
            m_battlecry = Globals.r.Next(2) == 0 ? p1.m_battlecry : p2.m_battlecry;
            m_windFury = Globals.r.Next(2) == 0 ? p1.m_windFury : p2.m_windFury;

        }

        //individuo mutado
        public Individuo(Individuo ind)
        {
            bool no_cambio = false;
            if (Globals.r.NextDouble() > ini_mut) no_cambio = true; ;
            vida = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.vida : Globals.r.NextDouble();
            numMano = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.numMano : Globals.r.NextDouble();
            numDeck = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.numDeck : Globals.r.NextDouble();
            clearOpBoard = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.clearOpBoard : Globals.r.NextDouble();

            minionLife = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.minionLife : Globals.r.NextDouble();
            minionAtq = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.minionAtq : Globals.r.NextDouble();

            m_prov = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_prov : Globals.r.NextDouble();
            m_ven = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_ven : Globals.r.NextDouble();
            m_ultAli = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_ultAli : Globals.r.NextDouble();
            m_insp = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_insp : Globals.r.NextDouble();
            m_escDiv = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_escDiv : Globals.r.NextDouble();
            m_lifesteal = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_lifesteal : Globals.r.NextDouble();
            m_cargar = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_cargar : Globals.r.NextDouble();
            m_stealth = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_stealth : Globals.r.NextDouble();
            m_battlecry = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_battlecry : Globals.r.NextDouble();
            m_windFury = (Globals.r.NextDouble() > mutProp || no_cambio) ? ind.m_windFury : Globals.r.NextDouble();
        }


        public double[] getAttributes()
        {
            return new double[] { vida, numMano, numDeck, clearOpBoard, minionLife,
                minionAtq, m_prov, m_ven, m_ultAli, m_insp, m_escDiv, m_lifesteal,
                m_cargar, m_stealth, m_battlecry, m_windFury };
        }
    }
}
