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
    public class LeaveReport_XLService : MarshalByRefObject, ILeaveReport_XLService
    {
        #region Private functions and declaration
        private LeaveReport_XL MapObject(NullHandler oReader)
        {
            LeaveReport_XL oLeaveReport_XL = new LeaveReport_XL();

            oLeaveReport_XL.EmployeeCode = oReader.GetString("Code");
            oLeaveReport_XL.EmployeeName = oReader.GetString("Name");
            oLeaveReport_XL.DepartmentName = oReader.GetString("DepartmentName");
            oLeaveReport_XL.DesignationName = oReader.GetString("DesignationName");
            oLeaveReport_XL.DateOfJoin = oReader.GetDateTime("DateOfJoin");

            oLeaveReport_XL.CL = oReader.GetDouble("CL");
            oLeaveReport_XL.SL = oReader.GetDouble("SL");
            oLeaveReport_XL.EL = oReader.GetDouble("EL");
            oLeaveReport_XL.LWP = oReader.GetDouble("LWP");
            oLeaveReport_XL.shortLeave = oReader.GetDouble("shortLeave");
            oLeaveReport_XL.IsActive = oReader.GetBoolean("IsActive");

            return oLeaveReport_XL;

        }

        private LeaveReport_XL CreateObject(NullHandler oReader)
        {
            LeaveReport_XL oLeaveReport_XL = MapObject(oReader);
            return oLeaveReport_XL;
        }

        private List<LeaveReport_XL> CreateObjects(IDataReader oReader)
        {
            List<LeaveReport_XL> oLeaveReport_XLs = new List<LeaveReport_XL>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LeaveReport_XL oItem = CreateObject(oHandler);
                oLeaveReport_XLs.Add(oItem);
            }
            return oLeaveReport_XLs;
        }

        #endregion

        #region Interface implementation
        public LeaveReport_XLService() { }
        public List<LeaveReport_XL> Gets(DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<LeaveReport_XL> oLeaveReport_XL = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LeaveReport_XLDA.Gets(StartDate, EndDate, tc);
                oLeaveReport_XL = CreateObjects(reader);
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
            return oLeaveReport_XL;
        }

        #endregion


    }
}
