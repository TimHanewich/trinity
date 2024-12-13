using System;

namespace Chess3
{
    public class Tools
    {
        public static string ULongToBits(ulong value)
        {
            return Convert.ToString((long)value, 2).PadLeft(64, '0');
        }

        public static void PrintBitboard(ulong board)
        {
            string bitstr = ULongToBits(board);
            for (int r = 0; r < 8; r++)
            {
                for (int f = 0; f < 8; f++)
                {
                    Console.Write(bitstr[(r*2) + f]);
                }
                Console.WriteLine();
            }
        }

        public static ulong SetBit(ulong board, Square position, bool value)
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
        public static bool BitOccupied(ulong board, Square position)
        {
            ulong mask = 1UL << Convert.ToInt32(position);
            ulong AfterAnd = board & mask; //An AND operation. This value will have a 1 in it if the bit of position indeed ALSO had a 1 in it. Thus, we can determine YES, this bit was occupied if the value is > 0 and NO it was not occupied if it was 0.
            return AfterAnd > 0; //If it is 0, it was not occupied. If it was some value > 0, that specific bit was occupied.
        }

        
    }
}