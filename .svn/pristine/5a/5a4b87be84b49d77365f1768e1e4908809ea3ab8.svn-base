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
    public class SalarySummary_MAMIYA_NatureWiseService : MarshalByRefObject, ISalarySummary_MAMIYA_NatureWiseService
    {
        #region Private functions and declaration
        private SalarySummary_MAMIYA_NatureWise MapObject(NullHandler oReader)
        {
            SalarySummary_MAMIYA_NatureWise oSalarySummary_MAMIYA_NatureWise = new SalarySummary_MAMIYA_NatureWise();

            oSalarySummary_MAMIYA_NatureWise.DepartmentID = oReader.GetInt32("DepartmentID");
            oSalarySummary_MAMIYA_NatureWise.DepartmentName = oReader.GetString("DepartmentName");
            oSalarySummary_MAMIYA_NatureWise.Wages = oReader.GetDouble("Wages");
            oSalarySummary_MAMIYA_NatureWise.OTWages = oReader.GetDouble("OTWages");
            oSalarySummary_MAMIYA_NatureWise.Salary = oReader.GetDouble("Salary");
            oSalarySummary_MAMIYA_NatureWise.OTSalary = oReader.GetDouble("OTSalary");
            oSalarySummary_MAMIYA_NatureWise.TotalWages = oReader.GetDouble("TotalWages");
            oSalarySummary_MAMIYA_NatureWise.TotalSalary = oReader.GetDouble("TotalSalary");
            oSalarySummary_MAMIYA_NatureWise.BonusWork = oReader.GetDouble("BonusWork");
            oSalarySummary_MAMIYA_NatureWise.BonusOth = oReader.GetDouble("BonusOth");

            return oSalarySummary_MAMIYA_NatureWise;

        }

        private SalarySummary_MAMIYA_NatureWise CreateObject(NullHandler oReader)
        {
            SalarySummary_MAMIYA_NatureWise oSalarySummary_MAMIYA_NatureWise = MapObject(oReader);
            return oSalarySummary_MAMIYA_NatureWise;
        }

        private List<SalarySummary_MAMIYA_NatureWise> CreateObjects(IDataReader oReader)
        {
            List<SalarySummary_MAMIYA_NatureWise> oSalarySummary_MAMIYA_NatureWises = new List<SalarySummary_MAMIYA_NatureWise>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SalarySummary_MAMIYA_NatureWise oItem = CreateObject(oHandler);
                oSalarySummary_MAMIYA_NatureWises.Add(oItem);
            }
            return oSalarySummary_MAMIYA_NatureWises;
        }

        #endregion

        #region Interface implementation
        public SalarySummary_MAMIYA_NatureWiseService() { }

        public List<SalarySummary_MAMIYA_NatureWise> Gets(DateTime StartDate, DateTime EndDate,string sEmpIDs, int LocationID, string sDepartmentIDs, string sDesignationIDs, string sSalarySchemeIDs, bool bBankPay, Int64 nUserID)
        {
            List<SalarySummary_MAMIYA_NatureWise> oSalarySummary_MAMIYA_NatureWises = new List<SalarySummary_MAMIYA_NatureWise>() ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SalarySummary_MAMIYA_NatureWiseDA.Gets( StartDate,  EndDate, sEmpIDs,  LocationID,  sDepartmentIDs,  sDesignationIDs,  sSalarySchemeIDs,bBankPay, tc);
                oSalarySummary_MAMIYA_NatureWises = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SalarySummary_MAMIYA_NatureWise", e);
                #endregion
            }
            return oSalarySummary_MAMIYA_NatureWises;
        }

        #endregion


    }
}
