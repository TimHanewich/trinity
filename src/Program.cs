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
            GameState starting = GameState.Load("rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1");
            Console.WriteLine(starting.MaterialDifference());
            Console.ReadLine();

            GameState gs = GameState.Load("r5k1/1p3p2/2p2qpb/2PpR3/3P2r1/pP1Q1NP1/P4PK1/7R b - - 1 33");
            gs.Print();

            
            GameState[] nextstates = gs.PossibleNextStates();
            Console.WriteLine(nextstates.Length.ToString() + " potential next states");
            Console.ReadLine();
            foreach (GameState ngs in nextstates)
            {
                ngs.Print();
                Console.ReadLine();
            }
        }

    }
}