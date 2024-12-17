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
                Console.Write("Evaluating my next move... ");
                DateTime t1 = DateTime.UtcNow;
                GameState OptimalNextState = GAME.OptimalNextState(depth);
                DateTime t2 = DateTime.UtcNow;
                TimeSpan ts = t2 - t1;
                Console.WriteLine("Selected in " + ts.TotalSeconds.ToString("#,##0.0") + " seconds.");


                //Deduce move
                string m = Tools.DeduceMove(GAME, OptimalNextState);
                Console.WriteLine("My move: " + m);
                Console.WriteLine("State after my move: " + OptimalNextState.ToString());

                //Ask them what their move is
                Console.WriteLine();
                Console.WriteLine("What about you? Enter in either the FEN of the state AFTER your move or the origin square of your move");
                Console.Write("State/Origin: ");
                string? ip = Console.ReadLine();
                if (ip != null)
                {
                    if (ip.Length == 2) //They gave us a square, so must ask for follow up
                    {
                        //First of all, "execute" the optimal move by making it the game
                        GAME = OptimalNextState;

                        Square origin = Enum.Parse<Square>(ip);
                        Console.Write("Destination: ");
                        ip = Console.ReadLine();
                        if (ip != null)
                        {
                            Square destination = Enum.Parse<Square>(ip);
                            
                            //Execute move
                            Console.Write("Executing move " + origin.ToString() + " to " + destination.ToString() + "... ");
                            GAME.MovePiece(origin, destination); //move
                            GAME.NextToMove = !GAME.NextToMove; //Flip next to move!
                            Console.WriteLine("Moved!");
                        }
                    }
                    else //it was a full FEN, so parse it
                    {
                        GAME = GameState.Load(ip);
                    }
                }
                Console.WriteLine();

                //Increment move counter purely for reporting
                move = move + 1;
            }
        }

        public static void PrintNextMoveEvals()
        {
            string FEN = "r2q2k1/1p3pb1/2p3p1/2PpR1N1/1P1Q4/p5P1/P4PK1/7R b - - 2 36";
            int depth = 3;

            List<(GameState, float)> evals = new List<(GameState, float)>();
            GameState gs = GameState.Load(FEN);
            foreach (GameState ngs in gs.PossibleNextStates())
            {
                float eval = ngs.Evaluate(depth - 1);
                Console.WriteLine(ngs.ToString() + " = " + eval.ToString());
                evals.Add((ngs, eval));
            }

            Console.WriteLine("-------");
            while (evals.Count > 0)
            {
                (GameState, float) HighestSeen = evals[0];
                foreach ((GameState, float) eval in evals)
                {
                    if (eval.Item2 > HighestSeen.Item2)
                    {
                        HighestSeen = eval;
                    }
                }

                Console.WriteLine(HighestSeen.Item1.ToString() + " = " + HighestSeen.Item2.ToString("#,##0.0"));
                evals.Remove(HighestSeen);
            }
        }
    }
}