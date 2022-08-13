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
    public class EmployeeSettlementSalaryService : MarshalByRefObject, IEmployeeSettlementSalaryService
    {
        #region Private functions and declaration
        private EmployeeSettlementSalary MapObject(NullHandler oReader)
        {
            EmployeeSettlementSalary oEmployeeSettlementSalary = new EmployeeSettlementSalary();
            oEmployeeSettlementSalary.CompOTRatePerHour = oReader.GetDouble("CompOTRatePerHour");
            oEmployeeSettlementSalary.CompOTHour = oReader.GetDouble("CompOTHour");
            oEmployeeSettlementSalary.CompNetAmount = oReader.GetDouble("CompNetAmount");
            oEmployeeSettlementSalary.CompGrossAmount = oReader.GetDouble("CompGrossAmount");

            oEmployeeSettlementSalary.CompTotalWorkingDay = oReader.GetInt32("CompTotalWorkingDay");
            oEmployeeSettlementSalary.CompTotalAbsent = oReader.GetInt32("CompTotalAbsent");
            oEmployeeSettlementSalary.CompTotalLate = oReader.GetInt32("CompTotalLate");
            oEmployeeSettlementSalary.CompTotalEarlyLeaving = oReader.GetInt32("CompTotalEarlyLeaving");
            oEmployeeSettlementSalary.CompTotalDayOff = oReader.GetInt32("CompTotalDayOff");
            oEmployeeSettlementSalary.CompTotalLeave = oReader.GetInt32("CompTotalLeave");
            oEmployeeSettlementSalary.CompTotalHoliday = oReader.GetInt32("CompTotalHoliday");

            oEmployeeSettlementSalary.EmployeeSalaryID = oReader.GetInt32("EmployeeSalaryID");
            oEmployeeSettlementSalary.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeSettlementSalary.LocationID = oReader.GetInt32("LocationID");
            oEmployeeSettlementSalary.DepartmentID = oReader.GetInt32("DepartmentID");
            oEmployeeSettlementSalary.TotalHoliday = oReader.GetInt32("TotalHoliday");
            oEmployeeSettlementSalary.DesignationID = oReader.GetInt32("DesignationID");
            oEmployeeSettlementSalary.GrossAmount = oReader.GetDouble("GrossAmount");
            oEmployeeSettlementSalary.OTHour = oReader.GetDouble("OTHour");
            oEmployeeSettlementSalary.OTRatePerHour = oReader.GetDouble("OTRatePerHour");
            oEmployeeSettlementSalary.NetAmount = oReader.GetDouble("NetAmount");
            oEmployeeSettlementSalary.TotalWorkingDay = oReader.GetInt32("TotalWorkingDay");
            oEmployeeSettlementSalary.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oEmployeeSettlementSalary.TotalDayOff = oReader.GetInt32("TotalDayOff");
            oEmployeeSettlementSalary.TotalEarlyLeaving = oReader.GetInt32("TotalEarlyLeaving");
            oEmployeeSettlementSalary.TotalLate = oReader.GetInt32("TotalLate");
            oEmployeeSettlementSalary.TotalPLeave = oReader.GetInt32("TotalPLeave");
            oEmployeeSettlementSalary.TotalUpLeave = oReader.GetInt32("TotalUpLeave");
            oEmployeeSettlementSalary.RevenueStemp = oReader.GetInt32("RevenueStemp");
            oEmployeeSettlementSalary.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeSettlementSalary.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeSettlementSalary.JoiningDate = oReader.GetDateTime("JoiningDate");
            oEmployeeSettlementSalary.LocationName = oReader.GetString("LocationName");
            oEmployeeSettlementSalary.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeSettlementSalary.DesignationName = oReader.GetString("DesignationName");
            oEmployeeSettlementSalary.StartDate = oReader.GetDateTime("StartDate");
            oEmployeeSettlementSalary.EndDate = oReader.GetDateTime("EndDate");

            return oEmployeeSettlementSalary;

        }

        private EmployeeSettlementSalary CreateObject(NullHandler oReader)
        {
            EmployeeSettlementSalary oEmployeeSettlementSalary = MapObject(oReader);
            return oEmployeeSettlementSalary;
        }

        private List<EmployeeSettlementSalary> CreateObjects(IDataReader oReader)
        {
            List<EmployeeSettlementSalary> oEmployeeSettlementSalary = new List<EmployeeSettlementSalary>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeSettlementSalary oItem = CreateObject(oHandler);
                oEmployeeSettlementSalary.Add(oItem);
            }
            return oEmployeeSettlementSalary;
        }



        #endregion

        #region Interface implementation
        public EmployeeSettlementSalaryService() { }

        public EmployeeSettlementSalary Get(string sSQL, Int64 nUserId)
        {
            EmployeeSettlementSalary oEmployeeSettlementSalary = new EmployeeSettlementSalary();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeSettlementSalaryDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeSettlementSalary = CreateObject(oReader);
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

            return oEmployeeSettlementSalary;
        }

        public List<EmployeeSettlementSalary> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeSettlementSalary> oEmployeeSettlementSalary = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeSettlementSalaryDA.Gets(sSQL, tc);
                oEmployeeSettlementSalary = CreateObjects(reader);
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
            return oEmployeeSettlementSalary;
        }
        #endregion
    }
}

