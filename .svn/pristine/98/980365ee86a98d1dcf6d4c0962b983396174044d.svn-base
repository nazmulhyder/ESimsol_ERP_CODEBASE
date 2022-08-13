using System;
using System.Runtime.Remoting.Messaging;
using System.Collections;
using System.Timers;

namespace ICS.Base.Framework
{
    public class CacheElement
    {
        private const Int64 _userMaxLoginTimeInMiliSec = 8 * 60 * 60 * 1000; //eight hour -> 8 * 60 * 60 * 1000
        private const Int64 _userRenewSessionInMiliSec = 20 * 60 * 1000; //twenty minutes -> 20 * 60 * 1000;

        private string _ProjectName;
        public string ProjectName
        {
            get { return _ProjectName; }
        }

        private EnumCacheType _ItemType;
        public EnumCacheType ItemType
        {
            get { return _ItemType; }
        }

        private DateTime _MaximumActiveTime;
        public DateTime MaximumActiveTime
        {
            get { return _MaximumActiveTime; }
        }

        private DateTime _expireTime;
        public DateTime expireTime
        {
            get { return _expireTime; }
        }

        public void RenewExpireTime()
        {
            if (DateTime.Now < this.MaximumActiveTime)
                _expireTime = DateTime.Now.AddMilliseconds(_userRenewSessionInMiliSec);
            else _expireTime = this.MaximumActiveTime;
        }

        public CacheElement(CacheElement oElement, double nLifeMilisecond)
        {
            _ProjectName = oElement.ProjectName;
            _ItemType = oElement.ItemType;
            if (nLifeMilisecond != 0)
            {
                _expireTime = DateTime.Now.AddMilliseconds(nLifeMilisecond);
                _MaximumActiveTime = DateTime.Now.AddMilliseconds(_userMaxLoginTimeInMiliSec);
            }
            else
            {
                _expireTime = oElement.expireTime; //oElement._expireTime;
                _MaximumActiveTime = DateTime.Now.AddMilliseconds(_userMaxLoginTimeInMiliSec);
            }

        }

        public CacheElement(string sProjectName, EnumCacheType sItemType, double nLifeMilisecond, bool bLifeOnInTimeOrLastActive)
        {
            _ProjectName = sProjectName;
            _ItemType = sItemType;
            _expireTime = DateTime.Now.AddMilliseconds(nLifeMilisecond);
            _MaximumActiveTime = DateTime.Now.AddMilliseconds(_userMaxLoginTimeInMiliSec);
        }
    }


    public class UserCache : CacheElement
    {
        private string _UserName;
        public string UserName
        {
            get { return _UserName; }
        }

        private Int64 _UserID;
        public Int64 UserID
        {
            get { return _UserID; }
        }

        private Guid _SessionKey;
        public Guid SessionKey
        {
            get { return _SessionKey; }
        }

        private Guid _ParityKey;
        public Guid ParityKey
        {
            get { return _ParityKey; }
        }

        public UserCache(CacheElement oBase, double nLifeMilisecond, string sUserName, Int64 nUserID, Guid oSessionKey, Guid oParityKey)
            : base(oBase, nLifeMilisecond)
        {
            _UserName = sUserName;
            _UserID = nUserID;
            _SessionKey = oSessionKey;
            _ParityKey = oParityKey;
        }
    }

    public class ProjectCache : CacheElement
    {
        public new string ProjectName//Hash Key
        {
            get { return base.ProjectName; }
        }

        private string _DLLName;
        public string DLLName
        {
            get { return _DLLName; }
        }

        private Version _DLLVersion;
        public Version DLLVersion
        {
            get { return _DLLVersion; }
        }

        public ProjectCache(CacheElement oBase, double nLifeMilisecond, string sDLLName, Version oDLLVersion)
            : base(oBase, nLifeMilisecond)
        {
            _DLLName = sDLLName;
            _DLLVersion = oDLLVersion;
        }
    }

    public class ServiceCache : CacheElement
    {
        public string HashKey
        {
            get { return base.ProjectName + this.ServiceName; }
        }
        private string _ServiceName;
        public string ServiceName
        {
            get { return _ServiceName; }
        }

        private string _ServiceFullName;
        public string ServiceFullName
        {
            get { return _ServiceFullName; }
        }

        private Type _Type;
        public Type TypeObject
        {
            get { return _Type; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oBase"></param>
        /// <param name="nLifeMilisecond"> if zero value passed, then ommit assignment in Base constructor</param>
        /// <param name="oType"></param>
        public ServiceCache(CacheElement oBase, double nLifeMilisecond, Type oType)
            : base(oBase, nLifeMilisecond)
        {
            _ServiceName = oType.Name;
            _ServiceFullName = oType.FullName;
            _Type = oType;
        }
    }
}