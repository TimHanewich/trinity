using System;
using System.IO.Compression;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            GameState gs = GameState.Load("pPPPPPPP/8/8/Q7/7Q/8/8/8");
            gs.Print();
            
            GameState[] upMoves = LinearMoves(gs, Square.A5, 0);

            Console.WriteLine("------");

            foreach (GameState ngs in upMoves)
            {
                ngs.Print();
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

            List<GameState> ToReturn = new List<GameState>();

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
            
                //Handle whether it is occupied by enemy, occupied by friend, or empty!
                if (FriendlyPieces.SquareOccupied(PotentialTarget)) //It is our own piece. Can't capture it, can't jump over it.End of the line for us in this direction.
                {
                    break;
                }
                else if (EnemyPieces.SquareOccupied(PotentialTarget)) //It is an enemy piece. Capture it, then we can go no further, so stop. 
                {
                    GameState pgs = state; // "copy" the game
                    pgs.MovePiece(origin, PotentialTarget); //Move the piece, also capturing.
                    ToReturn.Add(pgs);
                    break;
                }
                else //it is empty, so just add it!
                {
                    GameState pgs = state; // "copy" the game
                    pgs.MovePiece(origin, PotentialTarget); //Move the piece, also capturing.
                    ToReturn.Add(pgs);
                }
            }

            return ToReturn.ToArray();
        }

    }
}