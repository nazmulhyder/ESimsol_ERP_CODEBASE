using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ExtraBenefitService : MarshalByRefObject, IExtraBenefitService
    {
        #region Private functions and declaration
        private ExtraBenefit MapObject(NullHandler oReader)
        {
            ExtraBenefit oExtraBenefit = new ExtraBenefit();
            oExtraBenefit.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oExtraBenefit.BOAName = oReader.GetString("BOAName");
            oExtraBenefit.EmployeeID = oReader.GetInt32("EmployeeID");
            oExtraBenefit.EmployeeCode = oReader.GetString("EmployeeCode");
            oExtraBenefit.EmployeeName = oReader.GetString("EmployeeName");
            oExtraBenefit.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oExtraBenefit.BusinessUnitAddress = oReader.GetString("BusinessUnitAddress");
            oExtraBenefit.LocationName = oReader.GetString("LocationName");
            oExtraBenefit.DepartmentName = oReader.GetString("DepartmentName");
            oExtraBenefit.DesignationName = oReader.GetString("DesignationName");
            oExtraBenefit.JoiningDate = oReader.GetDateTime("JoiningDate");
            oExtraBenefit.Salary = oReader.GetDouble("Salary");
            oExtraBenefit.InTime = oReader.GetDateTime("InTime");
            oExtraBenefit.OutTime = oReader.GetDateTime("OutTime");

            oExtraBenefit.Percent = oReader.GetDouble("Percent");
            oExtraBenefit.PerDayAmount = oReader.GetDouble("PerDayAmount");
            oExtraBenefit.PayableAmount = oReader.GetDouble("PayableAmount");
            oExtraBenefit.IsActive = oReader.GetBoolean("IsActive");
            oExtraBenefit.DaysCount = oReader.GetInt32("DaysCount");
            return oExtraBenefit;
        }

        private ExtraBenefit CreateObject(NullHandler oReader)
        {
            ExtraBenefit oExtraBenefit = MapObject(oReader);
            return oExtraBenefit;
        }

        private List<ExtraBenefit> CreateObjects(IDataReader oReader)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExtraBenefit oItem = CreateObject(oHandler);
                oExtraBenefits.Add(oItem);
            }
            return oExtraBenefits;
        }
        #endregion

        #region Interface implementation
        public ExtraBenefitService() { }
        public List<ExtraBenefit> Gets(DateTime dDateFrom, DateTime dDateTo, string BOAIDs, string sEmployeeIDs, string sLocationID, string sDepartmentIDs, string sBusinessUnitIDs, double nStartSalary, double nEndSalary, Int64 nUserID)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            ExtraBenefit oExtraBenefit = new ExtraBenefit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExtraBenefitDA.Gets(dDateFrom, dDateTo, BOAIDs, sEmployeeIDs, sLocationID, sDepartmentIDs, sBusinessUnitIDs, nStartSalary, nEndSalary, nUserID, tc);
                oExtraBenefits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) { tc.HandleError(); }
                oExtraBenefits = new List<ExtraBenefit>();
                oExtraBenefit = new ExtraBenefit();
                oExtraBenefits.Add(oExtraBenefit);
                #endregion
            }
            return oExtraBenefits;
        }

        public List<ExtraBenefit> Gets(string sSQL, bool bIsComp, Int64 nUserID)
        {
            List<ExtraBenefit> oExtraBenefits = new List<ExtraBenefit>();
            ExtraBenefit oExtraBenefit = new ExtraBenefit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExtraBenefitDA.Gets(tc, sSQL, bIsComp);
                oExtraBenefits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) { tc.HandleError(); }
                oExtraBenefits = new List<ExtraBenefit>();
                oExtraBenefit = new ExtraBenefit();
                oExtraBenefits.Add(oExtraBenefit);
                #endregion
            }
            return oExtraBenefits;
        }
        #endregion
    }
}
