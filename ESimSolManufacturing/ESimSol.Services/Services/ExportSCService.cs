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
    public class ExportSCService : MarshalByRefObject, IExportSCService
    {
    
        #region Private functions and declaration
        private static ExportSC MapObject(NullHandler oReader)
        {
            ExportSC oExportSC = new ExportSC();
            oExportSC.ExportSCID = oReader.GetInt32("ExportSCID");
            oExportSC.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportSC.BUID = oReader.GetInt32("BUID");
            //oExportSC.TextileUnitInInt = oReader.GetInt32("TextileUnit");
            oExportSC.PaymentType = (EnumPIPaymentType)oReader.GetInt32("PaymentType");
            oExportSC.PaymentTypeInInt = oReader.GetInt32("PaymentType");
            oExportSC.PINo = oReader.GetString("PINo");
            oExportSC.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportSC.PIStatusInInt = oReader.GetInt32("PIStatus");
            oExportSC.IssueDate = oReader.GetDateTime("IssueDate");
            oExportSC.SCDate = oReader.GetDateTime("SCDate");
            oExportSC.ValidityDate = oReader.GetDateTime("ValidityDate");
            oExportSC.ContractorID = oReader.GetInt32("ContractorID");
            oExportSC.BuyerID = oReader.GetInt32("BuyerID");
            oExportSC.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportSC.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportSC.Qty_PI = oReader.GetDouble("Qty_PI");
            oExportSC.Amount_PI = oReader.GetDouble("Amount_PI");

            oExportSC.AdjQty = oReader.GetDouble("AdjQty");
            oExportSC.AdjAmount = oReader.GetDouble("AdjAmount");
            oExportSC.TotalAmount = oReader.GetDouble("TotalAmount");
            oExportSC.TotalQty = oReader.GetDouble("TotalQty");

            oExportSC.SampleInvoiceAdjAmount = oReader.GetDouble("SampleInvoiceAdjAmount");
            oExportSC.AdjManualy = oReader.GetDouble("AdjManualy");

            oExportSC.LCOpenDate = oReader.GetDateTime("AppLCOpenDate");
            oExportSC.DeliveryDate = oReader.GetDateTime("AppDeliveryDate");
            oExportSC.Note = oReader.GetString("Note");
            oExportSC.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oExportSC.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oExportSC.LCID = oReader.GetInt32("LCID");
            oExportSC.ColorInfo = oReader.GetString("ColorInfo");
            oExportSC.DepthOfShade = oReader.GetString("DepthOfShade");
            oExportSC.ContractorName = oReader.GetString("ContractorName");
            oExportSC.BuyerName = oReader.GetString("BuyerName");
         
            oExportSC.ContractorAddress = oReader.GetString("ContractorAddress");
            oExportSC.ContractorPhone = oReader.GetString("ContractorPhone");
            oExportSC.ContractorFax = oReader.GetString("ContractorFax");
            oExportSC.ContractorEmail = oReader.GetString("ContractorEmail");
            oExportSC.MKTPName = oReader.GetString("MKTPName");
            oExportSC.MKTPNickName = oReader.GetString("MKTPNickName");
            oExportSC.Currency = oReader.GetString("Currency");
            oExportSC.BankBranchID = oReader.GetInt32("BankBranchID");
            oExportSC.BranchName = oReader.GetString("BranchName");

            oExportSC.IsRevisePI = oReader.GetBoolean("IsRevisePI");
            oExportSC.IsClose = oReader.GetBoolean("IsClose");
            oExportSC.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportSC.CurrentStatus_LC = oReader.GetInt32("CurrentStatus_LC");
            oExportSC.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportSC.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportSC.AmendmentNo = oReader.GetInt32("AmendmentNo");
            oExportSC.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");

            oExportSC.ApprovedName = oReader.GetString("ApprovedName");
            oExportSC.Production_ControlBy = oReader.GetInt32("Production_ControlBy");
            oExportSC.Delivery_ControlBy = oReader.GetInt32("Delivery_ControlBy");

            oExportSC.RateUnit = oReader.GetInt32("RateUnit");
            oExportSC.ProductNature = (EnumProductNature) oReader.GetInt32("ProductNature");
            oExportSC.ProductNatureInInt = oReader.GetInt32("ProductNature");
            oExportSC.YetToProductionOrderQty = oReader.GetDouble("YetToProductionOrderQty");
            oExportSC.DeliveryToName = oReader.GetString("DeliveryToName");
            oExportSC.MotherBuyerName = oReader.GetString("MotherBuyerName");
            oExportSC.MotherBuyerID = oReader.GetInt32("MotherBuyerID");
            oExportSC.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oExportSC.ContractorContactPerson = oReader.GetInt32("ContractorContactPerson");
            oExportSC.PIType = (EnumPIType)oReader.GetInt32("PIType");
            oExportSC.BankName = oReader.GetString("BankName");
            oExportSC.AccountName = oReader.GetString("AccountName");
            oExportSC.BankAccountNo = oReader.GetString("BankAccountNo");

            oExportSC.RateAdjConID = oReader.GetInt32("RateAdjConID");
            oExportSC.QtyAdjConID = oReader.GetInt32("QtyAdjConID");
            oExportSC.DicChargeAdjConID = oReader.GetInt32("DicChargeAdjConID");

            return oExportSC;

         
        }

        public static ExportSC CreateObject(NullHandler oReader)
        {
            ExportSC oExportSC = new ExportSC();
            oExportSC = MapObject(oReader);            
            return oExportSC;
        }

        private List<ExportSC> CreateObjects(IDataReader oReader)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportSC oItem = CreateObject(oHandler);
                oExportSCs.Add(oItem);
            }
            return oExportSCs;
        }

        #endregion

        #region Interface implementation
        public ExportSCService() { }        
        public ExportSC Get(int id, Int64 nUserId)
        {
            ExportSC oExportSC = new ExportSC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oExportSC;
        }
        public ExportSC GetPI(int id, Int64 nUserId)
        {
            ExportSC oExportSC = new ExportSC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.GetPI(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oExportSC;
        }
        public ExportSC Get(string sPINo, int nTextTileUnit, Int64 nUserID)
        {
            ExportSC oExportSC = new ExportSC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.Get(tc, sPINo, nTextTileUnit);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export PI", e);
                #endregion
            }

            return oExportSC;
        }
        public ExportSC Save(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            String sExportSCDetaillIDs = "";
            try
            {
                oExportSCDetails = oExportSC.ExportSCDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportSC.ExportSCID <= 0)
                {
                    reader = ExportSCDA.InsertUpdate(tc, oExportSC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportSCDA.InsertUpdate(tc, oExportSC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();

                #region Terms & Condition Part
                if (oExportSCDetails != null)
                {
                    foreach (ExportSCDetail oItem in oExportSCDetails)
                    {
                        if ((oItem.Qty + oItem.OverQty) > 0)
                        {
                            IDataReader readertnc;
                            oItem.ExportSCID = oExportSC.ExportSCID;
                            if (oItem.ExportSCDetailID <= 0)
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTNC = new NullHandler(readertnc);
                            if (readertnc.Read())
                            {
                                sExportSCDetaillIDs = sExportSCDetaillIDs + oReaderTNC.GetString("ExportSCDetailID") + ",";
                            }
                            readertnc.Close();
                        }
                    }
                    if (sExportSCDetaillIDs.Length > 0)
                    {
                        sExportSCDetaillIDs = sExportSCDetaillIDs.Remove(sExportSCDetaillIDs.Length - 1, 1);
                    }
                    ExportSCDetail oExportSCDetail = new ExportSCDetail();
                    oExportSCDetail.ExportSCID = oExportSC.ExportSCID;
                    ExportSCDetailDA.Delete(tc, oExportSCDetail, EnumDBOperation.Delete, nUserID, sExportSCDetaillIDs);
                    sExportSCDetaillIDs = "";
                }
                #endregion
                                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }               
        public string Delete(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportSCDA.Delete(tc, oExportSC, EnumDBOperation.Delete, nUserID);                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportSC CancelPI(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.InsertUpdate(tc, oExportSC, EnumDBOperation.None, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                    oExportSC.ErrorMessage = "Cancelled Successfully";
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
                throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oExportSC;
        }
        public ExportSC Approved(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.InsertUpdate(tc, oExportSC, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC ApproveSalesContract(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.ApproveSalesContract(tc, oExportSC, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC ExportPIToPIOrderTransfer(int nExportPIID_TO, int nExportPIID_From, Int64 nUserID)
        {
            ExportSC oExportSC = new ExportSC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.ExportPIToPIOrderTransfer(tc,  nExportPIID_TO,  nExportPIID_From, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC UpdateExportSC(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.UpdateExportSC(tc, oExportSC);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC SaveLog(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            String sExportSCDetaillIDs = "";
            try
            {
                oExportSCDetails = oExportSC.ExportSCDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportSC.ExportSCID <= 0)
                {
                    reader = ExportSCDA.InsertUpdateLog(tc, oExportSC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportSCDA.InsertUpdateLog(tc, oExportSC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();

                #region Terms & Condition Part
                if (oExportSCDetails != null)
                {
                    foreach (ExportSCDetail oItem in oExportSCDetails)
                    {
                        if (oItem.Qty > 0)
                        {
                            IDataReader readertnc;
                            oItem.ExportSCID = oExportSC.ExportSCID;
                            if (oItem.ExportSCDetailID <= 0)
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTNC = new NullHandler(readertnc);

                            if (readertnc.Read())
                            {
                                sExportSCDetaillIDs = sExportSCDetaillIDs + oReaderTNC.GetString("ExportSCDetailID") + ",";
                            }
                            readertnc.Close();
                        }
                    }

                    if (sExportSCDetaillIDs.Length > 0)
                    {
                        sExportSCDetaillIDs = sExportSCDetaillIDs.Remove(sExportSCDetaillIDs.Length - 1, 1);
                    }
                    ExportSCDetail oExportSCDetail = new ExportSCDetail();
                    oExportSCDetail.ExportSCID = oExportSC.ExportSCID;
                    ExportSCDetailDA.Delete(tc, oExportSCDetail, EnumDBOperation.Delete, nUserID, sExportSCDetaillIDs);
                    sExportSCDetaillIDs = "";
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }               
        //use for plastic and integrated

        public ExportSC AcceptRevise(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            String sExportSCDetaillIDs = "";
            try
            {
                oExportSCDetails = oExportSC.ExportSCDetails;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportSCDA.AcceptRevise(tc, oExportSC, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();

                #region Details Part
                if (oExportSCDetails != null)
                {
                    foreach (ExportSCDetail oItem in oExportSCDetails)
                    {
                        if (oItem.Qty > 0)
                        {
                            IDataReader readertnc;
                            oItem.ExportSCID = oExportSC.ExportSCID;
                            if (oItem.ExportSCDetailID <= 0)
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readertnc = ExportSCDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTNC = new NullHandler(readertnc);

                            if (readertnc.Read())
                            {
                                sExportSCDetaillIDs = sExportSCDetaillIDs + oReaderTNC.GetString("ExportSCDetailID") + ",";
                            }
                            readertnc.Close();
                        }
                    }

                    if (sExportSCDetaillIDs.Length > 0)
                    {
                        sExportSCDetaillIDs = sExportSCDetaillIDs.Remove(sExportSCDetaillIDs.Length - 1, 1);
                    }
                    ExportSCDetail oExportSCDetail = new ExportSCDetail();
                    oExportSCDetail.ExportSCID = oExportSC.ExportSCID;
                    ExportSCDetailDA.Delete(tc, oExportSCDetail, EnumDBOperation.Delete, nUserID, sExportSCDetaillIDs);
                    sExportSCDetaillIDs = "";
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }               
        public List<ExportSC> Gets(Int64 nUserId)
        {
            List<ExportSC> oExportSCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.Gets(tc);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oExportSCs;
        }
        public List<ExportSC> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportSC> oExportSCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.Gets(tc, sSQL);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oExportSCs;
        }
        public List<ExportSC> GetsByPIIDs(string sIDs, Int64 nUserId)
        {
            List<ExportSC> oExportSCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.GetsByPIIDs(tc, sIDs);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PIs", e);
                #endregion
            }

            return oExportSCs;
        }
     
        public List<ExportSC> GetsWaitForApproval(Int64 nUserId)
        {
            List<ExportSC> oExportSCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.GetsWaitForApproval(tc);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PI", e);
                #endregion
            }

            return oExportSCs;
        }
    
        public ExportSC GetLogID(int nLogID)
        {
            ExportSC oExportSC = new ExportSC();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.GetLogID(tc, nLogID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportSC", e);
                #endregion
            }

            return oExportSC;
        }
        public List<ExportSC> GetsLog(int nPIID, Int64 nUserID)
        {
            List<ExportSC> oExportSCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportSCDA.GetsLog(tc, nPIID);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }
            return oExportSCs;
        }
        public List<ExportSC> Gets(int nContractorID, string sLCIDs, Int64 nUserID)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.Gets(tc, nContractorID, sLCIDs);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }

            return oExportSCs;
        }
        public List<ExportSC> Gets(int nContractorID, string sLCIDs, bool bPaymentType, Int64 nUserID)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.Gets(tc, nContractorID, sLCIDs, bPaymentType);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }

            return oExportSCs;
        }
        public List<ExportSC> GetsByBU(int nBUID, int nProductNature, Int64 nUserID)
        {
            List<ExportSC> oExportSCs = new List<ExportSC>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportSCDA.GetsByBU(tc, nBUID, nProductNature);
                oExportSCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }

            return oExportSCs;
        }
        public ExportSC Save_UP(ExportSC oExportSC, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ExportSCDetail> oExportSCDetails = new List<ExportSCDetail>();
            try
            {
                oExportSCDetails = oExportSC.ExportSCDetails;

                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = ExportSCDA.Get(tc, oExportSC.ExportSCID);
               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = new ExportSC();
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();

                #region Terms & Condition Part
                if (oExportSCDetails != null)
                {
                    foreach (ExportSCDetail oItem in oExportSCDetails)
                    {
                        if (oItem.Qty > 0)
                        {
                            IDataReader readertnc;
                            oItem.ExportSCID = oExportSC.ExportSCID;
                            if (oItem.ExportSCDetailID >0)
                            {
                                ExportSCDetailDA.Save_UP(tc, oItem, nUserID);
                            }
                            
                        }
                    }

                   
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC OrderClose(ExportSC oExportSC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.OrderClose(tc, oExportSC);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        public ExportSC UpdateBuyer(ExportSC oExportSC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportSCDA.UpdateBuyer(tc, oExportSC);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportSC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportSC = new ExportSC();
                oExportSC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportSC;
        }
        #endregion
    }
}
