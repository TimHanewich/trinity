using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //string FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            string FEN = "r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33";
            GameState gs = GameState.Load(FEN);
            Console.WriteLine(JsonSerializer.Serialize(gs));
            Console.WriteLine(gs.ToString());
        }
    }
}