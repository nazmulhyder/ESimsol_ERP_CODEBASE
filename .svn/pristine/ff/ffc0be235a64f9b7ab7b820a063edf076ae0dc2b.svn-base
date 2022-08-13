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
    public class ExportLCService : MarshalByRefObject, IExportLCService
    {
        bool _bIsLog = false;
        #region Private functions and declaration
        private static ExportLC MapObject(NullHandler oReader)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC.ExportLCID = oReader.GetInt32("ExportLCID");
            oExportLC.ExportLCLogID = oReader.GetInt32("ExportLCLogID");
            oExportLC.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportLC.FileNo = oReader.GetString("FileNo");
            oExportLC.OpeningDate = oReader.GetDateTime("OpeningDate");
            oExportLC.BBranchID_Advice = oReader.GetInt32("BankBranchID_Advice");
            oExportLC.BankBranchID_Forwarding = oReader.GetInt32("BankBranchID_Forwarding");
            oExportLC.BBranchID_Nego = oReader.GetInt32("BankBranchID_Negotiation");
            oExportLC.BBranchID_Issue = oReader.GetInt32("BankBranchID_Issue");
            oExportLC.ApplicantID = oReader.GetInt32("ApplicantID");
            oExportLC.ContactPersonalID = oReader.GetInt32("ContactPersonalID");
            oExportLC.DeliveryToID = oReader.GetInt32("DeliveryToID");
            oExportLC.Amount = oReader.GetDouble("Amount");
            oExportLC.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportLC.NegoDays = oReader.GetInt32("NegoDays");
            oExportLC.HSCode = oReader.GetString("HSCode");
            oExportLC.AreaCode = oReader.GetString("AreaCode");
            oExportLC.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oExportLC.ExpiryDate = oReader.GetDateTime("ExpiryDate");
            oExportLC.AtSightDiffered = oReader.GetBoolean("AtSightDiffered");
            oExportLC.ShipmentFrom = oReader.GetString("ShipmentFrom");
            oExportLC.PartialShipmentAllowed = oReader.GetBoolean("PartialShipmentAllowed");
            oExportLC.TransShipmentAllowed = oReader.GetBoolean("TransShipmentAllowed");
            oExportLC.CurrentStatus = (EnumExportLCStatus)oReader.GetInt32("CurrentStatus");
            oExportLC.CurrentStatusInInt = oReader.GetInt32("CurrentStatus");
            oExportLC.Remark = oReader.GetString("Remark");
            oExportLC.DBServerUserID = oReader.GetInt32("DBServerUserID");
            oExportLC.DBServerDate = oReader.GetDateTime("DBServerDate");
            oExportLC.LiborRate = oReader.GetBoolean("LiborRate");
            oExportLC.BBankFDD = oReader.GetBoolean("BBankFDD");
            oExportLC.OverDueRate = oReader.GetDouble("OverDueRate");
            oExportLC.VersionNo = oReader.GetInt32("VersionNo");
            oExportLC.LCRecivedDate = oReader.GetDateTime("LCRecivedDate");
            oExportLC.IsForeignBank = oReader.GetBoolean("ForeignBank");
            oExportLC.NoteQuery = oReader.GetString("NoteQuery");
            oExportLC.NoteUD = oReader.GetString("NoteUD");
            oExportLC.HaveQuery = oReader.GetBoolean("HaveQuery");
            oExportLC.FrightPrepaid = oReader.GetString("FrightPrepaid");
            oExportLC.DarkMedium = oReader.GetString("DarkMedium");
            oExportLC.ExportLCLogID = oReader.GetInt32("ExportLCLogID");
            //oExportLC.Year = oReader.GetString("Year");
            oExportLC.GetOriginalCopy = oReader.GetBoolean("GetOriginalCopy");
            oExportLC.DCharge = oReader.GetDouble("DCharge");
            oExportLC.LCTramsID = oReader.GetInt32("LCTramsID");
            oExportLC.Stability = oReader.GetBoolean("Stability");
            oExportLC.Stability = oReader.GetBoolean("Stability");
            oExportLC.GarmentsQty = oReader.GetString("GarmentsQty");
            oExportLC.AmendmentDate = oReader.GetDateTime("AmendmentDate");
            oExportLC.BUID = oReader.GetInt32("BUID");
            oExportLC.PaymentInstruction = (EnumPaymentInstruction) oReader.GetInt32("PaymentInstruction");
            oExportLC.MasterLCNos = oReader.GetString("MasterLCNos");
            oExportLC.MasterLCDates = oReader.GetString("MasterLCDates");

            ///Derive from view
            oExportLC.CurrencyName = oReader.GetString("CurrencyName");
            oExportLC.LCTermsName = oReader.GetString("LCTermsName");
            oExportLC.Currency = oReader.GetString("CurrencySymbol");
            oExportLC.BankName_Nego = oReader.GetString("BankName_Nego");
            oExportLC.BankBranchName_Nego = oReader.GetString("BBranchName_Nego");
            oExportLC.ApplicantName = oReader.GetString("ApplicantName");
            oExportLC.DeliveryToName = oReader.GetString("DeliveryToName");
            oExportLC.ApplicantAddress = oReader.GetString("ApplicantAddress");
            oExportLC.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportLC.BankName_Issue = oReader.GetString("BankName_Issue");

            oExportLC.BBranchName_Advice = oReader.GetString("BBranchName_Advice");
            oExportLC.BankName_Advice = oReader.GetString("BankName_Advice");
            oExportLC.AmountBill = oReader.GetDouble("AmountBill");
            oExportLC.ExportLCType = (EnumExportLCType)oReader.GetInt16("ExportLCType");
            oExportLC.ExportLCTypeInt = oReader.GetInt16("ExportLCType");
          
            return oExportLC;
        }

        public static ExportLC CreateObject(NullHandler oReader)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = MapObject(oReader);
            return oExportLC;
        }

        private List<ExportLC> CreateObjects(IDataReader oReader)
        {
            List<ExportLC> oExportLC = new List<ExportLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLC oItem = CreateObject(oHandler);
                oExportLC.Add(oItem);
            }
            return oExportLC;
        }

        #region DistinctItem
        private static ExportLC MapObject_DistinctItem(NullHandler oReader)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC.FrightPrepaid = oReader.GetString("DistinctItem");
            oExportLC.HSCode = oReader.GetString("DistinctItem");
            oExportLC.ExportLCID = 10;
            return oExportLC;
        }

        public static ExportLC CreateObject_DistinctItem(NullHandler oReader)
        {
            ExportLC oExportLC = new ExportLC();
            oExportLC = MapObject_DistinctItem(oReader);
            return oExportLC;
        }

        private List<ExportLC> CreateObjects_DistinctItem(IDataReader oReader)
        {
            List<ExportLC> oExportLCs = new List<ExportLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportLC oItem = CreateObject_DistinctItem(oHandler);
                oExportLCs.Add(oItem);
            }
            return oExportLCs;
        }
        #endregion

        #endregion

        #region Interface implementation
        public ExportLCService() { }

        public ExportLC Save(ExportLC oExportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ExportPILCMapping> oPILCMappings = new List<ExportPILCMapping>();
                oPILCMappings = oExportLC.ExportPILCMappings;

              
                IDataReader reader;
                if (oExportLC.ExportLCID <= 0)
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();

                #region ExportPILCMapping
                if (oPILCMappings != null)
                {
                    foreach (ExportPILCMapping oItem in oPILCMappings)
                    {
                        IDataReader readerPPC;

                        oItem.ExportLCID = oExportLC.ExportLCID;

                        if (oItem.ExportPILCMappingID <= 0)
                        {
                            oItem.LCReceiveDate = oExportLC.LCRecivedDate;
                            readerPPC = ExportPILCMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPPC = ExportPILCMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetailPPC = new NullHandler(readerPPC);
                        //if (readerPPC.Read())
                        //{
                        //    sPPCIDs = sPPCIDs + oReaderDetailPPC.GetString("PILCMappingID") + ",";
                        //}
                        readerPPC.Close();

                    }
                }
                ////if (sPPCIDs.Length > 0)
                ////{
                ////    sPPCIDs = sPPCIDs.Remove(sPPCIDs.Length - 1, 1);
                ////}
                ////oPurchasePaymentContract = new PurchasePaymentContract();
                ////oPurchasePaymentContract.ExportLCID = oExportLC.ExportLCID;
                ////PurchasePaymentContractDA.Delete(tc, oPurchasePaymentContract, EnumDBOperation.Delete, nUserID, sPPCIDs);
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }
        public ExportLC SaveMLC(ExportLC oExportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
         

                //List<MasterLC> oMasterLCs = new List<MasterLC>();
                //oMasterLCs = oExportLC.MasterLCs;

                IDataReader reader;
                if (oExportLC.ExportLCID <= 0)
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }

        public ExportLC SaveLog(ExportLC oExportLC, Int64 nUserID)
        {
            double nAmount = 0;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                List<ExportPILCMapping> oPILCMappings = new List<ExportPILCMapping>();
                oPILCMappings = oExportLC.ExportPILCMappings;

                //string sPPCIDs = "";

                IDataReader reader;
                if (oExportLC.ExportLCID <= 0)
                {
                    reader = ExportLCDA.InsertUpdateLog(tc, oExportLC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportLCDA.InsertUpdateLog(tc, oExportLC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();

                #region ExportPILCMapping
                if (oPILCMappings != null)
                {
                    foreach (ExportPILCMapping oItem in oPILCMappings)
                    {
                        IDataReader readerPPC;
                        if(oItem.Activity)
                        {
                            nAmount = nAmount + oItem.Amount;
                        }

                        oItem.ExportLCID = oExportLC.ExportLCID;

                        if (oItem.ExportPILCMappingID <= 0)
                        {
                            readerPPC = ExportPILCMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerPPC = ExportPILCMappingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetailPPC = new NullHandler(readerPPC);
                   
                        readerPPC.Close();

                    }
                }
                oExportLC.Amount = nAmount;
               
                #endregion


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }
    
        public ExportLC UpdateForGetOrginalCopy(int nExportLCID, Int64 nUserId)
        {
            ExportLC oExportLC = new ExportLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.UpdateForGetOrginalCopy(tc, nExportLCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0]; 
                #endregion
            }

            return oExportLC;
        }
        public ExportLC Approved(ExportLC oExportLC, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportLC.ExportLCID <= 0)
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                    reader = ExportLCDA.InsertUpdate(tc, oExportLC, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }

        public ExportLC UpdateExportLCStatus(ExportLC oExportLC, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
             
                reader = ExportLCDA.InsertUpdateLCStatus(tc, oExportLC, EnumDBOperation.Insert, nUserID);
               
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }

        public ExportLC UpdateUDInfo(ExportLC oExportLC, int nOperation,Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportLC.ExportPILCMappings != null)
                {
                    foreach (ExportPILCMapping oItem in oExportLC.ExportPILCMappings)
                    {
                        oItem.ExportLCID = oExportLC.ExportLCID;
                        ExportPILCMappingDA.UpdateUDInfo(tc, oItem, nUserID);
                    }
                }
                reader = ExportLCDA.UpdateUDInfo(tc, oExportLC, nOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = new ExportLC();
                    oExportLC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportLC.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportLC;

        }
      


        public string Delete(ExportLC oExportLC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportLCDA.Delete(tc, oExportLC, EnumDBOperation.Delete, nUserId);
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

        public ExportLC Get(int id, Int64 nUserId)
        {
            ExportLC oExportLC = new ExportLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get Export LC", e);
                #endregion
            }

            return oExportLC;
        }
        public ExportLC GetLog(int id, Int64 nUserId)
        {
            ExportLC oExportLC = new ExportLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.GetLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportLC", e);
                #endregion
            }

            return oExportLC;
        }

        public ExportLC GetByNo(int nBUID,string sExportLCNo, Int64 nUserID)
        {
            ExportLC oAccountHead = new ExportLC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.Get(tc,nBUID, sExportLCNo);
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
                throw new ServiceException("Failed to Get ExportLC", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ExportLC> Gets(int nBUID,DateTime dLCDate,Int64 nUserId)
        {
            List<ExportLC> oExportLC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.Gets(tc,nBUID, dLCDate);
                oExportLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLC", e);
                #endregion
            }

            return oExportLC;
        }

        public List<ExportLC> Gets(int nBUID,Int64 nUserId)
        {
            List<ExportLC> oExportLC = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.Gets(tc, nBUID);
                oExportLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export LC", e);
                #endregion
            }

            return oExportLC;
        }

        public List<ExportLC> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportLC> oExportLC = new List<ExportLC>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.Gets(tc, sSQL);
                oExportLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLC", e);
                #endregion
            }

            return oExportLC;
        }

        public List<ExportLC> GetsSQL(string sSQL, Int64 nUserId)
        {
            List<ExportLC> oExportLC = new List<ExportLC>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.GetsSQL(tc, sSQL);
                oExportLC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportLC", e);
                #endregion
            }

            return oExportLC;
        }


        public ExportLC GetLogForVersionNo(int nExportLCID, int nVersionNo, Int64 nUserId)
        {
            ExportLC oExportLC = new ExportLC();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.GetLogForVersionNo(tc, nExportLCID, nVersionNo);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get By Export LC Log.", e);
                #endregion
            }

            return oExportLC;
        }
        public List<ExportLC> GetsLog(int nExportLCID, Int64 nUserId)
        {
            List<ExportLC> oExportLCs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.GetsLog(tc, nExportLCID);
                _bIsLog = true;
                oExportLCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Export LC", e);
                #endregion
            }

            return oExportLCs;
        }
     

        public ExportLC GetByPIID(int nPIID, Int64 nUserID)
        {
            ExportLC oExportLC = new ExportLC();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportLCDA.GetByPIID(tc, nPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get By PIID", e);
                #endregion
            }

            return oExportLC;
        }

        public List<ExportLC> Gets_DistinctItem(string sSQL, Int64 nUserId)
        {
            List<ExportLC> oExportLCs = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportLCDA.GetsSQL(tc, sSQL);
                oExportLCs = CreateObjects_DistinctItem(reader);
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

            return oExportLCs;
        }
     

        #endregion
    }
}