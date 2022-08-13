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
    public class EmployeeLeaveInfoService : MarshalByRefObject, IEmployeeLeaveInfoService
    {
        #region Private functions and declaration
        private EmployeeLeaveInfo MapObject(NullHandler oReader)
        {
            EmployeeLeaveInfo oEmployeeLeaveInfo = new EmployeeLeaveInfo();
            oEmployeeLeaveInfo.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeLeaveInfo.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            oEmployeeLeaveInfo.LeaveCount = oReader.GetInt32("LeaveCount");
            oEmployeeLeaveInfo.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeLeaveInfo.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeLeaveInfo.DepartmentName = oReader.GetString("DepartmentName");
            oEmployeeLeaveInfo.DesignationName = oReader.GetString("DesignationName");
            return oEmployeeLeaveInfo;
        }

        private EmployeeLeaveInfo CreateObject(NullHandler oReader)
        {
            EmployeeLeaveInfo oEmployeeLeaveInfo = new EmployeeLeaveInfo();
            oEmployeeLeaveInfo = MapObject(oReader);
            return oEmployeeLeaveInfo;
        }

        private List<EmployeeLeaveInfo> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLeaveInfo> oEmployeeLeaveInfo = new List<EmployeeLeaveInfo>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLeaveInfo oItem = CreateObject(oHandler);
                oEmployeeLeaveInfo.Add(oItem);
            }
            return oEmployeeLeaveInfo;
        }

        #endregion

        #region Interface implementation
        public EmployeeLeaveInfoService() { }

        public List<EmployeeLeaveInfo> Gets(DateTime dtFrom,DateTime dtTo,int ACSID,int LeaveHeadId,string EmpIds,string DeptIds,string DesignationIds,  bool bReportingPerson,Int64 nUserId)
        {
            List<EmployeeLeaveInfo> oEmployeeLeaveInfo = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveInfoDA.Gets(tc, dtFrom, dtTo, ACSID, LeaveHeadId, EmpIds, DeptIds, DesignationIds, bReportingPerson ,nUserId);
                oEmployeeLeaveInfo = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeLeaveInfo", e);
                #endregion
            }

            return oEmployeeLeaveInfo;
        }

        #endregion
    }
}
