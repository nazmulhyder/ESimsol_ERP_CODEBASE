using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ESimSol.BusinessObjects.ReportingObject;
using ESimSol.Services.DataAccess.ReportingDA;

namespace ESimSol.Services.Services
{
    public class ProductReconciliationReportDetailService : MarshalByRefObject, IProductReconciliationReportDetailService
    {
        #region Private functions and declaration
        int i = 0;
        private ProductReconciliationReportDetail MapObject( NullHandler oReader)
        {
            i++;
           
             ProductReconciliationReportDetail oPRDetail=new ProductReconciliationReportDetail();
             oPRDetail.IssueDate = oReader.GetDateTime("IssueDate");
             oPRDetail.PINo = oReader.GetString("PINo");
             oPRDetail.ContractorName = oReader.GetString("ContractorName");
             oPRDetail.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
             oPRDetail.LCNo = oReader.GetString("ExportLCNo");
             oPRDetail.ProductName = oReader.GetString("ProductName");
             oPRDetail.Qty = oReader.GetDouble("Qty");
             oPRDetail.DeliveryQty = oReader.GetDouble("DeliveryQty");
             oPRDetail.PIQty = oReader.GetDouble("PIQty");
             oPRDetail.OrderQty = oReader.GetDouble("OrderQty");
             oPRDetail.Rate = oReader.GetDouble("UnitPrice");
             oPRDetail.LCNo = oReader.GetString("ExportLCNo");
             oPRDetail.CurrentStatus_LC = oReader.GetInt32("CurrentStatus_LC");
             oPRDetail.AmendmentDate = oReader.GetDateTime("AmendmentDate");
             oPRDetail.AmendmentNo = oReader.GetInt32("AmendmentNo");
             oPRDetail.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
             //oPRDetail.Qty = oPRDetail.OrderQty;
             oPRDetail.Qty_Prod = oReader.GetDouble("Qty_Prod");
             oPRDetail.Value = oPRDetail.Qty * oPRDetail.Rate;
             oPRDetail.YetToProduction = oPRDetail.OrderQty - oPRDetail.Qty_Prod;
            
             if (oPRDetail.AmendmentNo > 0)
             {
                 oPRDetail.LCNo = oPRDetail.LCNo + " A-" + oPRDetail.AmendmentNo;
             }
             return oPRDetail;
        }
        private ProductReconciliationReportDetail CreateObject(NullHandler oReader)
        {
            ProductReconciliationReportDetail oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
          oProductReconciliationReportDetail=  MapObject( oReader);
          return oProductReconciliationReportDetail;
        }
        private List<ProductReconciliationReportDetail> CreateObjects(IDataReader oReader)
        {
            List<ProductReconciliationReportDetail> oProductReconciliationReportDetails = new List<ProductReconciliationReportDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductReconciliationReportDetail oItem = CreateObject(oHandler);
                oProductReconciliationReportDetails.Add(oItem);
            }
            return oProductReconciliationReportDetails;
        }
        #endregion


        #region For DetailRepot
       
        private ProductReconciliationReportDetail MapObject_PR(NullHandler oReader)
        {

            ProductReconciliationReportDetail oPRDetail = new ProductReconciliationReportDetail();
            oPRDetail.IssueDate = oReader.GetDateTime("IssueDate");
            oPRDetail.PINo = oReader.GetString("PINo");
            oPRDetail.GRNNo = oReader.GetString("GRNNo");
            oPRDetail.BLNo = oReader.GetString("BLNo");
            oPRDetail.BLDate = oReader.GetDateTime("BLDate");
            oPRDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oPRDetail.LCNo = oReader.GetString("LCNo");
            oPRDetail.ContractorName = oReader.GetString("ContractorName");
            oPRDetail.BuyerName = oReader.GetString("BuyerName");
            oPRDetail.ProductName = oReader.GetString("ProductName");
            oPRDetail.ProductCode = oReader.GetString("ProductCode");
            oPRDetail.Qty = oReader.GetDouble("Qty");
            oPRDetail.OrderQty = oReader.GetDouble("OrderQty");
            oPRDetail.PIQty = oReader.GetDouble("PIQty");
            oPRDetail.Rate = oReader.GetDouble("UnitPrice");
            oPRDetail.POQty = oReader.GetDouble("InvoiceQty");
            oPRDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oPRDetail.LotNo = oReader.GetString("LotNo");
            oPRDetail.WUName = oReader.GetString("WUName");
            oPRDetail.Value = oPRDetail.Qty * oPRDetail.Rate;
            oPRDetail.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oPRDetail.Qty_Prod = oReader.GetDouble("ProductionFinishedQty");
            oPRDetail.YetToProduction = oReader.GetDouble("YetToProduction");
          
            return oPRDetail;
        }
        private ProductReconciliationReportDetail CreateObject_PR(NullHandler oReader)
        {
            ProductReconciliationReportDetail oProductReconciliationReportDetail = new ProductReconciliationReportDetail();
            oProductReconciliationReportDetail = MapObject_PR(oReader);
            return oProductReconciliationReportDetail;
        }
        private List<ProductReconciliationReportDetail> CreateObjects_PR(IDataReader oReader)
        {
            List<ProductReconciliationReportDetail> oProductReconciliationReportDetails = new List<ProductReconciliationReportDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductReconciliationReportDetail oItem = CreateObject_PR(oHandler);
                oProductReconciliationReportDetails.Add(oItem);
            }
            return oProductReconciliationReportDetails;
        }
        #endregion

        public DataSet Gets_ProductReconciliationReportDetail(int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID)
        {
            TransactionContext tc = null;
            DataSet oDataSet = new DataSet();
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductReconciliationReportDetailDA.Gets_ProductReconciliationReportDetail(tc, nProductID, dStartDate,  dEndDate, nReportType);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[5]);
                reader.Close();
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);

                throw new ServiceException("Fail to Get ProductReconciliationReportDetail", e);
                #endregion
            }
            return oDataSet;
        }
        public List<ProductReconciliationReportDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductReconciliationReportDetail> oPRDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductReconciliationReportDetailDA.Gets(tc, sSQL);
                oPRDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPRDetail;
        }

        public List<ProductReconciliationReportDetail> Gets_PRDetail(int nBUID, int nProductID, DateTime dStartDate, DateTime dEndDate, int nReportType, Int64 nUserID)
        {
            List<ProductReconciliationReportDetail> oPRDetail = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductReconciliationReportDetailDA.Gets_PRDetail(tc,  nBUID,  nProductID,  dStartDate,  dEndDate,  nReportType);
                oPRDetail = CreateObjects_PR(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oPRDetail;
        }
    }
}
