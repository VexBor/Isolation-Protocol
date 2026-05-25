using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.Json;

namespace Isolation_Protocol.Services;

public static class Logs
{
    private static readonly string _path = "Assets/Data/log.txt";

    private static string _format => DateTime.Now + " - ";

    private static List<string> _logs = new();

    public static void Init()
    {
        if (File.Exists(_path))
        {
            var data = File.ReadAllText(_path);
            _logs = JsonSerializer.Deserialize<List<string>>(data) ?? new List<string>();
        }
    }
    
    public static  void Add(string log)
    {
        _logs.Add(_format + log);
    }

    public static void Save()
    {
        File.WriteAllText(_path, JsonSerializer.Serialize(_logs));
    }

    public static string Get()
    {
        return string.Join(Environment.NewLine, _logs);
    }
}