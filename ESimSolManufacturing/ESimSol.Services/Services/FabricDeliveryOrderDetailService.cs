using System;
using System.Collections.Generic;
using System.Linq;
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
    public class FabricDeliveryOrderDetailService : MarshalByRefObject, IFabricDeliveryOrderDetailService
    {
        #region Private functions and declaration
        private FabricDeliveryOrderDetail MapObject(NullHandler oReader)
        {
            FabricDeliveryOrderDetail oFDODetail = new FabricDeliveryOrderDetail();
            oFDODetail.FDODID = oReader.GetInt32("FDODID");
            oFDODetail.FDOID = oReader.GetInt32("FDOID");
            oFDODetail.FabricID = oReader.GetInt32("FabricID");
            oFDODetail.ProductID = oReader.GetInt32("ProductID");
            oFDODetail.Qty = oReader.GetDouble("Qty");
            oFDODetail.MUID = oReader.GetInt32("MUID");
            oFDODetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFDODetail.FabricNo = oReader.GetString("FabricNo");
            oFDODetail.DONo = oReader.GetString("DONo");
            oFDODetail.Construction = oReader.GetString("Construction");
            oFDODetail.ColorInfo = oReader.GetString("ColorInfo");
            oFDODetail.MUName = oReader.GetString("MUName");
            oFDODetail.ProductCode = oReader.GetString("ProductCode");
            oFDODetail.ProductName = oReader.GetString("ProductName");
            //oFDODetail.FabricDeliveryChallanQty = oReader.GetDouble("FabricDeliveryChallanQty");
            oFDODetail.FEOID = oReader.GetInt32("FEOID");
            oFDODetail.ExportPIID = oReader.GetInt32("ExportPIID");
            oFDODetail.ExportPIDetailID = oReader.GetInt32("ExportPIDetailID");
            oFDODetail.FabricDesignName = oReader.GetString("FabricDesignName");
            oFDODetail.FinishType = oReader.GetString("FinishTypeName");
            oFDODetail.ProcessType = oReader.GetString("ProcessTypeName");
            oFDODetail.FabricWeave = oReader.GetString("FabricWeaveName");
            oFDODetail.Shrinkage = oReader.GetString("Shrinkage");

            
            oFDODetail.OrderType = (EnumFabricRequestType)oReader.GetInt32("OrderType");
            oFDODetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFDODetail.FEONo = oReader.GetString("SCNoFull");
            //oFDODetail.YetToDeliveryQty = oReader.GetDouble("YetToDeliveryQty");
            oFDODetail.LCNo = oReader.GetString("LCNo");
            oFDODetail.StyleNo = oReader.GetString("StyleNo");
            oFDODetail.BuyerRef = oReader.GetString("BuyerReference");
            oFDODetail.PINo = oReader.GetString("PINo");
            oFDODetail.DODate = oReader.GetDateTime("DODate");
            oFDODetail.Currency = oReader.GetString("Currency");
            oFDODetail.CurrencyID = oReader.GetInt32("PICurrency");
            oFDODetail.Qty_P0 = oReader.GetDouble("Qty_P0");
            oFDODetail.Qty_DC = oReader.GetDouble("Qty_DC");
            oFDODetail.Qty_RC = oReader.GetDouble("Qty_RC");
            oFDODetail.Weight = oReader.GetString("Weight");
            oFDODetail.FabricProductName = oReader.GetString("FabricProductName");
            oFDODetail.FabricWidth = oReader.GetString("FabricWidth");
            oFDODetail.ContractorName = oReader.GetString("ContractorName");
            oFDODetail.MKTPerson = oReader.GetString("MKTPerson");
            oFDODetail.BuyerName = oReader.GetString("BuyerName");
            oFDODetail.ExeNo = oReader.GetString("ExeNo");
            oFDODetail.ContractorID = oReader.GetInt32("ContractorID");
            oFDODetail.BuyerID = oReader.GetInt32("BuyerID");
            oFDODetail.DOPriceType = (EnumDOPriceType)oReader.GetInt32("DOPriceType");
            return oFDODetail;
        }

        private FabricDeliveryOrderDetail CreateObject(NullHandler oReader)
        {
            FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
            oFabricDeliveryOrderDetail = MapObject(oReader);
            return oFabricDeliveryOrderDetail;
        }

        private List<FabricDeliveryOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricDeliveryOrderDetail oItem = CreateObject(oHandler);
                oFabricDeliveryOrderDetails.Add(oItem);
            }
            return oFabricDeliveryOrderDetails;
        }
        #endregion

        #region Interface implementatio
        public FabricDeliveryOrderDetailService() { }

        public FabricDeliveryOrderDetail IUD(FabricDeliveryOrderDetail oFDODetail, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricDeliveryOrder oFDO = new FabricDeliveryOrder();
            List<FabricDeliveryOrderDetail> oFDODs = new List<FabricDeliveryOrderDetail>();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oFDODetail.FDOID == 0)
                {

                    if (oFDODetail.FDO != null)
                    {
                        oFDODetail.FDO.DeliveryPoint = (oFDODetail.FDO.DeliveryPoint == null) ? "" : oFDODetail.FDO.DeliveryPoint;
                        oFDODetail.FDO.Note = (oFDODetail.FDO.Note == null) ? "" : oFDODetail.FDO.Note;
                        oFDODetail.FDO.FDOType = (EnumDOType)oFDODetail.FDO.FDOTypeInInt;
                        reader = FabricDeliveryOrderDA.IUD(tc, oFDODetail.FDO, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFDO = FabricDeliveryOrderService.CreateObject(oReader);
                        }
                        reader.Close();
                        oFDODetail.FDOID = oFDO.FDOID;
                    }
                    else { throw new Exception("No fabric delivery order found to save."); }
                }

                if (oFDODetail.FabricDeliveryOrderDetails.Count() > 0)
                {
                    FabricDeliveryOrderDetail oFDOD = new FabricDeliveryOrderDetail();
                    foreach (FabricDeliveryOrderDetail oItem in oFDODetail.FabricDeliveryOrderDetails)
                    {
                        if (oItem.FDODID <= 0)
                        {
                            oItem.FDOID = oFDODetail.FDOID;
                            reader = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId,"");
                        }
                        else
                        {
                            reader = FabricDeliveryOrderDetailDA.IUD(tc, oItem, nDBOperation, nUserId,"");
                        }
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFDOD = new FabricDeliveryOrderDetail();
                            oFDOD = CreateObject(oReader);
                            oFDODs.Add(oFDOD);
                        }
                        reader.Close();
                    }
                }
                else
                {
                    reader = FabricDeliveryOrderDetailDA.IUD(tc, oFDODetail, nDBOperation, nUserId,"");
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFDODetail = new FabricDeliveryOrderDetail();
                        oFDODetail = CreateObject(oReader);
                    }
                    reader.Close();
                }

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFDODetail = new FabricDeliveryOrderDetail();
                    oFDODetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDODetail = new FabricDeliveryOrderDetail();
                oFDODetail.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                oFDODs = new List<FabricDeliveryOrderDetail>();
                oFDO = new FabricDeliveryOrder();
                #endregion
            }
            if (oFDO.FDOID > 0 && oFDODs.Count() > 0)
            {
                oFDO.Qty = (oFDODs.Count() > 0) ? oFDODs.Sum(x => x.Qty) : oFDODetail.Qty;
            }
            oFDODetail.FabricDeliveryOrderDetails = oFDODs;
            oFDODetail.FDO = oFDO;

            return oFDODetail;
        }

               

        public FabricDeliveryOrderDetail Save(FabricDeliveryOrderDetail oFabricDeliveryOrderDetail, Int64 nUserID)
        {
            FabricDeliveryOrder oFabricDeliveryOrder = new FabricDeliveryOrder();
            oFabricDeliveryOrder = oFabricDeliveryOrderDetail.FDO;
            List<FabricDeliveryOrderDetail> oFabricDeliveryOrderDetails = new List<FabricDeliveryOrderDetail>();
            List<FabricDeliveryOrderDetail> oFDODs = new List<FabricDeliveryOrderDetail>();
            oFabricDeliveryOrderDetails = oFabricDeliveryOrderDetail.FabricDeliveryOrderDetails;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                int Count = 0;
                int nFabricDeliveryOrderID = 0;
                #region New Code
                foreach (FabricDeliveryOrderDetail oItem in oFabricDeliveryOrderDetails)
                {
                    Count++;
                    #region FabricDeliveryOrder Part
                    if (oItem.FDO != null)
                    {
                        IDataReader reader;
                        if (oItem.FDO.FDOID <= 0)
                        {
                            reader = FabricDeliveryOrderDA.IUD(tc, oItem.FDO, (int)EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            reader = FabricDeliveryOrderDA.IUD(tc, oItem.FDO, (int)EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFabricDeliveryOrder = new FabricDeliveryOrder();
                            oFabricDeliveryOrder = FabricDeliveryOrderService.CreateObject(oReader);
                            nFabricDeliveryOrderID = oFabricDeliveryOrder.FDOID;
                        }
                        if (oFabricDeliveryOrder.FDOID <= 0)
                        {
                            oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                            oFabricDeliveryOrderDetail.ErrorMessage = "Invalid Fabric Delivery Order";
                            return oFabricDeliveryOrderDetail;
                        }
                        reader.Close();
                    }

                    #endregion

                    if (oFabricDeliveryOrderDetails[0].FDO != null)
                    {
                        oItem.FDOID = nFabricDeliveryOrderID;
                    }

                    IDataReader readerdetail;
                    string sOrderNo = oItem.OrderNo;
                    if (oItem.FDODID <= 0)
                    {
                        oItem.Qty = (oItem.Qty == 0 ? 1 : oItem.Qty);
                        readerdetail = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = FabricDeliveryOrderDetailDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                        oFabricDeliveryOrderDetail = CreateObject(oReaderDetail);
                        //oFabricDeliveryOrderDetail.MaxQty = nMaxQty;
                        //oFabricDeliveryOrderDetail.IsCheckMaxQty = bIsCheckMaxQty;
                        //oFabricDeliveryOrderDetail.OrderNo = sOrderNo;
                        if (Count == 1)
                        {
                            oFabricDeliveryOrderDetail.FDO = oFabricDeliveryOrder;
                        }
                    }
                    oFDODs.Add(oFabricDeliveryOrderDetail);
                    readerdetail.Close();
                }
                #endregion
                oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetail.FabricDeliveryOrderDetails = oFDODs;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricDeliveryOrderDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricDeliveryOrderDetail oFabricDeliveryOrderDetail = new FabricDeliveryOrderDetail();
                oFabricDeliveryOrderDetail.FDODID = id;
                FabricDeliveryOrderDetailDA.Delete(tc, oFabricDeliveryOrderDetail, (int)EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

         public FabricDeliveryOrderDetail Get(int nFEODID, Int64 nUserId)
        {
            FabricDeliveryOrderDetail oFDODetail = new FabricDeliveryOrderDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricDeliveryOrderDetailDA.Get(tc, nFEODID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFDODetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDODetail = new FabricDeliveryOrderDetail();
                oFDODetail.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFDODetail;
        }
        public List<FabricDeliveryOrderDetail> Gets(int nFDOID, Int64 nUserId)
        {
            List<FabricDeliveryOrderDetail> oFDODetails = new List<FabricDeliveryOrderDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryOrderDetailDA.Gets(tc, nFDOID, nUserId);
                oFDODetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDODetails = new List<FabricDeliveryOrderDetail>();
                #endregion
            }

            return oFDODetails;
        }
        public List<FabricDeliveryOrderDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricDeliveryOrderDetail> oFDODetails = new List<FabricDeliveryOrderDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricDeliveryOrderDetailDA.Gets(tc, sSQL, nUserId);
                oFDODetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFDODetails = new List<FabricDeliveryOrderDetail>();
                #endregion
            }

            return oFDODetails;
        }
        #endregion
    }
}