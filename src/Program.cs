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

            //Make a list
            List<ulong> ToReturn = new List<ulong>();
            ulong WhitePieces = gs.White; //all white occupancy
            ulong BlackPieces = gs.Black; //all black occupancy
            ulong current = gs.WhiteQueens;
            while (true)
            {
                Console.WriteLine("Before move:");
                Tools.PrintBitboard(current);
                Console.ReadLine();

                current = current << 8; //move up

                Console.WriteLine("After move");
                Tools.PrintBitboard(current);
                Console.ReadLine();

                ulong collisions = current & WhitePieces; //Find where they collide
                Console.WriteLine("COLLISIONS:");
                Tools.PrintBitboard(collisions);
                Console.ReadLine();

                //Kill the piece where there are collisions
                current = current ^ collisions; //using XOR operator

                //If the value still stands even after considering the collisions, add it.
                if (current > 0) //there are still valid queen moves on the board
                {
                    ToReturn.Add(current);
                }
                else
                {
                    break;
                }
            }

            Console.WriteLine("Next moves: ");
            foreach (ulong ul in ToReturn)
            {
                Console.WriteLine(ul);
            }
        }
    }
}