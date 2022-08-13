using System;
using System.Xml;
using System.Collections;
using System.Configuration;


namespace ICS.Core.Framework
{
    #region Framwwork: Cache Configuration
    public class CacheConfig
    {
        private const int DefaultClearInterval = 60; // Seconds

        private int _cacheClearInterval = DefaultClearInterval;
        private Hashtable _items = new Hashtable();

        public int CacheClearInterval
        {
            get { return _cacheClearInterval; }
            set { _cacheClearInterval = value; }
        }

        public int GetCacheDuration(Type type)
        {
            if (_items.Contains(type.FullName))
                return ((CacheConfigItem)_items[type.FullName]).Duration;
            else
                return 0;
        }

        public void Add(CacheConfigItem item)
        {
            _items.Add(item.TypeName, item);
        }
    }
    #endregion

    #region Framwwork: Cache Configuration Item
    public class CacheConfigItem
    {
        private string _typeName;
        private int _duration;

        public CacheConfigItem(string typeName)
        {
            _typeName = typeName;
            _duration = 0;
        }

        public string TypeName
        {
            get { return _typeName; }
            set { _typeName = value; }
        }

        public int Duration
        {
            get { return _duration; }
            set { _duration = value; }
        }
    }

    public class CacheConfigSectionHandler : IConfigurationSectionHandler
    {
        public object Create(object parent, object configContext, System.Xml.XmlNode section)
        {
            CacheConfig config = new CacheConfig();

            // Read Root Level Properties
            XmlAttribute clearIntervalAtt = section.Attributes["cacheClearInterval"];
            if (clearIntervalAtt != null)
            {
                try
                {
                    config.CacheClearInterval = int.Parse(clearIntervalAtt.Value);
                }
                catch (FormatException)
                {
                    throw new ConfigurationException("Invalid Cache Clear Interval Setting");
                }
            }

            foreach (XmlNode node in section.SelectNodes("cache"))
            {
                XmlAttribute typeNameAtt = node.Attributes["type"];
                if (typeNameAtt != null)
                {
                    CacheConfigItem item = new CacheConfigItem(typeNameAtt.Value);

                    XmlAttribute durationAtt = node.Attributes["duration"];

                    if (durationAtt != null)
                    {
                        try
                        {
                            item.Duration = int.Parse(durationAtt.Value);
                        }
                        catch (FormatException)
                        {
                            throw new ConfigurationException("Invalid Cache Setting for " + item.TypeName);
                        }
                    }

                    config.Add(item);
                }
            }

            return config;
        }
    }
    #endregion
}
