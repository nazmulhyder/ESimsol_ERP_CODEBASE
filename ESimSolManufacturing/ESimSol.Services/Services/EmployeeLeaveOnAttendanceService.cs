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
    public class EmployeeLeaveOnAttendanceService : MarshalByRefObject, IEmployeeLeaveOnAttendanceService
    {
        #region Private functions and declaration
        private EmployeeLeaveOnAttendance MapObject(NullHandler oReader)
        {
            EmployeeLeaveOnAttendance oEmployeeLeaveOnAttendance = new EmployeeLeaveOnAttendance();
            oEmployeeLeaveOnAttendance.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeLeaveOnAttendance.LeaveDays = oReader.GetInt32("LeaveDays");
            oEmployeeLeaveOnAttendance.LeaveHeadID = oReader.GetInt32("LeaveHeadID");
            //oEmployeeLeaveOnAttendance.CompLeaveHeadID = oReader.GetInt32("CompLeaveHeadID");
            oEmployeeLeaveOnAttendance.LeaveHeadName = oReader.GetString("LeaveHeadName");
            return oEmployeeLeaveOnAttendance;
        }

        public static EmployeeLeaveOnAttendance CreateObject(NullHandler oReader)
        {
            EmployeeLeaveOnAttendance oEmployeeLeaveOnAttendance = new EmployeeLeaveOnAttendance();
            EmployeeLeaveOnAttendanceService oService = new EmployeeLeaveOnAttendanceService();
            oEmployeeLeaveOnAttendance = oService.MapObject(oReader);
            return oEmployeeLeaveOnAttendance;
        }
        private List<EmployeeLeaveOnAttendance> CreateObjects(IDataReader oReader)
        {
            List<EmployeeLeaveOnAttendance> oEmployeeLeaveOnAttendances = new List<EmployeeLeaveOnAttendance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeLeaveOnAttendance oItem = CreateObject(oHandler);
                oEmployeeLeaveOnAttendances.Add(oItem);
            }
            return oEmployeeLeaveOnAttendances;
        }

        #endregion

        #region Interface implementation
        public EmployeeLeaveOnAttendanceService() { }

        public List<EmployeeLeaveOnAttendance> Gets(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            List<EmployeeLeaveOnAttendance> oEmployeeLeaveOnAttendances = new List<EmployeeLeaveOnAttendance>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveOnAttendanceDA.Gets(tc, employeeIDs, dtFrom, dtTo);
                oEmployeeLeaveOnAttendances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLeaveOnAttendance oEmployeeLeaveOnAttendance = new EmployeeLeaveOnAttendance();
                oEmployeeLeaveOnAttendance.ErrorMessage = e.Message;
                oEmployeeLeaveOnAttendances.Add(oEmployeeLeaveOnAttendance);
                #endregion
            }

            return oEmployeeLeaveOnAttendances;
        }

        public List<EmployeeLeaveOnAttendance> GetsComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            List<EmployeeLeaveOnAttendance> oEmployeeLeaveOnAttendances = new List<EmployeeLeaveOnAttendance>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveOnAttendanceDA.GetsComp(tc, employeeIDs, dtFrom, dtTo);
                oEmployeeLeaveOnAttendances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLeaveOnAttendance oEmployeeLeaveOnAttendance = new EmployeeLeaveOnAttendance();
                oEmployeeLeaveOnAttendance.ErrorMessage = e.Message;
                oEmployeeLeaveOnAttendances.Add(oEmployeeLeaveOnAttendance);
                #endregion
            }

            return oEmployeeLeaveOnAttendances;
        }
        public List<EmployeeLeaveOnAttendance> GetsActulaForComp(string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            List<EmployeeLeaveOnAttendance> oEmployeeLeaveOnAttendances = new List<EmployeeLeaveOnAttendance>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeLeaveOnAttendanceDA.GetsActulaForComp(tc, employeeIDs, dtFrom, dtTo);
                oEmployeeLeaveOnAttendances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeLeaveOnAttendance oEmployeeLeaveOnAttendance = new EmployeeLeaveOnAttendance();
                oEmployeeLeaveOnAttendance.ErrorMessage = e.Message;
                oEmployeeLeaveOnAttendances.Add(oEmployeeLeaveOnAttendance);
                #endregion
            }

            return oEmployeeLeaveOnAttendances;
        }
        


        #endregion
    }
}