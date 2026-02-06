public static string GetGreetingMessage(string name, double salary)
{
    double money = Math.Ceiling(salary);
    return "Hello, " + name + ", your salary is " + money;
}
