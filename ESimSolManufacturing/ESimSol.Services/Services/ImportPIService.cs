using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class ImportPIService : MarshalByRefObject, IImportPIService
    {
        #region Private functions and declaration
        private ImportPI MapObject(NullHandler oReader)
        {
            ImportPI oImportPI = new ImportPI();
            oImportPI.ImportPIID = oReader.GetInt32("ImportPIID");
            oImportPI.ImportPILogID = oReader.GetInt32("ImportPILogID");
            oImportPI.ImportPINo = oReader.GetString("ImportPINo");
            oImportPI.ProductTypeName = oReader.GetString("ProductTypeName");
            oImportPI.BUID = oReader.GetInt32("BUID");
            oImportPI.SLNo = oReader.GetString("SLNo");
            oImportPI.IssueDate = oReader.GetDateTime("IssueDate");
            oImportPI.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oImportPI.ImportPIStatus = (EnumImportPIState)oReader.GetInt32("ImportPIStatus");
            oImportPI.ImportPIStatusInt = oReader.GetInt32("ImportPIStatus");
            oImportPI.SupplierID = oReader.GetInt32("SupplierID");
            oImportPI.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oImportPI.ConcernPersonID = oReader.GetInt32("ConcernPersonID");
            oImportPI.CurrencyID = oReader.GetInt32("CurrencyID");
            oImportPI.TotalValue = oReader.GetDouble("TotalValue");
            oImportPI.BankBranchID_Advise = oReader.GetInt32("BankBranchID_Advise");
            oImportPI.LCTermID = oReader.GetInt32("LCTermID");
            oImportPI.ContainerNo = oReader.GetString("ContainerNo");
            oImportPI.ImportPIType = (EnumImportPIType)oReader.GetInt32("ImportPIType");
            oImportPI.ImportPITypeInt = oReader.GetInt32("ImportPIType");
            oImportPI.AskingDeliveryDate = oReader.GetDateTime("AskingDeliveryDate");
            oImportPI.IsTransShipmentAllow = oReader.GetBoolean("IsTransShipmentAllow");
            oImportPI.IsPartShipmentAllow = oReader.GetBoolean("IsPartShipmentAllow");
            oImportPI.OverDueRate = oReader.GetDouble("OverDueRate");
            oImportPI.IsLIBORrate = oReader.GetBoolean("IsLIBORrate");
            oImportPI.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oImportPI.DateOfApproved = oReader.GetDateTime("DateOfApproved");
            oImportPI.VersionNumber = oReader.GetInt32("VersionNumber");
            oImportPI.ValidityDate = oReader.GetDateTime("ValidityDate");
            oImportPI.AskingLCDate = oReader.GetDateTime("AskingLCDate");
            oImportPI.DeliveryClause = oReader.GetString("DeliveryClause");
            oImportPI.PaymentClause = oReader.GetString("PaymentClause");
            oImportPI.ShipmentBy = (EnumShipmentBy)oReader.GetInt32("ShipmentBy");
            oImportPI.ShipmentByInt = oReader.GetInt32("ShipmentBy");
            oImportPI.IsReviseRequest = oReader.GetBoolean("IsReviseRequest");
            oImportPI.PaymentInstructionType = (EnumPaymentInstruction)oReader.GetInt32("PaymentInstructionType");
            oImportPI.ShipmentTerm = (EnumShipmentTerms)oReader.GetInt32("ShipmentTerm");
            oImportPI.ShipmentTermInt = oReader.GetInt32("ShipmentTerm"); 
            oImportPI.PaymentInstructionTypeInt = oReader.GetInt32("PaymentInstructionType");
            oImportPI.Note = oReader.GetString("Note");
            oImportPI.SupplierName = oReader.GetString("SupplierName");
            oImportPI.SupplierNameCode = oReader.GetString("SupplierNameCode");
            oImportPI.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oImportPI.BankName = oReader.GetString("BankName");
            oImportPI.BranchName = oReader.GetString("BranchName");
            oImportPI.SwiftCode = oReader.GetString("SwiftCode");
            oImportPI.CPersonName = oReader.GetString("CPersonName");
            oImportPI.BUCode = oReader.GetString("BUCode");
            oImportPI.BUName = oReader.GetString("BUName");
            oImportPI.LCTermsName = oReader.GetString("LCTermsName");
            oImportPI.ConcernPersonName = oReader.GetString("ConcernPersonName");
            oImportPI.ApproveByName = oReader.GetString("ApproveByName");
            oImportPI.AgentID = oReader.GetInt32("AgentID");
            oImportPI.RateUnit = oReader.GetInt32("RateUnit");
            oImportPI.AgentContactPersonID = oReader.GetInt32("AgentContactPersonID");
            oImportPI.AgentName = oReader.GetString("AgentName");
            oImportPI.AgentContactPersonName = oReader.GetString("AgentContactPersonName");
            oImportPI.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            oImportPI.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPI.RefType = (EnumImportPIRefType)oReader.GetInt32("RefType");
            oImportPI.PIEntryType = (EnumImportPIEntryType)oReader.GetInt32("PIEntryType");
            oImportPI.RefTypeInInt = oReader.GetInt32("RefType");
            oImportPI.PIEntryTypeInInt = oReader.GetInt32("PIEntryType");
            oImportPI.ConvertionRate = oReader.GetDouble("ConvertionRate");
            oImportPI.DiscountAmount = oReader.GetDouble("DiscountAmount");
            oImportPI.Count = oReader.GetInt32("Count");
            return oImportPI;
        }
        private ImportPI CreateObject(NullHandler oReader)
        {
            ImportPI oPC = new ImportPI();
            oPC=MapObject(oReader);
            return oPC;
        }
        private List<ImportPI> CreateObjects(IDataReader oReader)
        {
            List<ImportPI> oPCs = new List<ImportPI>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPI oItem = CreateObject(oHandler);
                oPCs.Add(oItem);
            }
            return oPCs;
        }
        #endregion

        #region Interface implementation
        public ImportPIService() { }


        #region 

        public ImportPI Save(ImportPI oImportPI, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
                List<ImportPIReference> oImportPIReferences = new List<ImportPIReference>();
                ImportPIDetail oImportPIDetail = new ImportPIDetail();
               
                oImportPIDetails = oImportPI.ImportPIDetails;
                
                oImportPIReferences = oImportPI.ImportPIReferenceList;
                string sImportPIDetailIDs = "", sImportPIReferenceIDs = "";

                IDataReader reader;
                if (oImportPI.ImportPIID <= 0)
                {
                    reader = ImportPIDA.InsertUpdate(tc, oImportPI, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportPIDA.InsertUpdate(tc, oImportPI, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = new ImportPI();
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();

                #region ImportPI Detail Part
                if (oImportPIDetails != null)
                {
                    if (oImportPIDetails.Count > 0)
                    {
                        foreach (ImportPIDetail oItem in oImportPIDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ImportPIID = oImportPI.ImportPIID;
                            if (oItem.ImportPIDetailID <= 0)
                            {
                                readerdetail = ImportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                            }
                            else
                            {
                                readerdetail = ImportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sImportPIDetailIDs = sImportPIDetailIDs + oReaderDetail.GetString("ImportPIDetailID") + ",";
                            }
                            readerdetail.Close();
                        }

                        if (sImportPIDetailIDs.Length > 0)
                        {
                            sImportPIDetailIDs = sImportPIDetailIDs.Remove(sImportPIDetailIDs.Length - 1, 1);
                        }
                        oImportPIDetail = new ImportPIDetail();
                        oImportPIDetail.ImportPIID = oImportPI.ImportPIID;
                        ImportPIDetailDA.Delete(tc, oImportPIDetail, EnumDBOperation.Delete, nUserID, sImportPIDetailIDs);
                    }


                }                
                #endregion

                #region ImportPI REfference Part
                if (oImportPIReferences != null)
                {
                    if (oImportPIReferences.Count > 0)
                    {
                        foreach (ImportPIReference oItem in oImportPIReferences)
                        {
                            IDataReader readerReference;
                            oItem.ImportPIID = oImportPI.ImportPIID;
                            if (oItem.ImportPIReferenceID <= 0)
                            {
                                readerReference = ImportPIReferenceDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerReference = ImportPIReferenceDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderReference = new NullHandler(readerReference);
                            if (readerReference.Read())
                            {
                                sImportPIReferenceIDs = sImportPIReferenceIDs + oReaderReference.GetString("ImportPIReferenceID") + ",";
                            }
                            readerReference.Close();
                        }

                        if (sImportPIReferenceIDs.Length > 0)
                        {
                            sImportPIReferenceIDs = sImportPIReferenceIDs.Remove(sImportPIReferenceIDs.Length - 1, 1);
                        }
                        ImportPIReference oImportPIReference = new ImportPIReference();
                        oImportPIReference.ImportPIID = oImportPI.ImportPIID;
                        ImportPIReferenceDA.Delete(tc, oImportPIReference, EnumDBOperation.Delete, nUserID, sImportPIReferenceIDs);
                    }
                }
                #endregion

                #region ImportPI Get
                reader = ImportPIDA.Get(tc, oImportPI.ImportPIID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                //#region DUDyreing Guide Line For Servise Type PI
                //if (oDUDyeingGuideLine.OrderID > 0)
                //{
                //    IDataReader readerDGL;
                //    if (oDUDyeingGuideLine.DUDyeingGuideLineID <= 0)
                //    {
                //        readerDGL = DUDyeingGuideLineDA.InsertUpdate(tc, oDUDyeingGuideLine, EnumDBOperation.Insert, nUserID);
                //    }
                //    else
                //    {
                //        readerDGL = DUDyeingGuideLineDA.InsertUpdate(tc, oDUDyeingGuideLine, EnumDBOperation.Update, nUserID);
                //    }
                //    readerDGL.Close();
                //}
                //#endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportPI.ErrorMessage = e.Message.Split('~')[0]; 
                #endregion
            }

            return oImportPI;

        }

        public String Delete(ImportPI oImportPI, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPIDA.Delete(tc, oImportPI, EnumDBOperation.Delete, nUserID);
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
        public List<ImportPI> GetsByLCID(int nLCID, Int64 nUserID)
        {
            List<ImportPI> oImportPIs = new List<ImportPI>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDA.GetsByLCID(tc, nLCID);
                oImportPIs = CreateObjects(reader);
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

            return oImportPIs;
        }

        public ImportPI ApproveImportPI(ImportPI oImportPI, Int64 nUserID)
        {

            TransactionContext tc = null;
          
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = ImportPIDA.InsertUpdate(tc, oImportPI, EnumDBOperation.Approval, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = new ImportPI();
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();

                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                    if (tc != null)
                        tc.HandleError();

                    oImportPI.ErrorMessage = e.Message.Split('~')[0]; 
                #endregion
            }

            return oImportPI;
        }


        public ImportPI RequestForReviseImportPI(ImportPI oImportPI, Int64 nUserID)
        {

            TransactionContext tc = null;

            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = ImportPIDA.InsertUpdate(tc, oImportPI, EnumDBOperation.Request, nUserID);

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = new ImportPI();
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();

                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                    if (tc != null)
                        tc.HandleError();

                    oImportPI.ErrorMessage = e.Message;
                #endregion
            }

            return oImportPI;
        }

        public ImportPI ReviseImportPI(ImportPI oImportPI, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            List<ImportPIDetail> oImportPIDetails = new List<ImportPIDetail>();
            ImportPIDetail oImportPIDetail = new ImportPIDetail();
            oImportPIDetails = oImportPI.ImportPIDetails;

            string sImportPIDetailIDs = "";
            try
            {
                tc = TransactionContext.Begin(true);
                reader = ImportPIDA.AcceptRevise(tc, oImportPI, EnumDBOperation.Revise, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = new ImportPI();
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();

                #region ImportPI Detail Part
                if (oImportPIDetails != null)
                {
                    if (oImportPIDetails.Count > 0)
                    {
                        foreach (ImportPIDetail oItem in oImportPIDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ImportPIID = oImportPI.ImportPIID;
                            if (oItem.ImportPIDetailID <= 0)
                            {
                                readerdetail = ImportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID,"");
                            }
                            else
                            {
                                readerdetail = ImportPIDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID,"");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sImportPIDetailIDs = sImportPIDetailIDs + oReaderDetail.GetString("ImportPIDetailID") + ",";
                            }
                            readerdetail.Close();
                        }

                        if (sImportPIDetailIDs.Length > 0)
                        {
                            sImportPIDetailIDs = sImportPIDetailIDs.Remove(sImportPIDetailIDs.Length - 1, 1);
                        }
                        oImportPIDetail = new ImportPIDetail();
                        oImportPIDetail.ImportPIID = oImportPI.ImportPIID;
                        ImportPIDetailDA.Delete(tc, oImportPIDetail, EnumDBOperation.Delete, nUserID, sImportPIDetailIDs);
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

                oImportPI.ErrorMessage = e.Message;
                #endregion
            }

            return oImportPI;
        }
        

        #endregion



        public ImportPI Get(int nImportPIID,Int64 nUserId)
        {
            ImportPI oPC = new ImportPI();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIDA.Get(tc, nImportPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPI", e);
                #endregion
            }
            return oPC;
        }
        public ImportPI Get(string sPurchaseContactNo,Int64 nUserId)
        {
            ImportPI oPC = new ImportPI();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIDA.Get(tc, sPurchaseContactNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPI", e);
                #endregion
            }
            return oPC;
        }

        public List<ImportPI> GetsByImportPIType(string PCTypesIDs, Int64 nUserId)
        {
            List<ImportPI> oPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDA.GetsByImportPIType(tc, PCTypesIDs);
                oPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIs", e);
                #endregion
            }
            return oPCs;
        }
        public List<ImportPI> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPI> oPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDA.Gets(tc, sSQL);
                oPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIs", e);
                #endregion
            }
            return oPCs;
        }
        public List<ImportPI> Gets(int nContractorID,Int64 nUserId)
        {
            List<ImportPI> oPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDA.Gets(tc, nContractorID);
                oPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIs", e);
                #endregion
            }
            return oPCs;
        }

        public List<ImportPI> GetsImportPI(int nContractorID, string sStatus, string sPCType, Int64 nUserId)
        {
            List<ImportPI> oPCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIDA.GetsImportPI(tc,  nContractorID,  sStatus, sPCType);
                oPCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Purchase Payment", e);
                #endregion
            }
            return oPCs;
        }

        public ImportPI UpdateAmount(ImportPI oImportPI, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIDA.UpdateAmount(tc, oImportPI);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPI = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportPI = new ImportPI();
                oImportPI.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportPI;
        }

       #endregion


       
    }
}
