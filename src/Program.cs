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
            Console.WriteLine(gs.StaticEvaluate());
            
            
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

    }
}