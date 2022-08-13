using ICS.Core.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ESimSol.BusinessObjects
{
    #region SalaryFieldSetupDetail
    public class SalaryFieldSetupDetail : BusinessObject
    {
        public SalaryFieldSetupDetail()
        {
            SalaryFieldSetupDetailID = 0;
            SalaryFieldSetupID = 0;          
            SalaryField = EnumSalaryField.None;
            Remarks = "";
            DBUserID = 0;
            DBServerDateTime = DateTime.Now;
            UserName = "";
            LogInID = "";
            ErrorMessage = "";
        }

        #region Properties
        public int SalaryFieldSetupDetailID { get; set; }
        public int SalaryFieldSetupID { get; set; }
        public EnumSalaryField SalaryField { get; set; }
        public string Remarks { get; set; }
        public int DBUserID { get; set; }
        public DateTime DBServerDateTime { get; set; }
        public string UserName { get; set; }
        public string LogInID { get; set; }
        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public string SalaryFieldSt
        {
            get
            {
                return EnumObject.jGet(this.SalaryField);
            }
        }
        #endregion

        #region Functions
        public static List<SalaryFieldSetupDetail> Gets(int id, long nUserID)
        {
            return SalaryFieldSetupDetail.Service.Gets(id, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static ISalaryFieldSetupDetailService Service
        {
            get { return (ISalaryFieldSetupDetailService)Services.Factory.CreateService(typeof(ISalaryFieldSetupDetailService)); }
        }
        #endregion
    }
    #endregion

    #region ISalaryFieldSetupDetail interface
    public interface ISalaryFieldSetupDetailService
    {
        List<SalaryFieldSetupDetail> Gets(int id, Int64 nUserID);

    }
    #endregion
}