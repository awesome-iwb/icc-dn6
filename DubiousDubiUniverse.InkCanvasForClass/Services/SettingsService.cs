using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using DubiousDubiUniverse.InkCanvasForClass.Helpers;
using DubiousDubiUniverse.InkCanvasForClass.Models;
using DubiousDubiUniverse.InkCanvasForClass.Windows;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace DubiousDubiUniverse.InkCanvasForClass.Services
{
    /// <summary>
    /// Service to load, manage, modify, and persist application settings.
    /// </summary>
    public class SettingsService {
        private readonly ConfigurationFileHelper _configHelper;
        private readonly JsonSerializerSettings _jsonSettings;
        private Settings _settings;
        private readonly string _filePath;

        /// <summary>
        /// Creates a SettingsService for the given config file path.
        /// </summary>
        /// <param name="filePath">Path to the JSON settings file.</param>
        public SettingsService(string filePath) {
            if (string.IsNullOrWhiteSpace(filePath))
                throw new ArgumentNullException(nameof(filePath));
            _filePath = filePath;
            _configHelper = new ConfigurationFileHelper(filePath);
            _jsonSettings = new JsonSerializerSettings {
                Formatting = Formatting.Indented,
                ContractResolver = new DefaultContractResolver()
            };
        }

        /// <summary>
        /// Loads settings from file (or creates default) into memory.
        /// </summary>
        public async Task LoadAsync(CancellationToken ct = default) {
            // Read raw JSON (empty string yields default Settings)
            string raw = await _configHelper.ReadAsync(ct).ConfigureAwait(false);
            if (string.IsNullOrWhiteSpace(raw)) {
                _settings = new Settings();
            } else {
                _settings = JsonConvert.DeserializeObject<Settings>(raw, _jsonSettings)
                            ?? new Settings();
            }
        }

        /// <summary>
        /// Gets the loaded Settings object.
        /// </summary>
        public Settings Settings =>
            _settings ?? throw new InvalidOperationException("Settings not loaded. Call LoadAsync first.");

        /// <summary>
        /// Sets a setting by its JSON property path, e.g. "toolbar.behaviour.autoFading.enabled".
        /// Supports both fields and properties.
        /// </summary>
        /// <param name="jsonPath">Dot-delimited JSON property name.</param>
        /// <param name="value">New value (convertible to target type).</param>
        /// <returns>True if successful; false if not found or conversion failed.</returns>
        public bool SetSetting(string jsonPath, object value) {
            if (_settings == null)
                throw new InvalidOperationException("Settings not loaded.");

            var type = typeof(Settings);
            // Find member (field or property) with matching JsonProperty
            var member = type.GetMembers(BindingFlags.Public | BindingFlags.Instance)
                .FirstOrDefault(m => {
                    var attr = m.GetCustomAttribute<JsonPropertyAttribute>();
                    return attr != null && attr.PropertyName == jsonPath;
                });
            if (member == null)
                return false;

            Type targetType;
            bool isProperty = false;
            if (member is PropertyInfo pInfo) {
                targetType = pInfo.PropertyType;
                isProperty = true;
            } else if (member is FieldInfo fInfo) {
                targetType = fInfo.FieldType;
            } else {
                return false;
            }

            try {
                object converted;
                if (targetType.IsEnum) {
                    converted = Enum.Parse(targetType, value.ToString());
                } else {
                    converted = Convert.ChangeType(value, targetType);
                }

                if (isProperty)
                    ((PropertyInfo)member).SetValue(_settings, converted);
                else
                    ((FieldInfo)member).SetValue(_settings, converted);

                return true;
            }
            catch {
                return false;
            }
        }

        /// <summary>
        /// Persists in-memory settings back to file.
        /// </summary>
        /// <param name="enableBackup">Whether to create .bak before writing.</param>
        public async Task SaveAsync(bool enableBackup = true, CancellationToken ct = default) {
            if (_settings == null)
                throw new InvalidOperationException("Settings not loaded.");

            string raw = JsonConvert.SerializeObject(_settings, _jsonSettings);
            await _configHelper.WriteAsync(raw, enableBackup, ct).ConfigureAwait(false);
        }
    }
}
