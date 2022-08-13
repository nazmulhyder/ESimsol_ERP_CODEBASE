using System;

namespace ICS.Core.Framework
{
    #region Framwwork: Cache Parameter
    public class CacheParameter
    {
        private object[] _parameters;

        public CacheParameter(params object[] parameters)
        {
            _parameters = parameters;
        }

        public override bool Equals(object obj)
        {
            CacheParameter cacheParam2;

            cacheParam2 = obj as CacheParameter;

            if ((object)cacheParam2 == null)
                return false;

            if (_parameters.Length != cacheParam2._parameters.Length)
                return false;


            for (int i = 0; i < _parameters.Length; i++)
            {
                if (_parameters[i] == null)
                {
                    if (cacheParam2._parameters[i] != null)
                        return false;
                }
                else
                {
                    if (!_parameters[i].Equals(cacheParam2._parameters[i]))
                        return false;
                }
            }
            return true;
        }

        public static bool operator ==(CacheParameter cacheParameter1, CacheParameter cacheParameter2)
        {
            if ((object)cacheParameter1 != null)
                return cacheParameter1.Equals(cacheParameter2);
            else
                return ((object)cacheParameter2 == null);
        }

        public static bool operator !=(CacheParameter cacheParameter1, CacheParameter cacheParameter2)
        {
            if ((object)cacheParameter1 != null)
                return !cacheParameter1.Equals(cacheParameter2);
            else
                return ((object)cacheParameter2 != null);
        }

        public override int GetHashCode()
        {
            if (_parameters.Length == 0)
                return base.GetHashCode();
            else
            {
                int hash = 0;
                for (int i = 0; i < _parameters.Length; i++)
                {
                    if (_parameters[i] != null)
                    {
                        hash ^= _parameters[i].GetHashCode();
                    }
                }
                return hash;
            }
        }
    }
    #endregion
}
