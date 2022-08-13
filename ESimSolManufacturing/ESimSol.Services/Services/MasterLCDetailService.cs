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

    public class MasterLCDetailService : MarshalByRefObject, IMasterLCDetailService
    {
        #region Private functions and declaration
        private MasterLCDetail MapObject(NullHandler oReader)
        {
            MasterLCDetail oMasterLCDetail = new MasterLCDetail();

            oMasterLCDetail.MasterLCDetailID = oReader.GetInt32("MasterLCDetailID");
            oMasterLCDetail.MasterLCDetailLogID = oReader.GetInt32("MasterLCDetailLogID");
            oMasterLCDetail.MasterLCLogID = oReader.GetInt32("MasterLCLogID");
            oMasterLCDetail.MasterLCID = oReader.GetInt32("MasterLCID");
            oMasterLCDetail.MasterLCNo = oReader.GetString("MasterLCNo");
            oMasterLCDetail.LCValue = oReader.GetDouble("LCValue");
            oMasterLCDetail.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
            oMasterLCDetail.PINo = oReader.GetString("PINo");
            oMasterLCDetail.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
            oMasterLCDetail.PIStatusInInt = oReader.GetInt32("PIStatus");
            oMasterLCDetail.BuyerID = oReader.GetInt32("BuyerID");
            oMasterLCDetail.PIIssueDate = oReader.GetDateTime("PIIssueDate");
            oMasterLCDetail.PIQty = oReader.GetInt32("PIQty");
            oMasterLCDetail.BuyerName = oReader.GetString("BuyerName");
            oMasterLCDetail.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oMasterLCDetail.Amount = oReader.GetDouble("Amount");
            

            return oMasterLCDetail;
        }

        private MasterLCDetail CreateObject(NullHandler oReader)
        {
            MasterLCDetail oMasterLCDetail = new MasterLCDetail();
            oMasterLCDetail = MapObject(oReader);
            return oMasterLCDetail;
        }

        private List<MasterLCDetail> CreateObjects(IDataReader oReader)
        {
            List<MasterLCDetail> oMasterLCDetail = new List<MasterLCDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MasterLCDetail oItem = CreateObject(oHandler);
                oMasterLCDetail.Add(oItem);
            }
            return oMasterLCDetail;
        }

        #endregion

        #region Interface implementation
        public MasterLCDetailService() { }

        public MasterLCDetail Save(MasterLCDetail oMasterLCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            List<MasterLCDetail> _oMasterLCDetails = new List<MasterLCDetail>();
            oMasterLCDetail.ErrorMessage = "";
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Update, nUserID, "");
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMasterLCDetail = new MasterLCDetail();
                    oMasterLCDetail = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMasterLCDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oMasterLCDetail;
        }
        public MasterLCDetail SaveMLCDetailByOrderTrack(MasterLCDetail oMasterLCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            oMasterLCDetail.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);

                ProformaInvoice oProformaInvoice = new ProformaInvoice();
                ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
                List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();

                oProformaInvoice = oMasterLCDetail.ProformaInvoice;
                oProformaInvoiceDetails = oProformaInvoice.ProformaInvoiceDetails;

                string sProformaInvoiceDetailIDs = "";

                #region Proforma Invoice part
                IDataReader reader;
                if (oProformaInvoice.ProformaInvoiceID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Add);
                    reader = ProformaInvoiceDA.InsertUpdate(tc, oProformaInvoice, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProformaInvoice, EnumRoleOperationType.Edit);
                    reader = ProformaInvoiceDA.InsertUpdate(tc, oProformaInvoice, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProformaInvoice = new ProformaInvoice();
                    oProformaInvoice.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
                    oProformaInvoice.PINo = oReader.GetString("PINo");
                    oProformaInvoice.BuyerID = oReader.GetInt32("BuyerID");
                }
                reader.Close();
                #endregion

                #region Proforma Invoice Detail Part
                if (oProformaInvoiceDetails != null)
                {
                    foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        if (oItem.ProformaInvoiceDetailID <= 0)
                        {
                            readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs + oReaderDetail.GetString("ProformaInvoiceDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sProformaInvoiceDetailIDs.Length > 0)
                    {
                        sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs.Remove(sProformaInvoiceDetailIDs.Length - 1, 1);
                    }
                    oProformaInvoiceDetail = new ProformaInvoiceDetail();
                    oProformaInvoiceDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                    ProformaInvoiceDetailDA.Delete(tc, oProformaInvoiceDetail, EnumDBOperation.Delete, nUserID, sProformaInvoiceDetailIDs);
                }

                #endregion

                #region MasterLC Detail Part

                oMasterLCDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                oMasterLCDetail.PINo = oProformaInvoice.PINo;
                oMasterLCDetail.BuyerID = oProformaInvoice.BuyerID;

                IDataReader readerForMLCD = null;
                if (oMasterLCDetail.MasterLCDetailID <= 0)
                {
                    readerForMLCD = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    readerForMLCD = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReaderForMLCD = new NullHandler(readerForMLCD);
                if (readerForMLCD.Read())
                {
                    oMasterLCDetail = new MasterLCDetail();
                    oMasterLCDetail = CreateObject(oReaderForMLCD);
                }
                readerForMLCD.Close();

                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMasterLCDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oMasterLCDetail;
        }
        public MasterLCDetail AcceptPIReviseWithMLCDetail(MasterLCDetail oMasterLCDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProformaInvoice oProformaInvoice = new ProformaInvoice();
                ProformaInvoiceDetail oProformaInvoiceDetail = new ProformaInvoiceDetail();
                List<ProformaInvoiceDetail> oProformaInvoiceDetails = new List<ProformaInvoiceDetail>();

                oProformaInvoice = oMasterLCDetail.ProformaInvoice;
                oProformaInvoiceDetails = oProformaInvoice.ProformaInvoiceDetails;

                string sProformaInvoiceDetailIDs = "";
                double nTotalNewDetailQty = 0;//This value for Validation chek for Revise
                foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                {
                    nTotalNewDetailQty += oItem.Quantity;
                }

                #region Proforma Invoice part
                if (oProformaInvoice.ProformaInvoiceID > 0)
                {
                    IDataReader reader;
                    reader = ProformaInvoiceDA.AcceptProformaInvoiceRevise(tc, oProformaInvoice, nTotalNewDetailQty, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oProformaInvoice = new ProformaInvoice();
                        oProformaInvoice.ProformaInvoiceID = oReader.GetInt32("ProformaInvoiceID");
                        oProformaInvoice.PIStatus = (EnumPIStatus)oReader.GetInt32("PIStatus");
                        oProformaInvoice.PIStatusInInt = oReader.GetInt32("PIStatus"); 
                        oProformaInvoice.PINo = oReader.GetString("PINo");
                        oProformaInvoice.BuyerID = oReader.GetInt32("BuyerID");
                    }
                    reader.Close();

                    #region Proforma Invoice Detail Part
                    if (oProformaInvoiceDetails != null)
                    {
                        foreach (ProformaInvoiceDetail oItem in oProformaInvoiceDetails)
                        {
                            IDataReader readerdetail;
                            oItem.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                            if (oItem.ProformaInvoiceDetailID <= 0)
                            {
                                readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                            }
                            else
                            {
                                readerdetail = ProformaInvoiceDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                            }
                            NullHandler oReaderDetail = new NullHandler(readerdetail);
                            if (readerdetail.Read())
                            {
                                sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs + oReaderDetail.GetString("ProformaInvoiceDetailID") + ",";
                            }
                            readerdetail.Close();
                        }
                        if (sProformaInvoiceDetailIDs.Length > 0)
                        {
                            sProformaInvoiceDetailIDs = sProformaInvoiceDetailIDs.Remove(sProformaInvoiceDetailIDs.Length - 1, 1);
                        }
                        oProformaInvoiceDetail = new ProformaInvoiceDetail();
                        oProformaInvoiceDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                        ProformaInvoiceDetailDA.Delete(tc, oProformaInvoiceDetail, EnumDBOperation.Delete, nUserID, sProformaInvoiceDetailIDs);
                    }
                    #endregion
                }
                #endregion

                #region MasterLC Detail Part

                oMasterLCDetail.ProformaInvoiceID = oProformaInvoice.ProformaInvoiceID;
                oMasterLCDetail.PINo = oProformaInvoice.PINo;
                oMasterLCDetail.BuyerID = oProformaInvoice.BuyerID;
                oMasterLCDetail.PIStatus = oProformaInvoice.PIStatus;
                oMasterLCDetail.PIStatusInInt = oProformaInvoice.PIStatusInInt;

                IDataReader readerForMLCD = null;
                if (oMasterLCDetail.MasterLCDetailID <= 0)
                {
                    readerForMLCD = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Insert, nUserID, "");
                }
                else
                {
                    readerForMLCD = MasterLCDetailDA.InsertUpdate(tc, oMasterLCDetail, EnumDBOperation.Update, nUserID, "");
                }
                NullHandler oReaderForMLCD = new NullHandler(readerForMLCD);
                if (readerForMLCD.Read())
                {
                    oMasterLCDetail = new MasterLCDetail();
                    oMasterLCDetail = CreateObject(oReaderForMLCD);
                }
                readerForMLCD.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMasterLCDetail.ErrorMessage = e.Message;
                #endregion
            }
            return oMasterLCDetail;
        }
        public string DeleteMLCDeatil(int nMasterLCDetailID, int nProformaInvoiceID, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProformaInvoice oProformaInvoice = new ProformaInvoice();
                oProformaInvoice.ProformaInvoiceID = nProformaInvoiceID;
                ProformaInvoiceDA.Delete(tc, oProformaInvoice, EnumDBOperation.Delete, nUserId);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.MasterLC, EnumRoleOperationType.Delete);
                MasterLCDetailDA.DeleteSingleMLCDetail(tc, nMasterLCDetailID);
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
        public MasterLCDetail Get(int MasterLCDetailID, Int64 nUserId)
        {
            MasterLCDetail oAccountHead = new MasterLCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCDetailDA.Get(tc, MasterLCDetailID);
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
                throw new ServiceException("Failed to Get MasterLCDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        //GetByOrderRecap
        public MasterLCDetail GetByOrderRecap(int OrderRecapID, Int64 nUserId)
        {
            MasterLCDetail oAccountHead = new MasterLCDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MasterLCDetailDA.GetByOrderRecap(tc, OrderRecapID);
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
                throw new ServiceException("Failed to Get MasterLCDetail", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<MasterLCDetail> Gets(int LabDipOrderID, Int64 nUserID)
        {
            List<MasterLCDetail> oMasterLCDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDetailDA.Gets(LabDipOrderID, tc);
                oMasterLCDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCDetail", e);
                #endregion
            }

            return oMasterLCDetail;
        }

        public List<MasterLCDetail> GetsMasterLCLog(int ProformaInvoiceLogID, Int64 nUserID)
        {
            List<MasterLCDetail> oMasterLCDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDetailDA.GetsMasterLCLog(ProformaInvoiceLogID, tc);
                oMasterLCDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCDetail", e);
                #endregion
            }

            return oMasterLCDetail;
        }


        public List<MasterLCDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<MasterLCDetail> oMasterLCDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MasterLCDetailDA.Gets(tc, sSQL);
                oMasterLCDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MasterLCDetail", e);
                #endregion
            }

            return oMasterLCDetail;
        }
        #endregion
    }
    
    
}
