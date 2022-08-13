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
    public class EmployeeRequestOnAttendanceService : MarshalByRefObject, IEmployeeRequestOnAttendanceService
    {
        #region Private functions and declaration
        private EmployeeRequestOnAttendance MapObject(NullHandler oReader)
        {
            EmployeeRequestOnAttendance oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
            oEmployeeRequestOnAttendance.EROAID = oReader.GetInt32("EROAID");
            oEmployeeRequestOnAttendance.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeRequestOnAttendance.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oEmployeeRequestOnAttendance.IsOSD = oReader.GetInt32("IsOSD");
            oEmployeeRequestOnAttendance.Remark = oReader.GetString("Remark");
            oEmployeeRequestOnAttendance.EmployeeCode = oReader.GetString("EmployeeCode");
            oEmployeeRequestOnAttendance.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeRequestOnAttendance.CancelBy = oReader.GetInt32("CancelBy");
            oEmployeeRequestOnAttendance.ApproveDate = oReader.GetDateTime("ApproveDate");
            oEmployeeRequestOnAttendance.CancelDate = oReader.GetDateTime("CancelDate");
            oEmployeeRequestOnAttendance.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeRequestOnAttendance.EmployeeName = oReader.GetString("EmployeeName");
            return oEmployeeRequestOnAttendance;

        }

        private EmployeeRequestOnAttendance CreateObject(NullHandler oReader)
        {
            EmployeeRequestOnAttendance oEmployeeRequestOnAttendance = MapObject(oReader);
            return oEmployeeRequestOnAttendance;
        }

        private List<EmployeeRequestOnAttendance> CreateObjects(IDataReader oReader)
        {
            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendance = new List<EmployeeRequestOnAttendance>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeRequestOnAttendance oItem = CreateObject(oHandler);
                oEmployeeRequestOnAttendance.Add(oItem);
            }
            return oEmployeeRequestOnAttendance;
        }


        #endregion

        #region Interface implementation
        public EmployeeRequestOnAttendanceService() { }
        public List<EmployeeRequestOnAttendance> GetHierarchy(string sEmployeeIDs, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendances = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeRequestOnAttendanceDA.GetHierarchy(tc, sEmployeeIDs);
                oEmployeeRequestOnAttendances = CreateObjects(reader);
                reader.Close();

                reader.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }
            return oEmployeeRequestOnAttendances;
        }
        public EmployeeRequestOnAttendance IUD(EmployeeRequestOnAttendance oEmployeeRequestOnAttendance, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = EmployeeRequestOnAttendanceDA.IUD(tc, oEmployeeRequestOnAttendance, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                        oEmployeeRequestOnAttendance = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = EmployeeRequestOnAttendanceDA.IUD(tc, oEmployeeRequestOnAttendance, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oEmployeeRequestOnAttendance.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
                oEmployeeRequestOnAttendance.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oEmployeeRequestOnAttendance;
        }


        public EmployeeRequestOnAttendance Get(string sSQL, Int64 nUserId)
        {
            EmployeeRequestOnAttendance oEmployeeRequestOnAttendance = new EmployeeRequestOnAttendance();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeRequestOnAttendanceDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeRequestOnAttendance = CreateObject(oReader);
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

            return oEmployeeRequestOnAttendance;
        }

        public List<EmployeeRequestOnAttendance> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeRequestOnAttendance> oEmployeeRequestOnAttendance = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeRequestOnAttendanceDA.Gets(sSQL, tc);
                oEmployeeRequestOnAttendance = CreateObjects(reader);
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
            return oEmployeeRequestOnAttendance;
        }
        #endregion
    }
}


