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
    public class ExportDocTnCService : MarshalByRefObject, IExportDocTnCService
    {
        #region Private functions and declaration
        private ExportDocTnC MapObject(NullHandler oReader)
        {
            ExportDocTnC oExportDocTnC = new ExportDocTnC();
            oExportDocTnC.ExportDocTnCID = oReader.GetInt32("ExportDocTnCID");
            oExportDocTnC.ExportTRID = oReader.GetInt32("ExportTRID");
            oExportDocTnC.ReferenceID = oReader.GetInt32("ReferenceID");
            oExportDocTnC.RefType = (EnumMasterLCType)oReader.GetInt32("RefType");
            oExportDocTnC.RefTypeInInt = oReader.GetInt32("RefType");
            oExportDocTnC.IsPrintGrossNetWeight = oReader.GetBoolean("IsPrintGrossNetWeight");
            oExportDocTnC.IsPrintOriginal = oReader.GetBoolean("IsPrintOriginal");
            oExportDocTnC.DeliveryBy = oReader.GetString("DeliveryBy");
            oExportDocTnC.IsDeliveryBy = oReader.GetBoolean("IsDeliveryBy");
            oExportDocTnC.Term = oReader.GetString("Term");
            oExportDocTnC.IsTerm = oReader.GetBoolean("IsTerm");
            oExportDocTnC.TruckReceiptNo = oReader.GetString("TruckReceiptNo");
            oExportDocTnC.TruckReceiptDate = oReader.GetDateTime("TruckReceiptDate");
            oExportDocTnC.Carrier = oReader.GetString("Carrier");
            oExportDocTnC.TruckNo = oReader.GetString("TruckNo");
            oExportDocTnC.DriverName = oReader.GetString("DriverName");
            oExportDocTnC.MeasurementCarton = oReader.GetDouble("MeasurementCarton");
            oExportDocTnC.PerCartonWeight = oReader.GetDouble("PerCartonWeight");
            oExportDocTnC.CTPApplicant = oReader.GetString("CTPApplicant");
            oExportDocTnC.Certification = oReader.GetString("Certification");
            oExportDocTnC.GRPNonDate = oReader.GetString("GRPNonDate");
            oExportDocTnC.NotifyByInt = oReader.GetInt32("NotifyBy");
            oExportDocTnC.NotifyBy = (EnumNotifyBy)oReader.GetInt32("NotifyBy");
            return oExportDocTnC;
        }
        private ExportDocTnC CreateObject(NullHandler oReader)
        {
            ExportDocTnC oExportDocTnC = new ExportDocTnC();
            oExportDocTnC = MapObject(oReader);
            return oExportDocTnC;
        }
        private List<ExportDocTnC> CreateObjects(IDataReader oReader)
        {
            List<ExportDocTnC> oExportDocTnC = new List<ExportDocTnC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportDocTnC oItem = CreateObject(oHandler);
                oExportDocTnC.Add(oItem);
            }
            return oExportDocTnC;
        }
        #endregion

        #region Interface implementation
        public ExportDocTnC Save(ExportDocTnC oExportDocTnC, Int64 nUserID)
        {
            List<ExportPartyInfoBill> oExportPartyInfoBills = new List<ExportPartyInfoBill>();
            List<ExportDocForwarding> oExportDocForwardings = new List<ExportDocForwarding>();
            oExportPartyInfoBills = oExportDocTnC.ExportPartyInfoBills;
            oExportDocForwardings = oExportDocTnC.ExportDocForwardings;
            string sExportPartyInfoBillIDs = "", sExportDocForwardingIDs = "";
            int nRefTypeInInt = oExportDocTnC.RefTypeInInt;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportDocTnC.ExportDocTnCID <= 0)
                {
                    reader = ExportDocTnCDA.InsertUpdate(tc, oExportDocTnC, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportDocTnCDA.InsertUpdate(tc, oExportDocTnC, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportDocTnC = new ExportDocTnC();
                    oExportDocTnC = CreateObject(oReader);
                }
                reader.Close();
                #region  ExportPartyInfoBill 
                foreach (ExportPartyInfoBill oItem in oExportPartyInfoBills)
                {
                    IDataReader readerdetail;
                    oItem.ReferenceID = oExportDocTnC.ReferenceID;
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
                if (sExportPartyInfoBillIDs.Length > 0)
                {
                    sExportPartyInfoBillIDs = sExportPartyInfoBillIDs.Remove(sExportPartyInfoBillIDs.Length - 1, 1);
                }
                ExportPartyInfoBill oExportPartyInfoBill = new ExportPartyInfoBill();
                oExportPartyInfoBill.RefType = (EnumMasterLCType)nRefTypeInInt;
                oExportPartyInfoBill.ReferenceID = oExportDocTnC.ReferenceID;
                ExportPartyInfoBillDA.Delete(tc, oExportPartyInfoBill, EnumDBOperation.Delete, nUserID, sExportPartyInfoBillIDs);
                #endregion

                #region  ExportDocForwarding
                foreach (ExportDocForwarding oItem in oExportDocForwardings)
                {
                    IDataReader readerdetail;
                    oItem.ReferenceID = oExportDocTnC.ReferenceID;
                    if (oItem.ExportDocForwardingID <= 0)
                    {
                        readerdetail = ExportDocForwardingDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        readerdetail = ExportDocForwardingDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sExportDocForwardingIDs = sExportDocForwardingIDs + oReaderDetail.GetString("ExportDocForwardingID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sExportDocForwardingIDs.Length > 0)
                {
                    sExportDocForwardingIDs = sExportDocForwardingIDs.Remove(sExportDocForwardingIDs.Length - 1, 1);
                }
                ExportDocForwarding oExportDocForwarding = new ExportDocForwarding();
                oExportDocForwarding.RefType = (EnumMasterLCType)nRefTypeInInt;
                oExportDocForwarding.ReferenceID = oExportDocTnC.ReferenceID;
                ExportDocForwardingDA.Delete(tc, oExportDocForwarding, EnumDBOperation.Delete, nUserID, sExportDocForwardingIDs);
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oExportDocTnC = new ExportDocTnC();
                oExportDocTnC.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportDocTnC;
        }
        public string Delete(ExportDocTnC oExportDocTnC, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                ExportDocTnCDA.Delete(tc, oExportDocTnC, EnumDBOperation.Delete, nUserId);
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
            return "Data delete successfully";
        }
        public ExportDocTnC Get(int id, Int64 nUserId)
        {
            ExportDocTnC oAccountHead = new ExportDocTnC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportDocTnCDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ExportDocTnC", e);
                #endregion
            }

            return oAccountHead;
        }
        public ExportDocTnC GetByLCID(int nExportReferenceID, int nRefType, Int64 nUserId)
        {
            ExportDocTnC oAccountHead = new ExportDocTnC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportDocTnCDA.GetByLCID(tc, nExportReferenceID,nRefType);
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
                throw new ServiceException("Failed to Get ExportDocTnC", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ExportDocTnC> Gets(Int64 nUserID)
        {
            List<ExportDocTnC> oExportDocTnC = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportDocTnCDA.Gets(tc);
                oExportDocTnC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocTnC", e);
                #endregion
            }
            return oExportDocTnC;
        }
        public List<ExportDocTnC> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportDocTnC> oExportDocTnC = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportDocTnCDA.Gets(tc, sSQL);
                oExportDocTnC = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportDocTnC", e);
                #endregion
            }
            return oExportDocTnC;
        }

        #endregion
    }
}
