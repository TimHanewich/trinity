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
            Console.WriteLine("0000000000000000");
            
            GameState[] nextstates = gs.PossibleNextStates();
            foreach (GameState ngs in nextstates)
            {
                ngs.Print();
            }
        }

    }
}