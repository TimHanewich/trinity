using System;

namespace Chess3
{
    public struct GameState
    {

        //Bit boards
        public ulong WhitePawns {get; set;}
        public ulong WhiteKnights {get; set;}
        public ulong WhiteBishops {get; set;}
        public ulong WhiteRooks {get; set;}
        public ulong WhiteQueens {get; set;}
        public ulong WhiteKings {get; set;}
        public ulong BlackPawns {get; set;}
        public ulong BlackKnights {get; set;}
        public ulong BlackBishops {get; set;}
        public ulong BlackRooks {get; set;}
        public ulong BlackQueens {get; set;}
        public ulong BlackKings {get; set;}

        //Next to move
        public bool NextToMove {get; set;} //true = white, false = black

        public static GameState Load(string FEN)
        {
            GameState ToReturn = new GameState();

            //Strip out board squares portion
            string[] parts = FEN.Split(" ");

            //Board rep v2
            string BoardRepresentation = parts[0];
            string[] ranks = BoardRepresentation.Split("/");
            int onRank = 7;
            foreach (string rank in ranks)
            {
                //Replace numbers with actual single space representations (makes it easier to handle each)
                string row_fen = rank; //take a "copy of it" that we can manipulate
                row_fen = row_fen.Replace("8", "11111111");
                row_fen = row_fen.Replace("7", "1111111");
                row_fen = row_fen.Replace("6", "111111");
                row_fen = row_fen.Replace("5", "11111");
                row_fen = row_fen.Replace("4", "1111");
                row_fen = row_fen.Replace("3", "111");
                row_fen = row_fen.Replace("2", "11");

                int onFile = 0;
                foreach (char c in row_fen)
                {
                    if (char.IsNumber(c) == false)
                    {
                        Square ThisPosition = (Square)((8*onRank) + onFile);
                        if (c == 'P')
                        {
                            ToReturn.WhitePawns = ToReturn.WhitePawns.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'N')
                        {
                            ToReturn.WhiteKnights = ToReturn.WhiteKnights.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'B')
                        {
                            ToReturn.WhiteBishops = ToReturn.WhiteBishops.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'R')
                        {
                            ToReturn.WhiteRooks = ToReturn.WhiteRooks.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'Q')
                        {
                            ToReturn.WhiteQueens = ToReturn.WhiteQueens.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'K')
                        {
                            ToReturn.WhiteKings = ToReturn.WhiteKings.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'p')
                        {
                            ToReturn.BlackPawns = ToReturn.BlackPawns.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'n')
                        {
                            ToReturn.BlackKnights = ToReturn.BlackKnights.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'b')
                        {
                            ToReturn.BlackBishops = ToReturn.BlackBishops.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'r')
                        {
                            ToReturn.BlackRooks = ToReturn.BlackRooks.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'q')
                        {
                            ToReturn.BlackQueens = ToReturn.BlackQueens.SetSquare(ThisPosition, true);
                        }
                        else if (c == 'k')
                        {
                            ToReturn.BlackKings = ToReturn.BlackKings.SetSquare(ThisPosition, true);
                        }
                    }
                    onFile = onFile + 1; //increment the file we're on
                }  

                //Decrement the rank we are on (FEN notation starts @ rank 8 and goes down from there)
                onRank = onRank - 1;   
            }
            
            //Next to move
            if (parts.Length > 1)
            {
                if (parts[1] == "w")
                {
                    ToReturn.NextToMove = true;
                }
                else if (parts[1] == "b")
                {
                    ToReturn.NextToMove = false;
                }
                else
                {
                    throw new Exception("'" + parts[1] + "' not recognized as a valid next-to-move indicator");
                }
            }

            return ToReturn;
        }

