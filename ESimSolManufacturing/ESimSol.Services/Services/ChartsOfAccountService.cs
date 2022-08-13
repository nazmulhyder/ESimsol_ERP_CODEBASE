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
    public class ChartsOfAccountService : MarshalByRefObject, IChartsOfAccountService
    {
        #region Private functions and declaration
        private ChartsOfAccount MapObject(NullHandler oReader)
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount(); 
            oChartsOfAccount.AccountHeadID=oReader.GetInt32("AccountHeadID");
            oChartsOfAccount.DAHCID=oReader.GetInt32("DAHCID");
            oChartsOfAccount.AccountCode=oReader.GetString("AccountCode");
            oChartsOfAccount.AccountHeadName=oReader.GetString("AccountHeadName");
            oChartsOfAccount.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oChartsOfAccount.CurrencyID = oReader.GetInt32("CurrencyID");
            oChartsOfAccount.ReferenceObjectID=oReader.GetInt32("ReferenceObjectID");
            oChartsOfAccount.Description=oReader.GetString("Description");
            oChartsOfAccount.IsJVNode=oReader.GetBoolean("IsJVNode");
            oChartsOfAccount.IsDynamic=oReader.GetBoolean("IsDynamic");
            oChartsOfAccount.ParentHeadID=oReader.GetInt32("ParentHeadID");
            oChartsOfAccount.ParentHeadName = oReader.GetString("ParentHeadName");
            oChartsOfAccount.PathName = oReader.GetString("PathName");
            oChartsOfAccount.ComponentID = oReader.GetInt32("ComponentID");
            oChartsOfAccount.InventoryHeadID = oReader.GetInt32("InventoryHeadID");
            oChartsOfAccount.ComponentType = oReader.GetString("ComponentType");
            oChartsOfAccount.OpeningBalance = oReader.GetDouble("OpeningBalance");
            oChartsOfAccount.ReferenceTypeInt = oReader.GetInt16("ReferenceType");
            oChartsOfAccount.AccountHeadCodeName = oReader.GetString("AccountHeadCodeName");
            oChartsOfAccount.CName = oReader.GetString("CName");
            oChartsOfAccount.CSymbol = oReader.GetString("CSymbol");
            oChartsOfAccount.IsCostCenterApply = oReader.GetBoolean("IsCostCenterApply");
            oChartsOfAccount.IsBillRefApply = oReader.GetBoolean("IsBillRefApply");
            oChartsOfAccount.IsInventoryApply = oReader.GetBoolean("IsInventoryApply");
            oChartsOfAccount.IsOrderReferenceApply = oReader.GetBoolean("IsOrderReferenceApply");
            oChartsOfAccount.AccountOperationType = (EnumAccountOperationType)oReader.GetInt32("AccountOperationType");
            oChartsOfAccount.ParentAccountOperationType = (EnumAccountOperationType)oReader.GetInt32("ParentAccountOperationType");
            oChartsOfAccount.LedgerBalance = oReader.GetString("LedgerBalance");
            return oChartsOfAccount;         
        }

        private ChartsOfAccount CreateObject(NullHandler oReader)
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount=MapObject(oReader);
            return oChartsOfAccount;
        }

        private List<ChartsOfAccount> CreateObjects(IDataReader oReader)
        {
            List<ChartsOfAccount> oChartsOfAccount = new List<ChartsOfAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ChartsOfAccount oItem = CreateObject(oHandler);
                oChartsOfAccount.Add(oItem);
            }
            return oChartsOfAccount;
        }

        #endregion

        #region Interface implementation
        public ChartsOfAccountService() { }
        public ChartsOfAccount Save(ChartsOfAccount oChartsOfAccount, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oChartsOfAccount.AccountHeadID<=0)
                {
                    reader = ChartsOfAccountDA.InsertUpdate(tc, oChartsOfAccount, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                    reader = ChartsOfAccountDA.InsertUpdate(tc, oChartsOfAccount, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChartsOfAccount = new ChartsOfAccount();
                    oChartsOfAccount= CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChartsOfAccount.ErrorMessage = e.Message.Split('~')[0];                
                #endregion
            }
            return oChartsOfAccount;
        }
        public ChartsOfAccount SaveTemplateTree(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            TransactionContext tc = null;
            ChartsOfAccount oTempChartsOfAccount = new ChartsOfAccount();
            List<TChartsOfAccount> oTChartsOfAccounts = new List<TChartsOfAccount>();
            oTChartsOfAccounts = oChartsOfAccount.TChartsOfAccounts;
            try
            {
                tc = TransactionContext.Begin(true);                
                ChartsOfAccountDA.RestChartsOfAccount(tc);
                ChartsOfAccountDA.RestCompanyWiseAccountHead(tc);
                foreach (TChartsOfAccount oItemGroup in oTChartsOfAccounts)
                {                   
                   this.SaveCOAFromTemplate(oItemGroup, oItemGroup.AccountHeadId, tc, nUserID);                   
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oTempChartsOfAccount = new ChartsOfAccount();
                oTempChartsOfAccount.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTempChartsOfAccount;
        }

        private void SaveCOAFromTemplate(TChartsOfAccount oTChartsOfAccount, int nParentID, TransactionContext tc, int nUserID)
        {
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            oChartsOfAccount.AccountHeadID = 0;
            oChartsOfAccount.DAHCID = 0;
            oChartsOfAccount.AccountCode = oTChartsOfAccount.code;
            oChartsOfAccount.AccountHeadName = oTChartsOfAccount.text;
            oChartsOfAccount.AccountType = (EnumAccountType)oTChartsOfAccount.AccountTypeInInt;
            oChartsOfAccount.ReferenceObjectID = 0;
            oChartsOfAccount.Description = oTChartsOfAccount.Description;
            oChartsOfAccount.IsJVNode = false;
            oChartsOfAccount.IsDynamic = false;
            oChartsOfAccount.ParentHeadID = nParentID;            
            IDataReader reader;
            reader = ChartsOfAccountDA.InsertUpdate(tc, oChartsOfAccount, EnumDBOperation.Insert, nUserID);
            NullHandler oReader = new NullHandler(reader);
            if (reader.Read())
            {
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount = CreateObject(oReader);
            }
            reader.Close();

            if (oTChartsOfAccount.children != null)
            {
                if (oTChartsOfAccount.children.Count > 0)
                {
                    foreach (TChartsOfAccount oItem in oTChartsOfAccount.children)
                    {
                        this.SaveCOAFromTemplate(oItem, oChartsOfAccount.AccountHeadID, tc, nUserID);
                    }
                }
            }
        }
      
        public ChartsOfAccount Update_DynamicHead(ChartsOfAccount oChartsOfAccount, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                int nAccountHeadID = ChartsOfAccountDA.GetExistsAccount(tc, oChartsOfAccount.ReferenceObjectID, oChartsOfAccount.ReferenceTypeInt);
                if (nAccountHeadID > 0)
                {
                    if (oChartsOfAccount.AccountHeadID != nAccountHeadID)
                    {
                        throw new Exception("Selected reference already apply in another account head!");
                    }
                }
                ChartsOfAccountDA.Update_DynamicHead(tc, oChartsOfAccount);            
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChartsOfAccount.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save AccountHead. Because of " + e.Message, e);
                #endregion
            }
            return oChartsOfAccount;
        }


        public ChartsOfAccount Update_InventoryHead(ChartsOfAccount oChartsOfAccount, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChartsOfAccountDA.Update_InventoryHead(tc, oChartsOfAccount);
                IDataReader reader = ChartsOfAccountDA.Get(tc, oChartsOfAccount.AccountHeadID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChartsOfAccount = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChartsOfAccount.ErrorMessage = e.Message;
                #endregion
            }
            return oChartsOfAccount;
        }
        public ChartsOfAccount MoveChartOfAccount(ChartsOfAccount oChartsOfAccount, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = ChartsOfAccountDA.MoveChartOfAccount(tc, oChartsOfAccount, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oChartsOfAccount = new ChartsOfAccount();
                    oChartsOfAccount= CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChartsOfAccount.ErrorMessage = e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save AccountHead. Because of " + e.Message, e);
                #endregion
            }
            return oChartsOfAccount;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.AccountHeadID = id;                
                ChartsOfAccountDA.Delete(tc, oChartsOfAccount, EnumDBOperation.Delete, nUserId);
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
            return "Data Delete Successfully";
        }
        public string GetAccountCode(int nParentID, int nUserId)
        {
            string sAccountCode = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                sAccountCode = ChartsOfAccountDA.GetAccountCode(tc, nParentID);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                sAccountCode = e.Message;
                #endregion
            }
            return sAccountCode;
        }
        public string CopyCOA(string sAccountHeadIDs,  int nCompanyID, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ChartsOfAccountDA.CopyCOA(tc, sAccountHeadIDs, nCompanyID, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message;
                #endregion
            }
            return "Copy Successfully";
        }
        public ChartsOfAccount GetByCode(string sAccountCode, int nUserID)
        {
            ChartsOfAccount oAccountHead = new ChartsOfAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChartsOfAccountDA.GetByCode(tc, sAccountCode);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                if (oAccountHead.AccountHeadID > 0)
                { 
                    List<ChartsOfAccount> oAccountHeads=new List<ChartsOfAccount>();
                    oAccountHeads.Add(oAccountHead);
                    oAccountHeads= ConfigurationAttach(oAccountHeads, tc);
                    oAccountHead = oAccountHeads[0];
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountHead", e);
                #endregion
            }
            return oAccountHead;
        }
        public ChartsOfAccount Get(int id, int nUserId)
        {
            ChartsOfAccount oAccountHead = new ChartsOfAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ChartsOfAccountDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
                }
                reader.Close();
                if (oAccountHead.AccountHeadID > 0)
                { 
                    List<ChartsOfAccount> oAccountHeads=new List<ChartsOfAccount>();
                    oAccountHeads.Add(oAccountHead);
                    oAccountHeads= ConfigurationAttach(oAccountHeads, tc);
                    oAccountHead = oAccountHeads[0];
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get AccountHead", e);
                #endregion
            }
            return oAccountHead;
        }
        public List<ChartsOfAccount> GetsbyAccountsName(string sAccountsName,  int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsbyAccountsName(tc, sAccountsName);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                oChartsOfAccounts = this.ConfigurationAttach(oChartsOfAccounts, tc);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccounts;
        }
        public List<ChartsOfAccount> GetsbyAccountTypeOrName(int nAccountType, string sAccountsName, int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsbyAccountTypeOrName(tc,  nAccountType,sAccountsName);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                oChartsOfAccounts = this.ConfigurationAttach(oChartsOfAccounts, tc);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccounts;
        }
        public List<ChartsOfAccount> Gets(int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccount = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.Gets(tc);
                oChartsOfAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccount;
        }
        public List<ChartsOfAccount> GetsByParent(int nParentID, int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccount = new List<ChartsOfAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsByParent(tc, nParentID);
                oChartsOfAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccount;
        }
        public List<ChartsOfAccount> GetsForCOATemplate(int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccount = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsForCOATemplate(tc);
                oChartsOfAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccount;
        }
        public List<ChartsOfAccount> AccountHeadGets(int nVoucherTypeID, bool bIsDebit, int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChartsOfAccountDA.AccountHeadGets(tc, nVoucherTypeID, bIsDebit);
                oChartsOfAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccount;
        }
        
        public List<ChartsOfAccount> GetsByCodeOrName(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsByCodeOrName(tc, oChartsOfAccount);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = e.Message;
                oChartsOfAccounts.Add(oChartsOfAccount);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oChartsOfAccounts;
        }
        public List<ChartsOfAccount> GetsByCodeOrNameWithBUForVoucher(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            TransactionContext tc = null;
            try
            {
                int nHeadDisplayConfigureID = 0;
                tc = TransactionContext.Begin();
                IDataReader readerheaddisplay = HeadDisplayConfigureDA.GetTopConfigure(tc, oChartsOfAccount);
                NullHandler oReaderHeadDisplay = new NullHandler(readerheaddisplay);
                if (readerheaddisplay.Read())
                {
                    nHeadDisplayConfigureID = oReaderHeadDisplay.GetInt32("HeadDisplayConfigureID");
                }
                readerheaddisplay.Close();

                IDataReader reader = null;
                if (nHeadDisplayConfigureID > 0)
                {
                    reader = ChartsOfAccountDA.GetsByCodeOrNameWithBUForVoucher(tc, oChartsOfAccount, true, nUserID);
                }
                else
                {
                    reader = ChartsOfAccountDA.GetsByCodeOrNameWithBUForVoucher(tc, oChartsOfAccount, false, nUserID);
                }
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = e.Message;
                oChartsOfAccounts.Add(oChartsOfAccount);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oChartsOfAccounts;
        }
        public List<ChartsOfAccount> GetsByCodeOrNameWithBU(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsByCodeOrNameWithBU(tc, oChartsOfAccount);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = e.Message;
                oChartsOfAccounts.Add(oChartsOfAccount);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oChartsOfAccounts;
        }
        /// <summary>
        ///  multi purpose function
        /// parameters ComponentID,AccountType,AccountHeadCodeName. none compulsory
        /// WILL RETURN DATA ACCORDING TO PROVIDED PARAMETERS. see DA for paramter list
        /// </summary>
        /// <param name="oChartsOfAccount"></param>
        /// <param name="nUserID"></param>
        /// <returns></returns>
        public List<ChartsOfAccount> GetsByComponentAndCodeName(ChartsOfAccount oChartsOfAccount, int nUserID)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetsByComponentAndCodeName(tc, oChartsOfAccount);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccount = new ChartsOfAccount();
                oChartsOfAccount.ErrorMessage = e.Message;
                oChartsOfAccounts.Add(oChartsOfAccount);

                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }

            return oChartsOfAccounts;
        }

        public List<ChartsOfAccount> Gets(string sSQL, int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChartsOfAccountDA.Gets(tc,sSQL);
                oChartsOfAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }

            return oChartsOfAccounts;
        }
        public List<ChartsOfAccount> GetRefresh(int nParentHeadID, int nUserID)
        {
            List<ChartsOfAccount> oChartsOfAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ChartsOfAccountDA.GetRefresh(tc, nParentHeadID, nUserID);
                oChartsOfAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ChartsOfAccount", e);
                #endregion
            }

            return oChartsOfAccount;
        }

        public List<ChartsOfAccount> ConfigurationAttach(List<ChartsOfAccount> oChartsOfAccounts, TransactionContext tc)
        {

            string sChartsOfAccountIDs = ""; DataRow[] oRows;
            foreach (ChartsOfAccount oItem in oChartsOfAccounts)
            {
                sChartsOfAccountIDs = sChartsOfAccountIDs + oItem.AccountHeadID + ",";            
            }
            if (sChartsOfAccountIDs.Length > 0)
            {
                sChartsOfAccountIDs = sChartsOfAccountIDs.Remove((sChartsOfAccountIDs.Length - 1), 1);

                DataSet oDataSet = new DataSet();
                DataTable oDataTable = new DataTable();
                IDataReader reader = COA_AccountHeadConfigDA.Gets(tc, sChartsOfAccountIDs, 0);
                oDataSet.Load(reader, LoadOption.OverwriteChanges, new string[1]);
                oDataTable = oDataSet.Tables[0];
                reader.Close();

                foreach (ChartsOfAccount oItem in oChartsOfAccounts)
                {
                    oRows = null;
                    oRows = oDataTable.Select("AccountHeadID=" + oItem.AccountHeadID);
                    if (oRows != null)
                    {
                        if (oRows.Length > 0)
                        {
                            bool bIsBillRefApply = (oRows[0]["IsBillRefApply"] == DBNull.Value) ? false : Convert.ToBoolean(oRows[0]["IsBillRefApply"]);
                            bool bIsCostCenterApply = (oRows[0]["IsCostCenterApply"] == DBNull.Value) ? false : Convert.ToBoolean(oRows[0]["IsCostCenterApply"]);
                            bool bIsInventoryApply = (oRows[0]["IsInventoryApply"] == DBNull.Value) ? false : Convert.ToBoolean(oRows[0]["IsInventoryApply"]);
                            //bool bIsVoucherReferenceApply = (oRows[0]["IsVoucherReferenceApply"] == DBNull.Value) ? false : Convert.ToBoolean(oRows[0]["IsVoucherReferenceApply"]);

                            oItem.IsBillRefApply = bIsBillRefApply;
                            oItem.IsCostCenterApply = bIsCostCenterApply;
                            oItem.IsInventoryApply = bIsInventoryApply;
                            //oItem.IsChequeApply= bIsVoucherReferenceApply;
                        }
                    }
                }
            }
            return oChartsOfAccounts;
        }

        public List<ChartsOfAccount> SaveCopyAccountHeads(string IDs, int nUserId)
        {
            List<ChartsOfAccount> oChartsOfAccounts = new List<ChartsOfAccount>();
            ChartsOfAccount oChartsOfAccount = new ChartsOfAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                oChartsOfAccount.IDs = IDs;                
                if (oChartsOfAccount.IDs != "")
                {
                    oChartsOfAccount.IDs = oChartsOfAccount.IDs.Remove((oChartsOfAccount.IDs.Length - 1), 1);
                    reader = ChartsOfAccountDA.Insert(tc, oChartsOfAccount, nUserId);
                    oChartsOfAccounts = CreateObjects(reader);
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oChartsOfAccounts = new List<ChartsOfAccount>();
                oChartsOfAccounts[0].ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oChartsOfAccounts;
        }
        #endregion
    }
}
