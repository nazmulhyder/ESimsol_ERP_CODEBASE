using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    [Serializable]
    public class ExportDocSetupService : MarshalByRefObject, IExportDocSetupService
    {
        #region Private functions and declaration
        private ExportDocSetup MapObject(NullHandler oReader)
        {
            ExportDocSetup oExportDocSetup = new ExportDocSetup();
            oExportDocSetup.ExportDocSetupID = oReader.GetInt32("ExportDocSetupID");
            oExportDocSetup.ExportBillDocID = oReader.GetInt32("ExportBillDocID");
            oExportDocSetup.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportDocSetup.CompanyID = oReader.GetInt32("CompanyID");
            oExportDocSetup.DocumentType = (EnumDocumentType)oReader.GetInt32("DocumentType");
            oExportDocSetup.ExportLCType = (EnumExportLCType)oReader.GetInt32("ExportLCType");
            oExportDocSetup.DocumentTypeInt = oReader.GetInt32("DocumentType");
            oExportDocSetup.IsPrintHeader = oReader.GetBoolean("IsPrintHeader");
            oExportDocSetup.DocName = oReader.GetString("DocName");
            oExportDocSetup.DocHeader = oReader.GetString("DocHeader");
            oExportDocSetup.Beneficiary = oReader.GetString("Beneficiary");
            oExportDocSetup.BillNo = oReader.GetString("BillNo");
            oExportDocSetup.NoAndDateOfDoc = oReader.GetString("NoAndDateOfDoc");
            oExportDocSetup.ProformaInvoiceNoAndDate = oReader.GetString("ProformaInvoiceNoAndDate");
            oExportDocSetup.AccountOf = oReader.GetString("AccountOf");
            oExportDocSetup.DocumentaryCreditNoDate = oReader.GetString("DocumentaryCreditNoDate");
            oExportDocSetup.AgainstExportLC = oReader.GetString("AgainstExportLC");
            oExportDocSetup.PortofLoading = oReader.GetString("PortofLoading");
            oExportDocSetup.FinalDestination = oReader.GetString("FinalDestination");
            oExportDocSetup.IssuingBank = oReader.GetString("IssuingBank");
            oExportDocSetup.NegotiatingBank = oReader.GetString("NegotiatingBank");
            oExportDocSetup.CountryofOrigin = oReader.GetString("CountryofOrigin");
            oExportDocSetup.TermsofPayment = oReader.GetString("TermsofPayment");
            oExportDocSetup.AmountInWord = oReader.GetString("AmountInWord");
            oExportDocSetup.Wecertifythat = oReader.GetString("Wecertifythat");
            oExportDocSetup.Certification = oReader.GetString("Certification");

            oExportDocSetup.IRC = oReader.GetString("IRC");
            oExportDocSetup.GarmentsQty = oReader.GetString("GarmentsQty");
            oExportDocSetup.HSCode = oReader.GetString("HSCode");
            oExportDocSetup.AreaCode = oReader.GetString("AreaCode");
            oExportDocSetup.SpecialNote = oReader.GetString("SpecialNote");
            oExportDocSetup.Remark = oReader.GetString("Remark");
            oExportDocSetup.DeliveryTo = oReader.GetString("DeliveryTo");
            oExportDocSetup.IsVat = oReader.GetBoolean("IsVat");
            oExportDocSetup.IsRegistration = oReader.GetBoolean("IsRegistration");

            oExportDocSetup.IsPrintOriginal = oReader.GetBoolean("IsPrintOriginal");
            oExportDocSetup.IsPrintGrossNetWeight = oReader.GetBoolean("IsPrintGrossNetWeight");
            oExportDocSetup.IsPrintDeliveryBy = oReader.GetBoolean("IsPrintDeliveryBy");
            oExportDocSetup.IsPrintTerm = oReader.GetBoolean("IsPrintTerm");
            oExportDocSetup.IsPrintQty = oReader.GetBoolean("IsPrintQty");
            oExportDocSetup.IsPrintUnitPrice = oReader.GetBoolean("IsPrintUnitPrice");
            oExportDocSetup.IsPrintValue = oReader.GetBoolean("IsPrintValue");
            oExportDocSetup.IsPrintWeight = oReader.GetBoolean("IsPrintWeight");
            oExportDocSetup.IsPrintFrieghtPrepaid = oReader.GetBoolean("IsPrintFrieghtPrepaid");
            oExportDocSetup.IsShowAmendmentNo = oReader.GetBoolean("IsShowAmendmentNo");
            
            oExportDocSetup.ClauseOne = oReader.GetString("ClauseOne");
            oExportDocSetup.ClauseTwo = oReader.GetString("ClauseTwo");
            oExportDocSetup.ClauseThree = oReader.GetString("ClauseThree");
            oExportDocSetup.ClauseFour = oReader.GetString("ClauseFour");
            oExportDocSetup.Activity = oReader.GetBoolean("Activity");
            oExportDocSetup.CompanyName = oReader.GetString("CompanyName");
            oExportDocSetup.Carrier = oReader.GetString("Carrier");
            oExportDocSetup.NotifyParty = oReader.GetString("NotifyParty");
            oExportDocSetup.Remarks = oReader.GetString("Remarks");
            oExportDocSetup.Account = oReader.GetString("Account");
            oExportDocSetup.BUID = oReader.GetInt32("BUID");

            oExportDocSetup.IsPrintInvoiceDate = oReader.GetBoolean("IsPrintInvoiceDate");
            oExportDocSetup.PortofLoadingName = oReader.GetString("PortofLoadingName");
            oExportDocSetup.AuthorisedSignature = oReader.GetString("AuthorisedSignature");
            oExportDocSetup.ReceiverSignature = oReader.GetString("ReceiverSignature");
            oExportDocSetup.For = oReader.GetString("For");
            oExportDocSetup.MUnitName = oReader.GetString("MUnitName");
            oExportDocSetup.NetWeightName = oReader.GetString("NetWeightName");
            oExportDocSetup.GrossWeightName = oReader.GetString("GrossWeightName");
            oExportDocSetup.SellingOnAbout = oReader.GetString("SellingOnAbout");
            oExportDocSetup.FinalDestinationName = oReader.GetString("FinalDestinationName");

            oExportDocSetup.Bag_Name = oReader.GetString("Bag_Name");
            oExportDocSetup.CountryofOriginName = oReader.GetString("CountryofOriginName");

            oExportDocSetup.TO = oReader.GetString("TO");
            oExportDocSetup.TruckNo_Print = oReader.GetString("TruckNo_Print");
            oExportDocSetup.Driver_Print = oReader.GetString("Driver_Print");
            oExportDocSetup.Sequence = oReader.GetInt32("Sequence");
            oExportDocSetup.ShippingMark = oReader.GetString("ShippingMark");
            oExportDocSetup.ReceiverCluse = oReader.GetString("ReceiverCluse");
            oExportDocSetup.ForCaptionInDubleLine = oReader.GetBoolean("ForCaptionInDubleLine");

            oExportDocSetup.CarrierName = oReader.GetString("CarrierName");
            oExportDocSetup.DescriptionOfGoods = oReader.GetString("DescriptionOfGoods");
            oExportDocSetup.MarkSAndNos = oReader.GetString("MarkSAndNos");
            oExportDocSetup.QtyInOne = oReader.GetString("QtyInOne");
            oExportDocSetup.QtyInTwo = oReader.GetString("QtyInTwo");
            oExportDocSetup.ValueName = oReader.GetString("ValueName");
            oExportDocSetup.UPName = oReader.GetString("UPName");
            oExportDocSetup.NoOfBag = oReader.GetString("NoOfBag");
            oExportDocSetup.FontSize_Normal = (float)oReader.GetDouble("FontSize_Normal");
            oExportDocSetup.FontSize_Bold = oReader.GetDouble("FontSize_Bold");
            oExportDocSetup.FontSize_ULine = oReader.GetDouble("FontSize_ULine");
            oExportDocSetup.ChallanNo = oReader.GetString("ChallanNo");
            oExportDocSetup.CTPApplicant = oReader.GetString("CTPApplicant");
            oExportDocSetup.GRPNoDate = oReader.GetString("GRPNoDate");
            oExportDocSetup.ASPERPI = oReader.GetString("ASPERPI");
            oExportDocSetup.PrintOn = (EnumExcellColumn)oReader.GetInt32("PrintOn");
            oExportDocSetup.ProductPrintType = (EnumExcellColumn)oReader.GetInt32("ProductPrintType");
            oExportDocSetup.TRNo = oReader.GetString("TRNo");
            oExportDocSetup.TRDate = oReader.GetDateTime("TRDate");
            oExportDocSetup.TruckNo = oReader.GetString("TruckNo");
            oExportDocSetup.DriverName = oReader.GetString("DriverName");
            oExportDocSetup.BagCount = oReader.GetDouble("BagCount");
            oExportDocSetup.NotifyBy = (EnumNotifyBy)oReader.GetDouble("NotifyBy");
            oExportDocSetup.GoodsDesViewType = (EnumExportGoodsDesViewType)oReader.GetDouble("GoodsDesViewType");
            oExportDocSetup.ToTheOrderOf = oReader.GetString("ToTheOrderOf");
            oExportDocSetup.OrderOfBankType = (EnumBankType)oReader.GetInt32("OrderOfBankType");
            oExportDocSetup.OrderOfBankTypeInInt = (int)(EnumBankType)oReader.GetInt16("OrderOfBankType");
            oExportDocSetup.TermsOfShipment = oReader.GetString("TermsOfShipment");
            oExportDocSetup.TextWithGoodsCol = oReader.GetString("TextWithGoodsCol");
            oExportDocSetup.TextWithGoodsRow = oReader.GetString("TextWithGoodsRow");
            oExportDocSetup.GrossWeightPTage = oReader.GetDouble("GrossWeightPTage");
            oExportDocSetup.WeightPBag = oReader.GetDouble("WeightPBag");
            
            

            return oExportDocSetup;
        }

        private ExportDocSetup CreateObject(NullHandler oReader)
        {
            ExportDocSetup oExportDocSetup = new ExportDocSetup();
            oExportDocSetup=MapObject(oReader);
            return oExportDocSetup;
        }

        private List<ExportDocSetup> CreateObjects(IDataReader oReader)
        {
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportDocSetup oItem = CreateObject(oHandler);
                oExportDocSetups.Add(oItem);
            }
            return oExportDocSetups;
        }
        #endregion

        #region Interface implementation
        public ExportDocSetupService() { }

        public ExportDocSetup Get(int nID, Int64 nUserID)
        {
            ExportDocSetup oPLC = new ExportDocSetup();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportDocSetupDA.Get(tc, nID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get  ExportDocSetup", e);
                #endregion
            }

            return oPLC;
        }
        public ExportDocSetup GetBy(int nID, int nExportBillID, Int64 nUserID)
        {
            ExportDocSetup oPLC = new ExportDocSetup();
            int nExportBillDocID = 0;
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                /// if Find any by Bill then gets ExportBillDoc otherwise gets DocSetup
                tc = TransactionContext.Begin();
                nExportBillDocID = ExportDocSetupDA.GetExportBillDocID(tc, nID, nExportBillID);
                if (nExportBillDocID > 0)
                {
                     reader = ExportDocSetupDA.GetBy(tc, nID, nExportBillID);
                }
                else
                {
                     reader = ExportDocSetupDA.Get(tc, nID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get  ExportDocSetup", e);
                #endregion
            }

            return oPLC;
        }
        public ExportDocSetup Save(ExportDocSetup oExportDocSetup, Int64 nUserID)
        {
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportDocSetup.ExportDocSetupID <= 0)
                {
                    reader = ExportDocSetupDA.InsertUpdate(tc, oExportDocSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportDocSetupDA.InsertUpdate(tc, oExportDocSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportDocSetup = new ExportDocSetup();
                    oExportDocSetup = CreateObject(oReader);
                }
                reader.Close();

                


                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportDocSetup = new ExportDocSetup();
                oExportDocSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportDocSetup;

        }
        public ExportDocSetup Save_Bill(ExportDocSetup oExportDocSetup, Int64 nUserID)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            oExportPartyInfoBills = oExportDocSetup.ExportPartyInfoBills;
            string sExportPartyInfoBillIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                IDataReader reader;
                if (oExportDocSetup.ExportBillDocID <= 0)
                {
                    reader = ExportDocSetupDA.InsertUpdate_Bill(tc, oExportDocSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportDocSetupDA.InsertUpdate_Bill(tc, oExportDocSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportDocSetup = new ExportDocSetup();
                    oExportDocSetup = CreateObject(oReader);
                }
                reader.Close();
                #region  ExportPartyInfoBill
                if (oExportPartyInfoBills != null)
                {
                    foreach (ExportPartyInfoBill oItem in oExportPartyInfoBills)
                    {
                        IDataReader readerdetail;
                        oItem.ReferenceID = oExportDocSetup.ExportBillDocID;
                        if (oItem.ExportPartyInfoBillID <= 0)
                        {
                            readerdetail = ExportPartyInfoBillDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = ExportPartyInfoBillDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sExportPartyInfoBillIDs = sExportPartyInfoBillIDs + oReaderDetail.GetString("ExportPartyInfoBillID") + ",";
                        }
                        readerdetail.Close();
                    }
                }
                if (sExportPartyInfoBillIDs.Length > 0)
                {
                    sExportPartyInfoBillIDs = sExportPartyInfoBillIDs.Remove(sExportPartyInfoBillIDs.Length - 1, 1);
                }
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.RefType = EnumMasterLCType.CommercialDoc;
                oExportPartyInfoBill.ReferenceID = oExportDocSetup.ExportBillDocID;
                ExportPartyInfoBillDA.Delete(tc, oExportPartyInfoBill, EnumDBOperation.Delete, nUserID, sExportPartyInfoBillIDs);
                #endregion



                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportDocSetup = new ExportDocSetup();
                oExportDocSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oExportDocSetup;

        }
        public ExportDocSetup Activate(ExportDocSetup oExportDocSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ExportDocSetupDA.Activate(tc, oExportDocSetup);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportDocSetup = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportDocSetup = new ExportDocSetup();
                oExportDocSetup.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oExportDocSetup;
        }
        public List<ExportDocSetup> UpdateSequence(ExportDocSetup oExportDocSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<ExportDocSetup> oExportDocSetups = new List<ExportDocSetup>();
            oExportDocSetups = oExportDocSetup.ExportDocSetups;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (ExportDocSetup oItem in oExportDocSetups)
                {
                    ExportDocSetupDA.UpdateSequence(tc, oItem);
                }
                IDataReader reader = null;
                reader = ExportDocSetupDA.Gets(tc);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oExportDocSetups = new List<ExportDocSetup>();
                oExportDocSetup = new ExportDocSetup();
                oExportDocSetup.ErrorMessage = e.Message;
                oExportDocSetups.Add(oExportDocSetup);
                #endregion
            }
            return oExportDocSetups;
        }
        public List<ExportDocSetup> Gets(bool bActivity, int nBUID, Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.Gets(tc, bActivity, nBUID);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }
        public List<ExportDocSetup> GetsByType(int nExportLCType, int nBUID, Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.GetsByType(tc, nExportLCType, nBUID);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }

        public List<ExportDocSetup> BUWiseGets( int nBUID, Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.BUWiseGets(tc, nBUID);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }
        public List<ExportDocSetup> GetsBy(int nExportBillID, Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.GetsBy(tc, nExportBillID);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }
        public List<ExportDocSetup> Gets( Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.Gets(tc);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }

        public List<ExportDocSetup> Gets(string sSQL, Int64 nUserId)
        {
            List<ExportDocSetup> oExportDocSetups = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportDocSetupDA.Gets(sSQL,tc);
                oExportDocSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocSetups", e);
                #endregion
            }

            return oExportDocSetups;
        }

        public string Delete(ExportDocSetup oExportDocSetup, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);


                ExportDocSetupDA.Delete(tc, oExportDocSetup, EnumDBOperation.Delete, nUserId);
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

        #endregion
    }


}