        //Returns the FEN-notation of the piece on a specific square (i.e. "P" or "p" or "N" or "n")
        //If there is no piece on the square, returns null
        public char? PieceOnSquare(Square s)
        {
            if (WhitePawns.SquareOccupied(s))
            {
                return 'P';
            }
            else if (WhiteKnights.SquareOccupied(s))
            {
                return 'N';
            }
            else if (WhiteBishops.SquareOccupied(s))
            {
                return 'B';
            }
            else if (WhiteRooks.SquareOccupied(s))
            {
                return 'R';
            }
            else if (WhiteQueens.SquareOccupied(s))
            {
                return 'Q';
            }
            else if (WhiteKings.SquareOccupied(s))
            {
                return 'K';
            }
            else if (BlackPawns.SquareOccupied(s))
            {
                return 'p';
            }
            else if (BlackKnights.SquareOccupied(s))
            {
                return 'n';
            }
            else if (BlackBishops.SquareOccupied(s))
            {
                return 'b';
            }
            else if (BlackRooks.SquareOccupied(s))
            {
                return 'r';
            }
            else if (BlackQueens.SquareOccupied(s))
            {
                return 'q';
            }
            else if (BlackKings.SquareOccupied(s))
            {
                return 'k';
            }
            else
            {
                return null;
            }
        }
        
        public override string ToString()
        {
            string ToReturn = "";

            //Board representation
            for (int rank = 7; rank >= 0; rank--)
            {
                for (int file = 0; file < 8; file ++)
                {
                    Square OnSquare = (Square)((rank * 8) + file);
                    char? OccupyingPieceNotation = PieceOnSquare(OnSquare);
                    if (OccupyingPieceNotation != null)
                    {
                        ToReturn = ToReturn + OccupyingPieceNotation.Value.ToString();
                    }
                    else
                    {
                        ToReturn = ToReturn + "1";
                    }
                }

                //Add slash if not on the last rank (don't want a trailing rank at the end)
                if (rank != 0)
                {
                    ToReturn = ToReturn + "/";
                }
            }

            //Now condense the strands of 1's into groups
            ToReturn = ToReturn.Replace("11111111", "8");
            ToReturn = ToReturn.Replace("1111111", "7");
            ToReturn = ToReturn.Replace("111111", "6");
            ToReturn = ToReturn.Replace("11111", "5");
            ToReturn = ToReturn.Replace("1111", "4");
            ToReturn = ToReturn.Replace("111", "3");
            ToReturn = ToReturn.Replace("11", "2");
            
            //Next to move
            if (NextToMove)
            {
                ToReturn = ToReturn + " w";
            }
            else 
            {
                ToReturn = ToReturn + " b";
            }

            return ToReturn;

        }

        //Returns a COMBINED bitboard for every white piece, quickly showing you which squares are occupied by white pieces
        public ulong White
        {
            get
            {
                return WhitePawns | WhiteKnights | WhiteBishops | WhiteRooks | WhiteQueens | WhiteKings;
            }
        }

        //Returns a COMBINED bitboard for every black piece, quickly showing you which squares are occupied by black pieces
        public ulong Black
        {
            get
            {
                return BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKings;
            }
        }

        //Ensures the square is cleared (0) on all bitboards
        public void ClearSquare(Square s)
        {
            WhitePawns = WhitePawns.SetSquare(s, false);
            WhiteKnights = WhiteKnights.SetSquare(s, false);
            WhiteBishops = WhiteBishops.SetSquare(s, false);
            WhiteRooks = WhiteRooks.SetSquare(s, false);
            WhiteQueens = WhiteQueens.SetSquare(s, false);
            WhiteKings = WhiteKings.SetSquare(s, false);
            BlackPawns = BlackPawns.SetSquare(s, false);
            BlackKnights = BlackKnights.SetSquare(s, false);
            BlackBishops = BlackBishops.SetSquare(s, false);
            BlackRooks = BlackRooks.SetSquare(s, false);
            BlackQueens = BlackQueens.SetSquare(s, false);
            BlackKings = BlackKings.SetSquare(s, false);
        }

    
        public void Print()
        {
            Console.WriteLine("  --------");
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write((rank + 1).ToString() + "|");
                for (int file = 0; file < 8; file++)
                {
                    Square ThisSquare = (Square)((rank * 8) + file);
                    if (WhitePawns.SquareOccupied(ThisSquare)){Console.Write("P");}
                    else if (WhiteKnights.SquareOccupied(ThisSquare)){Console.Write("N");}
                    else if (WhiteBishops.SquareOccupied(ThisSquare)){Console.Write("B");}
                    else if (WhiteRooks.SquareOccupied(ThisSquare)){Console.Write("R");}
                    else if (WhiteQueens.SquareOccupied(ThisSquare)){Console.Write("Q");}
                    else if (WhiteKings.SquareOccupied(ThisSquare)){Console.Write("K");}
                    else if (BlackPawns.SquareOccupied(ThisSquare)){Console.Write("p");}
                    else if (BlackKnights.SquareOccupied(ThisSquare)){Console.Write("n");}
                    else if (BlackBishops.SquareOccupied(ThisSquare)){Console.Write("b");}
                    else if (BlackRooks.SquareOccupied(ThisSquare)){Console.Write("r");}
                    else if (BlackQueens.SquareOccupied(ThisSquare)){Console.Write("q");}
                    else if (BlackKings.SquareOccupied(ThisSquare)){Console.Write("k");}
                    else {Console.Write(" ");}
                }
                Console.WriteLine("|");
            }
            Console.WriteLine("  --------");
            Console.WriteLine("  ABCDEFGH");
        }
    
