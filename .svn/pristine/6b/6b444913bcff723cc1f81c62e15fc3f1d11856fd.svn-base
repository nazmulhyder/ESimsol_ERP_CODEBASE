using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 


namespace ESimSol.Services.Services
{
    public class OrderRecapService : MarshalByRefObject, IOrderRecapService
    {
        #region Private functions and declaration
        OrderRecap _oOrderRecap = new OrderRecap();
        private OrderRecap MapObject(NullHandler oReader)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            oOrderRecap.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oOrderRecap.OrderRecapLogID = oReader.GetInt32("OrderRecapLogID");
            oOrderRecap.BUID = oReader.GetInt32("BUID");  
            oOrderRecap.SLNo = oReader.GetString("SLNo");
            oOrderRecap.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oOrderRecap.OrderRecapStatusInInt = oReader.GetInt32("OrderRecapStatus");
            oOrderRecap.OrderRecapStatus = (EnumOrderRecapStatus)oReader.GetInt32("OrderRecapStatus");
            oOrderRecap.BusinessSessionID = oReader.GetInt32("BusinessSessionID");
            oOrderRecap.SessionName = oReader.GetString("SessionName");
            oOrderRecap.OrderType = (EnumOrderType)oReader.GetInt32("OrderType");
            oOrderRecap.OrderTypeInInt = oReader.GetInt32("OrderType");
            oOrderRecap.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oOrderRecap.ProductID = oReader.GetInt32("ProductID");
            oOrderRecap.AgentID = oReader.GetInt32("AgentID");
            oOrderRecap.ApproveBy = oReader.GetInt32("ApproveBy");
            oOrderRecap.ApproveDate = oReader.GetDateTime("ApproveDate");
            oOrderRecap.BuyerContactPersonID = oReader.GetInt32("BuyerContactPersonID");
            oOrderRecap.BuyerID = oReader.GetInt32("BuyerID");
            oOrderRecap.CollectionNo = oReader.GetString("CollectionNo");
            oOrderRecap.Description = oReader.GetString("Description");
            oOrderRecap.FabricID = oReader.GetInt32("FabricID");
            oOrderRecap.Incoterms = (EnumIncoterms)oReader.GetInt32("Incoterms");
            oOrderRecap.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oOrderRecap.OrderDate = oReader.GetDateTime("OrderDate");
            oOrderRecap.CurrencyID = oReader.GetInt32("CurrencyID");
            oOrderRecap.ProductionFactoryID = oReader.GetInt32("ProductionFactoryID");
            oOrderRecap.FactoryName = oReader.GetString("FactoryName");
            oOrderRecap.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oOrderRecap.BoardDate = oReader.GetDateTime("BoardDate");
            oOrderRecap.TransportType = (EnumTransportType)oReader.GetInt32("TransportType");
            oOrderRecap.AssortmentType = (EnumAssortmentType)oReader.GetInt32("AssortmentType");
            oOrderRecap.AssortmentTypeInt = oReader.GetInt32("AssortmentType");
            oOrderRecap.IsShippedOut = oReader.GetBoolean("IsShippedOut");
            oOrderRecap.StyleNo = oReader.GetString("StyleNo");
            oOrderRecap.BuyerName = oReader.GetString("BuyerName");
            oOrderRecap.BrandName = oReader.GetString("BrandName");
            oOrderRecap.YetToShipmentQty = oReader.GetDouble("YetToShipmentQty");
            oOrderRecap.AlreadyShipmentQty = oReader.GetDouble("AlreadyShipmentQty");
            oOrderRecap.YetToInvoicety = oReader.GetDouble("YetToInvoicety");
            
