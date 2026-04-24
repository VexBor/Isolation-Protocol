using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Avalonia.Input;
using Avalonia.Platform;
using Isolation_Protocol.Models;
using SkiaSharp;

namespace Isolation_Protocol.Services;

public static class Authorize
{
    public static User? CurrentUser = null;
    
    private static string _filePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Data/User.json");
    private static string _currentUserPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Data/UserData.json");
    private static Random _random = new Random();
    private static List<User>? _users = new List<User>();

    public static void Initialize()
    {
        if (File.Exists(_currentUserPath))
        {
            var json = File.ReadAllText(_currentUserPath);
        
            CurrentUser = JsonSerializer.Deserialize<User>(json);    
        }
    }
    
    public static bool Login(string username, string password)
    {
        if (!File.Exists(_filePath)) return false;
        
        var json = File.ReadAllText(_filePath);
        
        if(json.Length == 0) return false;
        
        _users = JsonSerializer.Deserialize<List<User>>(json);    
        
        if(_users == null) return false;

        var user = _users.FirstOrDefault(x => x.Username == username && Hasher.Verify(password, x.Password));
        
        if(user != null)
        {
            CurrentUser = user;
            File.WriteAllText(_currentUserPath, JsonSerializer.Serialize(CurrentUser));
            return true;
        }
        return false;
    }

    public static void Register(string username, string password, string email)
    {
        string hashedPassword = Hasher.HashPassword(password);
        
        User newUser = new User()
        {
            Id = _random.Next(),
            Username = username,
            Password = hashedPassword,
            Email = email
        };
        CurrentUser = newUser;
        _users.Add(newUser);
        File.WriteAllText(_filePath, JsonSerializer.Serialize(_users));
        File.WriteAllText(_currentUserPath, JsonSerializer.Serialize(CurrentUser));
    }
    
    public static bool FindUser(string username, string email)
    {
        var user = _users.FirstOrDefault(x => x.Username == username || x.Email == email);
        if(user == null) return false;
        return true;
    }

    public static User? GetCurrentUser() =>  CurrentUser;
}