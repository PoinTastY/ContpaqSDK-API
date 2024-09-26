using System;

public static class NumberConverter
{
    private static readonly string[] Units = { "", "UNO", "DOS", "TRES", "CUATRO", "CINCO", "SEIS", "SIETE", "OCHO", "NUEVE" };
    private static readonly string[] Tens = { "", "DIEZ", "VEINTE", "TREINTA", "CUARENTA", "CINCUENTA", "SESENTA", "SETENTA", "OCHENTA", "NOVENTA" };
    private static readonly string[] Teens = { "DIEZ", "ONCE", "DOCE", "TRECE", "CATORCE", "QUINCE", "DIECISÉIS", "DIECISIETE", "DIECIOCHO", "DIECINUEVE" };
    private static readonly string[] Hundreds = { "", "CIENTO", "DOSCIENTOS", "TRESCIENTOS", "CUATROCIENTOS", "QUINIENTOS", "SEISCIENTOS", "SETECIENTOS", "OCHOCIENTOS", "NOVECIENTOS" };

    public static string ConvertToText(double number)
    {
        if (number == 0) return "0";

        if (number < 0) return "-" + ConvertToText(Math.Abs(number));

        string words = "";

        int integerPart = (int)number;
        int decimalPart = (int)Math.Round((number - integerPart) * 100);

        words += ConvertIntegerToText(integerPart);

        if (decimalPart > 0)
        {
            words += " PESOS " + decimalPart + "/100 MN";
        }
        else
        {
            words += " PESOS 00/100 MN";
        }

        return words.Trim();
    }

    public static string ConvertIntegerToText(int number)
    {
        if (number == 0) return "";

        string words = "";

        if ((number / 1000000) > 0)
        {
            if (number / 1000000 == 1)
            {
                words += "UN MILLON ";
            }
            else
            {
                words += ConvertIntegerToText(number / 1000000) + " MILLONES ";
            }
            number %= 1000000;
        }

        if ((number / 1000) > 0)
        {
            if (number / 1000 == 1)
            {
                words += "MIL ";
            }
            else
            {
                words += ConvertIntegerToText(number / 1000) + " MIL ";
            }
            number %= 1000;
        }

        if ((number / 100) > 0)
        {
            if (number == 100)
            {
                words += "CIEN";
            }
            else
                words += Hundreds[number / 100] + " ";
            number %= 100;
        }

        if (number > 0)
        {
            if (number == 20)
                words += "VEINTE";
            else if (number < 10)
                words += Units[number];
            else if (number < 20)
                words += Teens[number - 10];
            else if (number < 30)
            {
                words += "VEINTI" + (number % 10 == 1 ? "UN" : Units[number % 10]);
            }
            else
            {
                words += Tens[number / 10];
                if ((number % 10) > 0)
                    words += " Y " + (number % 10 == 1 ? "UN" : Units[number % 10]);
            }
        }

        if (words.Contains("UNO") && words.Contains("MIL"))
        {
            words = words.Replace("UNO MIL", "UN MIL");
        }

        return words.Trim();

    }
}