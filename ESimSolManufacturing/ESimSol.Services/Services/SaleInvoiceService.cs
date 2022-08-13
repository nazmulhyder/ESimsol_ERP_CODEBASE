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
    public class SaleInvoiceService : MarshalByRefObject, ISaleInvoiceService
    {
        #region Private functions and declaration
        private SaleInvoice MapObject(NullHandler oReader)
        {
            SaleInvoice oSaleInvoice = new SaleInvoice();
            oSaleInvoice.SaleInvoiceID = oReader.GetInt32("SaleInvoiceID");
            oSaleInvoice.InvoiceNo = oReader.GetString("InvoiceNo");
            oSaleInvoice.SQNo = oReader.GetString("SQNo");
            oSaleInvoice.KommNo = oReader.GetString("KommNo");
            oSaleInvoice.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oSaleInvoice.ContractorID = oReader.GetInt32("ContractorID");
            oSaleInvoice.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oSaleInvoice.SalesQuotationID = oReader.GetInt32("SalesQuotationID");
            oSaleInvoice.IsNewOrder = oReader.GetBoolean("IsNewOrder");
            oSaleInvoice.VehicleLocation = oReader.GetInt32("VehicleLocation");
            oSaleInvoice.PRNo = oReader.GetString("PRNo");
            oSaleInvoice.SpecialInstruction = oReader.GetString("SpecialInstruction");
            oSaleInvoice.ETAAgreement = oReader.GetString("ETAAgreement");
            oSaleInvoice.ETAWeeks = oReader.GetString("ETAWeeks");
            oSaleInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oSaleInvoice.OTRAmount = oReader.GetDouble("OTRAmount");
            oSaleInvoice.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oSaleInvoice.NetAmount = oReader.GetDouble("NetAmount");
            oSaleInvoice.AdvanceAmount = oReader.GetDouble("AdvanceAmount");
            oSaleInvoice.OTRAmount = oReader.GetDouble("OTRAmount");
            oSaleInvoice.AdvanceDate = oReader.GetDateTime("AdvanceDate");
            oSaleInvoice.PaymentMode = oReader.GetInt32("PaymentMode");
            oSaleInvoice.BankID = oReader.GetInt32("BankID");
            oSaleInvoice.BankName = oReader.GetString("BankName");
            oSaleInvoice.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oSaleInvoice.AttachmentDoc = oReader.GetInt32("AttachmentDoc");
            oSaleInvoice.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oSaleInvoice.SalesQuotationImageID = oReader.GetInt32("SalesQuotationImageID");
            oSaleInvoice.SaleInvoiceID = oReader.GetInt32("SaleInvoiceID");
            oSaleInvoice.BUID = oReader.GetInt32("BUID");
            oSaleInvoice.InvoiceStatus = (EnumSaleInvoiceStatus) oReader.GetInt32("InvoiceStatus");
            oSaleInvoice.ChequeNo = oReader.GetString("ChequeNo");
            oSaleInvoice.ChequeDate = oReader.GetDateTime("ChequeDate");
            oSaleInvoice.ReceivedByName = oReader.GetString("ReceivedByName");
            oSaleInvoice.InteriorColorName = oReader.GetString("InteriorColorName");
            oSaleInvoice.ExteriorColorName = oReader.GetString("ExteriorColorName");
            oSaleInvoice.EngineNo = oReader.GetString("EngineNo");
            oSaleInvoice.ChassisNo = oReader.GetString("ChassisNo");
            oSaleInvoice.ModelNo = oReader.GetString("ModelNo");
            oSaleInvoice.CustomerName = oReader.GetString("CustomerName");
            oSaleInvoice.CustomerAddress = oReader.GetString("CustomerAddress");
            oSaleInvoice.CustomerCity = oReader.GetString("CustomerCity");
            oSaleInvoice.CustomerLandline = oReader.GetString("CustomerLandline");
            oSaleInvoice.CustomerCellPhone = oReader.GetString("CustomerCellPhone");
            oSaleInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oSaleInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oSaleInvoice.ReceivedOn = oReader.GetString("ReceivedOn");
            oSaleInvoice.ApprovedByName = oReader.GetString("ApprovedByName");
            oSaleInvoice.Remarks = oReader.GetString("Remarks");
            oSaleInvoice.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oSaleInvoice.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oSaleInvoice.MarketingAccountName = oReader.GetString("MarketingAccountName");
            oSaleInvoice.AdvanceDate = oReader.GetDateTime("AdvanceDate");
            oSaleInvoice.CRate = oReader.GetDouble("CRate");
            oSaleInvoice.ProductID = oReader.GetInt32("ProductID");
            oSaleInvoice.ProductName = oReader.GetString("ProductName");
            oSaleInvoice.PIID = oReader.GetInt32("PIID");
            oSaleInvoice.PINo = oReader.GetString("PINo");
            oSaleInvoice.VatAmount = oReader.GetDouble("VatAmount");
            oSaleInvoice.RegistrationFee = oReader.GetDouble("RegistrationFee");
            oSaleInvoice.TDSAmount = oReader.GetDouble("TDSAmount");
            oSaleInvoice.PDIChargeAmount = oReader.GetDouble("PDIChargeAmount");
            oSaleInvoice.FreeServiceChargeAmount = oReader.GetDouble("FreeServiceChargeAmount");
            return oSaleInvoice;
        }

        private SaleInvoice CreateObject(NullHandler oReader)
        {
            SaleInvoice oSaleInvoice = new SaleInvoice();
            oSaleInvoice = MapObject(oReader);
            return oSaleInvoice;
        }

        private List<SaleInvoice> CreateObjects(IDataReader oReader)
        {
            List<SaleInvoice> oSaleInvoice = new List<SaleInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SaleInvoice oItem = CreateObject(oHandler);
                oSaleInvoice.Add(oItem);
            }
            return oSaleInvoice;
        }

        #endregion

        #region Interface implementation
        public SaleInvoiceService() { }

        public SaleInvoice Save(SaleInvoice oSaleInvoice, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSaleInvoice.SaleInvoiceID <= 0)
                {
                    reader = SaleInvoiceDA.InsertUpdate(tc, oSaleInvoice, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = SaleInvoiceDA.InsertUpdate(tc, oSaleInvoice, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSaleInvoice = new SaleInvoice();
                    oSaleInvoice = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save SaleInvoice. Because of " + e.Message, e);
                #endregion
            }
            return oSaleInvoice;
        }

        public string UpdateStatus(SaleInvoice oSaleInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            string sMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);

                if (oSaleInvoice.nRequest ==2)//Approve
                {
                    sMessage = "Approved";
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SaleInvoice, EnumRoleOperationType.Approved);
                    DBTableReferenceDA.HasReference(tc, "SaleInvoice", oSaleInvoice.SaleInvoiceID);

                    oSaleInvoice.InvoiceStatus = EnumSaleInvoiceStatus.Approved;
                    SaleInvoiceDA.UpdateStatus(tc, oSaleInvoice, EnumDBOperation.Approval, nUserID);
                }
                //else if (oSaleInvoice.nRequest == 2)//Received
                //{
                //    sMessage = "Received";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SaleInvoice, EnumRoleOperationType.Received);
                //    DBTableReferenceDA.HasReference(tc, "SaleInvoice", oSaleInvoice.SaleInvoiceID);

                //    oSaleInvoice.OrderStatus = EnumSaleInvoiceStatus.Received;
                //    SaleInvoiceDA.UpdateStatus(tc, oSaleInvoice, EnumDBOperation.Receive, nUserID);
                //}
                //else if (oSaleInvoice.nRequest == 3)//Deliverd
                //{
                //    sMessage = "Delivered";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SaleInvoice, EnumRoleOperationType.DeliverToParty);
                //    DBTableReferenceDA.HasReference(tc, "SaleInvoice", oSaleInvoice.SaleInvoiceID);

                //    oSaleInvoice.OrderStatus = EnumSaleInvoiceStatus.Done;
                //    SaleInvoiceDA.UpdateStatus(tc, oSaleInvoice, EnumDBOperation.Delivered, nUserID);
                //}
                //else if (oSaleInvoice.nRequest == 4)//Cancel
                //{
                //    sMessage = "Cancel";
                //    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.SaleInvoice, EnumRoleOperationType.Cancel);
                //    DBTableReferenceDA.HasReference(tc, "SaleInvoice", oSaleInvoice.SaleInvoiceID);

                //    oSaleInvoice.OrderStatus = EnumSaleInvoiceStatus.Canceled;
                //    SaleInvoiceDA.UpdateStatus(tc, oSaleInvoice, EnumDBOperation.Cancel, nUserID);
                //}
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    return e.Message.Split('!')[0];
                }
                #endregion
            }
            return sMessage;
        }
	
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SaleInvoice oSaleInvoice = new SaleInvoice();
                oSaleInvoice.SaleInvoiceID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SaleInvoice, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SaleInvoice", id);
                SaleInvoiceDA.Delete(tc, oSaleInvoice, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Deleted";
        }

        public SaleInvoice Get(int id, Int64 nUserId)
        {
            SaleInvoice oAccountHead = new SaleInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SaleInvoiceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SaleInvoice", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SaleInvoice> Gets(Int64 nUserId)
        {
            List<SaleInvoice> oSaleInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SaleInvoiceDA.Gets(tc);
                oSaleInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SaleInvoice", e);
                #endregion
            }

            return oSaleInvoice;
        }


        public List<SaleInvoice> Gets(string sSQL, Int64 nUserId)
        {
            List<SaleInvoice> oSaleInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SaleInvoiceDA.Gets(tc, sSQL);
                oSaleInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SaleInvoice", e);
                #endregion
            }

            return oSaleInvoice;
        }

        #endregion
    }
}