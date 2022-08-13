using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class EmployeeLoanInfoService : MarshalByRefObject, IEmployeeLoanInfoService
    {
        #region Private functions and declaration
        private EmployeeLoanInfo MapObject(NullHandler oReader)
        {
            EmployeeLoanInfo oEmployeeLoanInfo = new EmployeeLoanInfo();
            oEmployeeLoanInfo.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeLoanInfo.Code = oReader.GetString("Code");
            oEmployeeLoanInfo.Name = oReader.GetString("Name");
            oEmployeeLoanInfo.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeLoanInfo.DesignationName = oReader.GetString("DesignationName");
            oEmployeeLoanInfo.LoanAmount = oReader.GetDouble("LoanAmount");
            oEmployeeLoanInfo.NoOfInstallment = oReader.GetInt16("NoOfInstallment");
            oEmployeeLoanInfo.InstallmentAmount = oReader.GetDouble("InstallmentAmount");
            oEmployeeLoanInfo.InterestRate = oReader.GetDouble("InterestRate");
            oEmployeeLoanInfo.DisburseDate = oReader.GetDateTime("DisburseDate");
            oEmployeeLoanInfo.InstallmentStartDate = oReader.GetDateTime("InstallmentStartDate");
            oEmployeeLoanInfo.InstallmentPaid = oReader.GetDouble("InstallmentPaid");
            oEmployeeLoanInfo.InterestPaid = oReader.GetDouble("InterestPaid");
            oEmployeeLoanInfo.InstallmentPayable = oReader.GetDouble("InstallmentPayable");
            oEmployeeLoanInfo.InterestPayable = oReader.GetDouble("InterestPayable");
            oEmployeeLoanInfo.RefundAmount = oReader.GetDouble("RefundAmount");
            oEmployeeLoanInfo.IsActive = oReader.GetBoolean("IsActive");
            return oEmployeeLoanInfo;
        }

        public static EmployeeLoanInfo CreateObject(NullHandler oReader)
        {
            EmployeeLoanInfo oEmployeeLoanInfo = new EmployeeLoanInfo();
            EmployeeLoanInfoService oService = new EmployeeLoanInfoService();
            oEmployeeLoanInfo = oService.MapObject(oReader);
            return oEmployeeLoanInfo;
        }
        private List<EmployeeLoanInfo> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLoanInfo> oEmployeeLoanInfos = new List<EmployeeLoanInfo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLoanInfo oItem = CreateObject(oHandler);
                oEmployeeLoanInfos.Add(oItem);
            }
            return oEmployeeLoanInfos;
        }

        #endregion

        #region Interface implementation
        public EmployeeLoanInfoService() { }

        public List<EmployeeLoanInfo> Gets(DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            List<EmployeeLoanInfo> oEmployeeLoanInfos = new List<EmployeeLoanInfo>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLoanInfoDA.Gets(tc, dtFrom, dtTo);
                oEmployeeLoanInfos = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLoanInfo oEmployeeLoanInfo = new EmployeeLoanInfo();
                oEmployeeLoanInfo.ErrorMessage = e.Message;
                oEmployeeLoanInfos.Add(oEmployeeLoanInfo);
                #endregion
            }

            return oEmployeeLoanInfos;
        }


        #endregion
    }
}