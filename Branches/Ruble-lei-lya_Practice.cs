using System;

namespace Pluralize
{
    public static class PluralizeTask
    {
        public static string PluralizeRubles(int number)
        {
            int mod100 = number % 100;
            int mod10 = number % 10;

            if (mod100 > 10 && mod100 < 15)
                return "рублей";

            if (mod10 == 1)
                return "рубль";
            else if (mod10 >= 2 && mod10 <= 4)
                return "рубля";
            else
                return "рублей";
        }
    }
}
