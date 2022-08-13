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
   public class ExportFollowupService :MarshalByRefObject ,IExportFollowupService
    {
        #region Private functions and declaration
        private static ExportFollowup MapObject(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            oExportFollowup.BUID = oReader.GetInt32("BUID");
            oExportFollowup.BUName = oReader.GetString("BUName");
            oExportFollowup.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportFollowup.Currency = oReader.GetString("Currency");
            oExportFollowup.Amount_SaleBudget = oReader.GetDouble("Amount_SaleBudget");
            oExportFollowup.Qty_Production = oReader.GetDouble("Qty_Production");
            oExportFollowup.Qty_Delivery_In = oReader.GetDouble("Qty_Delivery_In");
            oExportFollowup.Qty_Delivery_Out = oReader.GetDouble("Qty_Delivery_Out");
            oExportFollowup.Amount_Delivery = oReader.GetDouble("Amount_Delivery");
            oExportFollowup.Amount_Delivery_BC = oReader.GetDouble("Amount_Delivery_BC");
            oExportFollowup.Qty_PI = oReader.GetDouble("Qty_PI");
            oExportFollowup.Amount_PI = oReader.GetDouble("Amount_PI");
            oExportFollowup.Count_PI = oReader.GetDouble("Count_PI");
            oExportFollowup.Count_Cash = oReader.GetDouble("Count_Cash");
            oExportFollowup.Qty_Cash = oReader.GetDouble("Qty_Cash");
            oExportFollowup.Amount_Cash = oReader.GetDouble("Amount_Cash");
            oExportFollowup.Amount_Due = oReader.GetDouble("Amount_Due");
            oExportFollowup.Amount_ODue = oReader.GetDouble("Amount_ODue");
            oExportFollowup.LCQty = oReader.GetDouble("LCQty");
            oExportFollowup.Amount_LC = oReader.GetDouble("Amount_LC");            
            oExportFollowup.Count_LC = oReader.GetDouble("Count_LC");

            oExportFollowup.Name = oReader.GetString("Name");
            oExportFollowup.Name_R = oReader.GetString("Name_R");
            oExportFollowup.Name_Y = oReader.GetString("Name_Y");
            oExportFollowup.Part = oReader.GetInt16("Part");
            oExportFollowup.Count = oReader.GetInt16("Count");
            oExportFollowup.Count_R = oReader.GetInt16("Count_R");
            oExportFollowup.Count_Y = oReader.GetInt16("Count_Y");
            oExportFollowup.Qty = oReader.GetDouble("Qty");
            oExportFollowup.Qty_R = oReader.GetDouble("Qty_R");
            oExportFollowup.Qty_Y = oReader.GetDouble("Qty_Y");
            oExportFollowup.Amount = oReader.GetDouble("Amount");
            oExportFollowup.Amount_R = oReader.GetDouble("Amount_R");
            oExportFollowup.Amount_Y = oReader.GetDouble("Amount_Y");
            oExportFollowup.Shipment_Date = oReader.GetDateTime("Shipment_Date");            
            oExportFollowup.DeliveryChallanQty = oReader.GetDouble("DeliveryChallanQty");
            return oExportFollowup;
        }

        public static ExportFollowup CreateObject(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = MapObject(oReader);
            return oExportFollowup;
        }

        private List<ExportFollowup> CreateObjects(IDataReader oReader)
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportFollowup oItem = CreateObject(oHandler);
                oExportFollowups.Add(oItem);
            }
            return oExportFollowups;
        }

        #endregion
        #region Summary
        private static ExportFollowup MapObject_Sum(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            oExportFollowup.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportFollowup.Currency = oReader.GetString("Currency");
            oExportFollowup.BankBranchID = oReader.GetInt32("BankBranchID");
            oExportFollowup.BankName = oReader.GetString("BankName");
            oExportFollowup.Currency = oReader.GetString("Currency");
            oExportFollowup.BUID = oReader.GetInt32("BUID");
            oExportFollowup.BUName = oReader.GetString("BUName");
            oExportFollowup.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportFollowup.BOinHand = oReader.GetDouble("BOinHand");
            oExportFollowup.BOInCusHand = oReader.GetDouble("BOInCusHand");
            oExportFollowup.AcceptadBill = oReader.GetDouble("AcceptadBill");
            oExportFollowup.NegoTransit = oReader.GetDouble("NegoTransit");
            oExportFollowup.NegotiatedBill = oReader.GetDouble("NegotiatedBill");
            oExportFollowup.Discounted = oReader.GetDouble("Discounted");
            oExportFollowup.PaymentDone = oReader.GetDouble("PaymentDone");
            oExportFollowup.BFDDRecd = oReader.GetDouble("BFDDRecd");
            oExportFollowup.Amount_Due = oReader.GetDouble("Amount_Due");
            oExportFollowup.Amount_ODue = oReader.GetDouble("Amount_ODue");
            oExportFollowup.Amount = oReader.GetDouble("Amount");
            oExportFollowup.Amount_Bill = oReader.GetDouble("Amount_Bill");
            oExportFollowup.Qty_Bill = oReader.GetDouble("Qty_Bill");

            oExportFollowup.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportFollowup.ContractorID = oReader.GetInt32("ContractorID");
            oExportFollowup.ContractorName = oReader.GetString("ContractorName");
            oExportFollowup.BBranchID_Nego = oReader.GetInt32("BBranchID_Nego");
            oExportFollowup.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportFollowup.SName_Nego = oReader.GetString("SName_Nego");
            oExportFollowup.BBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportFollowup.Currency = oReader.GetString("Currency");
            oExportFollowup.BBranchID_Issue = oReader.GetInt32("BBranchID_Issue");
            oExportFollowup.BankName_Issue = oReader.GetString("BankName_Issue");
            oExportFollowup.SName_Issue = oReader.GetString("SName_Issue");
            oExportFollowup.BBranchName_Issue = oReader.GetString("BBranchName_Issue");

            oExportFollowup.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportFollowup.DeliveryQty = oReader.GetDouble("DeliveryQty");
            oExportFollowup.YetToInvoice = oReader.GetDouble("YetToInvoice");                                                                                                                                                                                     
            oExportFollowup.DeliveryValue = oReader.GetDouble("DeliveryValue");
            oExportFollowup.State = (EnumLCBillEvent)oReader.GetInt32("State");
            oExportFollowup.LCStatus = oReader.GetInt32("LCStatus");
            oExportFollowup.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportFollowup.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportFollowup.LCFileNo = oReader.GetString("LCFileNo");

            oExportFollowup.Qty = oReader.GetDouble("Qty");
            oExportFollowup.StartDate = oReader.GetDateTime("StartDate");
            oExportFollowup.SendToParty = oReader.GetDateTime("SendToParty");
            oExportFollowup.RecdFromParty = oReader.GetDateTime("RecdFromParty");
            oExportFollowup.SendToBank = oReader.GetDateTime("SendToBank");
            oExportFollowup.OverdueRate = oReader.GetDouble("OverdueRate");
            oExportFollowup.LCOpeningDate = oReader.GetDateTime("LCOpeningDate");
            oExportFollowup.MKTPName = oReader.GetString("MKTPName");
            oExportFollowup.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportFollowup.MaturityReceivedDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportFollowup.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            oExportFollowup.LDBCNo = oReader.GetString("LDBCNo");
            oExportFollowup.LDBPNo = oReader.GetString("LDBPNo");
            oExportFollowup.LDBPAmount = oReader.GetDouble("LDBPAmount");
            oExportFollowup.LDBCDate = oReader.GetDateTime("LDBCDate");
            oExportFollowup.AcceptanceDate = oReader.GetDateTime("AcceptanceDate");
            oExportFollowup.DiscountedDate = oReader.GetDateTime("DiscountedDate");
            oExportFollowup.BankFDDRecDate = oReader.GetDateTime("BankFDDRecDate");
            oExportFollowup.RelizationDate = oReader.GetDateTime("RelizationDate");
            oExportFollowup.EncashmentDate = oReader.GetDateTime("EncashmentDate");
            oExportFollowup.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oExportFollowup.Shipment_Date = oReader.GetDateTime("Shipment_Date");            
            oExportFollowup.DeliveryChallanQty = oReader.GetDouble("DeliveryChallanQty");
            oExportFollowup.PINo = oReader.GetString("PINo");
            oExportFollowup.PIDate = oReader.GetDateTime("PIDate");
            oExportFollowup.LCQty = oReader.GetDouble("LCQty");
            return oExportFollowup;
        }
        public static ExportFollowup CreateObject_Sum(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            oExportFollowup = MapObject_Sum(oReader);
            return oExportFollowup;
        }
        private List<ExportFollowup> CreateObjects_Sum(IDataReader oReader)
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportFollowup oItem = CreateObject_Sum(oHandler);
                oExportFollowups.Add(oItem);
            }
            return oExportFollowups;
        }
        #endregion

        #region Month Amount
        private static ExportFollowup MapObject_Month(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            oExportFollowup.StartDate = oReader.GetDateTime("StartDate");
            oExportFollowup.Amount = oReader.GetDouble("Amount");
            oExportFollowup.Amount_Cash = oReader.GetDouble("Amount_Cash");
            oExportFollowup.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportFollowup.Currency = oReader.GetString("Currency");
            oExportFollowup.BankName = oReader.GetString("BankName");
            oExportFollowup.Bank_Nego = oReader.GetString("Bank_Nego");
           
            return oExportFollowup;
        }
        public static ExportFollowup CreateObject_Month(NullHandler oReader)
        {
            ExportFollowup oExportFollowup = new ExportFollowup();
            oExportFollowup = MapObject_Month(oReader);
            return oExportFollowup;
        }
        private List<ExportFollowup> CreateObjects_Month(IDataReader oReader)
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportFollowup oItem = CreateObject_Month(oHandler);
                oExportFollowups.Add(oItem);
            }
            return oExportFollowups;
        }
        #endregion
        #region Function
        public List<ExportFollowup> GetsExportFollowup(int nBUID, DateTime StartDate ,DateTime Enddate, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = new List<ExportFollowup>();
            ExportFollowup oExportFollowup = new ExportFollowup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.GetsExportFollowup(tc, nBUID, StartDate, Enddate);
                oExportFollowups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oExportFollowup.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oExportFollowups.Add(oExportFollowup);
                #endregion
            }

            return oExportFollowups;
        }
        public List<ExportFollowup> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.Gets(tc, sSQL);
                oExportFollowups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFollowups", e);
                #endregion
            }

            return oExportFollowups;
        }
        public List<ExportFollowup> Gets_Summary(int nBUID, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.Gets_Summary(tc, nBUID);
                oExportFollowups = CreateObjects_Sum(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFollowups", e);
                #endregion
            }

            return oExportFollowups;
        }
        public List<ExportFollowup> Gets_Details(ExportFollowup oExportFollowup, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.Gets_Details(tc, oExportFollowup);
                oExportFollowups = CreateObjects_Sum(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFollowups", e);
                #endregion
            }

            return oExportFollowups;
        }
       public List<ExportFollowup> Gets_BillRealize(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.Gets_BillRealize(tc, nBUID, dStartDate, dEndDate);
                oExportFollowups = CreateObjects_Month(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFollowups", e);
                #endregion
            }

            return oExportFollowups;
        }
        public List<ExportFollowup> Gets_BillMaturity(int nBUID, DateTime dStartDate, DateTime dEndDate, Int64 nUserID)
        {
            List<ExportFollowup> oExportFollowups = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportFollowupDA.Gets_BillMaturity(tc, nBUID, dStartDate, dEndDate);
                oExportFollowups = CreateObjects_Month(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportFollowups", e);
                #endregion
            }

            return oExportFollowups;
        }
        #endregion
    }
}
