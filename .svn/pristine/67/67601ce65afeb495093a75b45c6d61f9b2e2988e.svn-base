using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region RSInQCSetup
    public class RSInQCSetup : BusinessObject
    {
        public RSInQCSetup()
        {
           RSInQCSetupID = 0;
           Name = "";
           YarnType = EnumYarnType.None;
           WorkingUnitID = 0;
           LocationID = 0;
           Activity = true;
           ErrorMessage = "";
        }

        #region Properties
        public int RSInQCSetupID { get; set; }
        public string Name { get; set; }
        public EnumYarnType YarnType { get; set; }
        public int LocationID { get; set; }
        public int WorkingUnitID { get; set; }
        public bool Activity { get; set; }
        public string ErrorMessage { get; set; }
       
        #endregion

        #region Derive Properties
        public string LocationName { get; set; }
        public string WUName { get; set; }
        public string YarnTypeStr
        {
            get
            {
                return Global.EnumerationFormatter(this.YarnType.ToString());
            }
            
        }

        #endregion

        #region Functions
        public static RSInQCSetup Get(int nId, long nUserID)
        {
            return RSInQCSetup.Service.Get(nId, nUserID);
        }

        public static List<RSInQCSetup> Gets(string sSQL, long nUserID)
        {
            return RSInQCSetup.Service.Gets(sSQL, nUserID);
        }
        public static List<RSInQCSetup> GetsBy(int nYarnType, long nUserID)
        {
            return RSInQCSetup.Service.GetsBy(nYarnType, nUserID);
        }

        public RSInQCSetup IUD(int nDBOperation, long nUserID)
        {
            return RSInQCSetup.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory

        internal static IRSInQCSetupService Service
        {
            get { return (IRSInQCSetupService)Services.Factory.CreateService(typeof(IRSInQCSetupService)); }
        }
        #endregion
    }
    #endregion

    #region IRSInQCSetup interface

    public interface IRSInQCSetupService
    {

        RSInQCSetup Get(int id, long nUserID);
        List<RSInQCSetup> Gets(string sSQL, long nUserID);
        List<RSInQCSetup> GetsBy(int nYarnType, long nUserID);
        RSInQCSetup IUD(RSInQCSetup oRSInQCSetup, int nDBOperation, long nUserID);
    }
    #endregion
}
