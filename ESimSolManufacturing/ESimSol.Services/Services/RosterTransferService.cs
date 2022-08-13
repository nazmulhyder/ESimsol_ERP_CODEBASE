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
    public class RosterTransferService : MarshalByRefObject, IRosterTransferService
    {
        #region Private functions and declaration
        private RosterTransfer MapObject(NullHandler oReader)
        {
            RosterTransfer oRosterTransfer = new RosterTransfer();

            oRosterTransfer.RosterTransferID = oReader.GetInt32("RosterTransferID");
            oRosterTransfer.EmployeeID = oReader.GetInt32("EmployeeID");
            oRosterTransfer.ShiftID = oReader.GetInt32("ShiftID");
            oRosterTransfer.Date = oReader.GetDateTime("Date");
            oRosterTransfer.EmployeeCode = oReader.GetString("EmployeeCode");
            oRosterTransfer.EmployeeName = oReader.GetString("EmployeeName");
            oRosterTransfer.ShiftName = oReader.GetString("ShiftName");
            oRosterTransfer.ShiftStartTime = oReader.GetDateTime("ShiftStartTime");
            oRosterTransfer.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            return oRosterTransfer;

        }

        private RosterTransfer CreateObject(NullHandler oReader)
        {
            RosterTransfer oRosterTransfer = MapObject(oReader);
            return oRosterTransfer;
        }

        private List<RosterTransfer> CreateObjects(IDataReader oReader)
        {
            List<RosterTransfer> oRosterTransfers = new List<RosterTransfer>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                RosterTransfer oItem = CreateObject(oHandler);
                oRosterTransfers.Add(oItem);
            }
            return oRosterTransfers;
        }

        #endregion

        #region Interface implementation
        public RosterTransferService() { }
        public RosterTransfer IUD(RosterTransfer oRosterTransfer, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = RosterTransferDA.IUD(tc, oRosterTransfer, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterTransfer = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oRosterTransfer.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oRosterTransfer.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oRosterTransfer;
        }

        public RosterTransfer Get(int nShiftBNID, Int64 nUserId)
        {
            RosterTransfer oRosterTransfer = new RosterTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterTransferDA.Get(nShiftBNID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterTransfer = CreateObject(oReader);
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

                oRosterTransfer.ErrorMessage = e.Message;
                #endregion
            }

            return oRosterTransfer;
        }

        public RosterTransfer Get(string sSQL, Int64 nUserId)
        {
            RosterTransfer oRosterTransfer = new RosterTransfer();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = RosterTransferDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oRosterTransfer = CreateObject(oReader);
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

                oRosterTransfer.ErrorMessage = e.Message;
                #endregion
            }

            return oRosterTransfer;
        }

        public List<RosterTransfer> Gets(Int64 nUserID)
        {
            List<RosterTransfer> oRosterTransfer = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterTransferDA.Gets(tc);
                oRosterTransfer = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterTransfer", e);
                #endregion
            }
            return oRosterTransfer;
        }

        public List<RosterTransfer> Gets(string sSQL, Int64 nUserID)
        {
            List<RosterTransfer> oRosterTransfer = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = RosterTransferDA.Gets(sSQL, tc);
                oRosterTransfer = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get RosterTransfer", e);
                #endregion
            }
            return oRosterTransfer;
        }

        public List<RosterTransfer> Gets(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate, Int64 nUserID)
        {
            List<RosterTransfer> oRosterTransfer = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterTransferDA.Gets(EmployeeIDs, ShiftID, StartDate, EndDate, tc);
                oRosterTransfer = CreateObjects(reader);
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
            return oRosterTransfer;
        }

        public List<RosterTransfer> Transfer(string EmployeeIDs, int ShiftID, DateTime StartDate, DateTime EndDate,int nDBOperation, Int64 nUserID)
        {
            List<RosterTransfer> oRosterTransfers = new List<RosterTransfer>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterTransferDA.Transfer(EmployeeIDs, ShiftID, StartDate, EndDate, nUserID, nDBOperation, tc);
                oRosterTransfers = CreateObjects(reader);
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
            return oRosterTransfers;
        }

        public List<RosterTransfer> Swap(int RosterPlanID, DateTime StartDate, DateTime EndDate, int nDBOperation, Int64 nUserID)
        {
            List<RosterTransfer> oRosterTransfers = new List<RosterTransfer>(); ;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = RosterTransferDA.Swap(RosterPlanID, StartDate, EndDate, nUserID, nDBOperation, tc);
                oRosterTransfers = CreateObjects(reader);
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
            return oRosterTransfers;
        }
        #endregion

    }
}
