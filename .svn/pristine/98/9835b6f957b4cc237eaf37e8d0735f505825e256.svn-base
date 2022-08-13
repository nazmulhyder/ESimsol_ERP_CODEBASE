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
    public class CommercialInvoiceDetailService : MarshalByRefObject, ICommercialInvoiceDetailService
    {
        #region Private functions and declaration
        private CommercialInvoiceDetail MapObject(NullHandler oReader)
        {
            CommercialInvoiceDetail oCommercialInvoiceDetail = new CommercialInvoiceDetail();
            oCommercialInvoiceDetail.CommercialInvoiceDetailID = oReader.GetInt32("CommercialInvoiceDetailID");
            oCommercialInvoiceDetail.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oCommercialInvoiceDetail.ReferenceDetailID = oReader.GetInt32("ReferenceDetailID");
            oCommercialInvoiceDetail.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oCommercialInvoiceDetail.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oCommercialInvoiceDetail.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oCommercialInvoiceDetail.TransferQty = oReader.GetDouble("TransferQty");
            oCommercialInvoiceDetail.YetToInvoiceQty = oReader.GetDouble("YetToInvoiceQty");
            oCommercialInvoiceDetail.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oCommercialInvoiceDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oCommercialInvoiceDetail.Discount = oReader.GetDouble("Discount");
            oCommercialInvoiceDetail.FOB = oReader.GetDouble("FOB");
            oCommercialInvoiceDetail.Amount = oReader.GetDouble("Amount");
            oCommercialInvoiceDetail.StyleNo = oReader.GetString("StyleNo");
            oCommercialInvoiceDetail.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oCommercialInvoiceDetail.OrderNo = oReader.GetString("OrderNo");
            oCommercialInvoiceDetail.ProductName = oReader.GetString("ProductName");
            oCommercialInvoiceDetail.Fabrication = oReader.GetString("Fabrication");
            oCommercialInvoiceDetail.InvoiceNo = oReader.GetString("InvoiceNo");
            oCommercialInvoiceDetail.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oCommercialInvoiceDetail.BuyerName = oReader.GetString("BuyerName");
            oCommercialInvoiceDetail.FactoryName = oReader.GetString("FactoryName");
            oCommercialInvoiceDetail.HSCode = oReader.GetString("HSCode");
            oCommercialInvoiceDetail.CAT = oReader.GetString("CAT");
            oCommercialInvoiceDetail.CartonQty = oReader.GetDouble("CartonQty");
            oCommercialInvoiceDetail.TotalGrossWeight = oReader.GetDouble("TotalGrossWeight");
            oCommercialInvoiceDetail.TotalNetWeight = oReader.GetDouble("TotalNetWeight");
            oCommercialInvoiceDetail.TotalVolume = oReader.GetDouble("TotalVolume");
            oCommercialInvoiceDetail.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oCommercialInvoiceDetail.AdditionInPercent = oReader.GetDouble("AdditionInPercent");
            oCommercialInvoiceDetail.UnitName = oReader.GetString("UnitName");
            oCommercialInvoiceDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oCommercialInvoiceDetail.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            return oCommercialInvoiceDetail;
        }

        private CommercialInvoiceDetail CreateObject(NullHandler oReader)
        {
            CommercialInvoiceDetail oCommercialInvoiceDetail = new CommercialInvoiceDetail();
            oCommercialInvoiceDetail = MapObject(oReader);
            return oCommercialInvoiceDetail;
        }

        private List<CommercialInvoiceDetail> CreateObjects(IDataReader oReader)
        {
            List<CommercialInvoiceDetail> oCommercialInvoiceDetail = new List<CommercialInvoiceDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommercialInvoiceDetail oItem = CreateObject(oHandler);
                oCommercialInvoiceDetail.Add(oItem);
            }
            return oCommercialInvoiceDetail;
        }

        #endregion

        #region Interface implementation
        public CommercialInvoiceDetailService() { }

        public CommercialInvoiceDetail Save(CommercialInvoiceDetail oCommercialInvoiceDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<CommercialInvoiceDetail> _oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
            oCommercialInvoiceDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CommercialInvoiceDetailDA.InsertUpdate(tc, oCommercialInvoiceDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                    oCommercialInvoiceDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oCommercialInvoiceDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oCommercialInvoiceDetail;
        }

        public CommercialInvoiceDetail Get(int CommercialInvoiceDetailID, Int64 nUserId)
        {
            CommercialInvoiceDetail oAccountHead = new CommercialInvoiceDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommercialInvoiceDetailDA.Get(tc, CommercialInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoiceDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CommercialInvoiceDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<CommercialInvoiceDetail> oCommercialInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDetailDA.Gets(LabDipOrderID, tc);
                oCommercialInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoiceDetail", e);
                #endregion
            }

            return oCommercialInvoiceDetail;
        }

        public List<CommercialInvoiceDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<CommercialInvoiceDetail> oCommercialInvoiceDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDetailDA.Gets(tc, sSQL);
                oCommercialInvoiceDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoiceDetail", e);
                #endregion
            }

            return oCommercialInvoiceDetail;
        }
        #endregion
    }
}
