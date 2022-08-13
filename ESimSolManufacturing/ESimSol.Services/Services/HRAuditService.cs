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
    public class HRAuditService : MarshalByRefObject, IHRAuditService
    {
        #region Private functions and declaration
        private HRAudit MapObject(NullHandler oReader)
        {
            HRAudit oHRAudit = new HRAudit();
            oHRAudit.EmployeeID = oReader.GetInt32("EmployeeID");
            oHRAudit.EWD = oReader.GetInt32("EWD");
            oHRAudit.ESID = oReader.GetInt32("ESID");
            oHRAudit.TotalDays = oReader.GetInt32("TotalDays");
            oHRAudit.NoRecord = oReader.GetInt32("NoRecord");
            oHRAudit.TotalOSD = oReader.GetInt32("TotalOSD");
            oHRAudit.TotalPresent = oReader.GetInt32("TotalPresent");
            oHRAudit.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oHRAudit.TotalDayoff = oReader.GetInt32("TotalDayoff");
            oHRAudit.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oHRAudit.TotalLeave = oReader.GetInt32("TotalLeave");
            oHRAudit.EarlyDays = oReader.GetInt32("EarlyDays");
            oHRAudit.LateDays = oReader.GetInt32("LateDays");
            oHRAudit.LateMin = oReader.GetInt32("LateMin");
            oHRAudit.JoiningDate = oReader.GetDateTime("JoiningDate");
            oHRAudit.ConfirmationDate = oReader.GetDateTime("ConfirmationDate");
            oHRAudit.LastIncrement = oReader.GetDateTime("LastIncrement");


            oHRAudit.FirstOT = oReader.GetDouble("FirstOT");
            oHRAudit.SecondOT = oReader.GetDouble("SecondOT");
            oHRAudit.OTHours = oReader.GetDouble("OTHours");
            oHRAudit.AvgOT = oReader.GetDouble("AvgOT");
            oHRAudit.OTRate = oReader.GetDouble("OTRate");
            oHRAudit.OTAmount = oReader.GetDouble("OTAmount");
            oHRAudit.AvgLate = oReader.GetDouble("AvgLate");
            oHRAudit.AvgEarly = oReader.GetDouble("AvgEarly");
            oHRAudit.Gross = oReader.GetDouble("Gross");
            oHRAudit.NetAmount = oReader.GetDouble("NetAmount");
            oHRAudit.LastGross = oReader.GetDouble("LastGross");
            oHRAudit.IncrementAmount = oReader.GetDouble("IncrementAmount");
            oHRAudit.TotalPunch = oReader.GetDouble("TotalPunch");
            oHRAudit.PunchNotFound = oReader.GetDouble("PunchNotFound");
            oHRAudit.OnePunchFound = oReader.GetDouble("OnePunchFound");
            oHRAudit.ExpectedWorkingHour = oReader.GetDouble("ExpectedWorkingHour");
            oHRAudit.ExpectedWorkingHourWithOT = oReader.GetDouble("ExpectedWorkingHourWithOT");
            oHRAudit.TotalWorkingHour = oReader.GetDouble("TotalWorkingHour");
            oHRAudit.TotalWorkingHourWithOT = oReader.GetDouble("TotalWorkingHourWithOT");
            oHRAudit.ExpectedAvgWorkingHour = oReader.GetDouble("ExpectedAvgWorkingHour");
            oHRAudit.WDPM = oReader.GetDouble("WDPM");
            oHRAudit.PPM = oReader.GetDouble("PPM");
            oHRAudit.APM = oReader.GetDouble("APM");
            oHRAudit.LWPPM = oReader.GetDouble("LWPPM");
            oHRAudit.LatePM = oReader.GetDouble("LatePM");
            oHRAudit.EarlyPM = oReader.GetDouble("EarlyPM");
            oHRAudit.ManualPM = oReader.GetDouble("ManualPM");
            oHRAudit.PNFPM = oReader.GetDouble("PNFPM");
            oHRAudit.ODPFPM = oReader.GetDouble("ODPFPM");
            oHRAudit.AvgBOAPM = oReader.GetDouble("AvgBOAPM");



            oHRAudit.EmployeeTypeName = oReader.GetString("EmployeeTypeName");
            oHRAudit.DesignationName = oReader.GetString("DesignationName");
            oHRAudit.DepartmentName = oReader.GetString("DepartmentName");
            oHRAudit.LocationName = oReader.GetString("LocationName");
            oHRAudit.BUName = oReader.GetString("BUName");
            oHRAudit.EmployeeName = oReader.GetString("EmployeeName");
            oHRAudit.Code = oReader.GetString("Code");
            oHRAudit.Gender = oReader.GetString("Gender");
            return oHRAudit;

        }

        private HRAudit CreateObject(NullHandler oReader)
        {
            HRAudit oHRAudit = MapObject(oReader);
            return oHRAudit;
        }

        private List<HRAudit> CreateObjects(IDataReader oReader)
        {
            List<HRAudit> oHRAudit = new List<HRAudit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                HRAudit oItem = CreateObject(oHandler);
                oHRAudit.Add(oItem);
            }
            return oHRAudit;
        }


        #endregion

        #region Interface implementation
        public HRAuditService() { }

        public DataSet GetAuditReport(string sBU, string sLocationID, string sDepartmentIDs, string sDesignationIDs
          , string sSalarySchemeIDs, string sBlockIDs, string sGroupIDs, string sEmpIDs, int nMonthID, int nYear, bool bNewJoin, int IsOutSheet
          , double nStartSalaryRange, double nEndSalaryRange, bool IsCompliance, int nPayType)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = HRAuditDA.GetAuditReport(tc, sBU, sLocationID, sDepartmentIDs, sDesignationIDs, sSalarySchemeIDs, sBlockIDs, sGroupIDs, sEmpIDs, nMonthID, nYear, bNewJoin, 0, nStartSalaryRange, nEndSalaryRange, IsCompliance, nPayType);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[4]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DataSet", e);
                #endregion
            }
            return oDataSet;
        }
        public HRAudit Get(string sSQL, Int64 nUserId)
        {
            HRAudit oHRAudit = new HRAudit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = HRAuditDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oHRAudit = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                //oAttendanceDaily.ErrorMessage = e.Message;
                #endregion
            }

            return oHRAudit;
        }

        public List<HRAudit> Gets(string sSQL, Int64 nUserID)
        {
            List<HRAudit> oHRAudit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = HRAuditDA.Gets(sSQL, tc);
                oHRAudit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oHRAudit;
        }
        #endregion
    }
}

