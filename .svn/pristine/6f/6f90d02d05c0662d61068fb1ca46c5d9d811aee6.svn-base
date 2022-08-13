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
    public class AttendanceAccessPointEmployeeService : MarshalByRefObject, IAttendanceAccessPointEmployeeService
    {
        #region Private functions and declaration
        private AttendanceAccessPointEmployee MapObject(NullHandler oReader)
        {
            AttendanceAccessPointEmployee oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();
            oAttendanceAccessPointEmployee.AAPEmployeeID = oReader.GetInt32("AAPEmployeeID");
            oAttendanceAccessPointEmployee.AAPID = oReader.GetInt32("AAPID");
            oAttendanceAccessPointEmployee.EmployeeID = oReader.GetInt32("EmployeeID");
            oAttendanceAccessPointEmployee.IsActive = oReader.GetBoolean("IsActive");
            oAttendanceAccessPointEmployee.InactiveDate = oReader.GetDateTime("InactiveDate");
            oAttendanceAccessPointEmployee.InactiveBy = oReader.GetInt32("InactiveBy");
            oAttendanceAccessPointEmployee.EmployeeName = oReader.GetString("EmployeeName");
            oAttendanceAccessPointEmployee.EmployeeCode = oReader.GetString("EmployeeCode");
            oAttendanceAccessPointEmployee.InactiveByName = oReader.GetString("InactiveByName");
            return oAttendanceAccessPointEmployee;
        }

        private AttendanceAccessPointEmployee CreateObject(NullHandler oReader)
        {
            AttendanceAccessPointEmployee oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();
            oAttendanceAccessPointEmployee = MapObject(oReader);
            return oAttendanceAccessPointEmployee;
        }

        private List<AttendanceAccessPointEmployee> CreateObjects(IDataReader oReader)
        {
            List<AttendanceAccessPointEmployee> oAttendanceAccessPointEmployees = new List<AttendanceAccessPointEmployee>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceAccessPointEmployee oItem = CreateObject(oHandler);
                oAttendanceAccessPointEmployees.Add(oItem);
            }
            return oAttendanceAccessPointEmployees;
        }

        #endregion

        #region Interface implementation
        public AttendanceAccessPointEmployeeService() { }

        public AttendanceAccessPointEmployee IUD(AttendanceAccessPointEmployee oAttendanceAccessPointEmployee, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            AttendanceAccessPoint oAttendanceAccessPoint = new AttendanceAccessPoint();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oAttendanceAccessPointEmployee.AAPID == 0)
                {
                    if (oAttendanceAccessPointEmployee.AAP != null)
                    {
                        reader = AttendanceAccessPointDA.IUD(tc, oAttendanceAccessPointEmployee.AAP, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oAttendanceAccessPoint = AttendanceAccessPointService.CreateObject(oReader);
                        }
                        reader.Close();
                        oAttendanceAccessPointEmployee.AAPID = oAttendanceAccessPoint.AAPID;
                    }
                    else { throw new Exception("No attendance access point information found to save."); }
                }


                reader = AttendanceAccessPointEmployeeDA.IUD(tc, oAttendanceAccessPointEmployee, nDBOperation, nUserId);

                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();
                    oAttendanceAccessPointEmployee = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();
                    oAttendanceAccessPointEmployee.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPointEmployee.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            oAttendanceAccessPointEmployee.AAP = oAttendanceAccessPoint;
            return oAttendanceAccessPointEmployee;
        }

        public AttendanceAccessPointEmployee Get(int nSRID, Int64 nUserId)
        {
            AttendanceAccessPointEmployee oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceAccessPointEmployeeDA.Get(tc, nSRID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceAccessPointEmployee = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPointEmployee.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }

            return oAttendanceAccessPointEmployee;
        }

        public List<AttendanceAccessPointEmployee> Gets(string sSQL, Int64 nUserId)
        {
            List<AttendanceAccessPointEmployee> oAttendanceAccessPointEmployees = new List<AttendanceAccessPointEmployee>();
            AttendanceAccessPointEmployee oAttendanceAccessPointEmployee = new AttendanceAccessPointEmployee();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceAccessPointEmployeeDA.Gets(tc, sSQL);
                oAttendanceAccessPointEmployees = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oAttendanceAccessPointEmployee.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                oAttendanceAccessPointEmployees.Add(oAttendanceAccessPointEmployee);
                #endregion
            }

            return oAttendanceAccessPointEmployees;
        }
        #endregion
    }
}
