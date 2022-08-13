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
    public class ApprovalHistoryService : MarshalByRefObject, IApprovalHistoryService
    {
        #region Private functions and declaration
        private ApprovalHistory MapObject(NullHandler oReader)
        {
            ApprovalHistory oApprovalHistory = new ApprovalHistory();
            oApprovalHistory.ApprovalHistoryID = oReader.GetInt32("ApprovalHistoryID");
            oApprovalHistory.ObjectRefID = oReader.GetInt32("ObjectRefID");
            oApprovalHistory.ApprovalHeadID = oReader.GetInt32("ApprovalHeadID");
            oApprovalHistory.SendToPersonID = oReader.GetInt32("SendToPersonID");
            oApprovalHistory.ApprovalHeadName = oReader.GetString("ApprovalHeadName");
            oApprovalHistory.SendToPersonName = oReader.GetString("SendToPersonName");
            oApprovalHistory.Note = oReader.GetString("Note");
            return oApprovalHistory;
        }

        private ApprovalHistory CreateObject(NullHandler oReader)
        {
            ApprovalHistory oApprovalHistory = MapObject(oReader);
            return oApprovalHistory;
        }

        private List<ApprovalHistory> CreateObjects(IDataReader oReader)
        {
            List<ApprovalHistory> oApprovalHistory = new List<ApprovalHistory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ApprovalHistory oItem = CreateObject(oHandler);
                oApprovalHistory.Add(oItem);
            }
            return oApprovalHistory;
        }


        #endregion

        #region Interface implementation
        public ApprovalHistoryService() { }
        public ApprovalHistory IUD(ApprovalHistory oApprovalHistory, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = ApprovalHistoryDA.IUD(tc, oApprovalHistory, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oApprovalHistory = new ApprovalHistory();
                        oApprovalHistory = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = ApprovalHistoryDA.IUD(tc, oApprovalHistory, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oApprovalHistory.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oApprovalHistory = new ApprovalHistory();
                oApprovalHistory.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oApprovalHistory;
        }


        public ApprovalHistory Get(string sSQL, Int64 nUserId)
        {
            ApprovalHistory oApprovalHistory = new ApprovalHistory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ApprovalHistoryDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oApprovalHistory = CreateObject(oReader);
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

            return oApprovalHistory;
        }

        public List<ApprovalHistory> Gets(string sSQL, Int64 nUserID)
        {
            List<ApprovalHistory> oApprovalHistory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ApprovalHistoryDA.Gets(sSQL, tc);
                oApprovalHistory = CreateObjects(reader);
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
            return oApprovalHistory;
        }
        #endregion
    }
}


