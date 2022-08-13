using System;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Timers;


namespace ICS.Base.Framework
{
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

    public enum EnumCacheType
    {
        User = 1, Project = 2, Service = 3, ObjectInstance = 4, MethodInfo = 5
    }
    

    public class CacheManager
    {
        private const string ConfigSectionName = "ICS.Cacheconfiguration";

        //private static CacheConfig _config;
        //public static CacheConfig Configuration
        //{
        //    get { return _config; }
        //}

        public static CacheOverride Override()
        {
            return new CacheOverride();
        }

        private static ArrayList _instances = new ArrayList();
        private static Timer _timer;
        static CacheManager()
        {
            //_config = (CacheConfig)ConfigurationManager.GetSection(ConfigSectionName);

            //if (_config == null)
            //{
            //    _config = new CacheConfig(); // Creating a Default Config
            //}

            _timer = new Timer(60 * 1000); //cache clear trigger time (every sixty second)
            _timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);

        }

        private Hashtable _userCache;
        private Hashtable _projectCache;
        private Hashtable _serviceCache;
        protected int _userCacheDuration = 60;
        public CacheManager()
        {
            lock (_instances.SyncRoot)
            {
                _instances.Add(this);
            }

            _userCache = new Hashtable();
            _projectCache = new Hashtable();
            _serviceCache = new Hashtable();

            //_cacheDuration = Configuration.GetCacheDuration(type);

            if (_userCacheDuration > 0)
            {
                _timer.Enabled = true;
            }
        }
        
        public object this[params object[] parameters]
        {
            get
            {
                if (_userCacheDuration == 0 || CallContext.GetData("CacheOverride") != null)
                    return null;

                if (parameters.Length != 2) return null;

                EnumCacheType eType = (EnumCacheType)parameters[0];
                object HashKey = parameters[1];

                switch (eType)
                {
                    case EnumCacheType.User:
                        lock (_userCache.SyncRoot)
                        {
                            if (_userCache.Contains(HashKey))
                            {
                                UserCache cachedItem = (UserCache)_userCache[HashKey];
                                //if (cachedItem.expireTime > DateTime.Now)
                                return cachedItem;
                            }
                        }
                        break;
                    case EnumCacheType.Project:
                        lock (_projectCache.SyncRoot)
                        {
                            if (_projectCache.Contains(HashKey))
                            {
                                ProjectCache cachedItem = (ProjectCache)_projectCache[HashKey];
                                //if (cachedItem.expireTime > DateTime.Now)
                                return cachedItem;
                            }
                        }
                        break;
                    case EnumCacheType.Service:
                        lock (_serviceCache.SyncRoot)
                        {
                            if (_serviceCache.Contains(HashKey))
                            {
                                ServiceCache cachedItem = (ServiceCache)_serviceCache[HashKey];
                                //if (cachedItem.expireTime > DateTime.Now)
                                return cachedItem;
                            }
                        }
                        break;
                    case EnumCacheType.ObjectInstance:
                    case EnumCacheType.MethodInfo:
                    default:
                        break;
                }


                return null;
            }
        }

        public void Add(object item)
        {
            EnumCacheType cacheType = ((CacheElement)item).ItemType;

            switch (cacheType)
            {
                case EnumCacheType.User:
                    if (_userCacheDuration > 0)
                    {
                        UserCache userCacheItem = (UserCache)item;
                        lock (_userCache.SyncRoot)
                        {
                            if (_userCache.ContainsKey(userCacheItem.SessionKey))
                                _userCache[userCacheItem.SessionKey] = userCacheItem;
                            else
                                _userCache.Add(userCacheItem.SessionKey, userCacheItem);
                        }
                    }
                    break;
                case EnumCacheType.Project:
                    ProjectCache projectCacheItem = (ProjectCache)item;
                    lock (_projectCache.SyncRoot)
                    {
                        if (_projectCache.ContainsKey(projectCacheItem.ProjectName))
                            _projectCache[projectCacheItem.ProjectName] = projectCacheItem;
                        else
                            _projectCache.Add(projectCacheItem.ProjectName, projectCacheItem);
                    }
                    break;
                case EnumCacheType.Service:
                    ServiceCache serviceCacheItem = item as ServiceCache;// (ServiceCache)item;
                    lock (_serviceCache.SyncRoot)
                    {
                        if (_serviceCache.ContainsKey(serviceCacheItem.HashKey))
                            _serviceCache[serviceCacheItem.HashKey] = serviceCacheItem;
                        else
                            _serviceCache.Add(serviceCacheItem.HashKey, serviceCacheItem);
                    }
                    break;
                case EnumCacheType.ObjectInstance:
                case EnumCacheType.MethodInfo:
                default:
                    break;
            }
        }

        private static void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            lock (_instances.SyncRoot)
            {
                foreach (CacheManager cacheObject in _instances)
                {
                    ArrayList keysToRemove = new ArrayList();

                    IDictionaryEnumerator enumerator = cacheObject._userCache.GetEnumerator();//here we are clearing only 'userCashe'
                    while (enumerator.MoveNext())
                    {
                        if (((CacheElement)enumerator.Value).expireTime <= DateTime.Now)
                            keysToRemove.Add(enumerator.Key);
                    }
                    foreach (object key in keysToRemove)
                    {
                        cacheObject._userCache.Remove(key);
                    }
                }
            }
        }

        public long Count
        {
            get { return _userCache.Count; }
        }

        public void ClearUser()
        {
            _userCache.Clear();
        }
    }
}
