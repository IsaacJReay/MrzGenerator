namespace MrzGenerator;

public class Program
{
    public static void Main()
    {
        var line1 = GenerateTd1Line1("id", "khm", "010956785");
        var line2 = GenerateTd1Line2("khm", "male", new DateTime(2000, 01, 14), new DateTime(2025, 4, 06), line1);
        var line3 = GenerateTd1Line3("ngor", "pichponereay", null);

        Console.WriteLine("{0}\n{1}\n{2}", line1, line2, line3);
    }

    static string GenerateTd1Line3(string familyName, string givenName, string? middleName)
    {
        middleName = middleName ?? string.Empty;
        var line3 = $"{familyName.ToUpper()}<<{givenName.ToUpper()}<{middleName.ToUpper()}".PadRight(30, '<');

        return line3[..30];
    }

    static string GenerateTd1Line2(string nationality, string gender, DateTime dob, DateTime expirationDate, string line1)
    {
        if (nationality.Length > 3)
        {
            throw new Exception("nationality exceed 3 characters!");
        }

        string dobStr = dob.ToString("yyMMdd");
        char dobCheckDigit = CalculateCheckDigit(dobStr);

        string expStr = expirationDate.ToString("yyMMdd");
        char expCheckDigit = CalculateCheckDigit(expStr);

        string line2 = $"{dobStr}{dobCheckDigit}{gender[0].ToString().ToUpper()}{expStr}{expCheckDigit}{nationality.ToUpper()}".PadRight(29, '<');
        var checkDigitLine2 = CalculateCheckDigit($"{line1[5..]}{dobStr}{dobCheckDigit}{expStr}{expCheckDigit}");

        line2 = $"{line2}" + checkDigitLine2;

        return line2[..30];
    }

    static string GenerateTd1Line1(string documentType, string nationality, string identityNumber)
    {
        if (nationality.Length > 3)
        {
            throw new Exception("nationality exceed 3 characters!");
        }

        if (identityNumber.Length != 9)
        {
            identityNumber = identityNumber.PadLeft(9, '0');
        }

        string line1 = $"{documentType.ToUpper().PadRight(2, '<')}{nationality.ToUpper()}{identityNumber.ToUpper()}{CalculateCheckDigit(identityNumber)}".PadRight(30, '<');
        return line1[..30];
    }

    static char CalculateCheckDigit(string input)
    {
        int[] weights = { 7, 3, 1 }; // MRZ weighting scheme
        int sum = 0;
        for (int i = 0; i < input.Length; i++)
        {
            int value = GetCharacterValue(input[i]);
            sum += value * weights[i % 3];
        }
        return (sum % 10).ToString()[0];
    }

    static int GetCharacterValue(char c)
    {
        if (char.IsDigit(c)) return c - '0';
        if (char.IsLetter(c)) return c - 'A' + 10;
        return 0; // For < and other invalid characters
    }
}
