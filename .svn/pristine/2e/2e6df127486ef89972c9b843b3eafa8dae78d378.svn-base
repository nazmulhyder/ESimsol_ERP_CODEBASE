using System;
using System.Timers;
using System.Collections;
using System.Configuration;
using System.Runtime.Remoting.Messaging;

namespace ICS.Core.Framework
{
    #region Framwwork: Cache Override Object
    public class CacheOverride : IDisposable, ILogicalThreadAffinative
    {
        internal CacheOverride()
        {
            CallContext.SetData("CacheOverride", this);
        }

        public void Dispose()
        {
            CallContext.SetData("CacheOverride", null);
        }
    }
    #endregion

    #region Framwwork: Cache object
    public class Cache
    {
        private const string ConfigSectionName = "ICS.Cacheconfiguration";

        private static CacheConfig _config;
        public static CacheConfig Configuration
        {
            get { return _config; }
        }

        public static CacheOverride Override()
        {
            return new CacheOverride();
        }

        private static ArrayList _instances = new ArrayList();
        private static Timer _timer;
        static Cache()
        {
            _config = (CacheConfig)ConfigurationSettings.GetConfig(ConfigSectionName);

            if (_config == null)
            {
                _config = new CacheConfig(); // Creating a Default Config
            }

            _timer = new Timer(Configuration.CacheClearInterval * 1000);
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

        }

        private Hashtable _cachedItems;
        protected int _cacheDuration;
        public Cache(Type type)
        {
            lock (_instances.SyncRoot)
            {
                _instances.Add(this);
            }

            _cachedItems = new Hashtable();
            _cacheDuration = Configuration.GetCacheDuration(type);

            if (_cacheDuration > 0)
            {
                _timer.Enabled = true;
            }
        }

        public object this[params object[] parameters]
        {
            get
            {
                if (_cacheDuration == 0 || CallContext.GetData("CacheOverride") != null)
                    return null;

                CacheParameter cacheParameter = new CacheParameter(parameters);
                lock (_cachedItems.SyncRoot)
                {
                    if (_cachedItems.Contains(cacheParameter))
                    {
                        CachedItem cachedItem = (CachedItem)_cachedItems[cacheParameter];
                        if (cachedItem.ExpDate > DateTime.Now)
                            return cachedItem.CachedObject;
                    }
                }

                return null;
            }
        }

        public void Add(object item, params object[] parameters)
        {
            if (_cacheDuration > 0)
            {
                CacheParameter cacheParameter = new CacheParameter(parameters);
                CachedItem cachedItem = new CachedItem(DateTime.Now.AddSeconds(_cacheDuration), item);
                lock (_cachedItems.SyncRoot)
                {
                    if (_cachedItems.ContainsKey(cacheParameter))
                        _cachedItems[cacheParameter] = cachedItem;
                    else
                        _cachedItems.Add(cacheParameter, cachedItem);
                }
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (_instances.SyncRoot)
            {
                foreach (Cache cache in _instances)
                {
                    ArrayList keysToRemove = new ArrayList();

                    IDictionaryEnumerator enumerator = cache._cachedItems.GetEnumerator();
                    while (enumerator.MoveNext())
                    {
                        if (((CachedItem)enumerator.Value).ExpDate <= DateTime.Now)
                            keysToRemove.Add(enumerator.Key);
                    }
                    foreach (object key in keysToRemove)
                    {
                        cache._cachedItems.Remove(key);
                    }
                }
            }
        }

        public long Count
        {
            get { return _cachedItems.Count; }
        }

        public void Clear()
        {
            _cachedItems.Clear();
        }
    }
    #endregion
}
