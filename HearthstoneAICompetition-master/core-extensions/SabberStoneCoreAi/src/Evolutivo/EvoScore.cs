using System;
using System.Collections.Generic;
using System.Linq;
using SabberStoneCore.Model.Entities;
using SabberStoneCoreAi.Agent;
using SabberStoneCoreAi.src;

namespace SabberStoneCoreAi.Score
{
	public class EvoScore : Score
	{
		/*
		0: vida
		1: cartas en mano
		2: cartas en el mazo
		3: ventaja de numero de minions en el campo
		4: vida del minion
		5: ataque del minion
		---- habilidades de los minions ----
		6: tener provocar/
		7: tener último aliento (algo al morir)/
		8: tener escudo divino/
		9: tener cargar/
		10: tener grito de batalla/
		11: tener viento furioso/
		12: peso total que se le da a el conjunto de minions
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
			opRes += Globals.WEIGHT[1] * OpHandCnt;

			//Cartas en Mazo (igual hace falta ponerlo para que solo lo compruebe con la partida bastante avanzada)
			if (Controller.BoardZone.Game.Turn > 10)
			{
				myRes += Globals.WEIGHT[2] * (Math.Sqrt(DeckCnt));
				opRes += Globals.WEIGHT[2] * (Math.Sqrt(OpDeckCnt));
			}


			// Ver la diferencia entre numero de criaturas en el campo
	
				myRes += Globals.WEIGHT[3] * (BoardZone.Count- OpBoardZone.Count);
				opRes += Globals.WEIGHT[3] * (OpBoardZone.Count - BoardZone.Count);

			//Estudiar Minions (se puede ampliar teniendo en cuenta las habilidades)

			myRes += Globals.WEIGHT[12] * getMinionValues(false);
			opRes += Globals.WEIGHT[12] * getMinionValues(true);

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
			if (minion.CanAttack) value += Globals.WEIGHT[5] * minion.AttackDamage;

			if (minion.HasTaunt) value += Globals.WEIGHT[6];
			if (minion.HasDeathrattle) value += Globals.WEIGHT[7];
			if (minion.HasDivineShield) value += Globals.WEIGHT[8];
			if (minion.HasCharge) value += Globals.WEIGHT[9];
			if (minion.HasBattleCry) value += Globals.WEIGHT[10];
			if (minion.HasWindfury) value += Globals.WEIGHT[11];

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
