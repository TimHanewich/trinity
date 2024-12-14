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

        public ulong Black
        {
            get
            {
                return BlackPawns | BlackKnights | BlackBishops | BlackRooks | BlackQueens | BlackKings;
            }
        }

        //Returns all the possible next states from this next move
        public GameState[] NextStates()
        {
            return new GameState[]{};
        }

    }
}