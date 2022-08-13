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
    public class DyeingOrderReportService : MarshalByRefObject, IDyeingOrderReportService
    {
        #region Private functions and declaration
        private DyeingOrderReport MapObject(NullHandler oReader)
        {
            DyeingOrderReport oPIReport = new DyeingOrderReport();
            oPIReport.OrderNo = oReader.GetString("OrderNo");
            oPIReport.OrderDate = oReader.GetDateTime("OrderDate");
            oPIReport.ReviseDate = oReader.GetDateTime("ReviseDate");
            oPIReport.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oPIReport.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oPIReport.ExportPIID = oReader.GetInt32("ExportPIID");
            oPIReport.ProductID = oReader.GetInt32("ProductID");
            oPIReport.ContractorName = oReader.GetString("ContractorName");
            oPIReport.MBuyerName = oReader.GetString("MBuyerName");
            oPIReport.DeliveryToName = oReader.GetString("DeliveryToName");
            oPIReport.MKTPName = oReader.GetString("MKTPName");
            oPIReport.RefNo = oReader.GetString("RefNo");
            oPIReport.StyleNo = oReader.GetString("StyleNo");
            oPIReport.CPName = oReader.GetString("CPName");
            oPIReport.DONote = oReader.GetString("DONote");
            oPIReport.ProductName = oReader.GetString("ProductName");
            oPIReport.SampleInvoiceNo = oReader.GetString("SampleInvoiceNo");
            oPIReport.Qty = oReader.GetDouble("Qty");
            oPIReport.UnitPrice = oReader.GetDouble("UnitPrice");
            oPIReport.MUName = oReader.GetString("MUName");
            oPIReport.PINo = oReader.GetString("PINo");
            oPIReport.ExportLCNo = oReader.GetString("ExportLCNo");
            oPIReport.DyeingOrderType = oReader.GetInt32("DyeingOrderType");
            oPIReport.PaymentType = oReader.GetInt32("PaymentType");
            oPIReport.NoCode = oReader.GetString("NoCode");
            oPIReport.OrderType = oReader.GetString("OrderType");
            oPIReport.Qty_DC = oReader.GetDouble("Qty_DC");
            oPIReport.Qty_DO = oReader.GetDouble("Qty_DO");
            oPIReport.Currency = oReader.GetString("Currency");
            oPIReport.StockInHand = oReader.GetDouble("StockInHand");
            oPIReport.ColorNo = oReader.GetString("ColorNo");
            oPIReport.LDNo = oReader.GetString("LDNo");
            oPIReport.ColorName = oReader.GetString("ColorName");
            oPIReport.PantonNo = oReader.GetString("PantonNo");
            oPIReport.BuyerRef = oReader.GetString("BuyerRef");
            oPIReport.RGB = oReader.GetString("RGB");
            //oPIReport.Qty_Schedule = oReader.GetDouble("Qty_Schedule");
            oPIReport.HankorCone = oReader.GetInt16("HankorCone");
            oPIReport.OrderTypeSt = oReader.GetString("OrderTypeSt");
            oPIReport.LabdipNo = oReader.GetString("LabdipNo");
            oPIReport.ApproveLotNo = oReader.GetString("ApproveLotNo");
            oPIReport.MUnitID = oReader.GetInt32("MUnitID");
            oPIReport.ExportSCDetailID = oReader.GetInt32("ExportSCDetailID");
            
            oPIReport.Shade = oReader.GetInt16("Shade");
            oPIReport.LabDipType = oReader.GetInt16("LabDipType");
            oPIReport.Status = oReader.GetInt16("Status");
            oPIReport.StatusDOD = (EnumDOState)oReader.GetInt16("StatusDOD");
            oPIReport.BuyerCombo = oReader.GetInt32("BuyerCombo");
            oPIReport.ReviseNote = oReader.GetString("ReviseNote");
            if (String.IsNullOrEmpty(oPIReport.ApproveLotNo))
            {
                oPIReport.ApproveLotNo = oPIReport.ShadeSt;
            }

            return oPIReport;
        }

        private DyeingOrderReport CreateObject(NullHandler oReader)
        {
            DyeingOrderReport oPIReport = new DyeingOrderReport();
            oPIReport = MapObject(oReader);
            return oPIReport;
        }

        private List<DyeingOrderReport> CreateObjects(IDataReader oReader)
        {
            List<DyeingOrderReport> oPIReport = new List<DyeingOrderReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DyeingOrderReport oItem = CreateObject(oHandler);
                oPIReport.Add(oItem);
            }
            return oPIReport;
        }

        #endregion

        #region Interface implementation
        public DyeingOrderReportService() { }

        public List<DyeingOrderReport> Gets(string sSQL, Int64 nUserID)
        {
            List<DyeingOrderReport> oPIReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderReportDA.Gets(tc, sSQL);
                oPIReport = CreateObjects(reader);
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

            return oPIReport;
        }
        public List<DyeingOrderReport> Gets(int nSampleInvoiceID, Int64 nUserID)
        {
            List<DyeingOrderReport> oDyeingOrderReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DyeingOrderReportDA.Gets(tc, nSampleInvoiceID);
                oDyeingOrderReport = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DyeingOrderReport", e);
                #endregion
            }
            return oDyeingOrderReport;
        }

        #endregion
    }
}