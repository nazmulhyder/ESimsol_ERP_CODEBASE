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

    public class FabricTransferableLotService : MarshalByRefObject, IFabricTransferableLotService
    {
        #region Private functions and declaration
        DateTime dDate = new DateTime();
        double nUnManageQty = 0;
        private FabricTransferableLot MapObject(NullHandler oReader)
        {
            FabricTransferableLot oFabricTransferableLot = new FabricTransferableLot();
            oFabricTransferableLot.FabricTransferableLotID = oReader.GetInt32("FabricTransferableLotID");
            oFabricTransferableLot.LotNo = oReader.GetString("LotNo");
            oFabricTransferableLot.LotID = oReader.GetInt32("LotID");
            oFabricTransferableLot.Qty = oReader.GetDouble("Qty");
            oFabricTransferableLot.Product = oReader.GetString("ProductName");
            oFabricTransferableLot.WUName = oReader.GetString("WUName");
            oFabricTransferableLot.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oFabricTransferableLot.RollQty = oReader.GetInt32("RollQty");

            return oFabricTransferableLot;
        }

        private FabricTransferableLot CreateObject(NullHandler oReader)
        {
            FabricTransferableLot oFabricTransferableLot = new FabricTransferableLot();
            oFabricTransferableLot = MapObject(oReader);
            return oFabricTransferableLot;
        }

        private List<FabricTransferableLot> CreateObjects(IDataReader oReader)
        {
            List<FabricTransferableLot> oFabricTransferableLot = new List<FabricTransferableLot>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricTransferableLot oItem = CreateObject(oHandler);
                oFabricTransferableLot.Add(oItem);
            }
            return oFabricTransferableLot;
        }

        #endregion

        #region Interface implementation
        public FabricTransferableLotService() { }

        public List<FabricTransferableLot> Gets(Int64 nUserId)
        {
            List<FabricTransferableLot> oFabricTransferableLots = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricTransferableLotDA.Gets(tc, nUserId);
                oFabricTransferableLots = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricTransferableLot", e);
                #endregion
            }

            return oFabricTransferableLots;
        }
        public List<FabricTransferableLot> SendToRequsition(List<FabricTransferableLot> oFabricTransferableLots, Int64 nUserID)
        {

            FabricTransferableLot oFabricTransferableLot = new FabricTransferableLot();
            List<FabricTransferableLot> oFabricTransferableLots_Return = new List<FabricTransferableLot>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                foreach (FabricTransferableLot oItem in oFabricTransferableLots)
                {
                    IDataReader readerdetail;

                    if (oItem.FabricTransferableLotID <= 0)
                    {
                        readerdetail = FabricTransferableLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = FabricTransferableLotDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }

                    NullHandler oReader = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oFabricTransferableLot = new FabricTransferableLot();
                        oFabricTransferableLot = CreateObject(oReader);
                        oFabricTransferableLots_Return.Add(oFabricTransferableLot);
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

                oFabricTransferableLot = new FabricTransferableLot();
                oFabricTransferableLot.ErrorMessage = e.Message.Split('~')[0];
                oFabricTransferableLots_Return = new List<FabricTransferableLot>();
                oFabricTransferableLots_Return.Add(oFabricTransferableLot);

                #endregion
            }
            return oFabricTransferableLots_Return;
        }
        public string Delete(FabricTransferableLot oFabricTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricTransferableLotDA.Delete(tc, oFabricTransferableLot, EnumDBOperation.Delete, nUserID);
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
        public string TransferToStore(FabricTransferableLot oFabricTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricTransferableLotDA.TransferToStore(tc, oFabricTransferableLot, nUserID);
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
        public string LotAdjustment(FabricTransferableLot oFabricTransferableLot, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                FabricTransferableLotDA.LotAdjustment(tc, oFabricTransferableLot, nUserID);
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
