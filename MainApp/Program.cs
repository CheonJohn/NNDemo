using System;
using System.IO;
using System.Collections.Generic;

public class MainApp
{

    static public void Main(string[] args)
    {


        if (args[0].Length > 0)
        {
            System.Console.WriteLine(GetMaxStringKaibun(args[0]));
        }
        else
        {
            var lines = GetStdin();
            for (int i = 0; i < lines.Length; i++)
            {
                string output = String.Format(GetMaxStringKaibun(lines[0]));
                Console.WriteLine(output);
            }
        }

    }

    private static string GetMaxStringKaibun(string data)
    {
        List<string> nominate = new List<string>();

        var allValues = GetAllCombination(data);

        var uniqueValues = Distinct(allValues);

        foreach (var item in uniqueValues)
        {
            var a = IsKaibun(item);
            if (item.Length > 1 && IsKaibun(item)) nominate.Add(item);
        }

        return getMaxLenghtString(nominate);
    }

    private static List<String> GetAllCombination(string str)
    {
        List<string> nominate = new List<string>();

        for (int i = 0; i < str.Length; i++)
        {
            for (int j = i; j < str.Length; j++)
            {
                nominate.Add(str.Substring(i, str.Length - j));
            }

        }

        return nominate;
    }

    private static string getMaxLenghtString(List<string> data)
    {
        var maxLengthData = "";

        foreach (var item in data)
        {
            if(item.Length > maxLengthData.Length)
            {
                maxLengthData = item;
            }
        }

        return maxLengthData;
    }

    private static List<string> Distinct(List<string> data) 
    {
        HashSet<string> distinctSet = new HashSet<string>(data);
        return  new List<string>(distinctSet);
    }

    private static bool IsKaibun(string s)
    {        
        return Reverse(s).Equals(s);
    }

    private static bool IsKaibun(int num)
    {        
        return IsKaibun(num.ToString());
    }

    private static string Reverse(string str)
    {
        var strCharArray = str.ToCharArray();
        Array.Reverse(strCharArray);
     
        return new String(strCharArray);

    }

}
