using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region ShiftBULocConfigure
    public class ShiftBULocConfigure
    {
        public ShiftBULocConfigure()
        {

            ShiftBULocID = 0;
            BUID = 0;
            LocationID = 0;
            ShiftID = 0;
            ErrorMessage = "";
            BusinessUnitName = "";
            LocationName = "";
            Name = "";
        }

        #region Properties
        public int ShiftBULocID { get; set; }
        public int BUID { get; set; }
        public int LocationID { get; set; }
        public int ShiftID { get; set; }
        public string ErrorMessage { get; set; }
        public string BusinessUnitName { get; set; }
        public string LocationName { get; set; }
        public string Name { get; set; }
        #endregion

        #region Functions


        public static List<ShiftBULocConfigure> Gets(string sSQL, long nUserID)
        {
            return ShiftBULocConfigure.Service.Gets(sSQL, nUserID);
        }
        public static ShiftBULocConfigure Get(string sSQL, long nUserID)
        {
            return ShiftBULocConfigure.Service.Get(sSQL, nUserID);
        }
        public ShiftBULocConfigure IUD( long nUserID)
        {
            return ShiftBULocConfigure.Service.IUD(this, nUserID);
        }
 
        #endregion

        #region ServiceFactory

        internal static IShiftBULocConfigureService Service
        {
            get { return (IShiftBULocConfigureService)Services.Factory.CreateService(typeof(IShiftBULocConfigureService)); }
        }
        #endregion
    }
    #endregion

    #region IShiftBULocConfigure interface

    public interface IShiftBULocConfigureService
    {
        List<ShiftBULocConfigure> Gets(string sSQL, Int64 nUserID);
        ShiftBULocConfigure Get(string sSQL, Int64 nUserID);
        ShiftBULocConfigure IUD(ShiftBULocConfigure oApprovalHead, Int64 nUserID);
       
      
    }
    #endregion
}