        //Moves a pice from point A to point B, regardless of what color.
        //If there is a piece in the destination, replaces it (captures it)
        public void MovePiece(Square origin, Square destination)
        {
            if (WhitePawns.SquareOccupied(origin))
            {
                WhitePawns = WhitePawns.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhitePawns = WhitePawns.SetSquare(destination, true);
            }
            else if (WhiteKnights.SquareOccupied(origin))
            {
                WhiteKnights = WhiteKnights.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhiteKnights = WhiteKnights.SetSquare(destination, true);
            }
            else if (WhiteBishops.SquareOccupied(origin))
            {
                WhiteBishops = WhiteBishops.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhiteBishops = WhiteBishops.SetSquare(destination, true);
            }
            else if (WhiteRooks.SquareOccupied(origin))
            {
                WhiteRooks = WhiteRooks.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhiteRooks = WhiteRooks.SetSquare(destination, true);
            }
            else if (WhiteQueens.SquareOccupied(origin))
            {
                WhiteQueens = WhiteQueens = WhiteQueens.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhiteQueens = WhiteQueens = WhiteQueens.SetSquare(destination, true);
            }
            else if (WhiteKings.SquareOccupied(origin))
            {
                WhiteKings = WhiteKings.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                WhiteKings = WhiteKings.SetSquare(destination, true);
            }
            else if (BlackPawns.SquareOccupied(origin))
            {
                BlackPawns = BlackPawns.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackPawns = BlackPawns.SetSquare(destination, true);
            }
            else if (BlackKnights.SquareOccupied(origin))
            {
                BlackKnights = BlackKnights.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackKnights = BlackKnights.SetSquare(destination, true);
            }
            else if (BlackBishops.SquareOccupied(origin))
            {
                BlackBishops = BlackBishops.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackBishops = BlackBishops.SetSquare(destination, true);
            }
            else if (BlackRooks.SquareOccupied(origin))
            {
                BlackRooks = BlackRooks.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackRooks = BlackRooks.SetSquare(destination, true);
            }
            else if (BlackQueens.SquareOccupied(origin))
            {
                BlackQueens = BlackQueens.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackQueens = BlackQueens.SetSquare(destination, true);
            }
            else if (BlackKings.SquareOccupied(origin))
            {
                BlackKings = BlackKings.SetSquare(origin, false);
                ClearSquare(destination); //Clear whatever is on the destination square, if anything
                BlackKings = BlackKings.SetSquare(destination, true);
            }
            else
            {
                throw new Exception("Unable to execute move from " + origin.ToString() + " to " + destination.ToString() + "! No piece detected on origin square " + origin.ToString() + ".");
            }
        }

