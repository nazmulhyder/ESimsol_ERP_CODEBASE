using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
namespace ESimSol.Services.Services
{
    public class LoanProductRateService : MarshalByRefObject, ILoanProductRateService
    {
        private LoanProductRate MapObject(NullHandler oReader)
        {
            LoanProductRate oLoanProductRate = new LoanProductRate();
            oLoanProductRate.LoanProductRateID = oReader.GetInt32("LoanProductRateID");
            oLoanProductRate.ProductID = oReader.GetInt32("ProductID");
            oLoanProductRate.ProductCode = oReader.GetString("ProductCode");
            oLoanProductRate.BUID = oReader.GetInt32("BUID");
            oLoanProductRate.UnitPrice = oReader.GetDouble("UnitPrice");
            oLoanProductRate.CurrencyID = oReader.GetInt32("CurrencyID");
            oLoanProductRate.MUnitID = oReader.GetInt32("MUnitID");
            oLoanProductRate.Remarks = oReader.GetString("Remarks");
            oLoanProductRate.ProductName = oReader.GetString("ProductName");
            oLoanProductRate.LastUpdateByName = oReader.GetString("LastUpdateByName");
            oLoanProductRate.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oLoanProductRate.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            oLoanProductRate.UnitName = oReader.GetString("UnitName");
            return oLoanProductRate;
        }

        private LoanProductRate CreateObject(NullHandler oReader)
        {
            LoanProductRate oLoanProductRate = new LoanProductRate();
            oLoanProductRate = MapObject(oReader);
            return oLoanProductRate;
        }

        private List<LoanProductRate> CreateObjects(IDataReader oReader)
        {
            List<LoanProductRate> oLoanProductRate = new List<LoanProductRate>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanProductRate oItem = CreateObject(oHandler);
                oLoanProductRate.Add(oItem);
            }
            return oLoanProductRate;
        }

        public LoanProductRate Save(LoanProductRate oLoanProductRate, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLoanProductRate.LoanProductRateID <= 0)
                {
                    reader = LoanProductRateDA.InsertUpdate(tc, oLoanProductRate, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LoanProductRateDA.InsertUpdate(tc, oLoanProductRate, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanProductRate = new LoanProductRate();
                    oLoanProductRate = CreateObject(oReader);
                }
                reader.Close();

                #region Get LoanProductRate
                reader = LoanProductRateDA.Get(tc, oLoanProductRate.LoanProductRateID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanProductRate = new LoanProductRate();
                    oLoanProductRate = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oLoanProductRate = new LoanProductRate();
                    oLoanProductRate.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLoanProductRate;
        }
        public LoanProductRate Get(int id, Int64 nUserId)
        {
            LoanProductRate oLoanProductRate = new LoanProductRate();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LoanProductRateDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanProductRate = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LoanProductRate", e);
                #endregion
            }

            return oLoanProductRate;
        }
        public List<LoanProductRate> Gets(string sSQL, Int64 nUserID)
        {
            List<LoanProductRate> oLoanProductRate = new List<LoanProductRate>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanProductRateDA.Gets(tc, sSQL);
                oLoanProductRate = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LoanProductRate", e);
                #endregion
            }
            return oLoanProductRate;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LoanProductRate oLoanProductRate = new LoanProductRate();
                oLoanProductRate.LoanProductRateID = id;
                //  AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.RecycleProcess, EnumRoleOperationType.Delete);
                //DBTableReferenceDA.HasReference(tc, "LotTransfer", id);
                LoanProductRateDA.Delete(tc, oLoanProductRate, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
    }
}
