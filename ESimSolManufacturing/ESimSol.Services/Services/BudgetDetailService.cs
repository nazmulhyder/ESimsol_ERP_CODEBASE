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
    public class BudgetDetailService : MarshalByRefObject, IBudgetDetailService
    {
        #region Private functions and declaration
        private BudgetDetail MapObject(NullHandler oReader)
        {
            BudgetDetail oBudgetDetail = new BudgetDetail();
            oBudgetDetail.BudgetDetailID = oReader.GetInt32("BudgetDetailID");
            oBudgetDetail.BudgetID = oReader.GetInt32("BudgetID");
            oBudgetDetail.ParentHeadID = oReader.GetInt32("ParentHeadID");
            oBudgetDetail.AccountHeadID = oReader.GetInt32("AccountHeadID");
            oBudgetDetail.BudgetAmount = oReader.GetDouble("BudgetAmount");
            oBudgetDetail.Remarks = oReader.GetString("Remarks");
            oBudgetDetail.AccountCode = oReader.GetString("AccountCode");
            oBudgetDetail.AccountHeadName = oReader.GetString("AccountHeadName");
            oBudgetDetail.IsJVNode = oReader.GetBoolean("IsJVNode");
            oBudgetDetail.AccountType = (EnumAccountType)oReader.GetInt32("AccountType");
            oBudgetDetail.AccountTypeInInt = oReader.GetInt32("AccountType");
            oBudgetDetail.ParentHeadID = oReader.GetInt32("ParentHeadID");

            return oBudgetDetail;
        }
        private BudgetDetail CreateObject(NullHandler oReader)
        {
            BudgetDetail oBudgetDetail = new BudgetDetail();
            oBudgetDetail = MapObject(oReader);
            return oBudgetDetail;
        }
        private List<BudgetDetail> CreateObjects(IDataReader oReader)
        {
            List<BudgetDetail> oBudgetDetail = new List<BudgetDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BudgetDetail oItem = CreateObject(oHandler);
                oBudgetDetail.Add(oItem);
            }
            return oBudgetDetail;
        }

        #endregion

        #region Interface implementation
        public BudgetDetailService() { }

        public List<BudgetDetail> Gets(string sSQL,Int64 nUserID)
        {
            List<BudgetDetail> oBudgetDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetDetailDA.Gets(tc,sSQL);
                oBudgetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BudgetDetail", e);
                #endregion
            }
            return oBudgetDetails;
        }

        public List<BudgetDetail> GetsByBID(int BID, Int64 nUserID)
        {
            List<BudgetDetail> oBudgetDetails = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BudgetDetailDA.GetsByBID(tc, BID);
                oBudgetDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BudgetDetail", e);
                #endregion
            }
            return oBudgetDetails;
        }
        public BudgetDetail Save(BudgetDetail oBudgetDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBudgetDetail.BudgetDetailID <= 0)
                {
                    reader = BudgetDetailDA.InsertUpdate(tc, oBudgetDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = BudgetDetailDA.InsertUpdate(tc, oBudgetDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBudgetDetail = new BudgetDetail();
                    oBudgetDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BudgetDetail. Because of " + e.Message, e);
                #endregion
            }
            return oBudgetDetail;
        }
        #endregion
    }   
}