        public GameState[] PossibleNextStates()
        {
            List<GameState> ToReturn = new List<GameState>();

            foreach (Square s in Enum.GetValues(typeof(Square)))
            {
                if (WhitePawns.SquareOccupied(s) && NextToMove)
                {

                }
                else if (BlackPawns.SquareOccupied(s) && !NextToMove)
                {

                }
                else if (WhiteKnights.SquareOccupied(s) && NextToMove)
                {

                }
                else if (BlackKnights.SquareOccupied(s) && !NextToMove)
                {

                }
                else if (WhiteBishops.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 9, -7, -9, 7));
                }
                else if (BlackBishops.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 9, -7, -9, 7));
                }
                else if (WhiteRooks.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 8, 1, -8, -1));
                }
                else if (BlackRooks.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 8, 1, -8, -1));
                }
                else if (WhiteQueens.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 8, 9, 1, -7, -8, -9, -1, 7)); //All directions
                }
                else if (BlackQueens.SquareOccupied(s) && NextToMove)
                {
                    ToReturn.AddRange(GenerateLinearMoves(s, 8, 9, 1, -7, -8, -9, -1, 7)); //All directions
                }
                else if (WhiteKings.SquareOccupied(s) && NextToMove)
                {

                }
                else if (BlackKings.SquareOccupied(s) && NextToMove)
                {

                }
            }

            return ToReturn.ToArray();
        }    
    
        #region "internal tools"

        // Performs a "linear" analysis for moves originating from a specific square, in multiple possible directions.
        // The "shifts" parameter allows you to specify what directions the piece can move in
        // For example, a queen can shift: [8, -8, -1, 1], which would be up 1 rank (+8 in bitboard), down 1 rank (-8 in bitboard), left 1 file (-1 on bitboard) and right 1 file (+1 on bitboard)
        public GameState[] GenerateLinearMoves(Square origin, params int[] shifts)
        {
            //Determine positions of "friendly" pieces and positions of "enemy" pieces
            ulong white = White; //Get bitboard of ALL white pieces
            ulong black = Black; //Get bitboard of ALL black pieces
            ulong friendlies; //"label" we will asign to either white/black
            ulong enemies; //"label" we will asign to either white/black
            if (white.SquareOccupied(origin)) //If the piece we are generating moves for is white, white are friendlies.
            {
                friendlies = white;
                enemies = black;
            }
            else if (black.SquareOccupied(origin)) //If the piece we are generating moves for is black, black are friendlies.
            {
                friendlies = black;
                enemies = white;
            }
            else
            {
                throw new Exception("Unable to generate linear moves for piece on square " + origin.ToString() + " because a piece wasn't detected on that square (of either color)!");
            }

            //Calculate potential next states
            List<GameState> ToReturn = new List<GameState>();
            foreach (int shift in shifts)
            {
                Square NewPosition = origin;
                while (true)
                {
                    //Determine the position of the potential target square
                    NewPosition = (Square)(NewPosition + shift); //move the potential target by one to simulate the movement of the piece, by just one step.
                    
                    //Disqualify #1: if we are OUT OF BOUNDS!
                    if (Convert.ToInt32(NewPosition) > 63 || Convert.ToInt32(NewPosition) < 0) //We are "out of bounds", not on the chess board, so break the loop.
                    {
                        break; 
                    }

                    //Disqualify #2: if it is a horizontal shift (left or right), ensure it isn't bleeding into another rank (by going up or down incrementally)
                    if (shift == 1) //horizontal shift to the right
                    {
                        if ((Convert.ToInt32(NewPosition) % 8) == 0) //If the new target it is cleanly divisible by 8 (there is no remainder), that means we just "bled into" the next rank. So stop!
                        {
                            break;
                        }
                    }
                    else if (shift == -1) //horizontal shift to the left
                    {
                        if ((Convert.ToInt32(NewPosition) % 8) == 7) //If the new target is divisible by eight with SEVEN left over, that means it "bled into" the rank below. So stop!
                        {
                            break;
                        }
                    }

                    //Disqualify #3: there is a friendly piece occupying that position
                    if (friendlies.SquareOccupied(NewPosition)) //If there is a "friendly" occupying this square, we can't move to it. So just break!
                    {
                        break; 
                    }

                    //We got this far at this point, which means the position IS a viable position
                    //It is viable either because 1) it is an enemey piece we can capture, or 2) It is an empty space we can move to.
                    //So determine which
                    if (enemies.SquareOccupied(NewPosition)) //There is an enemy piece in this position. that we can capture
                    {
                        GameState pgs = this; // "copy" the game
                        pgs.MovePiece(origin, NewPosition); //Move the piece, also capturing.
                        ToReturn.Add(pgs); //Add it to the list
                        break; //We can capture this piece but cannot continue to move "past" this piece, so break.
                    }
                    else //It is an empty space! So add it! And then continue!
                    {
                        GameState pgs = this; // "copy" the game
                        pgs.MovePiece(origin, NewPosition); //Move the piece, also capturing.
                        ToReturn.Add(pgs); //Add it to the list
                    }
                }
            }

            return ToReturn.ToArray();
        }

        #endregion
    
    }
}