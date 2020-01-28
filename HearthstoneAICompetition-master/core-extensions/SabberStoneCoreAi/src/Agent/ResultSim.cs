using System;
using System.Collections.Generic;
using System.Text;
using SabberStoneCore.Tasks.PlayerTasks;

namespace SabberStoneCoreAi.src.Agent
{
	class ResultSim
	{
		public POGame.POGame state;
		public PlayerTask task;
		public float value;
		public ResultSim(POGame.POGame state, PlayerTask task, float value)
		{
			this.state = state;
			this.value = value;
			this.task = task;
		}
	}
}
