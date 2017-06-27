using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LayIM.NetClient
{
    public static class GlobalConfigurationExtensions
    {
        public static IGlobalConfiguration<TStorage> UseStorage<TStorage>(this IGlobalConfiguration configuration, TStorage storage) where TStorage : LayimStorage
        {
            Error.ThrowIfNull(configuration, nameof(configuration));
            Error.ThrowIfNull(storage, nameof(storage));

            return configuration.Use(storage, x => LayimStorage.Current = x);
        }

        public static IGlobalConfiguration<T> Use<T>(this IGlobalConfiguration configuration, T entry, Action<T> entryAction) {
            Error.ThrowIfNull(configuration, nameof(configuration));

            entryAction(entry);

            return new ConfigurationEntry<T>(entry);
        }

        private class ConfigurationEntry<T> : IGlobalConfiguration<T>
        {
            public ConfigurationEntry(T entry)
            {
                Entry = entry;
            }
            public T Entry { get; }
        }
    }
}
