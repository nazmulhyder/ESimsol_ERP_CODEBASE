using System;
using System.Collections;

namespace ICS.Core.Utility
{
    #region Utility: Confiruration Parser
    public class ConfigStringParser
    {
        private Hashtable _attributes = new Hashtable();

        public void Parse(string configString)
        {
            _attributes.Clear();

            string[] tokens = configString.Trim().Split(';');
            foreach (string token in tokens)
            {
                string[] nameValue = token.Trim().Split('=');
                if (nameValue.Length >= 2)
                {
                    string name = nameValue[0].Trim();
                    string val = nameValue[1].Trim();

                    _attributes.Add(name, val);
                }
            }
        }

        public bool Contains(string key)
        {
            return _attributes.Contains(key);
        }

        public string this[string key]
        {
            get { return (string)_attributes[key]; }
        }

        public ConfigStringParser() { }
    }
    #endregion
}
