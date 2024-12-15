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
            PerfTest();

            GameState gs = GameState.Load("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
        
            
        }

        public static void PerfTest()
        {
            GameState gs = GameState.Load("r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33");
            gs.Print();

            DateTime t1 = DateTime.UtcNow;
            GameState ngs = gs.OptimalNextState();
            DateTime t2 = DateTime.UtcNow;
            TimeSpan ts = t2 - t1;
            Console.WriteLine(ts.TotalSeconds.ToString() + " seconds");
            Console.ReadLine();
            //ngs.Print();
        }

        public static void Game()
        {
            Console.WriteLine("Hello!");

            Console.Write("What depth do you want me to evaluate to? > ");
            string? idepth = Console.ReadLine();
            if (idepth == null)
            {
                Console.WriteLine("Must provide depth.");
                return;
            }
            int depth = Convert.ToInt32(idepth);

            Console.WriteLine("Give me the FEN of the state you want me to make the first move from.");
            Console.Write("FEN > ");
            string? starting_position = Console.ReadLine();
            if (starting_position == null)
            {
                Console.WriteLine("FEN not valid.");
                return;
            }
            GameState GAME = GameState.Load(starting_position);
            int move = 1;
            while (true)
            {
                Console.WriteLine("--- FRAME " + move.ToString() + " ---");
                Console.WriteLine("Current position: " + GAME.ToString());

                //Evaluate
                Console.Write("Evaluating my next move...");
                GameState OptimalNextState = GAME.OptimalNextState(depth);
                Console.WriteLine("Selected!");


                //Deduce move
                string m = Tools.DeduceMove(GAME, OptimalNextState);
                Console.WriteLine("My move: " + m);
                Console.WriteLine("State after my move: " + OptimalNextState.ToString());

                //Ask them what their move is
                Console.WriteLine();
                Console.WriteLine("What is the state after your move?");
                Console.Write("State: ");
                string? ns = Console.ReadLine();
                if (ns != null)
                {
                    GAME = GameState.Load(ns);
                }
                Console.WriteLine();

                move = move + 1;
            }
        }

    }
}