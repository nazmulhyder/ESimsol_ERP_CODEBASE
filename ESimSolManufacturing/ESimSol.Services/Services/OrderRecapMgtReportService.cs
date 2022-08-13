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


    public class OrderRecapMgtReportService : MarshalByRefObject, IOrderRecapMgtReportService
    {
        #region Private functions and declaration
        private OrderRecapMgtReport MapObject(NullHandler oReader)
        {
            OrderRecapMgtReport oOrderRecapMgtReport = new OrderRecapMgtReport();
            oOrderRecapMgtReport.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oOrderRecapMgtReport.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oOrderRecapMgtReport.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oOrderRecapMgtReport.MerchandiserID = oReader.GetInt32("MerchandiserID");
            oOrderRecapMgtReport.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oOrderRecapMgtReport.OrderDate = oReader.GetDateTime("OrderDate");
            oOrderRecapMgtReport.BuyerID = oReader.GetInt32("BuyerID");
            oOrderRecapMgtReport.AgentID = oReader.GetInt32("AgentID");
            oOrderRecapMgtReport.AgentName = oReader.GetString("AgentName");
            oOrderRecapMgtReport.FactoryID = oReader.GetInt32("FactoryID");
            oOrderRecapMgtReport.FabricID = oReader.GetInt32("FabricID");
            oOrderRecapMgtReport.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oOrderRecapMgtReport.StyleNo = oReader.GetString("StyleNo");
            oOrderRecapMgtReport.BuyerName = oReader.GetString("BuyerName");
            oOrderRecapMgtReport.FactoryName = oReader.GetString("FactoryName");
            oOrderRecapMgtReport.MerchandiserName = oReader.GetString("MerchandiserName");
            oOrderRecapMgtReport.ProductCategoryName = oReader.GetString("ProductCategoryName");
            oOrderRecapMgtReport.FabricName = oReader.GetString("FabricName");
            oOrderRecapMgtReport.FOB = oReader.GetDouble("FOB");
            oOrderRecapMgtReport.ONSQty = oReader.GetDouble("ONSQty");
            oOrderRecapMgtReport.ODSQty = oReader.GetDouble("ODSQty");
            oOrderRecapMgtReport.CMValue = oReader.GetDouble("CMValue");
            oOrderRecapMgtReport.OrderQty = oReader.GetDouble("OrderQty");
            oOrderRecapMgtReport.StyleInputDate = oReader.GetDateTime("StyleInputDate");
            oOrderRecapMgtReport.SeasonName = oReader.GetString("SeasonName");
            oOrderRecapMgtReport.GarmentsName = oReader.GetString("GarmentsName");
            oOrderRecapMgtReport.ContactPersonName = oReader.GetString("ContactPersonName");
            oOrderRecapMgtReport.ColorRange = oReader.GetString("ColorRange");
            oOrderRecapMgtReport.SizeRange = oReader.GetString("SizeRange");
            oOrderRecapMgtReport.CuttingQty = oReader.GetDouble("CuttingQty");
            oOrderRecapMgtReport.SweeingQty = oReader.GetDouble("SweeingQty");
            oOrderRecapMgtReport.ShipmentQty = oReader.GetDouble("ShipmentQty");
            oOrderRecapMgtReport.Remarks = oReader.GetString("Remarks");
            oOrderRecapMgtReport.FactoryFOB = oReader.GetDouble("FactoryFOB");
            oOrderRecapMgtReport.MasterLCNo = oReader.GetString("MasterLCNo");
            oOrderRecapMgtReport.LCTransferNo = oReader.GetString("LCTransferNo");
            oOrderRecapMgtReport.Dept = oReader.GetInt32("Dept");
            oOrderRecapMgtReport.DeptName = oReader.GetString("DeptName");
            oOrderRecapMgtReport.SubGender = (EnumSubGender)oReader.GetInt32("SubGender");
            oOrderRecapMgtReport.PIPrice = oReader.GetDouble("PIPrice");
            oOrderRecapMgtReport.LCShipmentDate = oReader.GetDateTime("LCShipmentDate");
            oOrderRecapMgtReport.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oOrderRecapMgtReport.FactoryShipmentDate = oReader.GetDateTime("FactoryShipmentDate");
            return oOrderRecapMgtReport;
        }

        private OrderRecapMgtReport CreateObject(NullHandler oReader)
        {
            OrderRecapMgtReport oOrderRecapMgtReport = new OrderRecapMgtReport();
            oOrderRecapMgtReport = MapObject(oReader);
            return oOrderRecapMgtReport;
        }

        private List<OrderRecapMgtReport> CreateObjects(IDataReader oReader)
        {
            List<OrderRecapMgtReport> oOrderRecapMgtReport = new List<OrderRecapMgtReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                OrderRecapMgtReport oItem = CreateObject(oHandler);
                oOrderRecapMgtReport.Add(oItem);
            }
            return oOrderRecapMgtReport;
        }

        #endregion



        #region Interface implementation


        public List<OrderRecapMgtReport> Gets(string sSql, int ReportFormat, Int64 nUserID)
        {
            List<OrderRecapMgtReport> oOrderRecapMgtReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = OrderRecapMgtReportDA.Gets(sSql, ReportFormat, tc);
                oOrderRecapMgtReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get OrderRecapMgtReport", e);
                #endregion
            }

            return oOrderRecapMgtReport;
        }


        #endregion


    } 
    

}
