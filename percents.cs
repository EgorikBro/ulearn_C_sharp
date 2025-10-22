public static double Calculate(string userInput)
{
    var data = userInput.Split();
    var sum = double.Parse(data[0]);
    var percent = double.Parse(data[1]);
    var months = int.Parse(data[2]);
    var monthlyRate = percent / 100 / 12;
    return sum * Math.Pow(1 + monthlyRate, months);
}
