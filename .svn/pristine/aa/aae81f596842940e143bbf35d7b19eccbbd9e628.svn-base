using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;


namespace ESimSol.BusinessObjects
{
    #region ShiftBreakName

    public class ShiftBreakName : BusinessObject
    {
        public ShiftBreakName()
        {
            ShiftBNID = 0;
            Name = "";
            IsActive = true;
            ErrorMessage = "";

        }

        #region Properties
        public int ShiftBNID { get; set; }
        public string Name { get; set; }
        public bool IsActive { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string Activity { get { if (IsActive)return "Active"; else return "Inactive"; } }
        #endregion

        #region Functions
        public static ShiftBreakName Get(int Id, long nUserID)
        {
            return ShiftBreakName.Service.Get(Id, nUserID);
        }
        public static ShiftBreakName Get(string sSQL, long nUserID)
        {
            return ShiftBreakName.Service.Get(sSQL, nUserID);
        }
        public static List<ShiftBreakName> Gets(long nUserID)
        {
            return ShiftBreakName.Service.Gets(nUserID);
        }
        public static List<ShiftBreakName> Gets(string sSQL, long nUserID)
        {
            return ShiftBreakName.Service.Gets(sSQL, nUserID);
        }

        public ShiftBreakName IUD(int nDBOperation, long nUserID)
        {
            return ShiftBreakName.Service.IUD(this, nDBOperation, nUserID);
        }

        public static ShiftBreakName Activite(int nId, long nUserID)
        {
            return ShiftBreakName.Service.Activite(nId, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IShiftBreakNameService Service
        {
            get { return (IShiftBreakNameService)Services.Factory.CreateService(typeof(IShiftBreakNameService)); }
        }

        #endregion
    }
    #endregion

    #region IShiftBreakName interface

    public interface IShiftBreakNameService
    {
        ShiftBreakName Get(int id, Int64 nUserID);
        ShiftBreakName Get(string sSQL, Int64 nUserID);
        List<ShiftBreakName> Gets(Int64 nUserID);
        List<ShiftBreakName> Gets(string sSQL, Int64 nUserID);
        ShiftBreakName IUD(ShiftBreakName oShiftBreakName, int nDBOperation, Int64 nUserID);
        ShiftBreakName Activite(int nId, Int64 nUserID);
    }
    #endregion
}
