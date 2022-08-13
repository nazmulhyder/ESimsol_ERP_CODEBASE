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
    public class ConsolidatePay_MAMIYAService : MarshalByRefObject, IConsolidatePay_MAMIYAService
    {
        #region Private functions and declaration
        private ConsolidatePay_MAMIYA MapObject(NullHandler oReader)
        {
            ConsolidatePay_MAMIYA oConsolidatePay_MAMIYA = new ConsolidatePay_MAMIYA();
            oConsolidatePay_MAMIYA.DepartmentName = oReader.GetString("DepartmentName");
            oConsolidatePay_MAMIYA.Wages = oReader.GetDouble("Wages");
            oConsolidatePay_MAMIYA.Salary = oReader.GetDouble("Salary");
            oConsolidatePay_MAMIYA.OTWages = oReader.GetDouble("OTWages");
            oConsolidatePay_MAMIYA.OTSalary = oReader.GetDouble("OTSalary");
            oConsolidatePay_MAMIYA.TotalWages = oReader.GetDouble("TotalWages");
            oConsolidatePay_MAMIYA.TotalSalary = oReader.GetDouble("TotalSalary");
            oConsolidatePay_MAMIYA.BonusWages = oReader.GetDouble("BonusWages");
            oConsolidatePay_MAMIYA.BonusSalary = oReader.GetDouble("BonusSalary");

            return oConsolidatePay_MAMIYA;
        }

        private ConsolidatePay_MAMIYA CreateObject(NullHandler oReader)
        {
            ConsolidatePay_MAMIYA oConsolidatePay_MAMIYA = MapObject(oReader);
            return oConsolidatePay_MAMIYA;
        }

        private List<ConsolidatePay_MAMIYA> CreateObjects(IDataReader oReader)
        {
            List<ConsolidatePay_MAMIYA> oConsolidatePay_MAMIYA = new List<ConsolidatePay_MAMIYA>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ConsolidatePay_MAMIYA oItem = CreateObject(oHandler);
                oConsolidatePay_MAMIYA.Add(oItem);
            }
            return oConsolidatePay_MAMIYA;
        }

        #endregion

        #region Interface implementation
        public ConsolidatePay_MAMIYAService() { }

        public List<ConsolidatePay_MAMIYA> Gets(DateTime dtDateFrom, DateTime dtDateTo,string sEmpIDs, int nLocationID, string sDepartmentIds, string sDesignationIDs, string sSalarySchemeIDs, int nMonthID, int nPayType, Int64 nUserID)
        {
            List<ConsolidatePay_MAMIYA> oConsolidatePay_MAMIYA = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ConsolidatePay_MAMIYADA.Gets( dtDateFrom,  dtDateTo,sEmpIDs, nLocationID, sDepartmentIds, sDesignationIDs, sSalarySchemeIDs, nMonthID, nPayType, tc);
                oConsolidatePay_MAMIYA = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ConsolidatePay_MAMIYA", e);
                #endregion
            }
            return oConsolidatePay_MAMIYA;
        }

        #endregion
    }
}