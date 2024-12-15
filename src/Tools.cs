using System;

namespace Chess3
{
    public static class Tools
    {
        public static string ULongToBits(ulong value)
        {
            return Convert.ToString((long)value, 2).PadLeft(64, '0');
        }

        public static void PrintBitboard(ulong board)
        {
            string bitstr = ULongToBits(board);
            for (int rank = 7; rank >= 0; rank--)
            {
                Console.Write((rank + 1).ToString() + "|");
                for (int file = 0; file < 8; file++)
                {
                    int index = 63 - ((rank*8) + file); 
                    Console.Write(bitstr[index].ToString());
                }
                Console.WriteLine();
            }
            Console.WriteLine("  --------");
            Console.WriteLine("  ABCDEFGH");
        }

        public static ulong SetSquare(this ulong board, Square position, bool value)
        {
            if (value)
            {
                ulong mask = 1UL << Convert.ToInt32(position);
                return board | mask; //OR operator.
            }
            else
            {
                ulong mask = 1UL << Convert.ToInt32(position); //Set up a mask with 1 at the target position
                mask = ~mask; //invert the mask to get a mask with 0 at the position and 1s everywhere else
                return board & mask; //use the AND operator to clear out the bit (if there is one) at the target position (the AND operator does NOT go to 1 if both in the mask are 0... it defaults to 0 in that case)
            }
        }

        //Checks if a specific bit at a specific position is 1
        public static bool SquareOccupied(this ulong board, Square position)
        {
            ulong mask = 1UL << Convert.ToInt32(position);
            ulong AfterAnd = board & mask; //An AND operation. This value will have a 1 in it if the bit of position indeed ALSO had a 1 in it. Thus, we can determine YES, this bit was occupied if the value is > 0 and NO it was not occupied if it was 0.
            return AfterAnd > 0; //If it is 0, it was not occupied. If it was some value > 0, that specific bit was occupied.
        }        
    
        //Counts the number of bits in a ulong that are set to 1 (regardless of their placement)
        //Made by llama3.2-3b
        public static int Count1Bits(this ulong value)
        {
            int ToReturn = 0;
            while (value != 0)
            {
                ToReturn = ToReturn + (int)(value & 1);
                value = value >> 1;
            }
            return ToReturn;
        }
    
        public static string DeduceMove(GameState before, GameState after)
        {
            string origin = "";
            string destination = "";
            foreach (Square s in Enum.GetValues(typeof(Square)))
            {
                if (before.White.SquareOccupied(s) && !after.White.SquareOccupied(s))
                {
                    origin = s.ToString();
                }
                else if (before.Black.SquareOccupied(s) && !after.Black.SquareOccupied(s))
                {
                    origin = s.ToString();
                }
                else if (after.White.SquareOccupied(s) && !before.White.SquareOccupied(s))
                {
                    destination = s.ToString();
                }
                else if (after.Black.SquareOccupied(s) && !before.Black.SquareOccupied(s))
                {
                    destination = s.ToString();
                }
                else
                {
                    throw new Exception("Unable to deduce move that resulted in state '" + after.ToString() + "' from '" + before.ToString() + "'.");
                }
            }
            return origin + " --> " + destination;
        }
    }
}