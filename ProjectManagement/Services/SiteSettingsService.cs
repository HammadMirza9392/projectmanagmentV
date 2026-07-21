using System.Text.Json;
using ProjectManagement.Models;

namespace ProjectManagement.Services
{
    // Reads/writes site branding settings to a JSON file in the app content root.
    // Registered as a singleton â€” settings are cached in memory and only re-read
    // from disk when the file changes (or on first access).
    public class SiteSettingsService
    {
        private readonly string _filePath;
        private readonly object _lock = new();
        private SiteSettings? _cached;
        private DateTime _cachedFileTime;

        private static readonly JsonSerializerOptions JsonOpts = new() { WriteIndented = true };

        public SiteSettingsService(IWebHostEnvironment env)
        {
            _filePath = Path.Combine(env.ContentRootPath, "sitesettings.json");
        }

        // Current settings (cached). Falls back to defaults if the file is missing/invalid.
        public SiteSettings Get()
        {
            lock (_lock)
            {
                try
                {
                    if (File.Exists(_filePath))
                    {
                        var lastWrite = File.GetLastWriteTimeUtc(_filePath);
                        if (_cached == null || lastWrite != _cachedFileTime)
                        {
                            var json = File.ReadAllText(_filePath);
                            _cached = JsonSerializer.Deserialize<SiteSettings>(json) ?? new SiteSettings();
                            _cachedFileTime = lastWrite;
                        }
                    }
                    else
                    {
                        _cached ??= new SiteSettings();
                    }
                }
                catch
                {
                    // On any read/parse error, fall back to defaults so the app never breaks.
                    _cached ??= new SiteSettings();
                }

                return _cached;
            }
        }

        // Persists the given settings to disk and refreshes the cache.
        public void Save(SiteSettings settings)
        {
            lock (_lock)
            {
                var json = JsonSerializer.Serialize(settings, JsonOpts);
                File.WriteAllText(_filePath, json);
                _cached = settings;
                _cachedFileTime = File.GetLastWriteTimeUtc(_filePath);
            }
        }
    }
}

