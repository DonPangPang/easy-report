using System.Collections;
using System.Data;
using EasyReport.Infrastructure.Dto;

namespace EasyReport.PwaClient.Service;

public class QueryHelpers
{
    public static string AddQueryString(string uri, Dictionary<string, object> @params)
    {
        var queryStringList = new List<string>();
        foreach (var item in @params)
        {
            if (item.Value is null) continue;
            if (item.Value is Array array)
            {
                foreach (var value in array)
                {
                    queryStringList.Add($"{item.Key}[]={value}");
                }
            }
            else
            {
                queryStringList.Add($"{item.Key}={item.Value}");
            }
        }

        var queryString = string.Join("&", queryStringList);
        return $"{uri}?{queryString}";
    }

    public static string AddQueryString(string uri, IQueryParameter @params)
    {
        var queryStringList = new List<string>();
        var props = @params.GetType().GetProperties();
        foreach (var item in @props)
        {
            var itemValue = item.GetValue(@params);

            if (itemValue is null) continue;
            if (itemValue.GetType() != typeof(string) && itemValue is Array or IEnumerable or IList or ICollection)
            {
                foreach (var value in (Array)itemValue)
                {
                    queryStringList.Add($"{item.Name}[]={value}");
                }
            }
            else
            {
                queryStringList.Add($"{item.Name}={itemValue}");
            }
        }

        var queryString = string.Join("&", queryStringList);
        return $"{uri}?{queryString}";
    }
}


public class ToolKitHelper
{
    public static string GetRandomRgbHexString()
    {
        var random = new Random();
        var r = random.Next(0, 255);
        var g = random.Next(0, 255);
        var b = random.Next(0, 255);
        return $"#{r:X2}{g:X2}{b:X2}";
    }
}