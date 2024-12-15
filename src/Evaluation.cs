using System;

namespace Chess3
{
    public static class Evaluation
    {
        //Evaluates the "static" position on the board (without "looking ahead" moves), considering things like material difference, positioning, control of board, etc.
        //The more criteria/considerations we add to this function, the longer it takes.
        //However, the benefit of adding these is, in a more positional game (not hand-to-hand combat, taking and taking back and forth), the engine can make better decisions looking at positioning instead of ONLY caring about material difference with no regard to positioning on the board
        public static float StaticEvaluate(this GameState gs)
        {
            float ToReturn = gs.MaterialDifference(); //Start with material difference

            //What to speed things up? Comment out the following additional static positional evaluations!
            //However, doing so will reduce the engine's ability to play positionally... it will become obsessed ONLY with material difference!

            //Add in center of board difference
            ToReturn = ToReturn + gs.CenterOfBoardEvaluation();

            //Add in pawn rank evaluation (higher pawn ranks are better!)
            ToReturn = ToReturn + gs.PawnRankingEvaluation();

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
            return gs.minimax_abp(depth, float.MinValue, float.MaxValue, gs.NextToMove);
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

        #region "internal tools"

        //Get additional scoring based on a particular colors center of board positioning
        private static float CenterOfBoardEvaluation(ulong all_color_pieces)
        {
            float[] CenterOfBoardSquareWeights = new float[]{0f,0f,0f,0f,0f,0f,0f,0f, 0f,0.05f,0.05f,0.05f,0.05f,0.05f,0.05f,0f, 0f,0.05f,0.1f,0.1f,0.1f,0.1f,0.05f,0f, 0f,0.05f,0.1f,0.15f,0.15f,0.1f,0.05f,0f, 0f,0.05f,0.1f,0.15f,0.15f,0.1f,0.05f,0f, 0f,0.05f,0.1f,0.1f,0.1f,0.1f,0.05f,0f, 0f,0.05f,0.05f,0.05f,0.05f,0.05f,0.05f,0f, 0f,0f,0f,0f,0f,0f,0f,0f}; //Weighted eval scores to boost pieces on certain squares, rewarding more for the center squares. In the order of A1, B1, C1, D1 etc... (same as Square enum) 
            float ToReturn = 0.0f;
            foreach (Square s in Enum.GetValues(typeof(Square)))
            {
                if (all_color_pieces.SquareOccupied(s))
                {
                    ToReturn = ToReturn + CenterOfBoardSquareWeights[Convert.ToInt32(s)];
                }
            }
            return ToReturn;
        }

        //Returns the center of board evaluation as a disparity gap (similar to material difference)
        public static float CenterOfBoardEvaluation(this GameState gs)
        {
            return CenterOfBoardEvaluation(gs.White) - CenterOfBoardEvaluation(gs.Black);
        }

        private static float PawnRankingEvaluation(ulong pawns, bool color)
        {
            float ToReturn = 0.0f;

            foreach (Square s in Enum.GetValues(typeof(Square)))
            {
                if (pawns.SquareOccupied(s))
                {
                    //Check the rank
                    //We do not need to handle rank 1 and rank 8 because it is impossible for a pawn to ever be on those ranks
                    if (s >= Square.A2 && s <= Square.H2)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.0f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.5f;
                        }
                    }
                    else if (s >= Square.A3 && s <= Square.H3)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.1f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.4f;
                        }
                    }
                    if (s >= Square.A4 && s <= Square.H4)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.2f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.3f;
                        }
                    }
                    if (s >= Square.A5 && s <= Square.H5)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.3f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.2f;
                        }
                    }
                    if (s >= Square.A6 && s <= Square.H6)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.4f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.1f;
                        }
                    }
                    if (s >= Square.A7 && s <= Square.H7)
                    {
                        if (color)
                        {
                            ToReturn = ToReturn + 0.5f;
                        }
                        else
                        {
                            ToReturn = ToReturn + 0.0f;
                        }
                    }
                }
            }

            return ToReturn;
        }
        
        public static float PawnRankingEvaluation(this GameState state)
        {
            return PawnRankingEvaluation(state.WhitePawns, true) - PawnRankingEvaluation(state.BlackPawns, false);
        }
        
        #endregion
    }
}