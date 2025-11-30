public static double Calculate(string userInput)
{
    string[] parts = userInput.Split(' ');
    double sum = double.Parse(parts[0]);
    double rate = double.Parse(parts[1]);
    int months = int.Parse(parts[2]);
    double monthly = rate / 12 / 100;
    double factor = 1 + monthly;
    double total = sum * Math.Pow(factor, months);
    return total;
}
