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
    public class ExportPIService : MarshalByRefObject, IExportPIService
    {
    
        #region Private functions and declaration
        private static ExportPI MapObject(NullHandler oReader)
        {
            ExportPI oExportPI = new ExportPI();
            oExportPI.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPI.ExportPILogID = oReader.GetInt32("ExportPILogID");
            oExportPI.PaymentType = (EnumPIPaymentType)oReader.GetInt32("PaymentType");
            oExportPI.PaymentTypeInInt = oReader.GetInt32("PaymentType");
            oExportPI.PINo = oReader.GetString("PINo");
            oExportPI.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oExportPI.PIStatusInInt = oReader.GetInt32("PIStatus");
            oExportPI.IssueDate = oReader.GetDateTime("IssueDate");
            oExportPI.ValidityDate = oReader.GetDateTime("ValidityDate");
            oExportPI.ContractorID = oReader.GetInt32("ContractorID");
            oExportPI.BuyerID = oReader.GetInt32("BuyerID");
            oExportPI.MKTEmpID = oReader.GetInt32("MKTEmpID");
            oExportPI.BankBranchID = oReader.GetInt32("BankBranchID");
            oExportPI.BankAccountID = oReader.GetInt32("BankAccountID");
            oExportPI.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportPI.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oExportPI.Qty = oReader.GetDouble("Qty");
            oExportPI.Amount = oReader.GetDouble("Amount");
            oExportPI.IsLIBORRate = oReader.GetBoolean("IsLIBORRate");
            oExportPI.IsBBankFDD = oReader.GetBoolean("IsBBankFDD");
            oExportPI.LCTermID = oReader.GetInt32("LCTermID");
            oExportPI.OverdueRate = oReader.GetDouble("OverdueRate");
            oExportPI.LCOpenDate = oReader.GetDateTime("LCOpenDate");
            oExportPI.DeliveryDate = oReader.GetDateTime("DeliveryDate");
            oExportPI.Note = oReader.GetString("Note");
            oExportPI.NoteTwo = oReader.GetString("NoteTwo");
            oExportPI.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oExportPI.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oExportPI.LCID = oReader.GetInt32("LCID");
            oExportPI.ColorInfo = oReader.GetString("ColorInfo");
            oExportPI.DepthOfShade = oReader.GetString("DepthOfShade");
            oExportPI.YarnCount = oReader.GetString("YarnCount");
            oExportPI.ExportPIPrintSetupID = oReader.GetInt32("ExportPIPrintSetupID");
            oExportPI.BankName = oReader.GetString("BankName");
            oExportPI.BranchName = oReader.GetString("BranchName");
            oExportPI.AccountName = oReader.GetString("AccountName");
            oExportPI.BankAccountNo = oReader.GetString("BankAccountNo");
            oExportPI.DeliveryToName = oReader.GetString("DeliveryToName");
            oExportPI.BranchAddress = oReader.GetString("BranchAddress");
            oExportPI.ContractorName = oReader.GetString("ContractorName");
            oExportPI.BuyerName = oReader.GetString("BuyerName");            
            oExportPI.ContractorAddress = oReader.GetString("ContractorAddress");
            oExportPI.ContractorPhone = oReader.GetString("ContractorPhone");
            oExportPI.ContractorFax = oReader.GetString("ContractorFax");
            oExportPI.ContractorEmail = oReader.GetString("ContractorEmail");
            oExportPI.MKTPName = oReader.GetString("MKTPName");
            oExportPI.MKTPNickName = oReader.GetString("MKTPNickName");
            oExportPI.Currency = oReader.GetString("Currency");
            oExportPI.LCTermsName = oReader.GetString("LCTermsName");
            oExportPI.ContractorContactPersonPhone = oReader.GetString("ContractorContactPersonPhone");    
            oExportPI.OrderSheetID = oReader.GetInt32("OrderSheetID");
            oExportPI.BUName = oReader.GetString("BUName");
            oExportPI.BUShortName = oReader.GetString("BUShortName");
            oExportPI.OrderSheetNo = oReader.GetString("OrderSheetNo");
            oExportPI.DeliveryToAddress = oReader.GetString("DeliveryToAddress");
            oExportPI.ShipmentTerm = (EnumShipmentTerms)oReader.GetInt32("ShipmentTerm");
            oExportPI.ShipmentTermInInt = oReader.GetInt32("ShipmentTerm");
            oExportPI.BUID = oReader.GetInt32("BUID");
            oExportPI.ReviseNo = oReader.GetInt32("ReviseNo");
            oExportPI.PIType = (EnumPIType)oReader.GetInt32("PIType");
            oExportPI.ContractorContactPerson = oReader.GetInt32("ContractorContactPerson");
            oExportPI.BuyerContactPerson = oReader.GetInt32("BuyerContactPerson");
            oExportPI.ContractorContactPersonName = oReader.GetString("ContractorContactPersonName");
            oExportPI.BuyerContactPersonName = oReader.GetString("BuyerContactPersonName");
            oExportPI.RateUnit = oReader.GetInt32("RateUnit");
            oExportPI.YetToProductionOrderQty = oReader.GetDouble("YetToProductionOrderQty");
            oExportPI.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportPI.CurrentStatus_LC = oReader.GetInt32("CurrentStatus_LC");
            //oExportPI.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportPI.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportPI.AmendmentNo = oReader.GetInt32("AmendmentNo");
            oExportPI.AmendmentRequired = oReader.GetBoolean("AmendmentRequired");
            oExportPI.ProductNature = (EnumProductNature)oReader.GetInt32("ProductNature");
            oExportPI.ConversionRate = oReader.GetDouble("ConversionRate");
            oExportPI.SCRemarks = oReader.GetString("SCRemarks");
            oExportPI.BankChargeInfo = oReader.GetString("BankChargeInfo");
            oExportPI.BankCharge = oReader.GetDouble("BankCharge");
            oExportPI.MasterContactNo = oReader.GetString("MasterContactNo");
            oExportPI.PartyName = oReader.GetString("PartyName");
            oExportPI.PartyAddress = oReader.GetString("PartyAddress");
            oExportPI.AttCount = oReader.GetString("AttCount");
            oExportPI.MotherBuyerID = oReader.GetInt32("MotherBuyerID");
            oExportPI.MotherBuyerName = oReader.GetString("MotherBuyerName");
            oExportPI.PrepareByName = oReader.GetString("UserName");
            oExportPI.OpeningDate = oReader.GetDateTime("OpeningDate");
            
            return oExportPI;

            /*This Mapping Used for : 
                1. View_ExportPI
                2. View_ExPIForPlanning
             */
        }

        public static ExportPI CreateObject(NullHandler oReader)
        {
            ExportPI oExportPI = new ExportPI();
            oExportPI = MapObject(oReader);            
            return oExportPI;
        }

        private List<ExportPI> CreateObjects(IDataReader oReader)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPI oItem = CreateObject(oHandler);
                oExportPIs.Add(oItem);
            }
            return oExportPIs;
        }

        #endregion

        #region Interface implementation
        public ExportPIService() { }        
        public ExportPI Get(int id, Int64 nUserId)
        {
            ExportPI oExportPI = new ExportPI();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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

            return oExportPI;
        }
        public ExportPI GetByLog(int nLogid, Int64 nUserId)
        {
            ExportPI oExportPI = new ExportPI();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDA.GetByLog(tc, nLogid);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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

            return oExportPI;
        }
        
        public ExportPI Get(string sPINo, int nTextTileUnit, Int64 nUserID)
        {
            ExportPI oExportPI = new ExportPI();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDA.Get(tc, sPINo, nTextTileUnit);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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

            return oExportPI;
        }
        public ExportPI Save(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<ExportPITandCClause> oExportPITandCClauses = new List<ExportPITandCClause>();
                oExportPITandCClauses = oExportPI.ExportPITandCClauses;
                List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
                List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
                ExportPIDetail  oExportPIDetail = new ExportPIDetail();
                oExportPIDetails = oExportPI.ExportPIDetails;
                oPISizerBreakDowns = oExportPI.PISizerBreakDowns;
                List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
                oMasterPIMappings = oExportPI.MasterPIMappings;
                string sMasterPIMappingIDs = "";
                string sDetailIDs = "", sBreakDownIDs = "" ;
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPI.ExportPIID <= 0)
                {
                    reader = ExportPIDA.InsertUpdate(tc, oExportPI, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    DBOperationArchiveDA.Insert(tc, EnumDBOperation.Update, EnumModuleName.ExportPI, oExportPI.ExportPIID, "View_ExportPI", "ExportPIID", "BUID", "PINo", nUserID);
                    reader = ExportPIDA.InsertUpdate(tc, oExportPI, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                #region Export Pi Detail Part
                foreach (ExportPIDetail oItem in oExportPIDetails)
                {
                    IDataReader readerdetail;
                    oItem.ExportPIID = oExportPI.ExportPIID;
                    if (oItem.ExportPIDetailID <= 0)
                    {
                        readerdetail = ExportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                    }
                    else
                    {
                        readerdetail = ExportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDetailIDs = sDetailIDs + oReaderDetail.GetString("ExportPIDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDetailIDs.Length > 0)
                {
                    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                }
                oExportPIDetail = new ExportPIDetail();
                oExportPIDetail.ExportPIID = oExportPI.ExportPIID;
                ExportPIDetailDA.Delete(tc, oExportPIDetail, EnumDBOperation.Delete, nUserID, sDetailIDs);
                #endregion

                #region Export Pi Mapping Part
                if (oExportPI.PIType==EnumPIType.MasterPI &&  oMasterPIMappings.Count > 0)
                {
                    foreach (MasterPIMapping oItem in oMasterPIMappings)
                    {
                        IDataReader readerMasterPIMapping;
                        oItem.MasterPIID = oExportPI.ExportPIID;
                        if (oItem.MasterPIMappingID <= 0)
                        {
                            readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerMasterPIMapping);
                        if (readerMasterPIMapping.Read())
                        {
                            sMasterPIMappingIDs = sMasterPIMappingIDs + oReaderDetail.GetString("MasterPIMappingID") + ",";
                        }
                        readerMasterPIMapping.Close();
                    }
                    if (sMasterPIMappingIDs.Length > 0)
                    {
                        sMasterPIMappingIDs = sMasterPIMappingIDs.Remove(sMasterPIMappingIDs.Length - 1, 1);
                    }
                    MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                    oMasterPIMapping.MasterPIID = oExportPI.ExportPIID;
                    MasterPIMappingDA.Delete(tc, oMasterPIMapping, EnumDBOperation.Delete, nUserID, sMasterPIMappingIDs);
                }
                #endregion

                #region Export Pi Sizer BreakDown Part

                    foreach (PISizerBreakDown oItem in oPISizerBreakDowns)
                    {
                        IDataReader readerBreakDown;
                        oItem.ExportPIID = oExportPI.ExportPIID;
                        if (oItem.PISizerBreakDownID <= 0)
                        {
                            readerBreakDown = PISizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerBreakDown = PISizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderBreakDown = new NullHandler(readerBreakDown);
                        if (readerBreakDown.Read())
                        {
                            sBreakDownIDs = sBreakDownIDs + oReaderBreakDown.GetString("PISizerBreakDownID") + ",";
                        }
                        readerBreakDown.Close();
                    }
                    if (sBreakDownIDs.Length > 0)
                    {
                        sBreakDownIDs = sBreakDownIDs.Remove(sBreakDownIDs.Length - 1, 1);
                    }
                    PISizerBreakDown oPISizerBreakDown = new PISizerBreakDown();
                    oPISizerBreakDown.ExportPIID = oExportPI.ExportPIID;
                    PISizerBreakDownDA.Delete(tc, oPISizerBreakDown, EnumDBOperation.Delete, nUserID, sBreakDownIDs);

                #endregion
                #region Terms & Condition Part
                if (oExportPITandCClauses != null)
                {
                    string sExportPITandCClauseIDs = "";
                    foreach (ExportPITandCClause oItem in oExportPITandCClauses)
                    {
                        IDataReader readertnc;
                        oItem.ExportPIID = oExportPI.ExportPIID;
                        if (oItem.ExportPITandCClauseID <= 0)
                        {
                            readertnc = ExportPITandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readertnc = ExportPITandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sExportPITandCClauseIDs = sExportPITandCClauseIDs + oReaderTNC.GetString("ExportPITandCClauseID") + ",";
                        }
                        readertnc.Close();
                    }
                    if (sExportPITandCClauseIDs.Length > 0)
                    {
                        sExportPITandCClauseIDs = sExportPITandCClauseIDs.Remove(sExportPITandCClauseIDs.Length - 1, 1);                    
                    }
                    ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
                    oExportPITandCClause.ExportPIID = oExportPI.ExportPIID;
                    ExportPITandCClauseDA.Delete(tc, oExportPITandCClause, EnumDBOperation.Delete, nUserID, sExportPITandCClauseIDs);
                }
                #endregion             

                #region Get PI
                reader = ExportPIDA.Get(tc, oExportPI.ExportPIID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oExportPI;
        }
        //SavePIMapping
        public ExportPI SavePIMapping(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
                oMasterPIMappings = oExportPI.MasterPIMappings;
                string sMasterPIMappingIDs = "";
                tc = TransactionContext.Begin(true);
               
                #region Export Pi Mapping Part
                foreach (MasterPIMapping oItem in oMasterPIMappings)
                {
                    IDataReader readerMasterPIMapping;
                    oItem.MasterPIID = oExportPI.ExportPIID;
                    if (oItem.MasterPIMappingID <= 0)
                    {
                        readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerMasterPIMapping);
                    if (readerMasterPIMapping.Read())
                    {
                        sMasterPIMappingIDs = sMasterPIMappingIDs + oReaderDetail.GetString("MasterPIMappingID") + ",";
                    }
                    readerMasterPIMapping.Close();
                }
                if (sMasterPIMappingIDs.Length > 0)
                {
                    sMasterPIMappingIDs = sMasterPIMappingIDs.Remove(sMasterPIMappingIDs.Length - 1, 1);
                }
                MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                oMasterPIMapping.MasterPIID = oExportPI.ExportPIID;
                MasterPIMappingDA.Delete(tc, oMasterPIMapping, EnumDBOperation.Delete, nUserID, sMasterPIMappingIDs);
                #endregion
                #region Get PI
                IDataReader reader = ExportPIDA.Get(tc, oExportPI.ExportPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to . Because of " + e.Message, e);
                #endregion
            }
            return oExportPI;
        }
        public ExportPI UpdatePIInfo(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIDA.UpdatePIInfo(tc, oExportPI);
                 IDataReader reader;
                reader = ExportPIDA.Get(tc,oExportPI.ExportPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message;
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public ExportPI UpdatePIStatus(ExportPI oExportPI, Int64 nUserID)
        {            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.UpdatePIStatus(tc, oExportPI, nUserID);                
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message;
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public ExportPI UpdatePaymentType(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.UpdatePaymentType(tc, oExportPI, nUserID, EnumDBOperation.Insert);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message;
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public ExportPI UpdatePINo(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.UpdatePINo(tc, oExportPI, nUserID, EnumDBOperation.Insert);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message;
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public ExportPI Approve(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.Approve(tc, oExportPI, nUserID, EnumDBOperation.Approval);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message;
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public string Delete(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DBOperationArchiveDA.Insert(tc, EnumDBOperation.Delete, EnumModuleName.ExportPI, oExportPI.ExportPIID, "View_ExportPI", "ExportPIID", "BUID", "PINo", nUserID);
                ExportPIDA.Delete(tc, oExportPI, EnumDBOperation.Delete, nUserID);                
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
        public ExportPI CancelPI(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.InsertUpdate(tc, oExportPI, EnumDBOperation.None, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                    oExportPI.ErrorMessage = "Cancelled Successfully";
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
            return oExportPI;
        }
        public List<ExportPI> Gets(Int64 nUserId)
        {
            List<ExportPI> oExportPIs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.Gets(tc);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public List<ExportPI> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportPI> oExportPIs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.Gets(tc, sSQL);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public List<ExportPI> GetsByPIIDs(string sIDs, Int64 nUserId)
        {
            List<ExportPI> oExportPIs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.GetsByPIIDs(tc, sIDs);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public List<ExportPI> GetsWaitForApproval(Int64 nUserId)
        {
            List<ExportPI> oExportPIs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.GetsWaitForApproval(tc);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public ExportPI GetLogID(int nLogID)
        {
            ExportPI oExportPI = new ExportPI();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportPIDA.GetLogID(tc, nLogID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPI", e);
                #endregion
            }

            return oExportPI;
        }
        public List<ExportPI> GetsLog(int nPIID, Int64 nUserID)
        {
            List<ExportPI> oExportPIs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIDA.GetsLog(tc, nPIID);
                oExportPIs = CreateObjects(reader);
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
            return oExportPIs;
        }
        public List<ExportPI> Gets(int nContractorID, string sLCIDs, Int64 nUserID)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.Gets(tc, nContractorID, sLCIDs);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public List<ExportPI> Gets(int nContractorID, string sLCIDs, bool bPaymentType, Int64 nUserID)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.Gets(tc, nContractorID, sLCIDs, bPaymentType);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }    
        public List<ExportPI> GetsByLCID(int nLCID, Int64 nUserID)
        {
            List<ExportPI> oExportPIs = new List<ExportPI>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportPIDA.GetsByLCID(tc, nLCID);
                oExportPIs = CreateObjects(reader);
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

            return oExportPIs;
        }
        public ExportPI Copy(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.Copy(tc, oExportPI, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }
        public ExportPI PISWAP(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ExportPIDA.PISWAP(tc, oExportPI, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPI = new ExportPI();
                    oExportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportPI;
        }               
        public ExportPI AcceptExportPIRevise(ExportPI oExportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIDetail oExportPIDetail = new ExportPIDetail();
                ExportPITandCClause oExportPITandCClause = new ExportPITandCClause();
                List<ExportPIDetail> oExportPIDetails = new List<ExportPIDetail>();
                List<ExportPITandCClause> oExportPITandCClauses = new List<ExportPITandCClause>();
                List<PISizerBreakDown> oPISizerBreakDowns = new List<PISizerBreakDown>();
                oExportPIDetails = oExportPI.ExportPIDetails;
                oExportPITandCClauses = oExportPI.ExportPITandCClauses;
                oPISizerBreakDowns = oExportPI.PISizerBreakDowns;
                List<MasterPIMapping> oMasterPIMappings = new List<MasterPIMapping>();
                oMasterPIMappings = oExportPI.MasterPIMappings;
                string sMasterPIMappingIDs = "";
                string sExportPIDetailIDs = "", sBreakDownIDs = "" ;
                string sExportPITandCClauseIDs = "";


                if (oExportPI.ExportPIID > 0)
                {
                    #region Export PI
                    IDataReader reader;
                    reader = ExportPIDA.AcceptExportPIRevise(tc, oExportPI, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportPI = new ExportPI();
                        oExportPI = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion

                    #region Export PI Detail
                    if (oExportPIDetails != null)
                    {
                        foreach (ExportPIDetail oItem in oExportPIDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ExportPIID = oExportPI.ExportPIID;
                            if (oItem.ExportPIDetailID <= 0)
                            {
                                readerdetail = ExportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ExportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sExportPIDetailIDs = sExportPIDetailIDs + oReaderDetail.GetString("ExportPIDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sExportPIDetailIDs.Length > 0)
                        {
                            sExportPIDetailIDs = sExportPIDetailIDs.Remove(sExportPIDetailIDs.Length - 1, 1);
                        }
                        oExportPIDetail = new ExportPIDetail();
                        oExportPIDetail.ExportPIID = oExportPI.ExportPIID;
                        ExportPIDetailDA.Delete(tc, oExportPIDetail, EnumDBOperation.Delete, nUserID, sExportPIDetailIDs);
                    }
                    #endregion

                    #region Export Pi Mapping Part
                    if (oExportPI.PIType == EnumPIType.MasterPI && oMasterPIMappings.Count > 0)
                    {
                        foreach (MasterPIMapping oItem in oMasterPIMappings)
                        {
                            IDataReader readerMasterPIMapping;
                            oItem.MasterPIID = oExportPI.ExportPIID;
                            if (oItem.MasterPIMappingID <= 0)
                            {
                                readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerMasterPIMapping = MasterPIMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerMasterPIMapping);
                            if (readerMasterPIMapping.Read())
                            {
                                sMasterPIMappingIDs = sMasterPIMappingIDs + oReaderDetail.GetString("MasterPIMappingID") + ",";
                            }
                            readerMasterPIMapping.Close();
                        }
                        if (sMasterPIMappingIDs.Length > 0)
                        {
                            sMasterPIMappingIDs = sMasterPIMappingIDs.Remove(sMasterPIMappingIDs.Length - 1, 1);
                        }
                        MasterPIMapping oMasterPIMapping = new MasterPIMapping();
                        oMasterPIMapping.MasterPIID = oExportPI.ExportPIID;
                        MasterPIMappingDA.Delete(tc, oMasterPIMapping, EnumDBOperation.Delete, nUserID, sMasterPIMappingIDs);
                    }
                    #endregion

                    #region Export Pi Sizer BreakDown Part

                    foreach (PISizerBreakDown oItem in oPISizerBreakDowns)
                    {
                        IDataReader readerBreakDown;
                        oItem.ExportPIID = oExportPI.ExportPIID;
                        if (oItem.PISizerBreakDownID <= 0)
                        {
                            readerBreakDown = PISizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerBreakDown = PISizerBreakDownDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderBreakDown = new NullHandler(readerBreakDown);
                        if (readerBreakDown.Read())
                        {
                            sBreakDownIDs = sBreakDownIDs + oReaderBreakDown.GetString("PISizerBreakDownID") + ",";
                        }
                        readerBreakDown.Close();
                    }
                    if (sBreakDownIDs.Length > 0)
                    {
                        sBreakDownIDs = sBreakDownIDs.Remove(sBreakDownIDs.Length - 1, 1);
                    }
                    PISizerBreakDown oPISizerBreakDown = new PISizerBreakDown();
                    oPISizerBreakDown.ExportPIID = oExportPI.ExportPIID;
                    PISizerBreakDownDA.Delete(tc, oPISizerBreakDown, EnumDBOperation.Delete, nUserID, sBreakDownIDs);

                    #endregion

                    #region ExportPI Terms and CClause
                    if (oExportPITandCClauses != null)
                    {
                        foreach (ExportPITandCClause oItem in oExportPITandCClauses)
                        {
                            IDataReader readerTermsAndCondition;
                            oItem.ExportPIID = oExportPI.ExportPIID;
                            if (oItem.ExportPITandCClauseID <= 0)
                            {
                                readerTermsAndCondition = ExportPITandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerTermsAndCondition = ExportPITandCClauseDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderTermsAndCondition = new NullHandler(readerTermsAndCondition);
                            if (readerTermsAndCondition.Read())
                            {
                                sExportPITandCClauseIDs = sExportPITandCClauseIDs + oReaderTermsAndCondition.GetString("ExportPITandCClauseID") + ",";
                            }
                            readerTermsAndCondition.Close();
                        }
                        if (sExportPITandCClauseIDs.Length > 0)
                        {
                            sExportPITandCClauseIDs = sExportPITandCClauseIDs.Remove(sExportPITandCClauseIDs.Length - 1, 1);
                        }
                        oExportPITandCClause = new ExportPITandCClause();
                        oExportPITandCClause.ExportPIID = oExportPI.ExportPIID;
                        ExportPITandCClauseDA.Delete(tc, oExportPITandCClause, EnumDBOperation.Delete, nUserID, sExportPITandCClauseIDs);
                    }
                    #endregion

                    #region PI Get
                    reader = ExportPIDA.Get(tc, oExportPI.ExportPIID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oExportPI = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion

                    tc.End();
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oExportPI.ErrorMessage = Message;
                #endregion
            }
            return oExportPI;
        }               
        #endregion
    }
}
