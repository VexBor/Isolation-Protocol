using System;
using System.Collections.Generic;
using System.IO;
using NetCoreAudio;

namespace Isolation_Protocol.Services;

public static class Sound
{
    private static Player _sfxPlayer = new();
    private static Player _musicPlayer = new();
    private static string _basePath = Path.Combine(Directory.GetCurrentDirectory(), "Assets/Sounds");
    
    private static Dictionary<string, string> SoundMap = new()
    {
        {"click", "ui_click.mp3"},
        {"chest", "opened_chest.mp3"},
        {"tree", "hit_tree.mp3"},
        {"stone", "hit_stone.mp3"},
        {"slot", "chance_slot.mp3"}
    };
    
    public static void PlaySfx(string tag)
    {
        if (SoundMap.TryGetValue(tag, out var fileName))
        {
            string fullPath = Path.Combine(_basePath, fileName);
            if (File.Exists(fullPath)) _sfxPlayer.Play(fullPath);
        }
    }
}