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

    public class OrderRecapSummeryService : MarshalByRefObject, IOrderRecapSummeryService
    {
        #region Private functions and declaration
        private OrderRecapSummery MapObject(NullHandler oReader)
        {
            OrderRecapSummery oOrderRecapSummery = new OrderRecapSummery();
            oOrderRecapSummery.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oOrderRecapSummery.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oOrderRecapSummery.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oOrderRecapSummery.StyleNo = oReader.GetString("StyleNo");
            oOrderRecapSummery.OrderDate = oReader.GetDateTime("OrderDate");
            oOrderRecapSummery.CurrencyID = oReader.GetInt32("CurrencyID");
            oOrderRecapSummery.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oOrderRecapSummery.ShipmentMode = (EnumTransportType)oReader.GetInt32("ShipmentMode");
            oOrderRecapSummery.LCReceivedDate = oReader.GetDateTime("LCReceivedDate");
            oOrderRecapSummery.FactoryName = oReader.GetString("FactoryName");
            oOrderRecapSummery.FabricID = oReader.GetInt32("FabricID");
            oOrderRecapSummery.YarnContent = oReader.GetString("YarnContent");
            oOrderRecapSummery.ProductID = oReader.GetInt32("ProductID");
            oOrderRecapSummery.ProductName = oReader.GetString("ProductName");
            oOrderRecapSummery.Count = oReader.GetString("Count");
            oOrderRecapSummery.GG = oReader.GetString("GG");
            oOrderRecapSummery.PaymentTerm = oReader.GetString("PaymentTerm");
            oOrderRecapSummery.Weight = oReader.GetString("Weight");
            oOrderRecapSummery.Button = oReader.GetString("Button");
            oOrderRecapSummery.Zipper = oReader.GetString("Zipper");
            oOrderRecapSummery.Print = oReader.GetString("Print");
            oOrderRecapSummery.FabricationID = oReader.GetInt32("FabricationID");
            oOrderRecapSummery.Fabrication = oReader.GetString("Fabrication");
            oOrderRecapSummery.Embrodery = oReader.GetString("Embrodery");
            oOrderRecapSummery.Badge = oReader.GetString("Badge");
            oOrderRecapSummery.Studs = oReader.GetString("Stud");
            oOrderRecapSummery.FabricAttachment = oReader.GetString("FabricAttachment");
            oOrderRecapSummery.Quantity = oReader.GetDouble("Quantity");
            oOrderRecapSummery.UnitName = oReader.GetString("UnitName");
            oOrderRecapSummery.Price = oReader.GetDouble("Price");
            oOrderRecapSummery.TotalAmount = oReader.GetDouble("TotalAmount");
            oOrderRecapSummery.ColorName = oReader.GetString("ColorName");
            oOrderRecapSummery.Label = oReader.GetString("Label");
            oOrderRecapSummery.HangTag = oReader.GetString("HangTag");
            oOrderRecapSummery.SizeRatio = oReader.GetString("SizeRatio");
            oOrderRecapSummery.RowNumber = oReader.GetInt32("RowNumber");
            oOrderRecapSummery.MaxRowNumber = oReader.GetInt32("MaxRowNumber");
            oOrderRecapSummery.BuyerID = oReader.GetInt32("BuyerID");
            oOrderRecapSummery.BuyerName = oReader.GetString("BuyerName");
            oOrderRecapSummery.BrandName = oReader.GetString("BrandName");
            oOrderRecapSummery.ApprovedByName = oReader.GetString("ApprovedByName");
            oOrderRecapSummery.PreparedBy = oReader.GetString("PreparedBy");
            oOrderRecapSummery.Incoterms = (EnumIncoterms)oReader.GetInt32("Incoterms");
            oOrderRecapSummery.SLNo = oReader.GetString("SLNo");
            oOrderRecapSummery.SessionName = oReader.GetString("SessionName");
            oOrderRecapSummery.CurrencyName = oReader.GetString("CurrencyName");
            oOrderRecapSummery.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oOrderRecapSummery.StyleDescription = oReader.GetString("StyleDescription");
            oOrderRecapSummery.KnittingPattern = oReader.GetInt32("KnittingPattern");
            oOrderRecapSummery.KnittingPatternName = oReader.GetString("KnittingPatternName");
            oOrderRecapSummery.MerchandiserName = oReader.GetString("MerchandiserName");
            oOrderRecapSummery.SpecialFinish = oReader.GetString("SpecialFinish");
            oOrderRecapSummery.Note = oReader.GetString("Note");
            oOrderRecapSummery.CMValue = oReader.GetDouble("CMValue");
            oOrderRecapSummery.LocalYarnSupplierID = oReader.GetInt32("LocalYarnSupplierID");
            oOrderRecapSummery.ImportYarnSupplierID = oReader.GetInt32("ImportYarnSupplierID");
            oOrderRecapSummery.LocalYarnSupplierName = oReader.GetString("LocalYarnSupplierName");
            oOrderRecapSummery.ImportYarnSupplierName = oReader.GetString("ImportYarnSupplierName");
            oOrderRecapSummery.CommercialRemarks = oReader.GetString("CommercialRemarks");
            return oOrderRecapSummery;
        }

        private OrderRecapSummery CreateObject(NullHandler oReader)
        {
            OrderRecapSummery oOrderRecapSummery = new OrderRecapSummery();
            oOrderRecapSummery = MapObject(oReader);
            return oOrderRecapSummery;
        }

        private List<OrderRecapSummery> CreateObjects(IDataReader oReader)
        {
            List<OrderRecapSummery> oOrderRecapSummery = new List<OrderRecapSummery>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderRecapSummery oItem = CreateObject(oHandler);
                oOrderRecapSummery.Add(oItem);
            }
            return oOrderRecapSummery;
        }

        #endregion

        #region Interface implementation
        public OrderRecapSummeryService() { }

        public List<OrderRecapSummery> GetsRecapWithOrderRecapSummerys(int nOT, int nStartRow, int nEndRow, string SQL, string sOrderRecapSummeryIDs, bool bIsPrint, int nSortBy, Int64 nUserID)
        {
            List<OrderRecapSummery> oOrderRecapSummery = new List<OrderRecapSummery>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapSummeryDA.GetsRecapWithOrderRecapSummerys(tc, nOT, nStartRow, nEndRow, SQL, sOrderRecapSummeryIDs, bIsPrint, nSortBy, nUserID);
                oOrderRecapSummery = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapSummery", e);
                #endregion
            }

            return oOrderRecapSummery;
        }                
        #endregion
    } 
    
  
}
