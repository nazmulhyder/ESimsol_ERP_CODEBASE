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
    public class EmployeeOTonAttendanceService : MarshalByRefObject, IEmployeeOTonAttendanceService
    {
        #region Private functions and declaration
        private EmployeeOTonAttendance MapObject(NullHandler oReader)
        {
            EmployeeOTonAttendance oEmployeeOTonAttendance = new EmployeeOTonAttendance();
            oEmployeeOTonAttendance.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeOTonAttendance.OTMinute = oReader.GetInt32("OTMinute");
            return oEmployeeOTonAttendance;
        }

        public static EmployeeOTonAttendance CreateObject(NullHandler oReader)
        {
            EmployeeOTonAttendance oEmployeeOTonAttendance = new EmployeeOTonAttendance();
            EmployeeOTonAttendanceService oService = new EmployeeOTonAttendanceService();
            oEmployeeOTonAttendance = oService.MapObject(oReader);
            return oEmployeeOTonAttendance;
        }
        private List<EmployeeOTonAttendance> CreateObjects(IDataReader oReader)
        {
            List<EmployeeOTonAttendance> oEmployeeOTonAttendances = new List<EmployeeOTonAttendance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeOTonAttendance oItem = CreateObject(oHandler);
                oEmployeeOTonAttendances.Add(oItem);
            }
            return oEmployeeOTonAttendances;
        }

        #endregion

        #region Interface implementation
        public EmployeeOTonAttendanceService() { }

        public List<EmployeeOTonAttendance> Gets(bool IsCompliance, string employeeIDs, DateTime dtFrom, DateTime dtTo, long nUserID)
        {
            List<EmployeeOTonAttendance> oEmployeeOTonAttendances = new List<EmployeeOTonAttendance>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeOTonAttendanceDA.Gets(tc, IsCompliance, employeeIDs, dtFrom, dtTo);
                oEmployeeOTonAttendances = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                EmployeeOTonAttendance oEmployeeOTonAttendance = new EmployeeOTonAttendance();
                oEmployeeOTonAttendance.ErrorMessage = e.Message;
                oEmployeeOTonAttendances.Add(oEmployeeOTonAttendance);
                #endregion
            }

            return oEmployeeOTonAttendances;
        }


        #endregion
    }
}