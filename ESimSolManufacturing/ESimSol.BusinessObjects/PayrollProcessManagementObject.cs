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
    #region PayrollProcessManagementObject
    public class PayrollProcessManagementObject : BusinessObject
    {
        public PayrollProcessManagementObject()
        {
            PPMOID = 0;
            PPMID = 0;
            PPMObject = EnumPPMObject.None;
            ObjectID = 0;
            ErrorMessage = "";
        }

        #region Properties

        public int PPMOID { get; set; }
        public int PPMID { get; set; }
        public EnumPPMObject PPMObject { get; set; }
        public int ObjectID { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int PPMObjectInt { get; set; }
        public string PPMObjectInString
        {
            get
            {
                return PPMObject.ToString();
            }
        }

        #endregion

        #region Functions
        public static PayrollProcessManagementObject Get(int id, long nUserID)
        {
            return PayrollProcessManagementObject.Service.Get(id, nUserID);
        }
        public static List<PayrollProcessManagementObject> Gets(long nUserID)
        {
            return PayrollProcessManagementObject.Service.Gets(nUserID);
        }
        public static List<PayrollProcessManagementObject> Gets(string sSQL, long nUserID)
        {
            return PayrollProcessManagementObject.Service.Gets(sSQL, nUserID);
        }
        public PayrollProcessManagementObject IUD(int nDBOperation, long nUserID)
        {
            return PayrollProcessManagementObject.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IPayrollProcessManagementObjectService Service
        {
            get { return (IPayrollProcessManagementObjectService)Services.Factory.CreateService(typeof(IPayrollProcessManagementObjectService)); }
        }

        #endregion
    }
    #endregion

    #region IPayrollProcessManagementObject interface
    public interface IPayrollProcessManagementObjectService
    {
        PayrollProcessManagementObject Get(int id, Int64 nUserID);
        List<PayrollProcessManagementObject> Gets(Int64 nUserID);//nUID=UserID
        List<PayrollProcessManagementObject> Gets(string sSQL, Int64 nUserID);
        PayrollProcessManagementObject IUD(PayrollProcessManagementObject oPayrollProcessManagementObject, int nDBOperation, Int64 nUserID);
        
      
    }
    #endregion
}
