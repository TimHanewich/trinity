using System;

namespace Chess3
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ulong board = 0;
            Console.WriteLine(board);
            board.SetBit(Square.A1, true);
            Console.WriteLine(board);
        }
    }
}