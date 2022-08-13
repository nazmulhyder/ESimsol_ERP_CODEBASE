using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.Services.DataAccess.ReportingDA;

namespace ESimSol.Services.Services
{
    public class ExtraOTDynamicService : MarshalByRefObject, IExtraOTDynamicService
    {
        #region Private functions and declaration
        private ExtraOTDynamic MapObject(NullHandler oReader)
        {
            ExtraOTDynamic oExtraOTDynamic = new ExtraOTDynamic();
            oExtraOTDynamic.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oExtraOTDynamic.EmployeeID = oReader.GetInt32("EmployeeID");
            oExtraOTDynamic.BUID = oReader.GetInt32("BUID");
            oExtraOTDynamic.LocationID = oReader.GetInt32("LocationID");
            oExtraOTDynamic.DepartmentID = oReader.GetInt32("DepartmentID");
            oExtraOTDynamic.DesignationID = oReader.GetInt32("DesignationID");
            oExtraOTDynamic.TotalDays = oReader.GetInt32("TotalDays");
            oExtraOTDynamic.LWP = oReader.GetInt32("LWP");
            oExtraOTDynamic.P = oReader.GetInt32("P");
            oExtraOTDynamic.Gross = oReader.GetDouble("Gross");
            oExtraOTDynamic.Basics = oReader.GetDouble("Basics");
            oExtraOTDynamic.OT_HR = oReader.GetDouble("OT_HR");
            oExtraOTDynamic.OT_Rate = oReader.GetDouble("OT_Rate");
            oExtraOTDynamic.OT_Amount = oReader.GetDouble("OT_Amount");
            oExtraOTDynamic.Code = oReader.GetString("Code");
            oExtraOTDynamic.Name = oReader.GetString("Name");
            oExtraOTDynamic.DateOfJoin = oReader.GetDateTime("DateOfJoin");
            oExtraOTDynamic.BUName = oReader.GetString("BUName");
            oExtraOTDynamic.LocName = oReader.GetString("LocName");
            oExtraOTDynamic.DptName = oReader.GetString("DptName");
            oExtraOTDynamic.DsgName = oReader.GetString("DsgName");
            oExtraOTDynamic.Grade = oReader.GetString("Grade");
            oExtraOTDynamic.EmpNameInBangla = oReader.GetString("EmpNameInBangla");
            oExtraOTDynamic.DsgNameInBangla = oReader.GetString("DsgNameInBangla");
            oExtraOTDynamic.BUAddress = oReader.GetString("BUAddress");
            return oExtraOTDynamic;

        }

        private ExtraOTDynamic CreateObject(NullHandler oReader)
        {
            ExtraOTDynamic oExtraOTDynamic = MapObject(oReader);
            return oExtraOTDynamic;
        }

        private List<ExtraOTDynamic> CreateObjects(IDataReader oReader)
        {
            List<ExtraOTDynamic> oExtraOTDynamic = new List<ExtraOTDynamic>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExtraOTDynamic oItem = CreateObject(oHandler);
                oExtraOTDynamic.Add(oItem);
            }
            return oExtraOTDynamic;
        }


        #endregion

        #region Interface implementation
        public ExtraOTDynamicService() { }


        public List<ExtraOTDynamic> Gets(string BUIDs, string LocationIDs, string DepartmentIDs, string DesignationIDs, string SalarySchemeIDs, string BlockIds, string EmployeeIDs, int nMonthID, int nYear, bool IsNewJoin, int IsOutSheet, double nMinSalary, double nMaxSalary, int nMOCID, Int64 nUserId)
        {
            List<ExtraOTDynamic> oAMGSalarySheets = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExtraOTDynamicDA.Gets(tc, BUIDs, LocationIDs, DepartmentIDs, DesignationIDs, SalarySchemeIDs, BlockIds, EmployeeIDs, nMonthID, nYear, IsNewJoin, IsOutSheet, nMinSalary, nMaxSalary, nMOCID);
                oAMGSalarySheets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new ServiceException("Failed to get ExtraOT.", e);
                #endregion
            }

            return oAMGSalarySheets;
        }
        #endregion
    }
}


