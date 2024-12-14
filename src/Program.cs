using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Chess3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            string FEN = "rnbqkbnr/pppppppp/8/8/8/8/PPPPPPPP/RNBQKBNR w KQkq - 0 1";
            GameState gs = GameState.Load(FEN);
            Console.WriteLine(Tools.ULongToBits(gs.WhitePawns));
            Tools.PrintBitboard(gs.WhitePawns);
        }
    }
}