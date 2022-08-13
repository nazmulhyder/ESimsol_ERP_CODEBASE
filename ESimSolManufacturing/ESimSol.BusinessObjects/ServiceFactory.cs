using System;
using System.Collections.Generic;
using System.Text;
using ICS.Core.Framework;

namespace ESimSol.BusinessObjects
{
    #region Delegate: Collection change Event
    public delegate void NewItemAdded();
    #endregion

    #region Services
    internal class Services
    {
        private Services() { }

        private static ServiceFactory _factory;
        internal static ServiceFactory Factory
        {
            get { return _factory; }
        }

        static Services()
        {
            _factory = new ServiceFactory("ICS.Core.ServiceFactory");
        }
    }
    #endregion
}
