using System;

namespace Chess3
{
    public static class Evaluation
    {

        //Evaluates the "static" position on the board (without "looking ahead" moves), considering things like material difference, positioning, control of board, etc.
        public static float StaticEvaluate(this GameState gs)
        {
            return Convert.ToSingle(gs.MaterialDifference());
        }

        //Minimax, with alpha-beta pruning
        private static float minimax_abp(this GameState gs, float depth, float alpha, float beta, bool color)
        {
            if (depth == 0)
            {
                return gs.StaticEvaluate();
            }

            if (color)
            {
                float BestValueSeenSoFar = int.MinValue;
                foreach (GameState ngs in gs.PossibleNextStates())
                {
                    float nbsValue = ngs.minimax_abp(depth - 1, alpha, beta, !color);
                    BestValueSeenSoFar = Math.Max(BestValueSeenSoFar, nbsValue);
                    alpha = Math.Max(alpha, BestValueSeenSoFar);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return BestValueSeenSoFar;
            }
            else
            {
                float BestValueSeenSoFar = int.MaxValue;
                foreach (GameState ngs in gs.PossibleNextStates())
                {
                    float nbsValue = ngs.minimax_abp(depth - 1, alpha, beta, !color);
                    BestValueSeenSoFar = Math.Min(BestValueSeenSoFar, nbsValue);
                    beta = Math.Min(beta, BestValueSeenSoFar);
                    if (beta <= alpha)
                    {
                        break;
                    }
                }
                return BestValueSeenSoFar;
            }
        }

        //Uses the minimax function to "peer ahead"
        public static float Evaluate(this GameState gs, int depth)
        {
            //return bs.minimax(depth, bs.NextToMove);
            return gs.minimax_abp(depth, int.MinValue, int.MaxValue, gs.NextToMove);
        }
    
        //Finds optimal move for the current color and returns the resulting board state after that move is made
        public static GameState OptimalNextState(this GameState gs, int eval_depth = 5)
        {
            GameState[] PotentialNextStates = gs.PossibleNextStates();

            //Select best
            GameState highest = PotentialNextStates[0];
            float highestEval = int.MinValue;
            GameState lowest = PotentialNextStates[0];
            float lowestEval = int.MaxValue;
            foreach (GameState pgs in PotentialNextStates)
            {
                float eval = pgs.Evaluate(eval_depth-1);
                if (eval > highestEval)
                {
                    highest = pgs;
                    highestEval = eval;
                }
                if (eval < lowestEval)
                {
                    lowest = pgs;
                    lowestEval = eval;
                }
            }

            //Return the ideal one, based on maximizing color
            if (gs.NextToMove) //it is white, so return highest
            {
                return highest;
            }
            else //it is black, so return lowest
            {
                return lowest;
            }
        }

    }
}