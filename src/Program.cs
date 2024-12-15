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

            GameState gs = GameState.Load("8/8/2Q5/8/8/8/8/8 w - - 0 1");
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