            oOrderRecap.MerchandiserName = oReader.GetString("MerchandiserName");
            oOrderRecap.ColorRange = oReader.GetString("ColorRange");
            oOrderRecap.SizeRange = oReader.GetString("SizeRange");
            oOrderRecap.ProductName = oReader.GetString("ProductName");
            oOrderRecap.FabricName = oReader.GetString("FabricName");
            oOrderRecap.BuyerContactPerson = oReader.GetString("BuyerContactPerson");
            oOrderRecap.Amount = oReader.GetDouble("Amount");
            oOrderRecap.UnitPrice = oReader.GetDouble("UnitPrice");
            oOrderRecap.TotalQuantity = oReader.GetDouble("TotalQuantity");
            oOrderRecap.AgentName = oReader.GetString("AgentName");
            oOrderRecap.ApproveByName = oReader.GetString("ApproveByName");
            oOrderRecap.CurrencyName = oReader.GetString("CurrencyName");
            oOrderRecap.GG = oReader.GetString("GG");
            oOrderRecap.Count = oReader.GetString("Count");
            oOrderRecap.SpecialFinish = oReader.GetString("SpecialFinish");
            oOrderRecap.Weight = oReader.GetString("Weight");
            oOrderRecap.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oOrderRecap.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oOrderRecap.IsActive = oReader.GetBoolean("IsActive");
            oOrderRecap.CMValue = oReader.GetDouble("CMValue");
            oOrderRecap.MachineQty = oReader.GetInt32("MachineQty");
            oOrderRecap.PIAttachQty = oReader.GetDouble("PIAttachQty");
            oOrderRecap.ORPackingPolicyCount = oReader.GetInt32("ORPackingPolicyCount");
            oOrderRecap.Dept = oReader.GetInt32("Dept");
            oOrderRecap.DeptName = oReader.GetString("DeptName");
            oOrderRecap.TSType = (EnumTSType)oReader.GetInt32("TSType");
            oOrderRecap.DeliveryTerm = oReader.GetString("DeliveryTerm");
            oOrderRecap.PaymentTerm = oReader.GetString("PaymentTerm");
            oOrderRecap.RequiredSample = oReader.GetString("RequiredSample");
            oOrderRecap.PackingInstruction = oReader.GetString("PackingInstruction");
            oOrderRecap.Assortment = oReader.GetString("Assortment");
            oOrderRecap.FactoryAddress = oReader.GetString("FactoryAddress");
            oOrderRecap.KnittingPattern = oReader.GetInt32("KnittingPattern");
            oOrderRecap.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oOrderRecap.StyleDescription = oReader.GetString("StyleDescription");
            oOrderRecap.YarnRequired  = oReader.GetDouble("YarnRequired");
            oOrderRecap.LCValue = oReader.GetDouble("LCValue");
            oOrderRecap.DBDate = oReader.GetDateTime("DBDate");
            oOrderRecap.LocalYarnSupplierID = oReader.GetInt32("LocalYarnSupplierID");
            oOrderRecap.ImportYarnSupplierID = oReader.GetInt32("ImportYarnSupplierID");
            oOrderRecap.CommercialRemarks = oReader.GetString("CommercialRemarks");
            oOrderRecap.LocalYarnSupplierName = oReader.GetString("LocalYarnSupplierName");
            oOrderRecap.ImportYarnSupplierName = oReader.GetString("ImportYarnSupplierName");
            oOrderRecap.BarCodeNo = oReader.GetString("BarCodeNo");
            oOrderRecap.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oOrderRecap.UnitName = oReader.GetString("UnitName");
            oOrderRecap.ONSQty = oReader.GetDouble("ONSQty");
            oOrderRecap.FactoryShipmentDate = oReader.GetDateTime("FactoryShipmentDate");
            oOrderRecap.FabricUnitType = (EnumUniteType)oReader.GetInt32("FabricUnitType");
            oOrderRecap.CartonQty = oReader.GetDouble("CartonQty");
            oOrderRecap.ShipmentCTNQty = oReader.GetDouble("ShipmentCTNQty");
            oOrderRecap.QtyPerCarton = oReader.GetDouble("QtyPerCarton");
            oOrderRecap.YetToScheduleQty = oReader.GetDouble("YetToScheduleQty");
            oOrderRecap.ProductionPlanDate = oReader.GetDateTime("ProductionPlanDate");
            oOrderRecap.TAPIssueDate = oReader.GetDateTime("TAPIssueDate");
            oOrderRecap.ClassName = oReader.GetString("ClassName");
            oOrderRecap.SubClassName = oReader.GetString("SubClassName");
            oOrderRecap.Wash = oReader.GetString("Wash");
            oOrderRecap.BUShortName = oReader.GetString("BUShortName");
            oOrderRecap.FabCode = oReader.GetString("FabCode");
            oOrderRecap.IsORExistInPlan = oReader.GetBoolean("IsORExistInPlan");
            oOrderRecap.YetToQcQty = oReader.GetDouble("YetToQcQty");
            oOrderRecap.AlreadyQCQty = oReader.GetDouble("AlreadyQCQty");
            oOrderRecap.PAMID = oReader.GetInt32("PAMID");
            oOrderRecap.PAMNo = oReader.GetString("PAMNo");
            return oOrderRecap;
        }

