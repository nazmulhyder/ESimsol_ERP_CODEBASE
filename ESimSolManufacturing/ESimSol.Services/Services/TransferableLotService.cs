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

    public class TransferableLotService : MarshalByRefObject, ITransferableLotService
    {
        #region Private functions and declaration
        DateTime dDate = new DateTime();
        double nUnManageQty = 0;
        private TransferableLot MapObject(NullHandler oReader)
        {
            TransferableLot oTransferableLot = new TransferableLot();
            oTransferableLot.TransferableLotID = oReader.GetInt32("TransferableLotID");
            oTransferableLot.LotNo = oReader.GetString("LotNo");
            oTransferableLot.LotID = oReader.GetInt32("LotID");
            oTransferableLot.Qty = oReader.GetDouble("Qty");
            oTransferableLot.Product = oReader.GetString("ProductName");
            oTransferableLot.WUName = oReader.GetString("WUName");
            oTransferableLot.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
          
          
            return oTransferableLot;
        }

        private TransferableLot CreateObject(NullHandler oReader)
        {
            TransferableLot oTransferableLot = new TransferableLot();
            oTransferableLot = MapObject(oReader);
            return oTransferableLot;
        }

        private List<TransferableLot> CreateObjects(IDataReader oReader)
        {
            List<TransferableLot> oTransferableLot = new List<TransferableLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TransferableLot oItem = CreateObject(oHandler);
                oTransferableLot.Add(oItem);
            }
            return oTransferableLot;
        }

        #endregion

        #region Interface implementation
        public TransferableLotService() { }

        public List<TransferableLot> Gets(Int64 nUserId)
        {
            List<TransferableLot> oTransferableLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TransferableLotDA.Gets(tc, nUserId);
                oTransferableLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TransferableLot", e);
                #endregion
            }

            return oTransferableLots;
        }
        public List<TransferableLot> SendToRequsition(List<TransferableLot> oTransferableLots, Int64 nUserID)
        {

            TransferableLot oTransferableLot = new TransferableLot();
            List<TransferableLot> oTransferableLots_Return = new List<TransferableLot>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (TransferableLot oItem in oTransferableLots)
                {
                    IDataReader readerdetail;

                    if (oItem.TransferableLotID <= 0)
                    {
                        readerdetail = TransferableLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = TransferableLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReader = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oTransferableLot = new TransferableLot();
                        oTransferableLot = CreateObject(oReader);
                        oTransferableLots_Return.Add(oTransferableLot);
                    }
                    readerdetail.Close();
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTransferableLot = new TransferableLot();
                oTransferableLot.ErrorMessage = e.Message.Split('~')[0];
                oTransferableLots_Return.Add(oTransferableLot);

                #endregion
            }
            return oTransferableLots_Return;
        }
        public string Delete(TransferableLot oTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                TransferableLotDA.Delete(tc, oTransferableLot, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public string TransferToStore(TransferableLot oTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                TransferableLotDA.TransferToStore(tc, oTransferableLot, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0]; 
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return "Data Save Succesfully";
        }
        public string LotAdjustment(TransferableLot oTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                TransferableLotDA.LotAdjustment(tc, oTransferableLot, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Bank. Because of " + e.Message, e);
                #endregion
            }
            return "Data Save Succesfully";
        }
        #endregion
    }    
    
  
}
