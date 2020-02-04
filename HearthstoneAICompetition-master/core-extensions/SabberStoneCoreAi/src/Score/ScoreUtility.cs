
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

			//Estudiar Minions (se puede ampliar teniendo en cuenta las habilidades)

			myRes += MinionTotAtk + MinionTotHealth;
			opRes += OpMinionTotAtk + OpMinionTotHealth;

			// Ver si el oponente tiene el campo libre
			if (OpBoardZone.Count == 0)
				myRes += 2 + Math.Min(10, Controller.BoardZone.Game.Turn);
			if (BoardZone.Count == 0)
				opRes += 2 + Math.Min(10, Controller.BoardZone.Game.Turn);


			
			return myRes - opRes;
		}

		public override Func<List<IPlayable>, List<int>> MulliganRule()
		{
			return p => p.Where(t => t.Cost > 3).Select(t => t.Id).ToList();
		}
	}
}
