using System;
using System.Collections.Generic;
using System.IO;
using Isolation_Protocol.View;
using NetCoreAudio;

namespace Isolation_Protocol.Services;

public static class Sound
{
    private static Player _sfxPlayer = new();
    private static string _basePath = "Assets/Sounds";
    
    private static Dictionary<string, string> SoundMap = new()
    {
        {"click", "ui_click.mp3"},
        {"chest", "opened_chest.mp3"},
        {"tree", "hit_tree.mp3"},
        {"stone", "hit_stone.mp3"},
        {"slot", "chance_slot.mp3"},
        {"music", "fon_music.mp3"}
    };
    
    public static void PlaySfx(string tag)
    {
        if (SoundMap.TryGetValue(tag, out var fileName))
        {
            if (File.Exists(_basePath + $"/{fileName}"))
            {
                _sfxPlayer.SetVolume((byte)SettingsViewModel.Settings.Volume);
                _sfxPlayer.Play(_basePath + $"/{fileName}");
            }
        }
    }
}