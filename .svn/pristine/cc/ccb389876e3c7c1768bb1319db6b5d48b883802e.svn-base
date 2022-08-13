using System;
using System.Runtime.Serialization;

namespace ICS.Core.Framework
{
    #region Framework: Service Exception Object
    [Serializable]
    public class ServiceException : ApplicationException
    {
        public ServiceException() : base() { }
        public ServiceException(string message) : base(message) { }
        public ServiceException(string message, Exception inner) : base(message, inner) { }
        public ServiceException(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
        }

    }
    #endregion
}
