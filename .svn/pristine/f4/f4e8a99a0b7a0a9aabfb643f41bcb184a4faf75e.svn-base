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
    public class DUProGuideLineService : MarshalByRefObject, IDUProGuideLineService
    {
        #region Private functions and declaration
        private DUProGuideLine MapObject(NullHandler oReader)
        {
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            oDUProGuideLine.DUProGuideLineID = oReader.GetInt32("DUProGuideLineID");
            oDUProGuideLine.BUID = oReader.GetInt32("BUID");
            oDUProGuideLine.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oDUProGuideLine.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            oDUProGuideLine.ProductTypeInt = oReader.GetInt32("ProductType");
            oDUProGuideLine.InOutTypeInt = oReader.GetInt32("InOutType");
            oDUProGuideLine.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oDUProGuideLine.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oDUProGuideLine.ApproveByID = oReader.GetInt32("ApproveByID");
            oDUProGuideLine.ReceiveByID = oReader.GetInt32("ReceiveByID");
            oDUProGuideLine.ReturnByID = oReader.GetInt32("ReturnByID");
            oDUProGuideLine.ApproveDate = oReader.GetDateTime("ApproveDate");
            oDUProGuideLine.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDUProGuideLine.OrderDate = oReader.GetDateTime("OrderDate");
            oDUProGuideLine.ReturnDate = oReader.GetDateTime("ReturnDate");
            oDUProGuideLine.IssueDate = oReader.GetDateTime("IssueDate");
            oDUProGuideLine.ContractorID = oReader.GetInt32("ContractorID");
            oDUProGuideLine.ContractorType = oReader.GetInt32("ContractorType");
            oDUProGuideLine.ContractorName = oReader.GetString("ContractorName");
            oDUProGuideLine.ApprovedByName = oReader.GetString("ApprovedByName");
            oDUProGuideLine.ReceivedByName = oReader.GetString("ReceivedByName");
            oDUProGuideLine.ReturnByName = oReader.GetString("ReturnByName");
            oDUProGuideLine.ReceiveStore = oReader.GetString("ReceiveStore");
            oDUProGuideLine.Note = oReader.GetString("Note");
            oDUProGuideLine.SLNo = oReader.GetString("SLNo");
            oDUProGuideLine.VehicleNo = oReader.GetString("VehicleNo");
            oDUProGuideLine.GateInNo = oReader.GetString("GateInNo");
            oDUProGuideLine.ChallanNo = oReader.GetString("ChallanNo");
            oDUProGuideLine.StyleNo = oReader.GetString("StyleNo");
            oDUProGuideLine.RefNo = oReader.GetString("RefNo");
            oDUProGuideLine.Qty = oReader.GetDouble("Qty");
            oDUProGuideLine.DyeingOrderNo = oReader.GetString("DyeingOrderNo");
            oDUProGuideLine.OrderTypeST = oReader.GetString("OrderTypeST");
            oDUProGuideLine.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            return oDUProGuideLine;
        }

        private DUProGuideLine CreateObject(NullHandler oReader)
        {
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            oDUProGuideLine = MapObject(oReader);
            return oDUProGuideLine;
        }

        private List<DUProGuideLine> CreateObjects(IDataReader oReader)
        {
            List<DUProGuideLine> oDUProGuideLines = new List<DUProGuideLine>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DUProGuideLine oItem = CreateObject(oHandler);
                oDUProGuideLines.Add(oItem);
            }
            return oDUProGuideLines;
        }

        #endregion

        #region Interface implementation
        public DUProGuideLineService() { }

        public DUProGuideLine Save(DUProGuideLine oDUProGuideLine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
                oDUProGuideLineDetails = oDUProGuideLine.DUProGuideLineDetails;
                string sDUProGuideLineDetailIDS = "";

                IDataReader reader;
                if (oDUProGuideLine.DUProGuideLineID <= 0)
                {
                    reader = DUProGuideLineDA.InsertUpdate(tc, oDUProGuideLine, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DUProGuideLineDA.InsertUpdate(tc, oDUProGuideLine, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
                }
                reader.Close();

                #region DUProGuideLine Part

                foreach (DUProGuideLineDetail oItem in oDUProGuideLineDetails)
                {
                    IDataReader readerdetail;
                    oItem.DUProGuideLineID = oDUProGuideLine.DUProGuideLineID;
                    if (oItem.Qty > 0)
                    {
                        if (oItem.DUProGuideLineDetailID <= 0)
                        {
                            readerdetail = DUProGuideLineDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = DUProGuideLineDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sDUProGuideLineDetailIDS = sDUProGuideLineDetailIDS + oReaderDetail.GetString("DUProGuideLineDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                DUProGuideLineDetail oDUProGuideLineDetail = new DUProGuideLineDetail();
                oDUProGuideLineDetail.DUProGuideLineID = oDUProGuideLine.DUProGuideLineID;
                if (sDUProGuideLineDetailIDS.Length > 0)
                {
                    sDUProGuideLineDetailIDS = sDUProGuideLineDetailIDS.Remove(sDUProGuideLineDetailIDS.Length - 1, 1);
                }
                DUProGuideLineDetailDA.Delete(tc, oDUProGuideLineDetail, EnumDBOperation.Delete, nUserID, sDUProGuideLineDetailIDS);
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
                oDUProGuideLine.ErrorMessage = Message;
                #endregion
            }

            return oDUProGuideLine;

        }

        public String Delete(DUProGuideLine oDUProGuideLine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DUProGuideLineDA.Delete(tc, oDUProGuideLine, EnumDBOperation.Delete, nUserID);
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
        public DUProGuideLine Get(int id, Int64 nUserId)
        {
            DUProGuideLine oDUProGuideLine = new DUProGuideLine();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDA.Get(id, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
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

            return oDUProGuideLine;
        }
        public List<DUProGuideLine> Gets(string sSQL, Int64 nUserID)
        {
            List<DUProGuideLine> oDUProGuideLine = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProGuideLineDA.Gets(tc, sSQL);
                oDUProGuideLine = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DUProGuideLine", e);
                #endregion
            }
            return oDUProGuideLine;
        }
        public List<DUProGuideLine> Gets(Int64 nUserId)
        {
            List<DUProGuideLine> oDUProGuideLines = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DUProGuideLineDA.Gets(tc);
                oDUProGuideLines = CreateObjects(reader);
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

            return oDUProGuideLines;
        }

        public DUProGuideLine Approve(DUProGuideLine oDUProGuideLine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDA.InsertUpdate(tc, oDUProGuideLine, EnumDBOperation.Approval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUProGuideLine = new DUProGuideLine();
                oDUProGuideLine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUProGuideLine;
        }
        public DUProGuideLine UndoApprove(DUProGuideLine oDUProGuideLine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDA.InsertUpdate(tc, oDUProGuideLine, EnumDBOperation.UnApproval, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUProGuideLine = new DUProGuideLine();
                oDUProGuideLine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUProGuideLine;
        }

        public DUProGuideLine Update_ReturnQty(DUProGuideLine oDUProGuideLine, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<DUProGuideLineDetail> oDUProGuideLineDetails = new List<DUProGuideLineDetail>();
                oDUProGuideLineDetails = oDUProGuideLine.DUProGuideLineDetails;
            
                #region DUProGuideLine Part

                foreach (DUProGuideLineDetail oItem in oDUProGuideLineDetails)
                {
                    oItem.DUProGuideLineID = oDUProGuideLine.DUProGuideLineID;
                    if (oItem.Qty > 0 && oDUProGuideLine.InOutType==EnumInOutType.Disburse)
                    {
                        if (oItem.DUProGuideLineDetailID >= 0)
                        {
                           // if(oItem.Qty-oItem.S)
                           DUProGuideLineDetailDA.UpdateReturnQty(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                    }
                }
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
                oDUProGuideLine.ErrorMessage = Message;
                #endregion
            }

            return oDUProGuideLine;

        }

        //(New DB Operation: Receive Added) 
        public DUProGuideLine Receive(DUProGuideLine oDUProGuideLine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDA.Receive(tc, oDUProGuideLine, EnumDBOperation.Receive, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUProGuideLine = new DUProGuideLine();
                oDUProGuideLine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUProGuideLine;
        }
        public DUProGuideLine Return(DUProGuideLine oDUProGuideLine, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DUProGuideLineDA.Return(tc, oDUProGuideLine, EnumDBOperation.Revise, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDUProGuideLine = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oDUProGuideLine = new DUProGuideLine();
                oDUProGuideLine.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oDUProGuideLine;
        }
        #endregion
    }
}
