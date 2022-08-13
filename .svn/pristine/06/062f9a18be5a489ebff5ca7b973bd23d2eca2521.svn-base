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
    public class AttendanceProcessManagementService : MarshalByRefObject, IAttendanceProcessManagementService
    {
        #region Private functions and declaration
        private AttendanceProcessManagement MapObject(NullHandler oReader)
        {
            AttendanceProcessManagement oAttendanceProcessManagement = new AttendanceProcessManagement();
            oAttendanceProcessManagement.APMID = oReader.GetInt32("APMID");
            oAttendanceProcessManagement.CompanyID = oReader.GetInt32("CompanyID");
            oAttendanceProcessManagement.LocationID = oReader.GetInt32("LocationID");
            oAttendanceProcessManagement.DepartmentID = oReader.GetInt32("DepartmentID");
            oAttendanceProcessManagement.ProcessType = (EnumProcessType)oReader.GetInt16("ProcessType");
            oAttendanceProcessManagement.ShiftID = oReader.GetInt32("ShiftID");
            oAttendanceProcessManagement.Status = (EnumProcessStatus)oReader.GetInt16("Status");
            oAttendanceProcessManagement.StatusInt = oReader.GetInt16("Status");
            oAttendanceProcessManagement.AttendanceDate = oReader.GetDateTime("AttendanceDate");
            oAttendanceProcessManagement.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            //derive
            oAttendanceProcessManagement.LocationName = oReader.GetString("LocationName");
            oAttendanceProcessManagement.DepartmenName = oReader.GetString("DepartmenName");
            oAttendanceProcessManagement.ShiftName = oReader.GetString("ShiftName");
            oAttendanceProcessManagement.BUName = oReader.GetString("BUName");
            oAttendanceProcessManagement.BUShortName = oReader.GetString("BUShortName");
            oAttendanceProcessManagement.EmpCount = oReader.GetInt32("EmpCount");
            return oAttendanceProcessManagement;

        }

        private AttendanceProcessManagement CreateObject(NullHandler oReader)
        {
            AttendanceProcessManagement oAttendanceProcessManagement = MapObject(oReader);
            return oAttendanceProcessManagement;
        }

        private List<AttendanceProcessManagement> CreateObjects(IDataReader oReader)
        {
            List<AttendanceProcessManagement> oAttendanceProcessManagement = new List<AttendanceProcessManagement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                AttendanceProcessManagement oItem = CreateObject(oHandler);
                oAttendanceProcessManagement.Add(oItem);
            }
            return oAttendanceProcessManagement;
        }

        #endregion

        #region Interface implementation
        public AttendanceProcessManagementService() { }

        public AttendanceProcessManagement IUD(AttendanceProcessManagement oAttendanceProcessManagement, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = AttendanceProcessManagementDA.IUD(tc, oAttendanceProcessManagement, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oAttPM = CreateObject(oReader);
                }
                reader.Close();
                //if (oAttendanceProcessManagement.APMID > 0)
                //{
                //    if (oAttendanceProcessManagement.Status == EnumProcessStatus.Processed || oAttendanceProcessManagement.Status == EnumProcessStatus.ReProcessed)
                //    {
                //        AttendanceProcessManagementDA.ProcessAttendanceDaily(tc, oAttendanceProcessManagement, nUserID);
                //    }
                ////}

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttPM.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oAttPM.APMID = 0;
                #endregion
            }
            return oAttPM;
        }
        public AttendanceProcessManagement IUD_V2(AttendanceProcessManagement oAttendanceProcessManagement, EnumDBOperation eDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                if (eDBOperation == EnumDBOperation.Insert)
                {
                    IDataReader reader;
                    reader = AttendanceProcessManagementDA.IUD_V2(tc, oAttendanceProcessManagement, nUserID, eDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {

                        oAttPM = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (eDBOperation == EnumDBOperation.Delete)
                {
                    AttendanceProcessManagementDA.Delete_V2(tc, oAttendanceProcessManagement, nUserID, eDBOperation);
                }
                else if (eDBOperation == EnumDBOperation.RollBack)
                {
                    IDataReader reader = null;
                    List<AttendanceProcessManagement> oAPMs = new List<AttendanceProcessManagement>();
                    reader = AttendanceProcessManagementDA.IUD_V2(tc, oAttendanceProcessManagement, nUserID, eDBOperation);
                    oAPMs = CreateObjects(reader);
                    oAttPM.AttendanceProcessManagements = oAPMs;
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttPM.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oAttPM.APMID = 0;
                #endregion
            }
            return oAttPM;
        }

        public AttendanceProcessManagement ManualAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, int nEmployeeID, DateTime dAttendanceDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                AttendanceProcessManagementDA.ManualAttendanceProcess(tc, sBUIDs, sLocationIDs, sDepartmentIDs, nEmployeeID, dAttendanceDate, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttPM = new AttendanceProcessManagement();
                oAttPM.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oAttPM;
        }

        public AttendanceProcessManagement ManualCompAttendanceProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dAttendanceDate, int nMOCID, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                AttendanceProcessManagementDA.ManualCompAttendanceProcess(tc, sBUIDs, sLocationIDs, sDepartmentIDs, dAttendanceDate, nMOCID, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttPM = new AttendanceProcessManagement();
                oAttPM.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oAttPM;
        }

        public string APMProcess(string sBUIDs, string sLocationIDs, string sDepartmentIDs, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            TransactionContext tc = null;
            AttendanceProcessManagement oAttPM = new AttendanceProcessManagement();
            try
            {
                tc = TransactionContext.Begin(true);
                AttendanceProcessManagementDA.APMProcess(tc, sBUIDs, sLocationIDs, sDepartmentIDs, dStartDate, dEndDate, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();                
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.SuccessMessage;
        }

       
        public RTPunchLog ProcessDataCollectionRT(List<RTPunchLog> oRTPs, Int64 nUserID)
        {
            TransactionContext tc = null;
            RTPunchLog oRTPL = new RTPunchLog();
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (RTPunchLog oRTP in oRTPs)
                {
                    if (oRTP.C_Unique != "")
                    {
                        AttendanceProcessManagementDA.ProcessDataCollectionRT(tc, oRTP, nUserID);
                    }
                }
                oRTPL.ErrorMessage = "Saved Successfully.!!";
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRTPL.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  

                #endregion
            }
            return oRTPL;
        }

        public AttendanceProcessManagement Get(int nAPMID, Int64 nUserId)
        {
            AttendanceProcessManagement oAttendanceProcessManagement = new AttendanceProcessManagement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = AttendanceProcessManagementDA.Get(tc, nAPMID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAttendanceProcessManagement = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get AttendanceProcessManagement", e);
                oAttendanceProcessManagement.ErrorMessage = e.Message;
                #endregion
            }

            return oAttendanceProcessManagement;
        }
        public List<AttendanceProcessManagement> Gets(Int64 nUserID)
        {
            List<AttendanceProcessManagement> oAttendanceProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceProcessManagementDA.Gets(tc);
                oAttendanceProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_AttendanceProcessManagement", e);
                #endregion
            }
            return oAttendanceProcessManagement;
        }

        public List<AttendanceProcessManagement> Gets(string sSQL, Int64 nUserID)
        {
            List<AttendanceProcessManagement> oAttendanceProcessManagement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = AttendanceProcessManagementDA.Gets(sSQL, tc);
                oAttendanceProcessManagement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to View_Get AttendanceProcessManagement", e);
                #endregion
            }
            return oAttendanceProcessManagement;
        }

        public int ProcessAttendanceDaily_V1(AttendanceProcessManagement oAttendanceProcessManagement, int nIndex, Int64 nUserId)
        {
            int nNewIndex = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                nNewIndex = AttendanceProcessManagementDA.ProcessAttendanceDaily_V1(tc, oAttendanceProcessManagement, nIndex, nUserId);
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
            return nNewIndex;
        }

        public AttendanceProcessManagement ProcessBreezeAbsent(DateTime StartDate, DateTime EndDate, long nUserId)
        {
            AttendanceProcessManagement oAttendanceProcessManagement = new AttendanceProcessManagement();
            int nIndex = 0;
            int nNewIndex = 1;
            TransactionContext tc = null;
            try
            {
                while (nNewIndex != 0)
                {
                    tc = TransactionContext.Begin(true);
                    nNewIndex = AttendanceProcessManagementDA.ProcessBreezeAbsent(tc, nIndex, StartDate, EndDate, nUserId);
                    nIndex = nNewIndex;
                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oAttendanceProcessManagement = new AttendanceProcessManagement();
                oAttendanceProcessManagement.ErrorMessage = e.Message;
                #endregion
            }
            return oAttendanceProcessManagement;
        }

        #endregion
    }
}