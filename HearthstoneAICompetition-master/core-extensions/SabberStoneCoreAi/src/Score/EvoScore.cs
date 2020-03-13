using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Score
{
    public class EvoScore : Score
    {
        List<double> weight { get; set; }

        public double Rate()
        {
            if (OpHeroHp < 1)
                return Double.MaxValue;

            if (HeroHp < 1)
                return Double.MinValue;

            double myRes = 0, opRes = 0;

            //Salud
            myRes = (weight[0] * Math.Sqrt(HeroHp));
            opRes = (weight[0] * Math.Sqrt(OpHeroHp));

            //Ventaja de cartas
            myRes += weight[1] * HandCnt;
            if (HandCnt > 3) myRes -= HandCnt - weight[1];
            opRes += weight[1] * OpHandCnt;
            if (OpHandCnt > 3) opRes -= OpHandCnt - weight[1];

            //Cartas en Mazo (igual hace falta ponerlo para que solo lo compruebe con la partida bastante avanzada)
            if (Controller.BoardZone.Game.Turn > 10)
            {
                myRes += weight[2] * (Math.Sqrt(DeckCnt) - Controller.Hero.Fatigue);
                opRes += weight[2] * (Math.Sqrt(OpDeckCnt) - Controller.Opponent.Hero.Fatigue);
            }
            //Estudiar Minions (se puede ampliar teniendo en cuenta las habilidades)

            myRes += getMinionValues(false);
            opRes += getMinionValues(true);

            // Ver si el oponente tiene el campo libre
            if (OpBoardZone.Count == 0)
                myRes += weight[3] * Math.Min(10, Controller.BoardZone.Game.Turn);
            if (BoardZone.Count == 0)
                opRes += weight[3] * Math.Min(10, Controller.BoardZone.Game.Turn);



            return myRes - opRes;
        }

        public override Func<List<IPlayable>, List<int>> MulliganRule()
        {
            return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
        }


        double getValueForMinion(Minion minion)
        {
            double value = 0;
            value += weight[4] * minion.Health;
            if (!minion.IsFrozen) value += weight[5] * minion.AttackDamage;

            if (minion.HasTaunt) value += weight[6] * minion.Health;
            if (minion.Poisonous) value += weight[7];
            if (minion.HasDeathrattle) value += weight[8];
            if (minion.HasInspire) value += weight[9];
            if (minion.HasDivineShield) value += weight[10];
            if (minion.HasLifeSteal) value += weight[11] * minion.AttackDamage;
            if (minion.HasCharge) value += weight[12] * minion.AttackDamage;
            if (minion.HasStealth) value += weight[13] * minion.AttackDamage;
            if (minion.HasBattleCry) value += weight[14];
            if (minion.HasWindfury) value += weight[15] * minion.AttackDamage;

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
