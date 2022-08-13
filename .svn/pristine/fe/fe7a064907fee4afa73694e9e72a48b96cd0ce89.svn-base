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
using ESimSol.BusinessObjects.ReportingObject;
namespace ESimSol.Services.Services
{
    public class rptMPRToGRNService : MarshalByRefObject, IrptMPRToGRNService
    {
        #region Private functions and declaration
        private rptMPRToGRN MapObject(NullHandler oReader)
        {
            rptMPRToGRN orptMPRToGRN = new rptMPRToGRN();
            orptMPRToGRN.PRID = oReader.GetInt32("PRID");
            orptMPRToGRN.PRDetailID = oReader.GetInt32("PRDetailID");
            orptMPRToGRN.MPRNo = oReader.GetString("MPRNo");
            orptMPRToGRN.MPRDate = oReader.GetDateTime("MPRDate");
            orptMPRToGRN.RequiredDate = oReader.GetDateTime("RequiredDate");
            orptMPRToGRN.DepartmentName = oReader.GetString("DepartmentName");
            orptMPRToGRN.MPRStatus = (EnumPurchaseRequisitionStatus)oReader.GetInt32("MPRStatus");
            orptMPRToGRN.RequisitonByName = oReader.GetString("RequisitonByName");
            orptMPRToGRN.ApprovedByName = oReader.GetString("ApprovedByName");
            orptMPRToGRN.ReqQty = oReader.GetDouble("ReqQty");


            orptMPRToGRN.ProductID = oReader.GetInt32("ProductID");
            orptMPRToGRN.ProductCode = oReader.GetString("ProductCode");
            orptMPRToGRN.ProductName = oReader.GetString("ProductName");
            orptMPRToGRN.ProductCategoryName = oReader.GetString("ProductCategoryName");
            orptMPRToGRN.ProductGroupName = oReader.GetString("ProductGroupName");
            orptMPRToGRN.Specification = oReader.GetString("Specification");


            orptMPRToGRN.NOAID = oReader.GetInt32("NOAID");
            orptMPRToGRN.NOANo = oReader.GetString("NOANo");
            orptMPRToGRN.NOADate = oReader.GetDateTime("NOADate");
            orptMPRToGRN.QuotationID = oReader.GetInt32("QuotationID");
            orptMPRToGRN.QuotationDetailID = oReader.GetInt32("QuotationDetailID");
            orptMPRToGRN.QuotationNo = oReader.GetString("QuotationNo");
            orptMPRToGRN.NOADetailID = oReader.GetInt32("NOADetailID");
            orptMPRToGRN.NOAQty = oReader.GetDouble("NOAQty");
            orptMPRToGRN.CSApprovedByName = oReader.GetString("CSApprovedByName");


            orptMPRToGRN.PODetailID = oReader.GetInt32("PODetailID");
            orptMPRToGRN.POID = oReader.GetInt32("POID");
            orptMPRToGRN.PORefType = (EnumPOReferenceType)oReader.GetInt32("PORefType");
            orptMPRToGRN.PONo = oReader.GetString("PONo");
            orptMPRToGRN.PODate = oReader.GetDateTime("PODate");
            orptMPRToGRN.POCrateByName = oReader.GetString("POCrateByName");
            orptMPRToGRN.POApprovedByName = oReader.GetString("POApprovedByName");
            orptMPRToGRN.PORefDetailID = oReader.GetInt32("PORefDetailID");
            orptMPRToGRN.POQty = oReader.GetDouble("POQty");


            orptMPRToGRN.PurchaseInvoiceDetailID = oReader.GetInt32("PurchaseInvoiceDetailID");
            orptMPRToGRN.PurchaseInvoiceID = oReader.GetInt32("PurchaseInvoiceID");
            orptMPRToGRN.InvoiceNo = oReader.GetString("InvoiceNo");
            orptMPRToGRN.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            orptMPRToGRN.InvoiceCreateByName = oReader.GetString("InvoiceCreateByName");
            orptMPRToGRN.InvoiceApprovedByName = oReader.GetString("InvoiceApprovedByName");
            orptMPRToGRN.InvQty = oReader.GetDouble("InvQty");


            orptMPRToGRN.GRNID = oReader.GetInt32("GRNID");
            orptMPRToGRN.GRNNo = oReader.GetString("GRNNo");
            orptMPRToGRN.GRNCreateByName = oReader.GetString("GRNCreateByName");
            orptMPRToGRN.GRNReceiveByName = oReader.GetString("GRNReceiveByName");
            orptMPRToGRN.GRNReceiveDate = oReader.GetDateTime("GRNReceiveDate");
            orptMPRToGRN.RefType = (EnumGRNType)oReader.GetInt32("RefType");
            orptMPRToGRN.RefObjectID = oReader.GetInt32("RefObjectID");
            orptMPRToGRN.RefQty = oReader.GetDouble("RefQty");
            orptMPRToGRN.MUnitID = oReader.GetInt32("MUnitID");
            orptMPRToGRN.UnitName = oReader.GetString("UnitName");
            orptMPRToGRN.RejectQty = oReader.GetDouble("RejectQty");
            orptMPRToGRN.ReceivedQty = oReader.GetDouble("ReceivedQty");
            orptMPRToGRN.YetToReceiveQty = oReader.GetDouble("YetToReceiveQty");
            orptMPRToGRN.UnitPrice = oReader.GetDouble("UnitPrice");
            orptMPRToGRN.Discount = oReader.GetDouble("Discount");
            orptMPRToGRN.Expense = oReader.GetDouble("Expense");
            orptMPRToGRN.TotalAmount = oReader.GetDouble("TotalAmount");
            orptMPRToGRN.PresentStock = oReader.GetDouble("PresentStock");
            return orptMPRToGRN;
        }

        private rptMPRToGRN CreateObject(NullHandler oReader)
        {
            rptMPRToGRN orptMPRToGRN = new rptMPRToGRN();
            orptMPRToGRN = MapObject(oReader);
            return orptMPRToGRN;
        }

        private List<rptMPRToGRN> CreateObjects(IDataReader oReader)
        {
            List<rptMPRToGRN> orptMPRToGRN = new List<rptMPRToGRN>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                rptMPRToGRN oItem = CreateObject(oHandler);
                orptMPRToGRN.Add(oItem);
            }
            return orptMPRToGRN;
        }

        #endregion

        #region Interface implementation
        public rptMPRToGRNService() { }

        public List<rptMPRToGRN> Gets(int nBUID, DateTime StartDate, DateTime EndDate, string sPRNo, Int64 nUserID)
        {
            List<rptMPRToGRN> orptMPRToGRNs = new List<rptMPRToGRN>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = rptMPRToGRNDA.Gets(tc, nBUID, StartDate, EndDate, sPRNo);
                orptMPRToGRNs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get rptMPRToGRN", e);
                #endregion
            }

            return orptMPRToGRNs;
        }

        #endregion
    }
}
