using System.ComponentModel.DataAnnotations;

namespace App.BLL.DTO.Enums;

public class Platform
{
    [MaxLength(64)] public string Name { get; }
    public Platform? ParentPlatform { get; }

    private Platform(string name, Platform? parentPlatform = null)
    {
        Name = name;
        ParentPlatform = parentPlatform;
    }

    // Enum-like behaviour
    public static readonly Platform This = new("This");
    public static readonly Platform YouTube = new("YouTube");

    public static readonly List<Platform> DefinedPlatforms = new()
    {
        YouTube
    };

    public override string ToString()
    {
        return Name;
    }

    private static Platform GetOrCreatePlatform(string name) => DefinedPlatforms.Find(p => p.Name == name) ??
                                                                new Platform(name);

    public static implicit operator Platform(string name) => GetOrCreatePlatform(name);

    public static implicit operator string(Platform platform) => platform.Name;
    public static implicit operator Platform(App.DAL.DTO.Enums.Platform platform) => GetOrCreatePlatform(platform.Name);
    public static implicit operator App.DAL.DTO.Enums.Platform(Platform platform) => platform.Name;
}