namespace Pluralize;

public static class PluralizeTask
{
    public static string PluralizeRubles(int count)
	{
		var lastTwoDigits = count % 100;
        var lastDigit = count % 10;
		var exceptionNumbers = lastTwoDigits < 20 && lastTwoDigits > 10;

        if (lastDigit == 1 && !exceptionNumbers)
			return "рубль";
        else if (lastDigit >= 2 && lastDigit <= 4 && !exceptionNumbers)
			return "рубля";
        return "рублей";
    }
}
