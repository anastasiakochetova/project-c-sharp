public static void Main()
{
    double pi = Math.PI;
    int tenThousand = 10000;
    float tenThousandPi = (float)pi*tenThousand;
    int roundedTenThousandPi = (int) Math.Round(tenThousandPi);
    int integerPartOfTenThousandPi = (int)tenThousandPi;
    Console.WriteLine(integerPartOfTenThousandPi);
    Console.WriteLine(roundedTenThousandPi);
}
