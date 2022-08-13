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

    public class BUWiseSubLedgerService : MarshalByRefObject, IBUWiseSubLedgerService
    {
        #region Private functions and declaration
        private BUWiseSubLedger MapObject(NullHandler oReader)
        {
            BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
            oBUWiseSubLedger.BUWiseSubLedgerID = oReader.GetInt32("BUWiseSubLedgerID");
            oBUWiseSubLedger.SubLedgerID = oReader.GetInt32("SubLedgerID");
            oBUWiseSubLedger.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            return oBUWiseSubLedger;
        }

        private BUWiseSubLedger CreateObject(NullHandler oReader)
        {
            BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
            oBUWiseSubLedger = MapObject(oReader);
            return oBUWiseSubLedger;
        }

        private List<BUWiseSubLedger> CreateObjects(IDataReader oReader)
        {
            List<BUWiseSubLedger> oBUWiseSubLedger = new List<BUWiseSubLedger>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BUWiseSubLedger oItem = CreateObject(oHandler);
                oBUWiseSubLedger.Add(oItem);
            }
            return oBUWiseSubLedger;
        }

        #endregion

        #region Interface implementation
        public BUWiseSubLedgerService() { }

        public BUWiseSubLedger Save(BUWiseSubLedger oBUWiseSubLedger, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Business Location
                List<BUWiseSubLedger> oBUWiseSubLedgers = new List<BUWiseSubLedger>();
                oBUWiseSubLedgers = oBUWiseSubLedger.BUWiseSubLedgers;


                if (oBUWiseSubLedgers != null)
                {
                    string sBUWiseSubLedgerIDs = "";
                    foreach (BUWiseSubLedger oItem in oBUWiseSubLedgers)
                    {
                        IDataReader readertnc;

                        if (oItem.BUWiseSubLedgerID <= 0)
                        {
                            readertnc = BUWiseSubLedgerDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                        }
                        else
                        {
                            readertnc = BUWiseSubLedgerDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                        }
                        NullHandler oReaderTNC = new NullHandler(readertnc);
                        if (readertnc.Read())
                        {
                            sBUWiseSubLedgerIDs = sBUWiseSubLedgerIDs + oReaderTNC.GetString("BUWiseSubLedgerID") + ",";
                        }
                        readertnc.Close();
                    }

                    if (sBUWiseSubLedgerIDs.Length > 0)
                    {
                        sBUWiseSubLedgerIDs = sBUWiseSubLedgerIDs.Remove(sBUWiseSubLedgerIDs.Length - 1, 1);
                    }
                    BUWiseSubLedger otempBUWiseSubLedger = new BUWiseSubLedger();
                    otempBUWiseSubLedger.BusinessUnitID = oBUWiseSubLedger.BusinessUnitID;
                    BUWiseSubLedgerDA.Delete(tc, otempBUWiseSubLedger, EnumDBOperation.Delete, nUserId, sBUWiseSubLedgerIDs);
                }
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBUWiseSubLedger = new BUWiseSubLedger();
                oBUWiseSubLedger.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oBUWiseSubLedger;
        }

        public BUWiseSubLedger SaveFromCC(BUWiseSubLedger oBUWiseSubLedger, int nUserId)
        {
            List<BUWiseSubLedger> oBUWiseSubLedgers = new List<BUWiseSubLedger>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);                
                oBUWiseSubLedgers = oBUWiseSubLedger.BUWiseSubLedgers;
                if (oBUWiseSubLedgers != null)
                {
                    string sBusinessUnitIDs = "";
                    foreach (BUWiseSubLedger oItem in oBUWiseSubLedgers)
                    {

                        sBusinessUnitIDs = sBusinessUnitIDs + oItem.BusinessUnitID + ",";
                    }
                    if (sBusinessUnitIDs.Length > 0)
                    {
                        sBusinessUnitIDs = sBusinessUnitIDs.Remove(sBusinessUnitIDs.Length - 1, 1);
                    }                    
                    BUWiseSubLedgerDA.IUDFromCC(tc, oBUWiseSubLedger, sBusinessUnitIDs, nUserId);
                }                
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBUWiseSubLedger = new BUWiseSubLedger();
                oBUWiseSubLedger.ErrorMessage = e.Message.Split('!')[0];
                
                #endregion
            }
            return oBUWiseSubLedger;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "Delete sucessfully";
            try
            {
                tc = TransactionContext.Begin(true);
                BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
                oBUWiseSubLedger.BUWiseSubLedgerID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ACCostCenter, EnumRoleOperationType.Delete);
                BUWiseSubLedgerDA.Delete(tc, oBUWiseSubLedger, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = e.Message.Split('!')[0];
            }

            return Global.DeleteMessage;
        }
        public string CopyBasicChartOfAccount(int nCompanyID, int nUserId)
        {
            TransactionContext tc = null;
            string sMessage = "Execution Successfully";
            try
            {
                tc = TransactionContext.Begin(true);
                BUWiseSubLedger oBUWiseSubLedger = new BUWiseSubLedger();
                BUWiseSubLedgerDA.CopyBasicChartOfAccount(tc, nCompanyID, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                sMessage = e.Message.Split('!')[0];
            }

            return sMessage;
        }
        public BUWiseSubLedger Get(int id, int nUserId)
        {
            BUWiseSubLedger oAccountHead = new BUWiseSubLedger();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BUWiseSubLedgerDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BUWiseSubLedger", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<BUWiseSubLedger> Gets(int nUserId)
        {
            List<BUWiseSubLedger> oBUWiseSubLedger = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BUWiseSubLedgerDA.Gets(tc);
                oBUWiseSubLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUWiseSubLedger", e);
                #endregion
            }

            return oBUWiseSubLedger;
        }
        public List<BUWiseSubLedger> Gets(int nBUID, int nUserID)
        {
            List<BUWiseSubLedger> oBUWiseSubLedger = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BUWiseSubLedgerDA.Gets(tc,nBUID);
                oBUWiseSubLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUWiseSubLedger", e);
                #endregion
            }

            return oBUWiseSubLedger;
        }
        public List<BUWiseSubLedger> GetsByCC(int nAHID, int nUserID)
        {
            List<BUWiseSubLedger> oBUWiseSubLedger = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BUWiseSubLedgerDA.GetsByCC(tc, nAHID);
                oBUWiseSubLedger = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BUWiseSubLedger", e);
                #endregion
            }

            return oBUWiseSubLedger;
        }
        #endregion
    } 
}
