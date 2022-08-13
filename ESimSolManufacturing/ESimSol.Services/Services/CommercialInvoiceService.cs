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
using System.Linq;
 

namespace ESimSol.Services.Services
{
    public class CommercialInvoiceService : MarshalByRefObject, ICommercialInvoiceService
    {
        #region Private functions and declaration
        private CommercialInvoice MapObject(NullHandler oReader)
        {
            CommercialInvoice oCommercialInvoice = new CommercialInvoice();
            oCommercialInvoice.CommercialInvoiceID = oReader.GetInt32("CommercialInvoiceID");
            oCommercialInvoice.LCTransferID = oReader.GetInt32("LCTransferID");
            oCommercialInvoice.MasterLCID = oReader.GetInt32("MasterLCID");
            oCommercialInvoice.InvoiceNo = oReader.GetString("InvoiceNo");
            oCommercialInvoice.IsSystemGeneratedInvoiceNo = oReader.GetBoolean("IsSystemGeneratedInvoiceNo");
            oCommercialInvoice.InvoiceDate = oReader.GetDateTime("InvoiceDate");
            oCommercialInvoice.InvoiceStatus = (EnumCommercialInvoiceStatus)oReader.GetInt32("InvoiceStatus");
            oCommercialInvoice.InvoiceStatusInInt = oReader.GetInt32("InvoiceStatus");
            oCommercialInvoice.BuyerID = oReader.GetInt32("BuyerID");
            oCommercialInvoice.ProductionFactoryID = oReader.GetInt32("ProductionFactoryID");
            oCommercialInvoice.InvoiceAmount = oReader.GetDouble("InvoiceAmount");
            oCommercialInvoice.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oCommercialInvoice.AdditionAmount = oReader.GetDouble("AdditionAmount");
            oCommercialInvoice.NetInvoiceAmount = oReader.GetDouble("NetInvoiceAmount");
            oCommercialInvoice.AnnualBonus = oReader.GetDouble("AnnualBonus");
            oCommercialInvoice.Note = oReader.GetString("Note");
            oCommercialInvoice.ReceiptNo = oReader.GetString("ReceiptNo");
            oCommercialInvoice.TransportNo = oReader.GetString("TransportNo");
            oCommercialInvoice.DriverName = oReader.GetString("DriverName");
            oCommercialInvoice.Carrier = oReader.GetString("Carrier");
            oCommercialInvoice.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oCommercialInvoice.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oCommercialInvoice.GSP = oReader.GetBoolean("GSP");
            oCommercialInvoice.IC = oReader.GetBoolean("IC");
            oCommercialInvoice.BL = oReader.GetBoolean("BL");
            oCommercialInvoice.MasterLCNo = oReader.GetString("MasterLCNo");
            oCommercialInvoice.LCStatus = (EnumLCStatus)oReader.GetInt32("LCStatus");
            oCommercialInvoice.LCValue = oReader.GetDouble("LCValue");
            oCommercialInvoice.ApprovedByName = oReader.GetString("ApprovedByName");
            oCommercialInvoice.TransferNo = oReader.GetString("TransferNo");
            oCommercialInvoice.TransferAmount = oReader.GetDouble("TransferAmount");
            oCommercialInvoice.BuyerName = oReader.GetString("BuyerName");
            oCommercialInvoice.ProductionFactoryName = oReader.GetString("ProductionFactoryName");
            oCommercialInvoice.InvoiceQty = oReader.GetDouble("InvoiceQty");
            oCommercialInvoice.AdviceBankAccount = oReader.GetString("AdviceBankAccount");
            oCommercialInvoice.YetToInvoiceAmount = Math.Round(oReader.GetDouble("YetToInvoiceAmount"),2);
            oCommercialInvoice.LCDate = oReader.GetDateTime("LCDate");
            oCommercialInvoice.TotalEndrosmentCommission =Math.Round( oReader.GetDouble("TotalEndrosmentCommission"),2);
            oCommercialInvoice.PaymentID = oReader.GetInt32("PaymentID");
            oCommercialInvoice.BUID = oReader.GetInt32("BUID");
            oCommercialInvoice.CIFormat =  (EnumClientOperationValueFormat) oReader.GetInt32("CIFormat");
            oCommercialInvoice.CIFormatInInt = oReader.GetInt32("CIFormat");
            
            oCommercialInvoice.PaymentApprovedBy = oReader.GetInt32("PaymentApprovedBy");
            oCommercialInvoice.ShipmentMode = (EnumTransportType)oReader.GetInt32("ShipmentMode");
            oCommercialInvoice.ShipmentModeInInt = oReader.GetInt32("ShipmentMode");
            oCommercialInvoice.CurrencyID = oReader.GetInt32("CurrencyID");
            oCommercialInvoice.BuyerOrigin = oReader.GetString("BuyerOrigin");
            oCommercialInvoice.BuyerAddress = oReader.GetString("BuyerAddress");
            oCommercialInvoice.BuyerEmailAddress = oReader.GetString("BuyerEmailAddress");
            oCommercialInvoice.ApprovedByEmailAddress = oReader.GetString("ApprovedByEmailAddress");
            oCommercialInvoice.ApprovedByContactNo = oReader.GetString("ApprovedByContactNo");
            oCommercialInvoice.IssueBankName = oReader.GetString("IssueBankName");
            oCommercialInvoice.AdviceBankName = oReader.GetString("AdviceBankName");
            oCommercialInvoice.CurrencyName = oReader.GetString("CurrencyName");
            oCommercialInvoice.CurrencySymbol = oReader.GetString("CurrencySymbol");

            return oCommercialInvoice;
        }

