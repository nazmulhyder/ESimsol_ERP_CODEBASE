using System;

namespace ICS.Core.Framework
{
    #region Framwwork: Cached Item
    public class CachedItem
    {

        private DateTime _expDate;
        public DateTime ExpDate
        {
            get { return _expDate; }
        }


        private object _cachedObject;
        public object CachedObject
        {
            get { return _cachedObject; }
        }

        public CachedItem(DateTime expDate, object cachedObject)
        {
            _expDate = expDate;
            _cachedObject = cachedObject;
        }
    }
    #endregion
}
