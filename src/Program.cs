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
            Console.WriteLine(Tools.ULongToBits(gs.WhiteQueens));
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

        public static GameState[] LinearMoves(GameState state, Square origin, int direction)
        {
            //Directions
            //0 = up
            //1 = up + right
            //2 = right
            //3 = down + right
            //4 = down
            //5 = down + left
            //6 = left
            //7 = up + left

            //Get current all whites and all blacks 
            ulong white = state.White;
            ulong black = state.Black;

            //Firstly, figure out what pieces are friends vs. enemies based on the color of the piece that is going to move
            ulong FriendlyPieces;
            ulong EnemyPieces;
            if (white.SquareOccupied(origin))
            {
                FriendlyPieces = white;
                EnemyPieces = black;
            }
            else if (black.SquareOccupied(origin))
            {
                FriendlyPieces = black;
                EnemyPieces = white;
            }
            else
            {
                throw new Exception("Unable to determine linear moves from square '" + origin.ToString() + "'! No piece detected on this square.");
            }

            Square PotentialTarget = origin;
            while (true)
            {
                //Modify potential target according to direction we are moving in
                if (direction == 0)
                {
                    PotentialTarget = PotentialTarget + 8;
                }
                else if (direction == 1)
                {
                    PotentialTarget = PotentialTarget + 9;
                }
                else if (direction == 2)
                {
                    PotentialTarget = PotentialTarget + 1;
                }
                else if (direction == 3)
                {
                    PotentialTarget = PotentialTarget - 7;
                }
                else if (direction == 4)
                {
                    PotentialTarget = PotentialTarget - 8;
                }
                else if (direction == 5)
                {
                    PotentialTarget = PotentialTarget - 9;
                }
                else if (direction == 6)
                {
                    PotentialTarget = PotentialTarget - 1;
                }
                else if (direction == 7)
                {
                    PotentialTarget = PotentialTarget + 7;
                }
            
                if (FriendlyPieces.SquareOccupied(PotentialTarget)) //It is our own piece. Can't capture it, can't jump over it.End of the line for us in this direction.
                {
                    break;
                }
                else if (EnemyPieces.SquareOccupied(PotentialTarget)) //It is an enemy piece
                {
                    GameState pgs = state; // "copy" the game
                }
            }

            return new GameState[]{};
        }

    }
}