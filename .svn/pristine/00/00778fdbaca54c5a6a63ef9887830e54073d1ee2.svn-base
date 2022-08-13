using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricDeliveryOrderService : MarshalByRefObject, IFabricDeliveryOrderService
    {            
        #region Private functions and declaration
        private FabricDeliveryOrder MapObject(NullHandler oReader)
        {
            FabricDeliveryOrder oFDO = new FabricDeliveryOrder();
            oFDO.FDOID = oReader.GetInt32("FDOID");
            oFDO.FDODID = oReader.GetInt32("FDODID");
            oFDO.FEOID = oReader.GetInt32("FEOID");
            oFDO.FDOType = (EnumDOType)oReader.GetInt32("FDOType");
            oFDO.FDOTypeInInt = oReader.GetInt32("FDOType");
            oFDO.DONo = oReader.GetString("DONo");
            oFDO.DOStatus = (EnumFabricDOStatus)oReader.GetInt16("DOStatus");
            oFDO.DOStatusInInt = oReader.GetInt16("DOStatus");
            oFDO.DODate = oReader.GetDateTime("DODate");
            //oFDO.DeliveryTo = oReader.GetInt32("DeliveryTo");
            oFDO.DeliveryPoint = oReader.GetString("DeliveryPoint");
            oFDO.ExpDeliveryDate = oReader.GetDateTime("ExpDeliveryDate");
            oFDO.CurrencyID = oReader.GetInt32("CurrencyID");
            oFDO.ContractorID = oReader.GetInt32("ContractorID");
            oFDO.Note = oReader.GetString("Note");
            oFDO.IsFinish = oReader.GetBoolean("IsFinish");
            oFDO.ApproveBy = oReader.GetInt32("ApproveBy");
            oFDO.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFDO.BuyerName = oReader.GetString("BuyerName");
            oFDO.DeliveryToName = oReader.GetString("DeliveryToName");
            oFDO.ContractorName = oReader.GetString("ContractorName");
            oFDO.MKTPerson = oReader.GetString("MKTPerson");
            oFDO.ApproveByName = oReader.GetString("ApproveByName");
            oFDO.PreparedByName = oReader.GetString("PreparedByName");
            oFDO.CheckedByName = oReader.GetString("CheckedByName");
            oFDO.BuyerAddress = oReader.GetString("BuyerAddress");
            oFDO.CurrencyName = oReader.GetString("CurrencyName");
            oFDO.Qty = oReader.GetDouble("Qty");
            //oFDO.DeliveryToContractorType = (EnumContractorType)oReader.GetInt16("DeliveryToContractorType");
            oFDO.BuyerID = oReader.GetInt32("BuyerID");
            oFDO.DeliveryZoneID = oReader.GetInt32("DeliveryZoneID");
            oFDO.BCPID = oReader.GetInt32("BCPID");
            oFDO.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oFDO.BuyerCPName = oReader.GetString("BuyerCPName");
            oFDO.BuyerCPPhone = oReader.GetString("BuyerCPPhone");
            //oFDO.LCAmount = oReader.GetDouble("LCAmount");
            //oFDO.PIAmount = oReader.GetDouble("PIAmount");
            oFDO.Qty_DC = oReader.GetDouble("Qty_DC");
            oFDO.BUID = oReader.GetInt32("BUID");
         
            return oFDO;
        }

        public static FabricDeliveryOrder CreateObject(NullHandler oReader)
        {
            FabricDeliveryOrder oFabricExecutionOrder = new FabricDeliveryOrder();
            FabricDeliveryOrderService oFEOService = new FabricDeliveryOrderService();
            oFabricExecutionOrder = oFEOService.MapObject(oReader);
            return oFabricExecutionOrder;
        }

        private List<FabricDeliveryOrder> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliveryOrder> oFabricExecutionOrders = new List<FabricDeliveryOrder>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliveryOrder oItem = CreateObject(oHandler);
                oFabricExecutionOrders.Add(oItem);
            }
            return oFabricExecutionOrders;
        }
        #endregion

        #region Interface implementatio
        public FabricDeliveryOrderService() { }

     
        public FabricDeliveryOrder IUD(FabricDeliveryOrder oFabricDeliveryOrder, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            String sFabricDeliveryOrderDetaillIDs = "";
            double  nQty=0;
            try
            {
                oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricDeliveryOrder.FDOID <= 0)
                {
                    reader = FabricDeliveryOrderDA.IUD(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricDeliveryOrderDA.IUD(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                #region FabricDeliveryOrderDetails Part
                if (oFabricDeliveryOrderDetails != null)
                {
                    foreach (FabricDeliveryOrderDetail oItem in oFabricDeliveryOrderDetails)
                    {
                        nQty=nQty+oItem.Qty;
                        IDataReader readertnc;
                        oItem.FDOID = oFabricDeliveryOrder.FDOID;
                        if (oItem.FDODID <= 0)
                        {
                            readertnc = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sFabricDeliveryOrderDetaillIDs = sFabricDeliveryOrderDetaillIDs + oReaderTNC.GetString("FDODID") + ",";
                        }
                        readertnc.Close();
                    }
                    oFabricDeliveryOrder.Qty = nQty;
                    if (sFabricDeliveryOrderDetaillIDs.Length > 0)
                    {
                        sFabricDeliveryOrderDetaillIDs = sFabricDeliveryOrderDetaillIDs.Remove(sFabricDeliveryOrderDetaillIDs.Length - 1, 1);
                    }
                    FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                    oFabricDeliveryOrderDetail.FDOID = oFabricDeliveryOrder.FDOID;
                    FabricDeliveryOrderDetailDA.Delete(tc, oFabricDeliveryOrderDetail, (int)EnumDBOperation.Delete, nUserID, sFabricDeliveryOrderDetaillIDs);
                    sFabricDeliveryOrderDetaillIDs = "";

                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }
        public FabricDeliveryOrder SaveLog(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            double nAmount = 0;
            double nQty = 0;
            String sFabricDeliveryOrderDetaillIDs = "";
            try
            {
                oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;

                foreach (FabricDeliveryOrderDetail oItem in oFabricDeliveryOrderDetails)
                {
                    nAmount = nAmount + oItem.Qty * oItem.UnitPrice;
                }

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDeliveryOrderDA.IUD_Log(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Revise, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                #region FabricDeliveryOrderDetails Part
                if (oFabricDeliveryOrderDetails != null)
                {
                    foreach (FabricDeliveryOrderDetail oItem in oFabricDeliveryOrderDetails)
                    {
                        IDataReader readertnc;
                        oItem.FDOID = oFabricDeliveryOrder.FDOID;
                        nQty = nQty + oItem.Qty;
                        if (oItem.FDODID <= 0)
                        {
                            readertnc = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);

                        if (readertnc.Read())
                        {
                            sFabricDeliveryOrderDetaillIDs = sFabricDeliveryOrderDetaillIDs + oReaderTNC.GetString("FDODID") + ",";
                        }
                        readertnc.Close();
                    }
                    oFabricDeliveryOrder.Qty = nQty;
                    if (sFabricDeliveryOrderDetaillIDs.Length > 0)
                    {
                        sFabricDeliveryOrderDetaillIDs = sFabricDeliveryOrderDetaillIDs.Remove(sFabricDeliveryOrderDetaillIDs.Length - 1, 1);
                    }
                    FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                    oFabricDeliveryOrderDetail.FDOID = oFabricDeliveryOrder.FDOID;
                    FabricDeliveryOrderDetailDA.Delete(tc, oFabricDeliveryOrderDetail, (int)EnumDBOperation.Delete, nUserID, sFabricDeliveryOrderDetaillIDs);
                    sFabricDeliveryOrderDetaillIDs = "";

                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }
        public FabricDeliveryOrder Approved(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDeliveryOrderDA.IUD(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Approval, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }
        public FabricDeliveryOrder Checked(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {

                List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
                FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetails = oFabricDeliveryOrder.FDODetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDeliveryOrderDA.IUD(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Request, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }
        public FabricDeliveryOrder Cancel(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDeliveryOrderDA.IUD(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Cancel, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oFabricDeliveryOrder = new FabricDeliveryOrder();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }
        public String Delete(FabricDeliveryOrder oFabricDeliveryOrder, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDeliveryOrderDA.Delete(tc, oFabricDeliveryOrder, (int)EnumDBOperation.Delete, nUserID);
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        public FabricDeliveryOrder UpdateFinish(int nFDOID, bool bIsFinish, Int64 nUserID)
        {
            FabricDeliveryOrder oFabricDeliveryOrder = new FabricDeliveryOrder();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = FabricDeliveryOrderDA.UpdateFinish(tc, nFDOID, bIsFinish, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricDeliveryOrder = new FabricDeliveryOrder();
                    oFabricDeliveryOrder = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricDeliveryOrder.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricDeliveryOrder;
        }               

        public FabricDeliveryOrder UpdateFDOStatus(int nFDOID,int nStatus,string sDeliveryPoint, Int64 nUserId)
        {
            FabricDeliveryOrder oFDO = new FabricDeliveryOrder();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryOrderDA.UpdateFDOStatus(tc, nFDOID, nStatus,sDeliveryPoint, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDO = new FabricDeliveryOrder();
                oFDO.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }

            return oFDO;
        }
        public FabricDeliveryOrder Get(int nFDOID, Int64 nUserId)
        {
            FabricDeliveryOrder oFDO = new FabricDeliveryOrder();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryOrderDA.Get(tc, nFDOID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDO = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDO = new FabricDeliveryOrder();
                oFDO.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFDO;
        }
        public List<FabricDeliveryOrder> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricDeliveryOrder> oFDOs = new List<FabricDeliveryOrder>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryOrderDA.Gets(tc, sSQL, nUserId);
                oFDOs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDOs = new List<FabricDeliveryOrder>();
                FabricDeliveryOrder oFDO1 = new FabricDeliveryOrder();
                oFDO1.ErrorMessage = e.Message;
                oFDOs.Add(oFDO1);
                #endregion
            }

            return oFDOs;
        }
        #endregion
    }
}
