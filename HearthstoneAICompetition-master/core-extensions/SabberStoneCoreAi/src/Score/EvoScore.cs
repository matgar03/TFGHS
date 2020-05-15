using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model.Entities;
using SabberStoneCoreAi.Agent;

namespace SabberStoneCoreAi.Score
{
    public class EvoScore : Score
    {
		/*
		0: vida
		1: cartas en mano
		2: cartas en el mazo
		3: el oponente tiene el campo vacío
		4: vida del minion
		5: ataque del minion
		---- habilidades de los minions ----
		6: tener provocar/
		7: tener último aliento (algo al morir)/
		8: tener escudo divino/
		9: tener cargar/
		10: tener grito de batalla/
		11: tener viento furioso/
		*/

        public double Rate()
        {
            if (OpHeroHp < 1)
                return Double.MaxValue;

            if (HeroHp < 1)
                return Double.MinValue;

            double myRes = 0, opRes = 0;

            //Salud
            myRes = (Globals.WEIGHT[0] * Math.Sqrt(HeroHp));
            opRes = (Globals.WEIGHT[0] * Math.Sqrt(OpHeroHp));

            //Ventaja de cartas
            myRes += Globals.WEIGHT[1] * HandCnt;
            if (HandCnt > 3) myRes -= HandCnt - Globals.WEIGHT[1];
            opRes += Globals.WEIGHT[1] * OpHandCnt;
            if (OpHandCnt > 3) opRes -= OpHandCnt - Globals.WEIGHT[1];

            //Cartas en Mazo (igual hace falta ponerlo para que solo lo compruebe con la partida bastante avanzada)
            if (Controller.BoardZone.Game.Turn > 10)
            {
                myRes += Globals.WEIGHT[2] * (Math.Sqrt(DeckCnt) - Controller.Hero.Fatigue);
                opRes += Globals.WEIGHT[2] * (Math.Sqrt(OpDeckCnt) - Controller.Opponent.Hero.Fatigue);
            }
            //Estudiar Minions (se puede ampliar teniendo en cuenta las habilidades)

            myRes += getMinionValues(false);
            opRes += getMinionValues(true);

            // Ver si el oponente tiene el campo libre
            if (OpBoardZone.Count == 0)
                myRes += Globals.WEIGHT[3] * Math.Min(10, Controller.BoardZone.Game.Turn);
            if (BoardZone.Count == 0)
                opRes += Globals.WEIGHT[3] * Math.Min(10, Controller.BoardZone.Game.Turn);



            return myRes - opRes;
        }

        public override Func<List<IPlayable>, List<int>> MulliganRule()
        {
            return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
        }


        double getValueForMinion(Minion minion)
        {
            double value = 0;
            value += Globals.WEIGHT[4] * minion.Health;
            if (!minion.IsFrozen) value += Globals.WEIGHT[5] * minion.AttackDamage;

            if (minion.HasTaunt) value += Globals.WEIGHT[6] * minion.Health;
            if (minion.HasDeathrattle) value += Globals.WEIGHT[7];
            if (minion.HasDivineShield) value += Globals.WEIGHT[8];
            if (minion.HasCharge) value += Globals.WEIGHT[9] * minion.AttackDamage;
            if (minion.HasBattleCry) value += Globals.WEIGHT[10];
            if (minion.HasWindfury) value += Globals.WEIGHT[11] * minion.AttackDamage;

            return value;
        }

        double getMinionValues(bool oponent)
        {
            double value = 0;
            Controller cont = Controller;
            if (oponent) cont = Controller.Opponent.Controller;
            foreach (Minion m in cont.BoardZone)
            {
                value += getValueForMinion(m);
            }

            return value;
        }
    }
}
