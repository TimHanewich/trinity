using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameState gs = GameState.Load("PPPPPPPP/8/8/Q7/7Q/8/8/8");
            gs.Print();
            Console.ReadLine();

            List<GameState> PotentialWhiteQueenUpMoves = new List<GameState>();
            foreach (Square s in Enum.GetValues(typeof(Square)))
            {
                if (gs.WhiteQueens.SquareOccupied(s)) //We found a queen!
                {
                    Square PotentialMove = s;
                    while (true)
                    {
                        PotentialMove = PotentialMove + 8; //1 rank up
                        if (gs.White.SquareOccupied(PotentialMove)) //occupied by our own pieces
                        {
                            break;
                        }
                        else if (gs.Black.SquareOccupied(PotentialMove)) //occupied by a black piece we can capture
                        {
                            GameState pgs = gs;
                            pgs.WhiteQueens = pgs.WhiteQueens.SetSquare(s, false); //The square the piece is coming from, set it to empty
                            pgs.ClearSquare(PotentialMove); //Clear the square it is going to (we do not know what particular black piece is there)
                            pgs.WhiteQueens = pgs.WhiteQueens.SetSquare(PotentialMove, true); //Put the queen piece where it belongs
                            PotentialWhiteQueenUpMoves.Add(pgs);
                            break;
                        }
                        else //It is not occupied!
                        {
                            GameState pgs = gs;
                            pgs.WhiteQueens = pgs.WhiteQueens.SetSquare(s, false); //The square the piece is coming from, set it to empty
                            pgs.WhiteQueens = pgs.WhiteQueens.SetSquare(PotentialMove, true); //Put the queen piece where it belongs
                            PotentialWhiteQueenUpMoves.Add(pgs);
                        }
                    }
                }
            }

            foreach (GameState ngs in PotentialWhiteQueenUpMoves)
            {
                ngs.Print();
                Console.ReadLine();
            }
        }
    }
}