
using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model.Entities;

namespace SabberStoneCoreAi.Score
{
	public class ScoreUtility : Score
	{
		public override int Rate()
		{
			if (OpHeroHp < 1)
				return int.MaxValue;

			if (HeroHp < 1)
				return int.MinValue;

			int myRes = 0, opRes=0;

			//Salud
			myRes = (int) (2 * Math.Sqrt(HeroHp));
			opRes = (int)(2 * Math.Sqrt(OpHeroHp));

			//Ventaja de cartas
			myRes += 3 * HandCnt;
			if (HandCnt > 3) myRes -= HandCnt - 3;
			opRes += 3 * OpHandCnt;
			if (OpHandCnt > 3) opRes -= OpHandCnt - 3;

			//Cartas en Mazo (igual hace falta ponerlo para que solo lo compruebe con la partida bastante avanzada)
			myRes += (int) Math.Sqrt(DeckCnt) - Controller.Hero.Fatigue;
			opRes += (int)Math.Sqrt(OpDeckCnt) - Controller.Opponent.Hero.Fatigue;

			

			// Ver si el oponente tiene el campo libre
			if (OpBoardZone.Count == 0)
				myRes += 2 + Math.Min(10, Controller.BoardZone.Game.Turn);
			if (BoardZone.Count == 0)
				opRes += 2 + Math.Min(10, Controller.BoardZone.Game.Turn);

			//Estudiar Minions (se puede ampliar teniendo en cuenta las habilidades)

			myRes *= 3;
			opRes *= 3;
			myRes += getMinionValues(false);
			opRes += getMinionValues(true);

			return myRes - opRes;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
		int getValueForMinion(Minion minion)
		{
			int value = 0;

			if (!minion.IsFrozen) value += minion.Health + minion.AttackDamage * Math.Max(minion.NumAttacksThisTurn - 1, 0);

			if (minion.HasTaunt) value += minion.Health;
			if (minion.Poisonous) value += 3;
			if (minion.HasDeathrattle) value += 3;
			if (minion.HasInspire) value += 3;
			if (minion.HasDivineShield)
			{
				value += minion.AttackDamage;
				if (minion.HasTaunt) value += minion.Health;
			}
			if (minion.HasLifeSteal) value += 2* minion.AttackDamage;
			if (minion.HasCharge) value += 3;
			if (minion.HasStealth) value +=  minion.AttackDamage;
			if (minion.HasBattleCry) value += 3;
			if (minion.HasWindfury) value += minion.AttackDamage;

			return value;
		}

		int getMinionValues(bool oponent)
		{
			int value = 0;
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
