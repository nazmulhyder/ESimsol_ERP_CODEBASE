using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class DURequisitionService : MarshalByRefObject, IDURequisitionService
    {
        #region Private functions and declaration
        private DURequisition MapObject(NullHandler oReader)
        {
            DURequisition oDURequisition = new DURequisition();
            oDURequisition.DURequisitionID = oReader.GetInt32("DURequisitionID");
            oDURequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oDURequisition.ReqDate = oReader.GetDateTime("ReqDate");
            oDURequisition.BUID_issue = oReader.GetInt32("BUID_issue");
            oDURequisition.BUID_Receive = oReader.GetInt32("BUID_Receive");
            oDURequisition.RequisitionType = (EnumInOutType)oReader.GetInt32("RequisitionType");
            oDURequisition.WorkingUnitID_Issue = oReader.GetInt32("WorkingUnitID_Issue");
            oDURequisition.WorkingUnitID_Receive = oReader.GetInt32("WorkingUnitID_Receive");

            oDURequisition.ApprovebyID = oReader.GetInt32("ApprovebyID");
            oDURequisition.IssuebyID = oReader.GetInt32("IssuebyID");
            oDURequisition.ReceiveByID = oReader.GetInt32("ReceiveByID");

            oDURequisition.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDURequisition.IssueDate = oReader.GetDateTime("IssueDate");
            oDURequisition.ReceiveDate = oReader.GetDateTime("ReceiveDate");

            oDURequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oDURequisition.IssuedByName = oReader.GetString("IssuedByName");
            oDURequisition.ReceivedByName = oReader.GetString("ReceivedByName");
            oDURequisition.OrderNo = oReader.GetString("OrderNo");
            oDURequisition.RequisitionByName = oReader.GetString("RequisitionByName");
            oDURequisition.IssueStore = oReader.GetString("IssueStore");
            oDURequisition.ReceiveStore = oReader.GetString("ReceiveStore");
            oDURequisition.SetupName = oReader.GetString("SetupName");
            oDURequisition.IsOpenOrder = oReader.GetBoolean("IsOpenOrder");

            oDURequisition.OpeartionUnitType = (EnumOperationUnitType)oReader.GetInt32("OperationUnitType");
            oDURequisition.Note = oReader.GetString("Note");
          
            return oDURequisition;
        }

        private DURequisition CreateObject(NullHandler oReader)
        {
            DURequisition oDURequisition = new DURequisition();
            oDURequisition = MapObject(oReader);
            return oDURequisition;
        }

        private List<DURequisition> CreateObjects(IDataReader oReader)
        {
            List<DURequisition> oDURequisitions = new List<DURequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DURequisition oItem = CreateObject(oHandler);
                oDURequisitions.Add(oItem);
            }
            return oDURequisitions;
        }

        #endregion

        #region Interface implementation
        public DURequisitionService() { }

        public DURequisition Save(DURequisition oDURequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<DURequisitionDetail> oDURequisitionDetails = new List<DURequisitionDetail>();
                oDURequisitionDetails = oDURequisition.DURequisitionDetails;
                string sDURequisitionDetailIDS = "";

                IDataReader reader;
                if (oDURequisition.DURequisitionID <= 0)
                {
                    reader = DURequisitionDA.InsertUpdate(tc, oDURequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DURequisitionDA.InsertUpdate(tc, oDURequisition, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisition = CreateObject(oReader);
                }
                reader.Close();

                #region DURequisition Part

                foreach (DURequisitionDetail oItem in oDURequisitionDetails)
                {
                    IDataReader readerdetail;
                    oItem.DURequisitionID = oDURequisition.DURequisitionID;
                    if (oItem.DURequisitionDetailID <= 0)
                    {
                        readerdetail = DURequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = DURequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDURequisitionDetailIDS = sDURequisitionDetailIDS + oReaderDetail.GetString("DURequisitionDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                DURequisitionDetail oDURequisitionDetail = new DURequisitionDetail();
                oDURequisitionDetail.DURequisitionID = oDURequisition.DURequisitionID;
                if (sDURequisitionDetailIDS.Length > 0)
                {
                    sDURequisitionDetailIDS = sDURequisitionDetailIDS.Remove(sDURequisitionDetailIDS.Length - 1, 1);
                }
                DURequisitionDetailDA.Delete(tc, oDURequisitionDetail, EnumDBOperation.Delete, nUserID, sDURequisitionDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oDURequisition.ErrorMessage = Message;
                #endregion
            }

            return oDURequisition;

        }

        public String Delete(DURequisition oDURequisition, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DURequisitionDA.Delete(tc, oDURequisition, EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                return Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        public DURequisition Get(int id, Int64 nUserId)
        {
            DURequisition oDURequisition = new DURequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionDA.Get(id, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisition = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDURequisition;
        }
        public List<DURequisition> Gets(string sSQL, Int64 nUserID)
        {
            List<DURequisition> oDURequisition = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionDA.Gets(tc, sSQL);
                oDURequisition = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DURequisition", e);
                #endregion
            }
            return oDURequisition;
        }
        public List<DURequisition> Gets(Int64 nUserId)
        {
            List<DURequisition> oDURequisitions = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DURequisitionDA.Gets(tc);
                oDURequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oDURequisitions;
        }

       public DURequisition Approve(DURequisition oDURequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionDA.Approve(tc, oDURequisition, EnumDBOperation.Approval, nUserId); 
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDURequisition = new DURequisition();
                oDURequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDURequisition;
        }
       //(DB Operation: 102 For Issue, 103 For Receive) 
       public DURequisition UndoApprove(DURequisition oDURequisition, Int64 nUserId)
       {
           TransactionContext tc = null;
           try
           {
               tc = TransactionContext.Begin();

               IDataReader reader = DURequisitionDA.UndoApprove(tc, oDURequisition, EnumDBOperation.UnApproval, nUserId);
               NullHandler oReader = new NullHandler(reader);
               if (reader.Read())
               {
                   oDURequisition = CreateObject(oReader);
               }
               reader.Close();
               tc.End();
           }
           catch (Exception e)
           {
               #region Handle Exception
               if (tc != null)
                   tc.HandleError();

               oDURequisition = new DURequisition();
               oDURequisition.ErrorMessage = e.Message.Split('~')[0];
               #endregion
           }
           return oDURequisition;
       }
       public DURequisition Issue(DURequisition oDURequisition, Int64 nUserId)
       {
           TransactionContext tc = null;
           try
           {
               tc = TransactionContext.Begin();

               IDataReader reader = DURequisitionDA.Issue(tc, oDURequisition, nUserId);
               NullHandler oReader = new NullHandler(reader);
               if (reader.Read())
               {
                   oDURequisition = CreateObject(oReader);
               }
               reader.Close();
               tc.End();
           }
           catch (Exception e)
           {
               #region Handle Exception
               if (tc != null)
                   tc.HandleError();

               oDURequisition = new DURequisition();
               string Message = "";
               Message = e.Message;
               Message = Message.Split('!')[0];
               oDURequisition.ErrorMessage = Message;
               #endregion
           }
           return oDURequisition;
       }
       public DURequisition UndoIssue(DURequisition oDURequisition, Int64 nUserId)
       {
           TransactionContext tc = null;
           try
           {
               tc = TransactionContext.Begin();

               IDataReader reader = DURequisitionDA.UndoIssue(tc, oDURequisition, EnumDBOperation.Undo, nUserId);
               NullHandler oReader = new NullHandler(reader);
               if (reader.Read())
               {
                   oDURequisition = CreateObject(oReader);
               }
               reader.Close();
               tc.End();
           }
           catch (Exception e)
           {
               #region Handle Exception
               if (tc != null)
                   tc.HandleError();

               oDURequisition = new DURequisition();
               string Message = "";
               Message = e.Message;
               Message = Message.Split('!')[0];
               oDURequisition.ErrorMessage = Message;
               #endregion
           }
           return oDURequisition;
       }
       public DURequisition Receive(DURequisition oDURequisition, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DURequisitionDA.Receive(tc, oDURequisition, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDURequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDURequisition = new DURequisition();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oDURequisition.ErrorMessage = Message;
                #endregion
            }
            return oDURequisition;
        }

        #endregion
    }
}