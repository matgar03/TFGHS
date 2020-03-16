using System.Linq;
using System.Collections.Generic;
using SabberStoneCore.Enums;
using SabberStoneCore.Tasks;
using SabberStoneCoreAi.Score;
using SabberStoneCore.Tasks.PlayerTasks;


namespace SabberStoneCoreAi.Agent.ExampleAgents
{
    class EvoGreedy : AbstractAgent
    {
        private List<double> weight;
        public override void InitializeAgent() { }
        public override void InitializeGame() { }
        public override void FinalizeGame() { }
        public override void FinalizeAgent() { }


        public override PlayerTask GetMove(POGame.POGame game)
        {
            var player = game.CurrentPlayer;

            // Get all simulation results for simulations that didn't fail
            var validOpts = game.Simulate(player.Options()).Where(x => x.Value != null);

            // If all simulations failed, play end turn option (always exists), else best according to score function
            return validOpts.Any() ?
                validOpts.OrderBy(x => Score(x.Value, player.PlayerId)).Last().Key :
                player.Options().First(x => x.PlayerTaskType == PlayerTaskType.END_TURN);
        }

        // Calculate different scores based on our hero's class
        private static double Score(POGame.POGame state, int playerId)
        {
            var p = state.CurrentPlayer.PlayerId == playerId ? state.CurrentPlayer : state.CurrentOpponent;
            return new EvoScore{ Controller = p }.Rate();
            /*switch ( state.CurrentPlayer.HeroClass )
            {
                case CardClass.WARRIOR: return new AggroScore { Controller = p }.Rate();
                case CardClass.MAGE: 	return new ControlScore { Controller = p }.Rate();
                default: 				return new MidRangeScore { Controller = p }.Rate();
            }
            */
        }
    }
}
