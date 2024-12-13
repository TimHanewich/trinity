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

        public static GameState Load(string FEN)
        {
            GameState ToReturn = new GameState();

            //Strip out board squares portion
            string[] parts = FEN.Split(" ");

            //Board rep
            string BoardRepresentation = parts[0];
            string[] ranks = BoardRepresentation.Split("/");
            ToReturn.LoadRowFEN(ranks[7], 7);
            ToReturn.LoadRowFEN(ranks[6], 6);
            ToReturn.LoadRowFEN(ranks[5], 5);
            ToReturn.LoadRowFEN(ranks[4], 4);
            ToReturn.LoadRowFEN(ranks[3], 3);
            ToReturn.LoadRowFEN(ranks[2], 2);
            ToReturn.LoadRowFEN(ranks[1], 1);
            ToReturn.LoadRowFEN(ranks[0], 0);

            return ToReturn;
        }

        //Loads a single "row" (rank) of the FEN's board representation into the appropriate bitboards
        //parameter "rank" should be 0-based... so rank 1 would be 0, rank 8 would be 7
        private void LoadRowFEN(string row_fen, int rank)
        {
            //Replace numbers with actual single space representations (makes it easier to handle each)
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
                    Square ThisPosition = (Square)((8*rank) + onFile);
                    if (c == 'P')
                    {
                        WhitePawns = WhitePawns.SetBit(ThisPosition, true);
                    }
                    else if (c == 'N')
                    {
                        WhiteKnights = WhiteKnights.SetBit(ThisPosition, true);
                    }
                    else if (c == 'B')
                    {
                        WhiteBishops = WhiteBishops.SetBit(ThisPosition, true);
                    }
                    else if (c == 'R')
                    {
                        WhiteRooks = WhiteRooks.SetBit(ThisPosition, true);
                    }
                    else if (c == 'Q')
                    {
                        WhiteQueens = WhiteQueens.SetBit(ThisPosition, true);
                    }
                    else if (c == 'K')
                    {
                        WhiteKings = WhiteKings.SetBit(ThisPosition, true);
                    }
                    else if (c == 'p')
                    {
                        BlackPawns = BlackPawns.SetBit(ThisPosition, true);
                    }
                    else if (c == 'n')
                    {
                        BlackKnights = BlackKnights.SetBit(ThisPosition, true);
                    }
                    else if (c == 'b')
                    {
                        BlackBishops = BlackBishops.SetBit(ThisPosition, true);
                    }
                    else if (c == 'r')
                    {
                        BlackRooks = BlackRooks.SetBit(ThisPosition, true);
                    }
                    else if (c == 'q')
                    {
                        BlackQueens = BlackQueens.SetBit(ThisPosition, true);
                    }
                    else if (c == 'k')
                    {
                        BlackKings = BlackKings.SetBit(ThisPosition, true);
                    }
                }
                onFile = onFile + 1; //increment the file we're on
            }     
        }
    }
}