        private CommercialInvoice CreateObject(NullHandler oReader)
        {
            CommercialInvoice oCommercialInvoice = new CommercialInvoice();
            oCommercialInvoice = MapObject(oReader);
            return oCommercialInvoice;
        }

        private List<CommercialInvoice> CreateObjects(IDataReader oReader)
        {
            List<CommercialInvoice> oCommercialInvoice = new List<CommercialInvoice>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommercialInvoice oItem = CreateObject(oHandler);
                oCommercialInvoice.Add(oItem);
            }
            return oCommercialInvoice;
        }

        #endregion

        #region Interface implementation
        public CommercialInvoiceService() { }

        public CommercialInvoice Save(CommercialInvoice oCommercialInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<CommercialInvoiceDetail> oCommercialInvoiceDetails = new List<CommercialInvoiceDetail>();
                CommercialInvoiceDetail oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                oCommercialInvoiceDetails = oCommercialInvoice.CommercialInvoiceDetails;
                List<MasterLCDetail> oMasterLCDetails = new List<MasterLCDetail>();
                string sCommercialInvoiceDetailIDs = ""; int nTempProformaInvoiceID = 0;

             

                #region CommercialInvoice part
                IDataReader reader;
                #region Check Pi Part
                reader = MasterLCDetailDA.Gets(tc,"SELECT top 1 * FROM View_MasterLCDetail where MasterLCID ="+ oCommercialInvoice.MasterLCID);
                NullHandler oReaderLCDetail = new NullHandler(reader);
                if (reader.Read())
                {
                    nTempProformaInvoiceID = oReaderLCDetail.GetInt32("ProformaInvoiceID");
                }
                reader.Close();
                if (nTempProformaInvoiceID<=0)
                {

                     //Entry Master LC Detail with this PI
                    MasterLCDetail oMasterLCDetail = new MasterLCDetail();
                    oMasterLCDetail.MasterLCID = oCommercialInvoice.MasterLCID;
                    oMasterLCDetail.PIStatus = EnumPIStatus.Approved; oMasterLCDetail.BuyerID = oCommercialInvoice.BuyerID; 

                    ProformaInvoice oProformaInvoice = new ProformaInvoice();
                    oProformaInvoice.BUID = oCommercialInvoice.BUID;
                    oProformaInvoice.PIStatus = EnumPIStatus.Approved; 
                    oProformaInvoice.BuyerID = oProformaInvoice.ApplicantID = oCommercialInvoice.BuyerID; 
                    oProformaInvoice.ApprovedBy = (int)nUserID;
                    //oProformaInvoice.GrossAmount = oProformaInvoice.NetAmount = oCommercialInvoiceDetails.Sum(x => x.Amount);
                    oProformaInvoice.CurrencyID = oCommercialInvoice.CurrencyID;
                    reader = ProformaInvoiceDA.InsertUpdate(tc, oProformaInvoice, EnumDBOperation.Insert, nUserID);
                    NullHandler oReaderPI = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oMasterLCDetail.ProformaInvoiceID = oReaderPI.GetInt32("ProformaInvoiceID");
                        oMasterLCDetail.PINo = oReaderPI.GetString("PINo"); 
                    }
                    reader.Close();
                    //insert Master LC Detail
                    reader = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Insert, nUserID,"");//insert MLC Detail
                    reader.Close();
                   
                }
                #endregion

                if (oCommercialInvoice.CommercialInvoiceID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialInvoice, EnumRoleOperationType.Add);
                    reader = CommercialInvoiceDA.InsertUpdate(tc, oCommercialInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialInvoice, EnumRoleOperationType.Edit);
                    reader = CommercialInvoiceDA.InsertUpdate(tc, oCommercialInvoice, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoice = new CommercialInvoice();
                    oCommercialInvoice = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region CommercialInvoice Detail Part
                if (oCommercialInvoiceDetails != null)
                {
                    foreach (CommercialInvoiceDetail oItem in oCommercialInvoiceDetails)
                    {
                        IDataReader readerdetail;
                        oItem.CommercialInvoiceID = oCommercialInvoice.CommercialInvoiceID;
                        if (oItem.CommercialInvoiceDetailID <= 0)
                        {
                            readerdetail = CommercialInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = CommercialInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sCommercialInvoiceDetailIDs = sCommercialInvoiceDetailIDs + oReaderDetail.GetString("CommercialInvoiceDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sCommercialInvoiceDetailIDs.Length > 0)
                    {
                        sCommercialInvoiceDetailIDs = sCommercialInvoiceDetailIDs.Remove(sCommercialInvoiceDetailIDs.Length - 1, 1);
                    }
                    oCommercialInvoiceDetail = new CommercialInvoiceDetail();
                    oCommercialInvoiceDetail.CommercialInvoiceID = oCommercialInvoice.CommercialInvoiceID;
                    CommercialInvoiceDetailDA.Delete(tc, oCommercialInvoiceDetail, EnumDBOperation.Delete, nUserID, sCommercialInvoiceDetailIDs);

                }

                #endregion

                #region CommercialInvoice Get
                reader = CommercialInvoiceDA.Get(tc, oCommercialInvoice.CommercialInvoiceID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoice = CreateObject(oReader);
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
                oCommercialInvoice.ErrorMessage = Message;

                #endregion
            }
            return oCommercialInvoice;
        }

        public string Delete(int nCommercialInvoiceID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CommercialInvoice oCommercialInvoice = new CommercialInvoice();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.CommercialInvoice, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, EnumModuleName.CommercialInvoice.ToString(), nCommercialInvoiceID);
                oCommercialInvoice.CommercialInvoiceID = nCommercialInvoiceID;
                CommercialInvoiceDA.Delete(tc, oCommercialInvoice, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

              #endregion
            }
            return Global.DeleteMessage;
        }

        public CommercialInvoice Approval(CommercialInvoice oCommercialInvoice, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
    
                #region CommercialInvoice part
                IDataReader reader;
                if (oCommercialInvoice.ApprovedBy==0) //Approve Region
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialInvoice, EnumRoleOperationType.Approved);
                    reader = CommercialInvoiceDA.InsertUpdate(tc, oCommercialInvoice, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    //Undo Approve Region
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.CommercialInvoice, EnumRoleOperationType.UnApproved);
                    reader = CommercialInvoiceDA.InsertUpdate(tc, oCommercialInvoice, EnumDBOperation.UnApproval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoice = new CommercialInvoice();
                    oCommercialInvoice = CreateObject(oReader);
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
                oCommercialInvoice.ErrorMessage = Message;

                #endregion
            }
            return oCommercialInvoice;
        }
            
       public CommercialInvoice ChangeField(CommercialInvoice oCommercialInvoice,Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region CommercialInvoice part
                CommercialInvoiceDA.ChangeField(tc, oCommercialInvoice, nUserID);
                IDataReader reader = CommercialInvoiceDA.Get(tc, oCommercialInvoice.CommercialInvoiceID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommercialInvoice = new CommercialInvoice();
                    oCommercialInvoice = CreateObject(oReader);
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
                oCommercialInvoice.ErrorMessage = Message;

                #endregion
            }
            return oCommercialInvoice;
        }
        public CommercialInvoice Get(int id, Int64 nUserId)
        {
            CommercialInvoice oAccountHead = new CommercialInvoice();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommercialInvoiceDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<CommercialInvoice> Gets(Int64 nUserID)
        {
            List<CommercialInvoice> oCommercialInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDA.Gets(tc);
                oCommercialInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }

            return oCommercialInvoice;
        }
        public List<CommercialInvoice> GetsByTransfer(int id, Int64 nUserID)
        {
            List<CommercialInvoice> oCommercialInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDA.GetsByTransfer(tc,id);
                oCommercialInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }

            return oCommercialInvoice;
        }
        
        public List<CommercialInvoice> GetsByLC(int id, Int64 nUserID)
        {
            List<CommercialInvoice> oCommercialInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDA.GetsByLC(tc, id);
                oCommercialInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }

            return oCommercialInvoice;
        }
        public List<CommercialInvoice> Gets(string sSQL, Int64 nUserID)
        {
            List<CommercialInvoice> oCommercialInvoice = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommercialInvoiceDA.Gets(tc, sSQL);
                oCommercialInvoice = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommercialInvoice", e);
                #endregion
            }

            return oCommercialInvoice;
        }
        #endregion
    }
}
