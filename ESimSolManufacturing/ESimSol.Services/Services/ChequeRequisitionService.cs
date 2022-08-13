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
    public class ChequeRequisitionService : MarshalByRefObject, IChequeRequisitionService
    {
        #region Private functions and declaration
        private ChequeRequisition MapObject(NullHandler oReader)
        {
            ChequeRequisition oChequeRequisition = new ChequeRequisition();
            oChequeRequisition.ChequeRequisitionID = oReader.GetInt32("ChequeRequisitionID");
            oChequeRequisition.RequisitionNo = oReader.GetString("RequisitionNo");
            oChequeRequisition.BUID = oReader.GetInt32("BUID");
            oChequeRequisition.RequisitionStatus = (EnumChequeRequisitionStatus)oReader.GetInt32("RequisitionStatus");
            oChequeRequisition.RequisitionStatusInt = oReader.GetInt32("RequisitionStatus");
            oChequeRequisition.RequisitionDate = oReader.GetDateTime("RequisitionDate");            
            oChequeRequisition.SubledgerID = oReader.GetInt32("SubledgerID");
            oChequeRequisition.PayTo = oReader.GetInt32("PayTo");
            oChequeRequisition.ChequeDate = oReader.GetDateTime("ChequeDate");
            oChequeRequisition.ChequeType = (EnumPaymentType)oReader.GetInt32("ChequeType");
            oChequeRequisition.ChequeTypeInt = oReader.GetInt32("ChequeType");
            oChequeRequisition.BankAccountID = oReader.GetInt32("BankAccountID");
            oChequeRequisition.BankBookID = oReader.GetInt32("BankBookID");
            oChequeRequisition.ChequeID = oReader.GetInt32("ChequeID");
            oChequeRequisition.ChequeAmount = oReader.GetDouble("ChequeAmount");
            oChequeRequisition.ApprovedBy = oReader.GetInt32("ApprovedBy");
            oChequeRequisition.Remarks = oReader.GetString("Remarks");
            oChequeRequisition.BUName = oReader.GetString("BUName");
            oChequeRequisition.BUCode = oReader.GetString("BUCode");            
            oChequeRequisition.SubledgerName = oReader.GetString("SubledgerName");
            oChequeRequisition.SubledgerCode = oReader.GetString("SubledgerCode");
            oChequeRequisition.ChequeIssueTo = oReader.GetString("ChequeIssueTo");
            oChequeRequisition.SecondLineIssueTo = oReader.GetString("SecondLineIssueTo");
            oChequeRequisition.AccountNo = oReader.GetString("AccountNo");
            oChequeRequisition.BankName = oReader.GetString("BankName");
            oChequeRequisition.BranchName = oReader.GetString("BranchName");
            oChequeRequisition.BookCode = oReader.GetString("BookCode");
            oChequeRequisition.ChequeNo = oReader.GetString("ChequeNo");
            oChequeRequisition.ChequeStatus = (EnumChequeStatus)oReader.GetInt32("ChequeStatus");
            oChequeRequisition.ApprovedByName = oReader.GetString("ApprovedByName");
            oChequeRequisition.IsWillVoucherEffect = oReader.GetBoolean("IsWillVoucherEffect");
            return oChequeRequisition;
        }

        private ChequeRequisition CreateObject(NullHandler oReader)
        {
            ChequeRequisition oChequeRequisition = new ChequeRequisition();
            oChequeRequisition = MapObject(oReader);
            return oChequeRequisition;
        }

        private List<ChequeRequisition> CreateObjects(IDataReader oReader)
        {
            List<ChequeRequisition> oChequeRequisitions = new List<ChequeRequisition>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChequeRequisition oItem = CreateObject(oHandler);
                oChequeRequisitions.Add(oItem);
            }
            return oChequeRequisitions;
        }
        #endregion

        #region Interface implementation
        public ChequeRequisitionService() { }

        public ChequeRequisition Save(ChequeRequisition oChequeRequisition, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ChequeRequisitionDetail> oChequeRequisitionDetails = new List<ChequeRequisitionDetail>();
                ChequeRequisitionDetail oChequeRequisitionDetail = new ChequeRequisitionDetail();

                oChequeRequisitionDetails = oChequeRequisition.ChequeRequisitionDetails;
                string sChequeRequisitionDetailIDs = "";

                #region ChequeRequisition part
                IDataReader reader;
                if (oChequeRequisition.ChequeRequisitionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChequeRequisition, EnumRoleOperationType.Add);
                    reader = ChequeRequisitionDA.InsertUpdate(tc, oChequeRequisition, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChequeRequisition, EnumRoleOperationType.Edit);
                    reader = ChequeRequisitionDA.InsertUpdate(tc, oChequeRequisition, EnumDBOperation.Update, nUserID);
                }

                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisition = new ChequeRequisition();
                    oChequeRequisition = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region Valid ChequeRequisition
                if (oChequeRequisition.ChequeRequisitionID <= 0)
                {
                    throw new Exception("Invalid ChequeRequisition!");
                }
                #endregion

                #region ChequeRequisition Detail Part
                if (oChequeRequisitionDetails != null)
                {
                    foreach (ChequeRequisitionDetail oItem in oChequeRequisitionDetails)
                    {
                        IDataReader readerdetail;
                        oItem.ChequeRequisitionID = oChequeRequisition.ChequeRequisitionID;
                        if (oItem.ChequeRequisitionDetailID <= 0)
                        {
                            readerdetail = ChequeRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = ChequeRequisitionDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sChequeRequisitionDetailIDs = sChequeRequisitionDetailIDs + oReaderDetail.GetString("ChequeRequisitionDetailID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sChequeRequisitionDetailIDs.Length > 0)
                    {
                        sChequeRequisitionDetailIDs = sChequeRequisitionDetailIDs.Remove(sChequeRequisitionDetailIDs.Length - 1, 1);
                    }
                    oChequeRequisitionDetail = new ChequeRequisitionDetail();
                    oChequeRequisitionDetail.ChequeRequisitionID = oChequeRequisition.ChequeRequisitionID;
                    ChequeRequisitionDetailDA.Delete(tc, oChequeRequisitionDetail, EnumDBOperation.Delete, nUserID, sChequeRequisitionDetailIDs);
                }

                #endregion

                #region Get ChequeRequisition
                reader = ChequeRequisitionDA.Get(tc, oChequeRequisition.ChequeRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisition = CreateObject(oReader);
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

                oChequeRequisition = new ChequeRequisition();
                oChequeRequisition.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oChequeRequisition;
        }

        public ChequeRequisition Approved(ChequeRequisition oChequeRequisition, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region Valid ChequeRequisition
                if (oChequeRequisition.ChequeRequisitionID <= 0)
                {
                    throw new Exception("Invalid ChequeRequisition!");
                }
                if (oChequeRequisition.ApprovedBy != 0)
                {
                    throw new Exception("Your Selected ChequeRequisition Already Approved!");
                }
                #endregion

                #region Approved PurchaseInvocie
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChequeRequisition, EnumRoleOperationType.Approved);
                ChequeRequisitionDA.Approved(tc, oChequeRequisition, nUserID);
                #endregion

                #region Get ChequeRequisition
                IDataReader reader;
                NullHandler oReader;
                reader = ChequeRequisitionDA.Get(tc, oChequeRequisition.ChequeRequisitionID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisition = CreateObject(oReader);
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

                oChequeRequisition = new ChequeRequisition();
                string Message = "";
                Message = e.Message;
                Message = Message.Split('!')[0];
                oChequeRequisition.ErrorMessage = Message;

                #endregion
            }
            return oChequeRequisition;
        }

        public string Delete(ChequeRequisition oChequeRequisition, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ChequeRequisition, EnumRoleOperationType.Delete);
                ChequeRequisitionDA.Delete(tc, oChequeRequisition, EnumDBOperation.Delete, nUserID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //throw new Exception(e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ChequeRequisition Get(int nChequeRequisitionID, int nUserID)
        {
            ChequeRequisition oChequeRequisition = new ChequeRequisition();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChequeRequisitionDA.Get(tc, nChequeRequisitionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                throw new Exception("Failed to Get ChequeRequisition \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisition;
        }

        public List<ChequeRequisition> Gets(int nUserID)
        {
            List<ChequeRequisition> oChequeRequisitions = new List<ChequeRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDA.Gets(tc);
                oChequeRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisition \n" + e.Message, e);
                #endregion
            }

            return oChequeRequisitions;
        }

        public List<ChequeRequisition> Gets(string sSQL, int nUserID)
        {
            List<ChequeRequisition> oChequeRequisitions = new List<ChequeRequisition>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDA.Gets(tc, sSQL);
                oChequeRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisitions \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisitions;
        }
        public List<ChequeRequisition> GetsInitialChequeRequisitions(int nUserID)
        {
            List<ChequeRequisition> oChequeRequisitions = new List<ChequeRequisition>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChequeRequisitionDA.GetsInitialChequeRequisitions(tc, nUserID);
                oChequeRequisitions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                throw new Exception("Failed to Get ChequeRequisitions \n" + e.Message, e);
                #endregion
            }
            return oChequeRequisitions;
        }

        public ChequeRequisition UpdateVoucherEffect(ChequeRequisition oChequeRequisition, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChequeRequisitionDA.UpdateVoucherEffect(tc, oChequeRequisition);
                IDataReader reader;
                reader = ChequeRequisitionDA.Get(tc, oChequeRequisition.ChequeRequisitionID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChequeRequisition = new ChequeRequisition();
                    oChequeRequisition = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChequeRequisition = new ChequeRequisition();
                oChequeRequisition.ErrorMessage = e.Message.Split('~')[0];

                #endregion
            }

            return oChequeRequisition;

        }
        #endregion
    }
}
