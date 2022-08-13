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
    public class LabDipReportService : MarshalByRefObject, ILabDipReportService
    {
        #region Private functions and declaration
        private LabDipReport MapObject(NullHandler oReader)
        {
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.LabDipID = oReader.GetInt32("LabDipID");
            oLabDipReport.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oLabDipReport.LabdipNo = oReader.GetString("LabdipNo");
            oLabDipReport.LDNo = oReader.GetString("LDNo");
            oLabDipReport.BuyerRefNo = oReader.GetString("BuyerRefNo");
            oLabDipReport.PriorityLevel = oReader.GetInt16("PriorityLevel");
            oLabDipReport.Note = oReader.GetString("Note");
            oLabDipReport.OrderStatus = oReader.GetInt16("OrderStatus");
            oLabDipReport.LabDipFormat = oReader.GetInt16("LabDipFormat");
            oLabDipReport.OrderReferenceType = oReader.GetInt16("OrderReferenceType");
           
            oLabDipReport.SeekingDate = oReader.GetDateTime("SeekingDate");
            oLabDipReport.OrderDate = oReader.GetDateTime("OrderDate");
           
            oLabDipReport.ISTwisted = oReader.GetBoolean("ISTwisted");
            oLabDipReport.ContractorName = oReader.GetString("ContractorName");
            oLabDipReport.DeliveryToName = oReader.GetString("DeliveryToName");
            //oLabDipReport.DeliveryZoneName = oReader.GetString("DeliveryZoneName");
            oLabDipReport.MktPerson = oReader.GetString("MktPerson");
            oLabDipReport.LightSourceName = oReader.GetString("LightSourceName");
            oLabDipReport.ContractorCPName = oReader.GetString("ContractorCPName");
            //oLabDipReport.DeliveryToCPName = oReader.GetString("DeliveryToCPName");
            oLabDipReport.NoOfColor = oReader.GetInt32("NoOfColor");
            ///LD Detail Property
            oLabDipReport.ColorSet = oReader.GetInt16("ColorSet");
            oLabDipReport.ShadeCount = oReader.GetInt16("ShadeCount");
            oLabDipReport.KnitPlyYarn = oReader.GetInt16("KnitPlyYarn");
            oLabDipReport.ColorName = oReader.GetString("ColorName");
            oLabDipReport.RefNo = oReader.GetString("RefNo");
            oLabDipReport.PantonNo = oReader.GetString("PantonNo");
            oLabDipReport.RGB = oReader.GetString("RGB");
            oLabDipReport.ColorNo = oReader.GetString("ColorNo");
            oLabDipReport.Combo = oReader.GetInt16("Combo");
            oLabDipReport.LotNo = oReader.GetString("LotNo");
            oLabDipReport.ProductID = oReader.GetInt16("ProductID");
            oLabDipReport.ChallanNo = oReader.GetString("ChallanNo");
            oLabDipReport.ProductCode = oReader.GetString("ProductCode");
            oLabDipReport.ProductName = oReader.GetString("ProductName");
            oLabDipReport.EndBuyer = oReader.GetString("EndBuyer");
            oLabDipReport.DeliveyDate = oReader.GetDateTime("DeliveyDate");
            return oLabDipReport;
        }
        private LabDipReport CreateObject(NullHandler oReader)
        {
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport = MapObject(oReader);
            return oLabDipReport;
        }
        private List<LabDipReport> CreateObjects(IDataReader oReader)
        {
            List<LabDipReport> oLabDipReport = new List<LabDipReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDipReport oItem = CreateObject(oHandler);
                oLabDipReport.Add(oItem);
            }
            return oLabDipReport;
        }
        #region Product
        private LabDipReport MapObject_Product(NullHandler oReader)
        {
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport.LabDipID = oReader.GetInt32("LabDipID");
            //oLabDipReport.LabDipDetailID = oReader.GetInt32("LabDipDetailID");
            oLabDipReport.LabdipNo = oReader.GetString("LabdipNo");
            oLabDipReport.BuyerRefNo = oReader.GetString("BuyerRefNo");
            oLabDipReport.PriorityLevel = oReader.GetInt16("PriorityLevel");
            oLabDipReport.Note = oReader.GetString("Note");
            oLabDipReport.OrderStatus = oReader.GetInt16("OrderStatus");
            oLabDipReport.LabDipFormat = oReader.GetInt16("LabDipFormat");
            oLabDipReport.OrderReferenceType = oReader.GetInt16("OrderReferenceType");

            oLabDipReport.SeekingDate = oReader.GetDateTime("SeekingDate");
            oLabDipReport.OrderDate = oReader.GetDateTime("OrderDate");

            oLabDipReport.ISTwisted = oReader.GetBoolean("ISTwisted");
            oLabDipReport.ContractorName = oReader.GetString("ContractorName");
            oLabDipReport.DeliveryToName = oReader.GetString("DeliveryToName");
            //oLabDipReport.DeliveryZoneName = oReader.GetString("DeliveryZoneName");
            oLabDipReport.MktPerson = oReader.GetString("MktPerson");
            oLabDipReport.LightSourceName = oReader.GetString("LightSourceName");
            oLabDipReport.ContractorCPName = oReader.GetString("ContractorCPName");
            //oLabDipReport.DeliveryToCPName = oReader.GetString("DeliveryToCPName");
            oLabDipReport.NoOfColor = oReader.GetInt32("NoOfColor");
            ///LD Detail Property
            oLabDipReport.DeliveyDate = oReader.GetDateTime("DeliveyDate");
            oLabDipReport.ProductCode = oReader.GetString("ProductCode");
            oLabDipReport.ProductName = oReader.GetString("ProductName");
            oLabDipReport.EndBuyer = oReader.GetString("EndBuyer");
            return oLabDipReport;
        }
        private LabDipReport CreateObject_Product(NullHandler oReader)
        {
            LabDipReport oLabDipReport = new LabDipReport();
            oLabDipReport = MapObject_Product(oReader);
            return oLabDipReport;
        }
        private List<LabDipReport> CreateObjects_Product(IDataReader oReader)
        {
            List<LabDipReport> oLabDipReport = new List<LabDipReport>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LabDipReport oItem = CreateObject_Product(oHandler);
                oLabDipReport.Add(oItem);
            }
            return oLabDipReport;
        }
        #endregion
        #endregion

        #region Interface implementation
        public LabDipReportService() { }

        public List<LabDipReport> Gets(string sSQL, Int64 nUserID)
        {
            List<LabDipReport> oLabDipReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipReportDA.Gets(tc, sSQL);
                oLabDipReport = CreateObjects(reader);
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

            return oLabDipReport;
        }

        public List<LabDipReport> GetsSql(string sSQL, Int64 nUserID)
        {
            List<LabDipReport> oLabDipReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipReportDA.GetsSql(tc, sSQL);
                oLabDipReport = CreateObjects(reader);
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

            return oLabDipReport;
        }
        public List<LabDipReport> Gets_Product(string sSQL, Int64 nUserID)
        {
            List<LabDipReport> oLabDipReport = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LabDipReportDA.Gets_Product(tc, sSQL);
                oLabDipReport = CreateObjects_Product(reader);
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

            return oLabDipReport;
        }
       

        #endregion
    }
}