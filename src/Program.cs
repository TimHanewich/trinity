using System;
using System.Data.SqlTypes;
using System.IO.Compression;
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
            
            GameState [] moves = LinearMovesV2(gs, Square.A5, new int[]{8});
            Console.WriteLine("--------");
            foreach (GameState ngs in moves)
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

        public static GameState[] LinearMovesV2(GameState state, Square origin, int[] shifts)
        {

            //Determine positions of "friendly" pieces and positions of "enemy" pieces
            ulong white = state.White; //Get bitboard of ALL white pieces
            ulong black = state.Black; //Get bitboard of ALL black pieces
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
                    NewPosition = (Square)(NewPosition + shift); //move the potential target by one to simulate the movement of the piece, by just one step.
                    if (Convert.ToInt32(NewPosition) > 63 || Convert.ToInt32(NewPosition) < 0) //We are "out of bounds", not on the chess board, so break the loop.
                    {
                        break; 
                    }
                    else if (friendlies.SquareOccupied(NewPosition)) //If there is a "friendly" occupying this square, we can't move to it. So just break!
                    {
                        break; 
                    }
                    else if (enemies.SquareOccupied(NewPosition)) //There is an enemy piece in this position. 
                    {
                        GameState pgs = state; // "copy" the game
                        pgs.MovePiece(origin, NewPosition); //Move the piece, also capturing.
                        ToReturn.Add(pgs); //Add it to the list
                        break; //We can capture this piece but cannot continue to move "past" this piece, so break.
                    }
                    else //It is an empty space! So add it! And then continue!
                    {
                        GameState pgs = state; // "copy" the game
                        pgs.MovePiece(origin, NewPosition); //Move the piece, also capturing.
                        ToReturn.Add(pgs); //Add it to the list
                    }
                }
            }

            return ToReturn.ToArray();
        }

    }
}