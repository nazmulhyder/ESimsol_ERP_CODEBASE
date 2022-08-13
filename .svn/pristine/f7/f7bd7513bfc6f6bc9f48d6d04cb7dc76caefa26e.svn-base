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

    public class PurchaseQuotationService : MarshalByRefObject, IPurchaseQuotationService
    {
        #region Private functions and declaration
        private PurchaseQuotation MapObject(NullHandler oReader)
        {
            PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
            oPurchaseQuotation.PurchaseQuotationLogID = oReader.GetInt32("PurchaseQuotationLogID");
            oPurchaseQuotation.PurchaseQuotationID = oReader.GetInt32("PurchaseQuotationID");
            oPurchaseQuotation.PurchaseQuotationNo = oReader.GetString("PurchaseQuotationNo");
            oPurchaseQuotation.QuotationStatus = (EnumQuotationStatus)oReader.GetInt32("QuotationStatus");
            oPurchaseQuotation.QuotationStatusInInt = oReader.GetInt32("QuotationStatus");
            oPurchaseQuotation.SupplierReference = oReader.GetString("SupplierReference");
            oPurchaseQuotation.RateCollectDate = oReader.GetDateTime("RateCollectDate");
            oPurchaseQuotation.ExpiredDate = oReader.GetDateTime("ExpiredDate");
            oPurchaseQuotation.SCPerson = oReader.GetInt32("SCPerson");
            oPurchaseQuotation.SCPersonName = oReader.GetString("SCPersonName");
            oPurchaseQuotation.CollectBy = oReader.GetInt32("CollectBy");
            oPurchaseQuotation.CurrencyID = oReader.GetInt32("CurrencyID");
            oPurchaseQuotation.CurrencyName = oReader.GetString("CurrencyName");
            oPurchaseQuotation.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPurchaseQuotation.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oPurchaseQuotation.ApprovedDate = oReader.GetDateTime("ApprovedDate");
            oPurchaseQuotation.Remarks = oReader.GetString("Remarks");
            oPurchaseQuotation.SupplierID = oReader.GetInt32("SupplierID");
            oPurchaseQuotation.BuyerID = oReader.GetInt32("BuyerID");
            oPurchaseQuotation.BuyerName = oReader.GetString("BuyerName");
            oPurchaseQuotation.SupplierName = oReader.GetString("SupplierName");
            oPurchaseQuotation.CollectByName = oReader.GetString("CollectByName");
            oPurchaseQuotation.ApprovedByName = oReader.GetString("ApprovedByName");
            oPurchaseQuotation.Source = (EnumSource)oReader.GetInt32("Source");
            oPurchaseQuotation.SourceInInt = oReader.GetInt32("Source");
            oPurchaseQuotation.Activity = oReader.GetBoolean("Activity");
            oPurchaseQuotation.SupplierAddress = oReader.GetString("SupplierAddress");
            oPurchaseQuotation.PaymentTerm = oReader.GetString("PaymentTerm");
            oPurchaseQuotation.BUID = oReader.GetInt32("BUID");
            oPurchaseQuotation.BUName = oReader.GetString("BUName");
            oPurchaseQuotation.DiscountInAmount = oReader.GetDouble("DiscountInAmount");
            oPurchaseQuotation.DiscountInPercent = oReader.GetDouble("DiscountInPercent");
            oPurchaseQuotation.VatInAmount = oReader.GetDouble("VatInAmount");
            oPurchaseQuotation.VatInPercent = oReader.GetDouble("VatInPercent");
            oPurchaseQuotation.TransportCostInAmount = oReader.GetDouble("TransportCostInAmount");
            oPurchaseQuotation.TransportCostInPercent = oReader.GetDouble("TransportCostInPercent");
            return oPurchaseQuotation;
        }

        private PurchaseQuotation CreateObject(NullHandler oReader)
        {
            PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
            oPurchaseQuotation = MapObject(oReader);
            return oPurchaseQuotation;
        }

        private List<PurchaseQuotation> CreateObjects(IDataReader oReader)
        {
            List<PurchaseQuotation> oPurchaseQuotation = new List<PurchaseQuotation>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PurchaseQuotation oItem = CreateObject(oHandler);
                oPurchaseQuotation.Add(oItem);
            }
            return oPurchaseQuotation;
        }

        #endregion

        #region Interface implementation
        public PurchaseQuotationService() { }

        public PurchaseQuotation Save(PurchaseQuotation oPurchaseQuotation, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            List<PQTermsAndCondition> oPQTermsAndConditions = new List<PQTermsAndCondition>();
            oPurchaseQuotationDetails = oPurchaseQuotation.PurchaseQuotationDetails;
            oPQTermsAndConditions = oPurchaseQuotation.PQTermsAndConditions;
            string sPurchaseQuotationDetailIDs = "";
            string sPQTermsAndConditionIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //IDataReader readerdetail
                if (oPurchaseQuotation.PurchaseQuotationID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseQuotation, EnumRoleOperationType.Add);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseQuotation, EnumRoleOperationType.Edit);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = new PurchaseQuotation();
                    oPurchaseQuotation = CreateObject(oReader);
                }
                reader.Close();
                #region PurchaseQuotationDetail
                if (oPurchaseQuotationDetails != null)
                {
                    foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                        if (oItem.PurchaseQuotationDetailID <= 0)
                        {
                            readerdetail = PurchaseQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs + oReaderDetail.GetString("PurchaseQuotationDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseQuotationDetailIDs.Length > 0)
                    {
                        sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs.Remove(sPurchaseQuotationDetailIDs.Length - 1, 1);
                    }
                    PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();
                    oPurchaseQuotationDetail.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                    PurchaseQuotationDetailDA.Delete(tc, oPurchaseQuotationDetail, EnumDBOperation.Delete, nUserID, sPurchaseQuotationDetailIDs);

                }

                #endregion

                #region PQTermsAndConditions
                if (oPQTermsAndConditions != null)
                {
                    foreach (PQTermsAndCondition oItem in oPQTermsAndConditions)
                    {
                        IDataReader readerdetail;
                        oItem.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                        if (oItem.PQTermsAndConditionID <= 0)
                        {
                            readerdetail = PQTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PQTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPQTermsAndConditionIDs = sPQTermsAndConditionIDs + oReaderDetail.GetString("PQTermsAndConditionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPQTermsAndConditionIDs.Length > 0)
                    {
                        sPQTermsAndConditionIDs = sPQTermsAndConditionIDs.Remove(sPQTermsAndConditionIDs.Length - 1, 1);
                    }
                    PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
                    oPQTermsAndCondition.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                    PQTermsAndConditionDA.Delete(tc, oPQTermsAndCondition, EnumDBOperation.Delete, nUserID, sPQTermsAndConditionIDs);

                }

                #endregion


                reader = PurchaseQuotationDA.Get(tc, oPurchaseQuotation.PurchaseQuotationID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = new PurchaseQuotation();
                    oPurchaseQuotation = CreateObject(oReader);
                }

                reader.Close();
                IDataReader readerdetails = null;
                readerdetails = PurchaseQuotationDetailDA.Gets(oPurchaseQuotation.PurchaseQuotationID, tc);
                PurchaseQuotationDetailService obj = new PurchaseQuotationDetailService();
                oPurchaseQuotation.PurchaseQuotationDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseQuotation = new PurchaseQuotation();
                oPurchaseQuotation.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseQuotation;
        }

        public PurchaseQuotation AcceptRevise(PurchaseQuotation oPurchaseQuotation, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            List<PQTermsAndCondition> oPQTermsAndConditions = new List<PQTermsAndCondition>();
            oPurchaseQuotationDetails = oPurchaseQuotation.PurchaseQuotationDetails;
            oPQTermsAndConditions = oPurchaseQuotation.PQTermsAndConditions;
            string sPurchaseQuotationDetailIDs = "";
            string sPQTermsAndConditionIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                //IDataReader readerdetail
                if (oPurchaseQuotation.PurchaseQuotationID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseQuotation, EnumRoleOperationType.Add);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PurchaseQuotation, EnumRoleOperationType.Edit);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = new PurchaseQuotation();
                    oPurchaseQuotation = CreateObject(oReader);
                }
                reader.Close();
                #region PurchaseQuotationDetail
                if (oPurchaseQuotationDetails != null)
                {
                    foreach (PurchaseQuotationDetail oItem in oPurchaseQuotationDetails)
                    {
                        IDataReader readerdetail;
                        oItem.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                        if (oItem.PurchaseQuotationDetailID <= 0)
                        {
                            readerdetail = PurchaseQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PurchaseQuotationDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs + oReaderDetail.GetString("PurchaseQuotationDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPurchaseQuotationDetailIDs.Length > 0)
                    {
                        sPurchaseQuotationDetailIDs = sPurchaseQuotationDetailIDs.Remove(sPurchaseQuotationDetailIDs.Length - 1, 1);
                    }
                    PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();
                    oPurchaseQuotationDetail.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                    PurchaseQuotationDetailDA.Delete(tc, oPurchaseQuotationDetail, EnumDBOperation.Delete, nUserID, sPurchaseQuotationDetailIDs);

                }

                #endregion

                #region PQTermsAndConditions
                if (oPQTermsAndConditions != null)
                {
                    foreach (PQTermsAndCondition oItem in oPQTermsAndConditions)
                    {
                        IDataReader readerdetail;
                        oItem.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                        if (oItem.PQTermsAndConditionID <= 0)
                        {
                            readerdetail = PQTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = PQTermsAndConditionDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sPQTermsAndConditionIDs = sPQTermsAndConditionIDs + oReaderDetail.GetString("PQTermsAndConditionID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sPQTermsAndConditionIDs.Length > 0)
                    {
                        sPQTermsAndConditionIDs = sPQTermsAndConditionIDs.Remove(sPQTermsAndConditionIDs.Length - 1, 1);
                    }
                    PQTermsAndCondition oPQTermsAndCondition = new PQTermsAndCondition();
                    oPQTermsAndCondition.PurchaseQuotationID = oPurchaseQuotation.PurchaseQuotationID;
                    PQTermsAndConditionDA.Delete(tc, oPQTermsAndCondition, EnumDBOperation.Delete, nUserID, sPQTermsAndConditionIDs);

                }

                #endregion


                reader = PurchaseQuotationDA.Get(tc, oPurchaseQuotation.PurchaseQuotationID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = new PurchaseQuotation();
                    oPurchaseQuotation = CreateObject(oReader);
                }

                reader.Close();
                IDataReader readerdetails = null;
                readerdetails = PurchaseQuotationDetailDA.Gets(oPurchaseQuotation.PurchaseQuotationID, tc);
                PurchaseQuotationDetailService obj = new PurchaseQuotationDetailService();
                oPurchaseQuotation.PurchaseQuotationDetails = obj.CreateObjects(readerdetails);
                readerdetails.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseQuotation = new PurchaseQuotation();
                oPurchaseQuotation.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseQuotation;
        }
        public PurchaseQuotation Approve(PurchaseQuotation oPurchaseQuotation, Int64 nUserID)
        {
            List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
            oPurchaseQuotationDetails = oPurchaseQuotation.PurchaseQuotationDetails;
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPurchaseQuotation.PurchaseQuotationID <= 0)
                {
                   // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Approval, nUserID);
                }
                else
                {
                   // AuthorizationRoleDA.CheckUserPermission(tc, nUserID, "PurchaseQuotation", EnumRoleOperationType.Approved);
                    reader = PurchaseQuotationDA.InsertUpdate(tc, oPurchaseQuotation, EnumDBOperation.Approval, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = new PurchaseQuotation();
                    oPurchaseQuotation = CreateObject(oReader);
                }
                reader.Close();
           

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oPurchaseQuotation = new PurchaseQuotation();
                oPurchaseQuotation.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPurchaseQuotation;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
                oPurchaseQuotation.PurchaseQuotationID = id;
                //AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "PurchaseQuotation", EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "PurchaseQuotation", id);
                PurchaseQuotationDA.Delete(tc, oPurchaseQuotation, EnumDBOperation.Delete, nUserId);
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
            return "deleted";
        }
        public PurchaseQuotation RequestQuotationRevise(PurchaseQuotation oPurchaseQuotation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<PurchaseQuotationDetail> oPurchaseQuotationDetails = new List<PurchaseQuotationDetail>();
                PurchaseQuotationDetail oPurchaseQuotationDetail = new PurchaseQuotationDetail();


                oPurchaseQuotationDetails = oPurchaseQuotation.PurchaseQuotationDetails;

                string sPurchaseQuotationDetailIDs = "";

                #region PurchaseQuotation  part

                if (oPurchaseQuotation.PurchaseQuotationID > 0)
                {
                    IDataReader reader;
                    oPurchaseQuotation.QuotationStatusInInt = (int)EnumQuotationStatus.RequestRevise;
                    reader = PurchaseQuotationDA.RequestQuotationRevise(tc, oPurchaseQuotation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseQuotation = new PurchaseQuotation();
                        oPurchaseQuotation = CreateObject(oReader);
                    }
                    reader.Close();

                #endregion



                    #region PurchaseRequisition Get
                    reader = PurchaseQuotationDA.Get(tc, oPurchaseQuotation.PurchaseQuotationID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oPurchaseQuotation = CreateObject(oReader);
                    }
                    reader.Close();
                }
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
                oPurchaseQuotation.ErrorMessage = Message;

                #endregion
            }
            return oPurchaseQuotation;
        }
        public PurchaseQuotation Get(int id, Int64 nUserId)
        {
            PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseQuotationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotation;
        }
        public PurchaseQuotation GetByLog(int id, Int64 nUserId)
        {
            PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PurchaseQuotationDA.GetByLog(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotation;
        }

        public PurchaseQuotation SendToMgt(int id, Int64 nUserId)
        {
            PurchaseQuotation oPurchaseQuotation = new PurchaseQuotation();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                PurchaseQuotationDA.SendToMgt(tc,id);
                IDataReader reader = PurchaseQuotationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPurchaseQuotation = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotation;
        }


      
        public List<PurchaseQuotation> Gets(Int64 nUserID)
        {
            List<PurchaseQuotation> oPurchaseQuotation = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDA.Gets(tc);
                oPurchaseQuotation = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotation;
        }

        public List<PurchaseQuotation> Gets(string sSQL, Int64 nUserID)
        {
            List<PurchaseQuotation> oPurchaseQuotations = new List<PurchaseQuotation>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDA.Gets(tc, sSQL);
                oPurchaseQuotations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotations;
        }
        public List<PurchaseQuotation> GetsByLog(string sSQL, Int64 nUserID)
        {
            List<PurchaseQuotation> oPurchaseQuotations = new List<PurchaseQuotation>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDA.GetsByLog(tc, sSQL);
                oPurchaseQuotations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotations;
        }

        //GetsByBU
        public List<PurchaseQuotation> GetsByBU(int nBUID, Int64 nUserID)
        {
            List<PurchaseQuotation> oPurchaseQuotations = new List<PurchaseQuotation>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PurchaseQuotationDA.GetsByBU(nBUID, tc);
                oPurchaseQuotations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchaseQuotation", e);
                #endregion
            }

            return oPurchaseQuotations;
        }
        #endregion
    }   
    
    
}
