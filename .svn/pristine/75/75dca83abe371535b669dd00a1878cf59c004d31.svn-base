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
    public class MarketingAccountService : MarshalByRefObject, IMarketingAccountService
    {
        #region Private functions and declaration
        private MarketingAccount MapObject(NullHandler oReader)
        {
            MarketingAccount oMarketingAccount = new MarketingAccount();
            oMarketingAccount.MarketingAccountID = oReader.GetInt32("MarketingAccountID");
            oMarketingAccount.Name = oReader.GetString("Name");
            oMarketingAccount.EmployeeID = oReader.GetInt32("EmployeeID");
            oMarketingAccount.Phone = oReader.GetString("Phone");
            oMarketingAccount.EmployeeCode = oReader.GetString("EmployeeCode");
            oMarketingAccount.Email = oReader.GetString("Email");
            oMarketingAccount.ShortName = oReader.GetString("ShortName");
            oMarketingAccount.Note = oReader.GetString("Note");
            oMarketingAccount.Phone2 = oReader.GetString("Phone2");
            oMarketingAccount.Activity = oReader.GetBoolean("Activity");
            oMarketingAccount.IsGroup = oReader.GetBoolean("IsGroup");
            oMarketingAccount.IsGroupHead = oReader.GetBoolean("IsGroupHead");
            oMarketingAccount.GroupID = oReader.GetInt32("GroupID");
            oMarketingAccount.GroupName = oReader.GetString("GroupName");
            oMarketingAccount.UserID = oReader.GetInt32("UserID");
            oMarketingAccount.UserName = oReader.GetString("UserName");
            oMarketingAccount.Name_Group = oReader.GetString("Name_Group");
            
            return oMarketingAccount;
        }

        private MarketingAccount CreateObject(NullHandler oReader)
        {
            MarketingAccount oMarketingAccount = new MarketingAccount();
            oMarketingAccount = MapObject(oReader);
            return oMarketingAccount;
        }

        private List<MarketingAccount> CreateObjects(IDataReader oReader)
        {
            List<MarketingAccount> oMarketingAccount = new List<MarketingAccount>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MarketingAccount oItem = CreateObject(oHandler);
                oMarketingAccount.Add(oItem);
            }
            return oMarketingAccount;
        }

        #endregion

        #region Interface implementation
        public MarketingAccountService() { }

        public MarketingAccount Save(MarketingAccount oMarketingAccount, int nUserId)
        {
            TransactionContext tc = null;

           
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
            oBusinessUnits = oMarketingAccount.BusinessUnits;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMarketingAccount.MarketingAccountID <= 0)
                {
                   // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MarketingAccount", EnumRoleOperationType.Add);
                    reader = MarketingAccountDA.InsertUpdate(tc, oMarketingAccount, EnumDBOperation.Insert,nUserId);
                }
                else
                {
                  //  AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MarketingAccount", EnumRoleOperationType.Edit);
                    reader = MarketingAccountDA.InsertUpdate(tc, oMarketingAccount, EnumDBOperation.Update,nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMarketingAccount = new MarketingAccount();
                    oMarketingAccount = CreateObject(oReader);
                }
                reader.Close();
             

                #region 
                if (oBusinessUnits != null)
                {
                    MarketingAccount_BU oMarketingAccount_BU = new MarketingAccount_BU();
                    oMarketingAccount_BU.MarketingAccountID = oMarketingAccount.MarketingAccountID;
                    MarketingAccount_BUDA.Delete(tc, oMarketingAccount_BU, EnumDBOperation.Delete, nUserId);

                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerMABU;
                        oMarketingAccount_BU = new MarketingAccount_BU();
                        oMarketingAccount_BU.MarketingAccountID = oMarketingAccount.MarketingAccountID;
                        oMarketingAccount_BU.BUID = oItem.BusinessUnitID;
                        readerMABU = MarketingAccount_BUDA.InsertUpdate(tc, oMarketingAccount_BU, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerMABU);
                        readerMABU.Close();
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
                
              oMarketingAccount.ErrorMessage =  "Failed to Save MarketingAccount. Because of " + e.Message;
                #endregion
            }
            return oMarketingAccount;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                MarketingAccount oMarketingAccount = new MarketingAccount();
                oMarketingAccount.MarketingAccountID = id;
               // AuthorizationRoleDA.CheckUserPermission(tc, nUserId, "MarketingAccount", EnumRoleOperationType.Delete);
                MarketingAccountDA.Delete(tc, oMarketingAccount, EnumDBOperation.Delete,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public MarketingAccount Get(int id, int nUserId)
        {
            MarketingAccount oAccountHead = new MarketingAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MarketingAccountDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oAccountHead;
        }
        public MarketingAccount GetByUser( int nUserId)
        {
            MarketingAccount oAccountHead = new MarketingAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MarketingAccountDA.GetByUser(tc, nUserId);
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
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oAccountHead;
        }

        public MarketingAccount CommitActivity(int id, bool ActiveInActive, int nUserId)
        {
            MarketingAccount oAccountHead = new MarketingAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                MarketingAccountDA.CommitActivity(tc, id, ActiveInActive, nUserId);
                IDataReader reader = MarketingAccountDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oAccountHead;
        }

        public MarketingAccount GroupActivity(int id,string sMarketingIDs, bool ActiveInActive, int nUserId)
        {
            MarketingAccount oAccountHead = new MarketingAccount();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                MarketingAccountDA.GroupActivity(tc, sMarketingIDs, ActiveInActive, nUserId);
                IDataReader reader = MarketingAccountDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oAccountHead;
        }
        
        
       

        public List<MarketingAccount> Gets(int nUserId)
        {
            List<MarketingAccount> oMarketingAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingAccountDA.Gets(tc);
                oMarketingAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oMarketingAccount;
        }
        public List<MarketingAccount> GetsByBU(int nBUID, int nUserID)
        {
            List<MarketingAccount> oMarketingAccounts =new List<MarketingAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MarketingAccountDA.GetsByBU(tc, nBUID);
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oMarketingAccounts;
        }
        public List<MarketingAccount> GetsByBUAndGroup(int nBUID, int nUserID)
        {
            List<MarketingAccount> oMarketingAccounts = new List<MarketingAccount>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MarketingAccountDA.GetsByBUAndGroup(tc, nBUID, nUserID);
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();


                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oMarketingAccounts;
        }


        public List<MarketingAccount> Gets(string sSQL, int nUserId)
        {
            List<MarketingAccount> oMarketingAccount = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingAccountDA.Gets(tc, sSQL);
                oMarketingAccount = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccount", e);
                #endregion
            }

            return oMarketingAccount;
        }
        

        public List<MarketingAccount> GetsByName(string sName, int nBUID, int nUserId)
        {
            List<MarketingAccount> oMarketingAccounts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingAccountDA.GetsByName(tc, sName, nBUID);
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccounts", e);
                #endregion
            }

            return oMarketingAccounts;
        }
        public List<MarketingAccount> GetsGroupHead(string sName, int nBUID, int nUserId)
        {
            List<MarketingAccount> oMarketingAccounts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingAccountDA.GetsGroupHead(tc, sName, nBUID);
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccounts", e);
                #endregion
            }

            return oMarketingAccounts;
        }
        public List<MarketingAccount> GetsByNameGroup(string sName, int nBUID, int nUserId)
        {
            List<MarketingAccount> oMarketingAccounts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MarketingAccountDA.GetsByNameGroup(tc, sName, nBUID, nUserId);
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccounts", e);
                #endregion
            }

            return oMarketingAccounts;
        }

        public List<MarketingAccount> GetsGroup(string sName, int nBUID, int nUserId)
        {
            List<MarketingAccount> oMarketingAccounts = null;
            TransactionContext tc = null;
            int nCount = 0;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                 nCount = MarketingAccountDA.GetIsMKTUser(tc, nUserId);
                 if (nCount > 0)
                 {
                     reader = MarketingAccountDA.GetsOnlyGroupByUser(tc, sName, nBUID, nUserId);
                 }
                 else
                 {
                     reader = MarketingAccountDA.GetsOnlyGroup(tc, sName, nBUID, nUserId);
                 }
                
                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccounts", e);
                #endregion
            }

            return oMarketingAccounts;
        }

        public List<MarketingAccount> GetsByUser(int nBUID, int nUserId)
        {
            List<MarketingAccount> oMarketingAccounts = null;
            TransactionContext tc = null;
            int nCount = 0;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                nCount = MarketingAccountDA.GetIsMKTUser(tc, nUserId);
                if (nCount > 0)
                {
                    reader = MarketingAccountDA.GetsByBUAndGroup(tc, nBUID, nUserId);
                }
                else
                {
                    reader = MarketingAccountDA.GetsByBU(tc,  nBUID);
                }

                oMarketingAccounts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MarketingAccounts", e);
                #endregion
            }

            return oMarketingAccounts;
        }
   

     
        #endregion
    } 
}