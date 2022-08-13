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
    public class WorkingHourReportService : MarshalByRefObject, IWorkingHourReportService
    {
        #region Private functions and declaration
        private WorkingHourReport MapObject(NullHandler oReader)
        {
            WorkingHourReport oWorkingHourReport = new WorkingHourReport();
            oWorkingHourReport.Hour8 = oReader.GetInt32("Hour8");
            oWorkingHourReport.Hour9 = oReader.GetInt32("Hour9");
            oWorkingHourReport.Hour10 = oReader.GetInt32("Hour10");
            oWorkingHourReport.Hour11 = oReader.GetInt32("Hour11");
            oWorkingHourReport.Hour12 = oReader.GetInt32("Hour12");
            oWorkingHourReport.Hour12P5 = oReader.GetInt32("Hour12P5");
            oWorkingHourReport.Hour13 = oReader.GetInt32("Hour13");
            oWorkingHourReport.LocationName = oReader.GetString("LocationName");
            oWorkingHourReport.BlockName = oReader.GetString("BlockName");
            return oWorkingHourReport;

        }

        private WorkingHourReport CreateObject(NullHandler oReader)
        {
            WorkingHourReport oWorkingHourReport = MapObject(oReader);
            return oWorkingHourReport;
        }

        private List<WorkingHourReport> CreateObjects(IDataReader oReader)
        {
            List<WorkingHourReport> oWorkingHourReport = new List<WorkingHourReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                WorkingHourReport oItem = CreateObject(oHandler);
                oWorkingHourReport.Add(oItem);
            }
            return oWorkingHourReport;
        }


        #endregion

        #region Interface implementation
        public WorkingHourReportService() { }
        public List<WorkingHourReport> Gets(DateTime dDate, string sBusinessUnitIds, string sLocationID, string sDepartmentIds, string sDesignationIds, string sEmployeeIDs, string sShiftIDs, Int64 nUserID)
        {
            List<WorkingHourReport> oWorkingHourReport = new List<WorkingHourReport>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = WorkingHourReportDA.Gets(dDate, sBusinessUnitIds, sLocationID, sDepartmentIds, sDesignationIds, sEmployeeIDs, sShiftIDs, nUserID, tc);
                oWorkingHourReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed To Get Data", e);
                #endregion
            }
            return oWorkingHourReport;
        }
        public WorkingHourReport Get(string sSQL, Int64 nUserId)
        {
            WorkingHourReport oWorkingHourReport = new WorkingHourReport();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = WorkingHourReportDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oWorkingHourReport = CreateObject(oReader);
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

            return oWorkingHourReport;
        }

        public List<WorkingHourReport> Gets(string sSQL, Int64 nUserID)
        {
            List<WorkingHourReport> oWorkingHourReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = WorkingHourReportDA.Gets(sSQL, tc);
                oWorkingHourReport = CreateObjects(reader);
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
            return oWorkingHourReport;
        }
        #endregion
    }
}

