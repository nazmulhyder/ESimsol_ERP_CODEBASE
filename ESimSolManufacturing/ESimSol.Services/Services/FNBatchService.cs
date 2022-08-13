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
    public class FNBatchService : MarshalByRefObject, IFNBatchService
    {
        #region Private functions and declaration
        private FNBatch MapObject(NullHandler oReader)
        {
            FNBatch oFNBatch = new FNBatch();
            oFNBatch.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatch.BatchNo = oReader.GetString("BatchNo");
            oFNBatch.FNExOID = oReader.GetInt32("FNExOID");
            oFNBatch.Qty = oReader.GetDouble("Qty");
            oFNBatch.FNBatchStatus = (EnumFNBatchStatus)oReader.GetInt16("FNBatchStatus");
            oFNBatch.GLM = oReader.GetDouble("GLM");
            oFNBatch.FNPPID = oReader.GetInt32("FNPPID");
            oFNBatch.IssueDate = oReader.GetDateTime("IssueDate");
            oFNBatch.ExpectedDeliveryDate = oReader.GetDateTime("ExpectedDeliveryDate");
            oFNBatch.GreyGSM = oReader.GetDouble("GreyGSM");
            oFNBatch.FNExONo = oReader.GetString("FNExONo");
            //oFNBatch.GSM = oReader.GetDouble("GSM");
            oFNBatch.ExeQty = oReader.GetDouble("ExeQty");
            oFNBatch.CountName = oReader.GetString("CountName");
            oFNBatch.BuyerID = oReader.GetInt32("BuyerID");
            oFNBatch.BuyerName = oReader.GetString("BuyerName");
            oFNBatch.Construction = oReader.GetString("Construction");
            oFNBatch.GreyWidth = oReader.GetDouble("GreyWidth");
            oFNBatch.FinishWidth = oReader.GetString("FinishWidth");
            oFNBatch.FinishType = oReader.GetInt32("FinishType");
            oFNBatch.FinishTypeName = oReader.GetString("FinishTypeName");
            oFNBatch.OutQty = oReader.GetDouble("OutQty");
            oFNBatch.QCQty = oReader.GetDouble("QCQty");
            //oFNBatch.Params = oReader.GetString("ExeNo");
            oFNBatch.MUnit = oReader.GetString("MUnit");
            oFNBatch.FabricNo = oReader.GetString("FabricNo");
            oFNBatch.Note = oReader.GetString("Note");
            oFNBatch.SCNoFull = oReader.GetString("SCNoFull");
            oFNBatch.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFNBatch.PrepareByName = oReader.GetString("PrepareByName");

            oFNBatch.FNBatchTransferHistoryID = oReader.GetInt32("FNBatchTransferHistoryID");
            oFNBatch.SourceBatchID = oReader.GetInt32("SourceBatchID");
            oFNBatch.SourceBatchNo = oReader.GetString("SourceBatchNo");
            oFNBatch.DestinationBatchID = oReader.GetInt32("DestinationBatchID");
            oFNBatch.DestinationBatchNo = oReader.GetString("DestinationBatchNo");
            oFNBatch.UserID = oReader.GetInt32("UserID");
            oFNBatch.UserName = oReader.GetString("UserName");
            oFNBatch.TransferTime = oReader.GetDateTime("TransferTime");

            
            return oFNBatch;
        }

        private FNBatch CreateObject(NullHandler oReader)
        {
            FNBatch oFNBatch = MapObject(oReader);
            return oFNBatch;
        }

        private List<FNBatch> CreateObjects(IDataReader oReader)
        {
            List<FNBatch> oFNBatch = new List<FNBatch>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatch oItem = CreateObject(oHandler);
                oFNBatch.Add(oItem);
            }
            return oFNBatch;
        }

        #endregion

        #region Interface implementation
        public FNBatchService() { }

        public FNBatch IUD(FNBatch oFNBatch, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = FNBatchDA.IUD(tc, oFNBatch, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatch = new FNBatch();
                        oFNBatch = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FNBatchDA.IUD(tc, oFNBatch, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFNBatch.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNBatch;
        }
        public FNBatch SaveNote(string sSQL, Int64 nUserID)
        {
            FNBatch oFNBatch = new FNBatch();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = FNBatchDA.SaveNote(tc, sSQL, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatch = new FNBatch();
                    oFNBatch = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oFNBatch;
        }

        public FNBatch Get(int nFNBatchID, Int64 nUserId)
        {
            FNBatch oFNBatch = new FNBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchDA.Get(tc, nFNBatchID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = ex.Message;
                #endregion
            }

            return oFNBatch;
        }

        public List<FNBatch> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatch> oFNBatchs = new List<FNBatch>();
            FNBatch oFNBatch = new FNBatch();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchDA.Gets(tc, sSQL);
                oFNBatchs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatch.ErrorMessage = ex.Message;
                oFNBatchs.Add(oFNBatch);
                #endregion
            }

            return oFNBatchs;
        }

        public FNBatch TransferFNBatchCard(FNBatch oFNBatch, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FNBatchDA.TransferFNBatchCard(tc, oFNBatch, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatch = new FNBatch();
                    oFNBatch = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFNBatch = new FNBatch();
                oFNBatch.ErrorMessage = e.Message.Split('~')[0];
                #endregion

            }
            return oFNBatch;
        }

        #endregion
    }
}
