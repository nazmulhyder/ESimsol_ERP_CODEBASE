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
    #region EmployeeCodeDetail

    public class EmployeeCodeDetail : BusinessObject
    {
        public EmployeeCodeDetail()
        {
            ECDID = 0;
            EmployeeCodeID = 0;
            ECDType = EnumVoucherCodeType.None;
            Value = "";
            Length = 0;
            Restart = EnumRestartPeriod.None;
            Sequence = 0;  
            ErrorMessage = "";
        }



        #region Properties

        public int ECDID { get; set; }
        public int EmployeeCodeID { get; set; }
        public EnumVoucherCodeType ECDType { get; set; }
        public string Value { get; set; }
        public int Length { get; set; }
        public EnumRestartPeriod Restart { get; set; }
        public int Sequence { get; set; }
        public int ECDTypeInInt { get; set; }
        public int RestartInInt { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public string ECDTypeInString
        {
            get { return ECDType.ToString(); }
        }

        public string RestartInString
        {
            get { return Restart.ToString(); }
        }


        #endregion

        #region Functions
        public static EmployeeCodeDetail Get(int id, long nUserID)
        {
            return EmployeeCodeDetail.Service.Get(id, nUserID);
        }

        public static List<EmployeeCodeDetail> Gets(long nUserID)
        {
            return EmployeeCodeDetail.Service.Gets(nUserID);
        }

        public static List<EmployeeCodeDetail> Gets(string sSQL, long nUserID)
        {
            return EmployeeCodeDetail.Service.Gets(sSQL, nUserID);
        }

        public EmployeeCodeDetail IUD(int nDBOperation, long nUserID)
        {
            return EmployeeCodeDetail.Service.IUD(this, nDBOperation, nUserID);
        }

   

        #endregion

        #region ServiceFactory
        internal static IEmployeeCodeDetailService Service
        {
            get { return (IEmployeeCodeDetailService)Services.Factory.CreateService(typeof(IEmployeeCodeDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeCodeDetail interface
    public interface IEmployeeCodeDetailService
    {
        EmployeeCodeDetail Get(int id, Int64 nUserID);
        List<EmployeeCodeDetail> Gets(Int64 nUserID);
        List<EmployeeCodeDetail> Gets(string sSQL, Int64 nUserID);
        EmployeeCodeDetail IUD(EmployeeCodeDetail oEmployeeCodeDetail, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
