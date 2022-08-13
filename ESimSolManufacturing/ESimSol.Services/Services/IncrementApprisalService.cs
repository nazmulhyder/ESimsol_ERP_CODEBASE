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
    public class IncrementApprisalService : MarshalByRefObject, IIncrementApprisalService
    {
        #region Private functions and declaration
        private IncrementApprisal MapObject(NullHandler oReader)
        {
            IncrementApprisal oIncrementApprisal = new IncrementApprisal();
            oIncrementApprisal.EmployeeID = oReader.GetInt32("EmployeeID");
            oIncrementApprisal.JoiningDate = oReader.GetDateTime("JoiningDate");
            oIncrementApprisal.LastPromotionDate = oReader.GetDateTime("LastPromotionDate");
            oIncrementApprisal.EmployeeCode = oReader.GetString("EmployeeCode");
            oIncrementApprisal.EmployeeName = oReader.GetString("EmployeeName");
            oIncrementApprisal.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oIncrementApprisal.BusinessUnitName = oReader.GetString("BusinessUnitName");
            oIncrementApprisal.LocationID = oReader.GetInt32("LocationID");
            oIncrementApprisal.LocationName = oReader.GetString("LocationName");
            oIncrementApprisal.DepartmentID = oReader.GetInt32("DepartmentID");
            oIncrementApprisal.DepartmentName = oReader.GetString("DepartmentName");
            oIncrementApprisal.DesignationName = oReader.GetString("DesignationName");
            oIncrementApprisal.BeforeIncrement = oReader.GetDouble("BeforeIncrement");
            oIncrementApprisal.AttendancePercent = oReader.GetDouble("AttendancePercent");
            oIncrementApprisal.PresentSalary = oReader.GetDouble("PresentSalary");
            oIncrementApprisal.BeforeEffectDate = oReader.GetDateTime("BeforeEffectDate");
            oIncrementApprisal.RecentIncrement = oReader.GetDouble("RecentIncrement");
            oIncrementApprisal.RecentEffectDate = oReader.GetDateTime("RecentEffectDate");
            oIncrementApprisal.Education = oReader.GetString("Education");
            oIncrementApprisal.TotalLate = oReader.GetInt32("TotalLate");
            oIncrementApprisal.TotalLeave = oReader.GetInt32("TotalLeave");
            oIncrementApprisal.TotalAbsent = oReader.GetInt32("TotalAbsent");
            oIncrementApprisal.Warning = oReader.GetInt32("Warning");
            return oIncrementApprisal;

        }

        private IncrementApprisal CreateObject(NullHandler oReader)
        {
            IncrementApprisal oIncrementApprisal = MapObject(oReader);
            return oIncrementApprisal;
        }

        private List<IncrementApprisal> CreateObjects(IDataReader oReader)
        {
            List<IncrementApprisal> oIncrementApprisal = new List<IncrementApprisal>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                IncrementApprisal oItem = CreateObject(oHandler);
                oIncrementApprisal.Add(oItem);
            }
            return oIncrementApprisal;
        }


        #endregion

        #region Interface implementation
        public IncrementApprisalService() { }


        public List<IncrementApprisal> Search(DateTime UpToDate, string EmpIDs, string BUIDs, string LocationIDs, string DeptIDs, string DesignationIDs, DateTime JoiningDate, bool IsMultipleMonth, string sMonths, string sYears, bool IsJoinDate, double minsalary, double maxsalary, string BlockIDs, string GroupIDs, Int64 nUserId)
        {
            List<IncrementApprisal> oIncrementApprisals = new List<IncrementApprisal>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = IncrementApprisalDA.Search(tc, UpToDate, EmpIDs, BUIDs, LocationIDs, DeptIDs, DesignationIDs, JoiningDate, IsMultipleMonth, sMonths, sYears, IsJoinDate, minsalary, maxsalary, BlockIDs, GroupIDs, nUserId);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    IncrementApprisal oIncrementApprisal = new IncrementApprisal();
                    oIncrementApprisal.EmployeeID = oreader.GetInt32("EmployeeID");
                    oIncrementApprisal.AttendancePercent = oreader.GetDouble("AttendancePercent");
                    oIncrementApprisal.PresentSalary = oreader.GetDouble("PresentSalary");
                    oIncrementApprisal.JoiningDate = oreader.GetDateTime("JoiningDate");
                    oIncrementApprisal.EmployeeCode = oreader.GetString("EmployeeCode");
                    oIncrementApprisal.EmployeeName = oreader.GetString("EmployeeName");
                    oIncrementApprisal.BusinessUnitID = oreader.GetInt32("BusinessUnitID");
                    oIncrementApprisal.BusinessUnitName = oreader.GetString("BusinessUnitName");
                    oIncrementApprisal.LocationID = oreader.GetInt32("LocationID");
                    oIncrementApprisal.LocationName = oreader.GetString("LocationName");
                    oIncrementApprisal.DepartmentID = oreader.GetInt32("DepartmentID");
                    oIncrementApprisal.DepartmentName = oreader.GetString("DepartmentName");
                    oIncrementApprisal.DesignationName = oreader.GetString("DesignationName");
                    oIncrementApprisal.BeforeIncrement = oreader.GetDouble("BeforeIncrement");
                    oIncrementApprisal.BeforeEffectDate = oreader.GetDateTime("BeforeEffectDate");
                    oIncrementApprisal.LastPromotionDate = oreader.GetDateTime("LastPromotionDate");
                    oIncrementApprisal.RecentIncrement = oreader.GetDouble("RecentIncrement");
                    oIncrementApprisal.RecentEffectDate = oreader.GetDateTime("RecentEffectDate");
                    oIncrementApprisal.Education = oreader.GetString("Education");
                    oIncrementApprisal.TotalLate = oreader.GetInt32("TotalLate");
                    oIncrementApprisal.TotalLeave = oreader.GetInt32("TotalLeave");
                    oIncrementApprisal.TotalAbsent = oreader.GetInt32("TotalAbsent");
                    oIncrementApprisal.Warning = oreader.GetInt32("Warning");

                    oIncrementApprisals.Add(oIncrementApprisal);
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
                #endregion
            }
            return oIncrementApprisals;
        }

        public IncrementApprisal Get(string sSQL, Int64 nUserId)
        {
            IncrementApprisal oIncrementApprisal = new IncrementApprisal();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = IncrementApprisalDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oIncrementApprisal = CreateObject(oReader);
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

            return oIncrementApprisal;
        }

        public List<IncrementApprisal> Gets(string sSQL, Int64 nUserID)
        {
            List<IncrementApprisal> oIncrementApprisal = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = IncrementApprisalDA.Gets(sSQL, tc);
                oIncrementApprisal = CreateObjects(reader);
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
            return oIncrementApprisal;
        }
        #endregion
    }
}


