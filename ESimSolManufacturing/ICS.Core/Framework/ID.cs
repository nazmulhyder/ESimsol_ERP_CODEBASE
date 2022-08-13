using System;
using System.Collections;

namespace ICS.Core.Framework
{
    #region Framework: ID Generation
    [Serializable]
    public struct ID
    {
        public static readonly ID Empty = new ID(0);
        private int _val;

        public int ToInt32
        {
            get { return _val; }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return ((ID)obj)._val == _val;
        }

        public override int GetHashCode()
        {
            return ((int)_val ^ (int)(_val >> 16));
        }

        public static bool operator ==(ID id1, ID id2)
        {
            return id1._val == id2._val;
        }

        public static bool operator !=(ID id1, ID id2)
        {
            return id1._val != id2._val;
        }

        public ID(int id)
        {
            _val = id;
        }

        public override string ToString()
        {
            return _val.ToString();
        }
    }
    #endregion
}
