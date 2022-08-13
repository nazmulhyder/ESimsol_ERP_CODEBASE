using System;
using System.Xml;
using System.Collections;
using System.Configuration;

namespace ICS.Core.Framework
{
    #region Framework: Message configuration
    [Serializable]
    public class MessageService : CollectionBase
    {
        #region Declaration
        private const string FileName = "MessageService.xml";
        private MessageService _config = null;
        private MessageService _configItems = null;
        #endregion

        #region Constructor
        public MessageService()
        {
            InnerList.Clear();
        }

        public MessageService(string sName)
        {
            ReadConfig();
            MakeItems(sName);
        }

        private void ReadConfig()
        {
            if (_config == null)
            {
                try
                {
                    _config = MessageConfigSectionHandler.GetConfigItems(FileName);

                }
                catch
                {
                    if (_config == null)
                    {
                        _config = new MessageService(); // Creating a Default Config
                    }
                }
            }
        }
        #endregion

        #region Functions
        public MessageService Configuration
        {
            get { return _configItems; }
        }

        private void MakeItems(string name)
        {
            _configItems = new MessageService();
            foreach (MessageServiceItem oItem in _config)
            {
                if (oItem.RealatingTable == name)
                {
                    _configItems.Add(oItem);
                }
            }
        }
        public void Add(MessageServiceItem oItem)
        {
            InnerList.Add(oItem);
        }
        #endregion

        #region Message string
        private string _message = "";
        public string CurrentMessage
        {
            get { return _message; }
            set { _message = value; }
        }
        #endregion

        #region Indexer
        public MessageServiceItem this[int index]
        {
            get { return (MessageServiceItem)InnerList[index]; }
        }

        #endregion
    }
    #endregion

    #region Framework: Message configuration item
    [Serializable]
    public class MessageServiceItem
    {
        #region Relating Table & value
        public MessageServiceItem(string relatingTable)
        {
            _message = "";
            _realtingValue = 0;
            _realtedTable = "";
            _realtedField = "";
            _realtingTable = relatingTable;
        }
        private string _realtingTable = "";
        public string RealatingTable
        {
            get { return _realtingTable; }
            set { _realtingTable = value; }
        }
        private object _realtingValue = 0;
        public object RealatingValue
        {
            get { return _realtingValue; }
            set { _realtingValue = value; }
        }

        #endregion
        #region Related Table & Field
        private string _realtedTable = "";
        public string RealatedTable
        {
            get { return _realtedTable; }
            set { _realtedTable = value; }
        }
        private string _realtedField = "";
        public string RealatedField
        {
            get { return _realtedField; }
            set { _realtedField = value; }
        }

        #endregion
        #region Message
        private string _message = "";
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }
        public string CommandText
        {
            get { return "SELECT COUNT(*) FROM " + _realtedTable + " WHERE " + _realtedField + " = " + _realtingValue; }
        }
        #endregion
    }
    #endregion

    #region Framework: Message Config Section Handler
    internal class MessageConfigSectionHandler
    {
        private const string ConfigSectionName = "ICS.MessageService";

        static MessageConfigSectionHandler() { }
        public static MessageService GetConfigItems(string sFileSpec)
        {
            MessageService config = new MessageService();

            XmlNode section = null;
            XmlDocument oDoc = new XmlDocument();
            XmlTextReader reader = new XmlTextReader(sFileSpec);
            oDoc.Load(reader);
            reader.Close();

            if (oDoc.GetElementsByTagName(ConfigSectionName).Count <= 0)
            {
                throw new ConfigurationException("Invalid Section Setting");
            }

            // Read Root Level Properties
            section = oDoc.GetElementsByTagName(ConfigSectionName).Item(0);

            foreach (XmlNode node in section.SelectNodes("relation"))
            {
                XmlAttribute rlngNameAtt = node.Attributes["relating"];
                if (rlngNameAtt != null)
                {
                    MessageServiceItem item = new MessageServiceItem(rlngNameAtt.Value);

                    XmlAttribute rltdAtt = node.Attributes["related"];
                    if (rltdAtt != null)
                    {
                        try
                        {
                            string[] flds = rltdAtt.Value.Split(';');
                            if (flds.Length > 1)
                            {
                                item.RealatedTable = flds[0];
                                item.RealatedField = flds[1];
                            }
                        }
                        catch (FormatException)
                        {
                            throw new ConfigurationException("Invalid Message Setting for " + item.RealatingTable);
                        }
                    }

                    XmlAttribute msgAtt = node.Attributes["message"];
                    if (msgAtt != null)
                    {
                        try
                        {
                            item.Message = msgAtt.Value;
                        }
                        catch (FormatException)
                        {
                            throw new ConfigurationException("Invalid Message Setting for " + item.RealatingTable);
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