        private OrderRecap CreateObject(NullHandler oReader)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            oOrderRecap = MapObject(oReader);
            return oOrderRecap;
        }

        private List<OrderRecap> CreateObjects(IDataReader oReader)
        {
            List<OrderRecap> oOrderRecaps = new List<OrderRecap>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderRecap oItem = CreateObject(oHandler);
                oOrderRecaps.Add(oItem);
            }
            return oOrderRecaps;
        }

        #endregion

        #region Interface implementation
        public OrderRecapService() { }

        public OrderRecap Save(OrderRecap oOrderRecap, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
                List<ORAssortment> oORAssortments = new List<ORAssortment>();
                List<ORBarCode> oORBarCodes = new List<ORBarCode>();
                List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
    
                OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                ORAssortment oORAssortment = new ORAssortment();
                ORBarCode oORBarCode = new ORBarCode();
                oOrderRecapDetails = oOrderRecap.OrderRecapDetails;
                oORAssortments = oOrderRecap.ORAssortments;
                oORBarCodes = oOrderRecap.ORBarCodes;
                oOrderRecapYarns = oOrderRecap.OrderRecapYarns;
                
                string sOrderRecapDetailIDs = ""; string sORAssortmentIDs = ""; string sORBarCodeIDs = ""; string sOrderRecapYarnIDs = ""; 

               
                #region Sale Order Part
                IDataReader reader;
                oOrderRecap.OrderType = EnumOrderType.BulkOrder;
                if (oOrderRecap.OrderRecapID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Add);
                    reader = OrderRecapDA.InsertUpdate(tc, oOrderRecap, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Edit);
                    reader = OrderRecapDA.InsertUpdate(tc, oOrderRecap, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = new OrderRecap();
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Sale Order Detail Part
                foreach (OrderRecapDetail oItem in oOrderRecapDetails)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.OrderRecapDetailID <= 0)
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sOrderRecapDetailIDs = sOrderRecapDetailIDs + oReaderDetail.GetString("OrderRecapDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sOrderRecapDetailIDs.Length > 0)
                {
                    sOrderRecapDetailIDs = sOrderRecapDetailIDs.Remove(sOrderRecapDetailIDs.Length - 1, 1);
                }
                oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.OrderRecapID = oOrderRecap.OrderRecapID;
                OrderRecapDetailDA.Delete(tc, oOrderRecapDetail, EnumDBOperation.Delete, nUserId, sOrderRecapDetailIDs);
                #endregion

                #region Order Recap Yarn Part
                foreach (OrderRecapYarn oOrderRecapYarn in oOrderRecapYarns)
                {
                    //if (oOrderRecap.FabricID != oOrderRecapYarn.YarnID)
                    //{
                        IDataReader readerRecapYar;
                        oOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                        oOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                        if (oOrderRecapYarn.OrderRecapYarnID <= 0)
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Insert, nUserId,"");
                        }
                        else
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderRecapYarn = new NullHandler(readerRecapYar);
                        if (readerRecapYar.Read())
                        {
                            sOrderRecapYarnIDs = sOrderRecapYarnIDs + oReaderRecapYarn.GetString("OrderRecapYarnID") + ",";
                        }
                        readerRecapYar.Close();
                    //}
                }
                if (sOrderRecapYarnIDs.Length > 0)
                {
                    sOrderRecapYarnIDs = sOrderRecapYarnIDs.Remove(sOrderRecapYarnIDs.Length - 1, 1);
                }
                OrderRecapYarn oTempOrderRecapYarn = new OrderRecapYarn();
                oTempOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                oTempOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                OrderRecapYarnDA.Delete(tc, oTempOrderRecapYarn, EnumDBOperation.Delete, nUserId, sOrderRecapYarnIDs);
                #endregion

                #region Color Assortment  Part
                foreach (ORAssortment oItem in oORAssortments)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.ORAssortmentID <= 0)
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sORAssortmentIDs = sORAssortmentIDs + oReaderDetail.GetString("ORAssortmentID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sORAssortmentIDs.Length > 0)
                {
                    sORAssortmentIDs = sORAssortmentIDs.Remove(sORAssortmentIDs.Length - 1, 1);
                }
                oORAssortment = new ORAssortment();
                oORAssortment.OrderRecapID = oOrderRecap.OrderRecapID;
                ORAssortmentDA.Delete(tc, oORAssortment, EnumDBOperation.Delete, nUserId, sORAssortmentIDs);
                #endregion

                #region BarCode  Part
                foreach (ORBarCode oItem in oORBarCodes)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.ORBarCodeID <= 0)
                    {
                        readerdetail = ORBarCodeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = ORBarCodeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sORBarCodeIDs = sORBarCodeIDs + oReaderDetail.GetString("ORBarCodeID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sORBarCodeIDs.Length > 0)
                {
                    sORBarCodeIDs = sORBarCodeIDs.Remove(sORBarCodeIDs.Length - 1, 1);
                }
                oORBarCode = new ORBarCode();
                oORBarCode.OrderRecapID = oOrderRecap.OrderRecapID;
                ORBarCodeDA.Delete(tc, oORBarCode, EnumDBOperation.Delete, nUserId, sORBarCodeIDs);
                #endregion

                #region Get Sale Order for Actual Order Qty
                reader = OrderRecapDA.Get(tc, oOrderRecap.OrderRecapID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = new OrderRecap();
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oOrderRecap.ErrorMessage = Message;
                #endregion
            }
            return oOrderRecap;
        }

     

        public OrderRecap AcceptRevise(OrderRecap oOrderRecap, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
                List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
                OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                ORAssortment oORAssortment = new ORAssortment();
                List<ORAssortment> oORAssortments = new List<ORAssortment>();
                List<ORBarCode> oORBarCodes = new List<ORBarCode>();
                oOrderRecapDetails = oOrderRecap.OrderRecapDetails;
                oOrderRecapYarns = oOrderRecap.OrderRecapYarns;
                oORAssortments = oOrderRecap.ORAssortments;
                oORBarCodes = oOrderRecap.ORBarCodes;
                string sOrderRecapDetailIDs = ""; string sOrderRecapYarnIDs = ""; string sORAssortmentIDs = ""; string sOrderRecapDetailsIDsInfo = "", sORBarCodeIDs = "" ;
                foreach(OrderRecapDetail oItem in oOrderRecapDetails)
                {
                    sOrderRecapDetailsIDsInfo += oItem.ColorID + "," + oItem.SizeID + "," + oItem.Quantity + "~";
                }
               
                #region Sale Order Part
                IDataReader reader;
                oOrderRecap.OrderType = EnumOrderType.BulkOrder;
                reader = OrderRecapDA.AcceptRevise(tc, oOrderRecap, sOrderRecapDetailsIDsInfo,  nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = new OrderRecap();
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Sale Order Detail Part
                foreach (OrderRecapDetail oItem in oOrderRecapDetails)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.OrderRecapDetailID <= 0)
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sOrderRecapDetailIDs = sOrderRecapDetailIDs + oReaderDetail.GetString("OrderRecapDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sOrderRecapDetailIDs.Length > 0)
                {
                    sOrderRecapDetailIDs = sOrderRecapDetailIDs.Remove(sOrderRecapDetailIDs.Length - 1, 1);
                }
                oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.OrderRecapID = oOrderRecap.OrderRecapID;
                OrderRecapDetailDA.Delete(tc, oOrderRecapDetail, EnumDBOperation.Delete, nUserId, sOrderRecapDetailIDs);
                #endregion

                #region Color Assortment  Part
                foreach (ORAssortment oItem in oORAssortments)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.ORAssortmentID <= 0)
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sORAssortmentIDs = sORAssortmentIDs + oReaderDetail.GetString("ORAssortmentID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sORAssortmentIDs.Length > 0)
                {
                    sORAssortmentIDs = sORAssortmentIDs.Remove(sORAssortmentIDs.Length - 1, 1);
                }
                oORAssortment = new ORAssortment();
                oORAssortment.OrderRecapID = oOrderRecap.OrderRecapID;
                ORAssortmentDA.Delete(tc, oORAssortment, EnumDBOperation.Delete, nUserId, sORAssortmentIDs);
                #endregion

                #region Order Recap Yarn Part
                foreach (OrderRecapYarn oOrderRecapYarn in oOrderRecapYarns)
                {
                    if (oOrderRecap.FabricID != oOrderRecapYarn.YarnID)
                    {
                        IDataReader readerRecapYar;
                        oOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                        oOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                        if (oOrderRecapYarn.OrderRecapYarnID <= 0)
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Insert, nUserId,"");
                        }
                        else
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Update, nUserId,"");
                        }
                        NullHandler oReaderRecapYarn = new NullHandler(readerRecapYar);
                        if (readerRecapYar.Read())
                        {
                            sOrderRecapYarnIDs = sOrderRecapYarnIDs + oReaderRecapYarn.GetString("OrderRecapYarnID") + ",";
                        }
                        readerRecapYar.Close();
                    }
                }
                if (sOrderRecapYarnIDs.Length > 0)
                {
                    sOrderRecapYarnIDs = sOrderRecapYarnIDs.Remove(sOrderRecapYarnIDs.Length - 1, 1);
                }
                OrderRecapYarn oTempOrderRecapYarn = new OrderRecapYarn();
                oTempOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                oTempOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                OrderRecapYarnDA.Delete(tc, oTempOrderRecapYarn, EnumDBOperation.Delete, nUserId, sOrderRecapYarnIDs);
                #endregion

                #region BarCode  Part
                foreach (ORBarCode oItem in oORBarCodes)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.ORBarCodeID <= 0)
                    {
                        readerdetail = ORBarCodeDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = ORBarCodeDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sORBarCodeIDs = sORBarCodeIDs + oReaderDetail.GetString("ORBarCodeID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sORBarCodeIDs.Length > 0)
                {
                    sORBarCodeIDs = sORBarCodeIDs.Remove(sORBarCodeIDs.Length - 1, 1);
                }
                ORBarCode oORBarCode = new ORBarCode();
                oORBarCode.OrderRecapID = oOrderRecap.OrderRecapID;
                ORBarCodeDA.Delete(tc, oORBarCode, EnumDBOperation.Delete, nUserId, sORBarCodeIDs);
                #endregion

                #region Get Sale Order for Actual Order Qty
                reader = OrderRecapDA.Get(tc, oOrderRecap.OrderRecapID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = new OrderRecap();
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oOrderRecap.ErrorMessage = Message;
                #endregion
            }
            return oOrderRecap;
        }
        public OrderRecap ActiveInActive(OrderRecap oOrderRecap, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderRecapDA.ActiveInActive(tc, oOrderRecap, nUserId);
                IDataReader reader = OrderRecapDA.Get(tc, oOrderRecap.OrderRecapID);
                oOrderRecap = new OrderRecap();
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oOrderRecap.ErrorMessage = Message;
                #endregion
            }
            return oOrderRecap;
        }

        public OrderRecap ShippedUnShipped(OrderRecap oOrderRecap, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderRecapDA.ShippedUnShipped(tc, oOrderRecap, nUserId);
                IDataReader reader = OrderRecapDA.Get(tc, oOrderRecap.OrderRecapID);
                oOrderRecap = new OrderRecap();
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oOrderRecap.ErrorMessage = Message;
                #endregion
            }
            return oOrderRecap;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                OrderRecap oOrderRecap = new OrderRecap();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "OrderRecap", id);
                oOrderRecap.OrderRecapID = id;
                OrderRecapDA.Delete(tc, oOrderRecap, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                #endregion
                return e.Message.Split('!')[0];
            }
            return "Data delete successfully";
        }

        public OrderRecap Get(int id, Int64 nUserId)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OrderRecapDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                #endregion
            }

            return oOrderRecap;
        }


        public OrderRecap GetByLog(int id, Int64 nUserId)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OrderRecapDA.GetByLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                #endregion
            }

            return oOrderRecap;
        }
        public OrderRecap UpdateCMValue(int id, double nCMValue, Int64 nUserId)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            int nSCApprovedBy= 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                //IDataReader readerCMValue ;
                //readerCMValue = OrderRecapDA.GetSContractCM(tc,id);
                //NullHandler oReaderCMValue = new NullHandler(readerCMValue);
                //if (readerCMValue.Read())
                //{
                //    nSCApprovedBy = oReaderCMValue.GetInt32("ApprovedBy");
                //}
                //readerCMValue.Close();
                //if (nSCApprovedBy != 0)
                //{
                //        oOrderRecap = new OrderRecap();
                //        oOrderRecap.ErrorMessage = "Sales Contract Already Approved for Selected Order Recap";
                //        return oOrderRecap;
                //}
                //else
                //{

                    OrderRecapDA.UpdateCMValue(tc, id, nCMValue);
                    IDataReader reader = OrderRecapDA.Get(tc, id);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oOrderRecap = CreateObject(oReader);
                    }
                    reader.Close();
                //}
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                #endregion
            }

            return oOrderRecap;
        }


        public OrderRecap UpdateInfo(OrderRecap oOrderRecap, Int64 nUserId)
        {
            OrderRecap oNewOrderRecap = new OrderRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                OrderRecapDA.UpdateInfo(tc, oOrderRecap, nUserId);
                 IDataReader reader = OrderRecapDA.Get(tc, oOrderRecap.OrderRecapID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oNewOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oNewOrderRecap = new OrderRecap();
                oNewOrderRecap.ErrorMessage = e.Message;
                #endregion
            }

            return oNewOrderRecap;
        }

        public List<OrderRecap> Gets(Int64 nUserID)
        {
            List<OrderRecap> oOrderRecaps = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDA.Gets(tc);
                oOrderRecaps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                OrderRecap oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                oOrderRecaps.Add(oOrderRecap);
                #endregion
            }

            return oOrderRecaps;
        }
        public List<OrderRecap> GetsByBUWithOrderType(int nBUIID, string sOTypes, Int64 nUserID)
        {
            List<OrderRecap> oOrderRecaps = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDA.GetsByBUWithOrderType(tc, nBUIID, sOTypes);
                oOrderRecaps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                OrderRecap oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                oOrderRecaps.Add(oOrderRecap);
                #endregion
            }

            return oOrderRecaps;
        }

        public string PickNewColor(List<TechnicalSheetColor> oTechnicalSheetColors, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                foreach (TechnicalSheetColor oItem in oTechnicalSheetColors)
                {
                    reader = TechnicalSheetColorDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    NullHandler oReaderDetail = new NullHandler(reader);
                    if (reader.Read())
                    {
                    }
                    reader.Close();
                }
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
            return "Data Save Successfully";
        }
              
        public List<OrderRecap> Gets(string sSQL, Int64 nUserId)
        {
            List<OrderRecap> oOrderRecap = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapDA.Gets(tc, sSQL);
                oOrderRecap = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecap", e);
                #endregion
            }

            return oOrderRecap;
        }

        public OrderRecap Gets_byTechnicalSheet(int nTechnicalSheetID, int nOrderTypeId, Int64 nUserId)
        {
            OrderRecap oOrderRecap = new OrderRecap();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = OrderRecapDA.Gets_byTechnicalSheet(tc, nTechnicalSheetID, nOrderTypeId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oOrderRecap = new OrderRecap();
                oOrderRecap.ErrorMessage = e.Message;
                #endregion
            }

            return oOrderRecap;
        }
        
        public OrderRecap ChangeStatus(OrderRecap oOrderRecap, Int64 nUserID)
        {
            TransactionContext tc = null;

             _oOrderRecap = new OrderRecap();
             ApprovalRequest oApprovalRequest = new ApprovalRequest();
             oApprovalRequest = oOrderRecap.ApprovalRequest;
             ReviseRequest oReviseRequest = new ReviseRequest();
             oReviseRequest = oOrderRecap.ReviseRequest;
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oOrderRecap.ActionType == EnumActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.OrderRecap, EnumRoleOperationType.Approved);
                }

                reader = OrderRecapDA.ChangeStatus(tc, oOrderRecap, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    _oOrderRecap = CreateObject(oReader);
                }
                reader.Close();

                if (oOrderRecap.OrderRecapStatus == EnumOrderRecapStatus.RequestForApproval)
                {
                    IDataReader ApprovalRequestreader;
                    ApprovalRequestreader = ApprovalRequestDA.InsertUpdate(tc, oApprovalRequest, EnumDBOperation.Insert, nUserID);
                    if (ApprovalRequestreader.Read())
                    {

                    }
                    ApprovalRequestreader.Close();
                    
                }
                if (oOrderRecap.OrderRecapStatus == EnumOrderRecapStatus.Request_For_Revise)
                {
                    IDataReader ReviseRequestreader;
                    ReviseRequestreader = ReviseRequestDA.InsertUpdate(tc, oReviseRequest, EnumDBOperation.Insert, nUserID);
                    if (ReviseRequestreader.Read())
                    {

                    }
                    ReviseRequestreader.Close();

                }

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
                _oOrderRecap.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save OrderRecapDetail. Because of " + e.Message, e);
                #endregion
            }
            return _oOrderRecap;
        }

        public OrderRecap ApprovedOrderRecap(OrderRecap oOrderRecap, Int64 nUserId)
        {
            TransactionContext tc = null;          
            try
            {
                tc = TransactionContext.Begin(true);                
                List<OrderRecapDetail> oOrderRecapDetails = new List<OrderRecapDetail>();
                List<OrderRecapYarn> oOrderRecapYarns = new List<OrderRecapYarn>();
                OrderRecapDetail oOrderRecapDetail = new OrderRecapDetail();
                List<ORAssortment> oORAssortments = new List<ORAssortment>();
                ORAssortment oORAssortment = new ORAssortment();
                oOrderRecapDetails = oOrderRecap.OrderRecapDetails;
                oOrderRecapYarns = oOrderRecap.OrderRecapYarns;
                oORAssortments = oOrderRecap.ORAssortments;
                string sOrderRecapDetailIDs = ""; string sOrderRecapYarnIDs = ""; string sORAssortmentIDs = "";

                #region Check Revise Qty
                if (oOrderRecap.OrderRecapID > 0)
                {
                    double nTotalOrderQty = 0; double nAlreadyPlanQty = 0;
                    foreach (OrderRecapDetail oTemp in oOrderRecapDetails)
                    {
                        nTotalOrderQty = nTotalOrderQty + oTemp.Quantity;
                    }
                    nAlreadyPlanQty = OrderRecapDA.GetProductionPlanQty(tc, oOrderRecap.OrderRecapID);
                    if (nTotalOrderQty < nAlreadyPlanQty)
                    {
                        throw new Exception("Your Selected Order Recap Already Contain Production Plan?\nYour Revise Qty must be Greater  than or equal " + Global.MillionFormat(nAlreadyPlanQty, 0));
                    }
                }
                #endregion

                #region Sale Order Part
                IDataReader reader;                
                if (oOrderRecap.OrderRecapID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Add);
                    reader = OrderRecapDA.InsertUpdate(tc, oOrderRecap, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Edit);
                    reader = OrderRecapDA.InsertUpdate(tc, oOrderRecap, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = new OrderRecap();
                    oOrderRecap = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Sale Order Detail Part
                foreach (OrderRecapDetail oItem in oOrderRecapDetails)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.OrderRecapDetailID <= 0)
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = OrderRecapDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sOrderRecapDetailIDs = sOrderRecapDetailIDs + oReaderDetail.GetString("OrderRecapDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sOrderRecapDetailIDs.Length > 0)
                {
                    sOrderRecapDetailIDs = sOrderRecapDetailIDs.Remove(sOrderRecapDetailIDs.Length - 1, 1);
                }
                oOrderRecapDetail = new OrderRecapDetail();
                oOrderRecapDetail.OrderRecapID = oOrderRecap.OrderRecapID;
                OrderRecapDetailDA.Delete(tc, oOrderRecapDetail, EnumDBOperation.Delete, nUserId, sOrderRecapDetailIDs);
                #endregion

                #region Order Recap Yarn Part
                foreach (OrderRecapYarn oOrderRecapYarn in oOrderRecapYarns)
                {
                    if (oOrderRecap.FabricID != oOrderRecapYarn.YarnID)
                    {
                        IDataReader readerRecapYar;
                        oOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                        oOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                        if (oOrderRecapYarn.OrderRecapYarnID <= 0)
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Insert, nUserId,"");
                        }
                        else
                        {
                            readerRecapYar = OrderRecapYarnDA.InsertUpdate(tc, oOrderRecapYarn, EnumDBOperation.Update, nUserId,"");
                        }
                        NullHandler oReaderRecapYarn = new NullHandler(readerRecapYar);
                        if (readerRecapYar.Read())
                        {
                            sOrderRecapYarnIDs = sOrderRecapYarnIDs + oReaderRecapYarn.GetString("OrderRecapYarnID") + ",";
                        }
                        readerRecapYar.Close();
                    }
                }
                if (sOrderRecapYarnIDs.Length > 0)
                {
                    sOrderRecapYarnIDs = sOrderRecapYarnIDs.Remove(sOrderRecapYarnIDs.Length - 1, 1);
                }
                OrderRecapYarn oTempOrderRecapYarn = new OrderRecapYarn();
                oTempOrderRecapYarn.RefObjectID = oOrderRecap.OrderRecapID;
                oTempOrderRecapYarn.RefType = EnumRecapRefType.OrderRecap;
                OrderRecapYarnDA.Delete(tc, oTempOrderRecapYarn, EnumDBOperation.Delete, nUserId, sOrderRecapYarnIDs);
                #endregion

                #region Color Assortment  Part
                foreach (ORAssortment oItem in oORAssortments)
                {
                    IDataReader readerdetail;
                    oItem.OrderRecapID = oOrderRecap.OrderRecapID;
                    if (oItem.ORAssortmentID <= 0)
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                    }
                    else
                    {
                        readerdetail = ORAssortmentDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sORAssortmentIDs = sORAssortmentIDs + oReaderDetail.GetString("ORAssortmentID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sORAssortmentIDs.Length > 0)
                {
                    sORAssortmentIDs = sORAssortmentIDs.Remove(sORAssortmentIDs.Length - 1, 1);
                }
                oORAssortment = new ORAssortment();
                oORAssortment.OrderRecapID = oOrderRecap.OrderRecapID;
                ORAssortmentDA.Delete(tc, oORAssortment, EnumDBOperation.Delete, nUserId, sORAssortmentIDs);
                #endregion

                #region Approved OrderRecap
                if (oOrderRecap.ActionType == EnumActionType.Approve)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.OrderRecap, EnumRoleOperationType.Approved);
                }
                oOrderRecap.ActionType = EnumActionType.Approve;
                reader = OrderRecapDA.ChangeStatus(tc, oOrderRecap, EnumDBOperation.Insert, nUserId);
                NullHandler oTempReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oOrderRecap = CreateObject(oTempReader);
                }
                reader.Close();
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
                oOrderRecap.ErrorMessage = Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save OrderRecapDetail. Because of " + e.Message, e);
                #endregion
            }
            return oOrderRecap;
        }

        #endregion
    }
}
