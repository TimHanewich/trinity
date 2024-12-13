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
                            ToReturn.WhitePawns = ToReturn.WhitePawns.SetBit(ThisPosition, true);
                        }
                        else if (c == 'N')
                        {
                            ToReturn.WhiteKnights = ToReturn.WhiteKnights.SetBit(ThisPosition, true);
                        }
                        else if (c == 'B')
                        {
                            ToReturn.WhiteBishops = ToReturn.WhiteBishops.SetBit(ThisPosition, true);
                        }
                        else if (c == 'R')
                        {
                            ToReturn.WhiteRooks = ToReturn.WhiteRooks.SetBit(ThisPosition, true);
                        }
                        else if (c == 'Q')
                        {
                            ToReturn.WhiteQueens = ToReturn.WhiteQueens.SetBit(ThisPosition, true);
                        }
                        else if (c == 'K')
                        {
                            ToReturn.WhiteKings = ToReturn.WhiteKings.SetBit(ThisPosition, true);
                        }
                        else if (c == 'p')
                        {
                            ToReturn.BlackPawns = ToReturn.BlackPawns.SetBit(ThisPosition, true);
                        }
                        else if (c == 'n')
                        {
                            ToReturn.BlackKnights = ToReturn.BlackKnights.SetBit(ThisPosition, true);
                        }
                        else if (c == 'b')
                        {
                            ToReturn.BlackBishops = ToReturn.BlackBishops.SetBit(ThisPosition, true);
                        }
                        else if (c == 'r')
                        {
                            ToReturn.BlackRooks = ToReturn.BlackRooks.SetBit(ThisPosition, true);
                        }
                        else if (c == 'q')
                        {
                            ToReturn.BlackQueens = ToReturn.BlackQueens.SetBit(ThisPosition, true);
                        }
                        else if (c == 'k')
                        {
                            ToReturn.BlackKings = ToReturn.BlackKings.SetBit(ThisPosition, true);
                        }
                    }
                    onFile = onFile + 1; //increment the file we're on
                }  

                //Decrement the rank we are on (FEN notation starts @ rank 8 and goes down from there)
                onRank = onRank - 1;   
            }
            

            return ToReturn;
        }
    }
}