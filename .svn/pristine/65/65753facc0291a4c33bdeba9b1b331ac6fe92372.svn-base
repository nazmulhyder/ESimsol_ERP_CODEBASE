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
    public class CostSetupService : MarshalByRefObject, ICostSetupService
    {
        #region Private functions and declaration
        private CostSetup MapObject(NullHandler oReader)
        {
            CostSetup oCostSetup = new CostSetup();
            oCostSetup.CostSetupID = oReader.GetInt32("CostSetupID");
            oCostSetup.CustomsDuty = oReader.GetDouble("CustomsDuty");
            oCostSetup.RegulatoryDuty = oReader.GetDouble("RegulatoryDuty");
            oCostSetup.SupplementaryDuty = oReader.GetDouble("SupplementaryDuty");
            oCostSetup.ValueAddedTxt = oReader.GetDouble("ValueAddedTxt");
            oCostSetup.AdvanceIncomeTax = oReader.GetDouble("AdvanceIncomeTax");
            oCostSetup.AdvanceTradeVat = oReader.GetDouble("AdvanceTradeVat");
            oCostSetup.ATVDeductedProfit = oReader.GetDouble("ATVDeductedProfit");
            oCostSetup.CustomClearingAndInsuranceFee = oReader.GetDouble("CustomClearingAndInsuranceFee");
            oCostSetup.MarginRate = oReader.GetDouble("MarginRate");
            oCostSetup.CurrencyRate = oReader.GetDouble("CurrencyRate");

            return oCostSetup;
        }

        private CostSetup CreateObject(NullHandler oReader)
        {
            CostSetup oCostSetup = new CostSetup();
            oCostSetup = MapObject(oReader);
            return oCostSetup;
        }

        private List<CostSetup> CreateObjects(IDataReader oReader)
        {
            List<CostSetup> oCostSetup = new List<CostSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CostSetup oItem = CreateObject(oHandler);
                oCostSetup.Add(oItem);
            }
            return oCostSetup;
        }

        #endregion

        #region Interface implementation
        public CostSetupService() { }

        public CostSetup Save(CostSetup oCostSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCostSetup.CostSetupID <= 0)
                {
                    reader = CostSetupDA.InsertUpdate(tc, oCostSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = CostSetupDA.InsertUpdate(tc, oCostSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSetup = new CostSetup();
                    oCostSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save CostSetup. Because of " + e.Message, e);
                #endregion
            }
            return oCostSetup;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CostSetup oCostSetup = new CostSetup();
                oCostSetup.CostSetupID = id;
                CostSetupDA.Delete(tc, oCostSetup, EnumDBOperation.Delete, nUserId);
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

        public CostSetup Get(int id, Int64 nUserId)
        {
            CostSetup oCostSetup = new CostSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = CostSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCostSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CostSetup", e);
                #endregion
            }
            return oCostSetup;
        }

        public List<CostSetup> Gets(Int64 nUserID)
        {
            List<CostSetup> oCostSetups = new List<CostSetup>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSetupDA.Gets(tc);
                oCostSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSetup", e);
                #endregion
            }
            return oCostSetups;
        }
        public List<CostSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<CostSetup> oCostSetups = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CostSetupDA.Gets(tc, sSQL);
                oCostSetups = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CostSetup", e);
                #endregion
            }
            return oCostSetups;
        }
        #endregion
    }
}