using System;

namespace Chess3
{
    public static class Evaluation
    {
        //Minimax, with alpha-beta pruning
        private static int minimax_abp(this GameState gs, int depth, int alpha, int beta, bool color)
        {
            if (depth == 0)
            {
                return gs.MaterialDifference(); //using the raw material difference right now, but in the future should probably consider things like control of board, position, etc.
            }

            if (color)
            {
                int BestValueSeenSoFar = int.MinValue;
                foreach (GameState ngs in gs.PossibleNextStates())
                {
                    int nbsValue = ngs.minimax_abp(depth - 1, alpha, beta, !color);
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
                int BestValueSeenSoFar = int.MaxValue;
                foreach (GameState ngs in gs.PossibleNextStates())
                {
                    int nbsValue = ngs.minimax_abp(depth - 1, alpha, beta, !color);
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
        public static int Evaluate(this GameState gs, int depth)
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
            int highestEval = int.MinValue;
            GameState lowest = PotentialNextStates[0];
            int lowestEval = int.MaxValue;
            foreach (GameState pgs in PotentialNextStates)
            {
                int eval = pgs.Evaluate(eval_depth-1);
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