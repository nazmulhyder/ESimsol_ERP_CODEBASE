using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class PunchLogService : MarshalByRefObject, IPunchLogService
    {
        #region Private functions and declaration
        private PunchLog MapObject(NullHandler oReader)
        {
            PunchLog oPunchLog = new PunchLog();
            oPunchLog.PunchLogID = oReader.GetInt32("PunchLogID");
            oPunchLog.MachineSLNo = oReader.GetString("MachineSLNo");
            oPunchLog.EmployeeID = oReader.GetInt32("EmployeeID");
            oPunchLog.LocationID = oReader.GetInt32("LocationID");
            oPunchLog.DepartmentID = oReader.GetInt32("DepartmentID");
            oPunchLog.DesignationID = oReader.GetInt32("DesignationID");
            oPunchLog.CardNo = oReader.GetString("CardNo");
            oPunchLog.PunchDateTime = oReader.GetDateTime("PunchDateTime");
            //derive
            oPunchLog.EmployeeName = oReader.GetString("EmployeeName");
            oPunchLog.EmployeeCode = oReader.GetString("EmployeeCode");
            oPunchLog.DepartmentName = oReader.GetString("DepartmentName");
            oPunchLog.DesignationName = oReader.GetString("DesignationName");
            oPunchLog.LocationName = oReader.GetString("LocationName");
            oPunchLog.BUName = oReader.GetString("BUName");
            

            return oPunchLog;

        }

        private PunchLog CreateObject(NullHandler oReader)
        {
            PunchLog oPunchLog = MapObject(oReader);
            return oPunchLog;
        }

        private List<PunchLog> CreateObjects(IDataReader oReader)
        {
            List<PunchLog> oPunchLog = new List<PunchLog>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PunchLog oItem = CreateObject(oHandler);
                oPunchLog.Add(oItem);
            }
            return oPunchLog;
        }

        #endregion

        #region Interface implementation
        public PunchLogService() { }

        public PunchLog IUD(string sEmployeeIds, DateTime dtPunchTime, int nDBOperation, Int64 nUserID)
        {
            PunchLog oPunchLog = new PunchLog();
            TransactionContext tc = null;
            try
            {
                string[] sEmpIDs;
                sEmpIDs = sEmployeeIds.Split(',');
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                foreach (string sEmpID in sEmpIDs)
                {
                    int nEmployeeID = Convert.ToInt32(sEmpID);
                    reader = PunchLogDA.IUD(tc, nEmployeeID, dtPunchTime, nUserID, nDBOperation);
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oPunchLog.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oPunchLog;
        }


        public PunchLog Get(int nEPSID, Int64 nUserId)
        {
            PunchLog oPunchLog = new PunchLog();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PunchLogDA.Get(nEPSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPunchLog = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PunchLog", e);
                oPunchLog.ErrorMessage = e.Message;
                #endregion
            }

            return oPunchLog;
        }

        public PunchLog Get(string sSql, Int64 nUserId)
        {
            PunchLog oPunchLog = new PunchLog();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PunchLogDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPunchLog = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get PunchLog", e);
                oPunchLog.ErrorMessage = e.Message;
                #endregion
            }

            return oPunchLog;
        }

        public List<PunchLog> Gets(Int64 nUserID)
        {
            List<PunchLog> oPunchLog = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PunchLogDA.Gets(tc);
                oPunchLog = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PunchLog", e);
                #endregion
            }
            return oPunchLog;
        }

        public List<PunchLog> Gets(string sSQL, Int64 nUserID)
        {
            List<PunchLog> oPunchLog = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PunchLogDA.Gets(sSQL, tc);
                oPunchLog = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_PunchLog", e);
                #endregion
            }
            return oPunchLog;
        }

        //public string Delete(int PunchID, DateTime PunchDate, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    string S = "";
        //    try
        //    {
        //        string sPunchLogSql = "SELECT * FROM AttendanceProcessManagement WHERE AttendanceDate = '" + PunchDate.ToString("dd MMM yyyy") + "' AND Status=4";
        //        tc = TransactionContext.Begin(true);
        //        bool IsAttFreeze = PunchLogDA.IsAttFreeze(sPunchLogSql, tc);
        //        tc.End();
        //        if (IsAttFreeze == true)
        //        {
        //            throw new Exception("Att. already processed . So deletion is not possible !");
        //        }
        //        tc = TransactionContext.Begin(true);
        //        IDataReader reader;
        //        reader=PunchLogDA.Delete(PunchID, tc);
        //        reader.Close();
        //        tc.End();
        //        S = Global.DeleteMessage;
        //    }
        //    catch (Exception e)
        //    {
        //        S = e.Message;
        //    }
        //    return S;
        //}


        public bool Delete(string PunchIDs, string PunchDate, Int64 nUserId)
        {
            string[] sIDs;
            sIDs = PunchIDs.Split(',');
            string[] sDates;
            sDates = PunchDate.Split(',');
            TransactionContext tc = null;
            bool S = true;
            bool IsAttFreeze = true;
            try
            {
                foreach (string item in sDates)
                {
                    DateTime dt = new DateTime();
                    dt = Convert.ToDateTime(item);
                    string sPunchLogSql = "SELECT * FROM AttendanceProcessManagement WHERE AttendanceDate = '" + dt.ToString("dd MMM yyyy") + "' AND Status=4";
                    tc = TransactionContext.Begin(true);
                    IsAttFreeze = PunchLogDA.IsAttFreeze(sPunchLogSql, tc);
                    if (IsAttFreeze == true)
                    {
                        break;
                    }
                    tc.End();
                }
                if (IsAttFreeze == true)
                {
                    throw new Exception("Att. already processed for some dates . So deletion is not possible !");
                }
                foreach (string item in sIDs)
                {
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    reader = PunchLogDA.Delete(Convert.ToInt32(item), tc);
                    reader.Close();
                    tc.End();
                }
            }
            catch (Exception e)
            {
                S = false;
            }
            return S;
        }

        #region UploadXL
        public List<PunchLog> UploadXL(List<PunchLog> oPunchLogXLs, Int64 nUserID)
        {
            PunchLog oTempPunchLog = new PunchLog();
            List<PunchLog> oTempPunchLogs = new List<PunchLog>();
            TransactionContext tc = null;
            try
            {
                int nCount = 0;
                foreach (PunchLog oItem in oPunchLogXLs)
                {
                    nCount = nCount + 1;
                    tc = TransactionContext.Begin(true);
                    IDataReader reader;
                    oTempPunchLog = new PunchLog();
                    reader = PunchLogDA.UploadXL(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempPunchLog = CreateObject(oReader);
                        if (oTempPunchLog.PunchLogID > 0)
                            oTempPunchLogs.Add(oTempPunchLog);
                    }
                    else{
                        oTempPunchLogs.Add(oItem);
                    }


                    //if (nCount < 100)
                    //{
                    //    NullHandler oReader = new NullHandler(reader);
                    //    if (reader.Read())
                    //    {
                    //        oTempPunchLog = CreateObject(oReader);
                    //    }
                    //    if (oTempPunchLog.PunchLogID > 0)
                    //    {
                    //        oTempPunchLogs.Add(oTempPunchLog);
                    //        nCount++;
                    //    }
                    //}    
                
                    reader.Close();
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new ServiceException(e.Message.Split('!')[0]);
                #endregion
            }
            return oTempPunchLogs;
        }
        #endregion UploadXL

        #endregion

    }
}
