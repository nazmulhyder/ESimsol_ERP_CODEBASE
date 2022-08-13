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
using System.Linq;
namespace ESimSol.Services.Services
{
    public class LoanExportLCService : MarshalByRefObject, ILoanExportLCService
    {
        #region Private functions and declaration

        private LoanExportLC MapObject(NullHandler oReader)
        {
            LoanExportLC oLoanExportLC = new LoanExportLC();
            oLoanExportLC.LoanID = oReader.GetInt32("LoanID");
            oLoanExportLC.ExportLCID = oReader.GetInt32("ExportLCID");
            oLoanExportLC.LoanExportLCID = oReader.GetInt32("LoanExportLCID");
            oLoanExportLC.Amount = oReader.GetDouble("Amount");
            oLoanExportLC.Remarks = oReader.GetString("Remarks");
            oLoanExportLC.ExportLCNo = oReader.GetString("ExportLCNo");
            oLoanExportLC.ExportLCCurrencySymbol = oReader.GetString("ExportLCCurrencySymbol");
            oLoanExportLC.ExportLCAmount = oReader.GetDouble("ExportLCAmount");
            return oLoanExportLC;
        }

        private LoanExportLC CreateObject(NullHandler oReader)
        {
            LoanExportLC oLoanExportLC = new LoanExportLC();
            oLoanExportLC = MapObject(oReader);
            return oLoanExportLC;
        }

        private List<LoanExportLC> CreateObjects(IDataReader oReader)
        {
            List<LoanExportLC> oLoanExportLC = new List<LoanExportLC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LoanExportLC oItem = CreateObject(oHandler);
                oLoanExportLC.Add(oItem);
            }
            return oLoanExportLC;
        }

        #endregion

        #region Interface implementation
        public LoanExportLC Save(LoanExportLC oLoanExportLC, Int64 nUserID)
        {
            TransactionContext tc = null;
            LoanExportLC oTempLoanExportLC = new LoanExportLC();
            List<LoanExportLC> oLoanExportLCs = new List<LoanExportLC>();

            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader=null ;
                if (oLoanExportLC.LoanExportLCs.Any())
                {
                    
                    foreach (LoanExportLC oItem in oLoanExportLC.LoanExportLCs)
                    {
                       // NullHandler oReader = new NullHandler(reader);
                        if (oItem.LoanExportLCID <= 0)
                        {

                            reader = LoanExportLCDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {

                            reader = LoanExportLCDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                       
                     
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempLoanExportLC = new LoanExportLC();
                            oTempLoanExportLC = CreateObject(oReader);
                            oLoanExportLCs.Add(oTempLoanExportLC);
                        }
                        reader.Close();
                    }
                    oLoanExportLC.LoanExportLCs = oLoanExportLCs;
                    LoanExportLCDA.DeleteByIDs(tc, oLoanExportLCs.First().LoanID, string.Join(",", oLoanExportLCs.Select(x => x.ExportLCID)), nUserID);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oLoanExportLC = new LoanExportLC();
                    oLoanExportLC.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLoanExportLC;
        }

    
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LoanExportLC oLoanExportLC = new LoanExportLC();
                oLoanExportLC.LoanExportLCID = id;
                LoanExportLCDA.Delete(tc, oLoanExportLC, EnumDBOperation.Delete, nUserId);
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
         public LoanExportLC Get(int id, Int64 nUserId)
        {
            LoanExportLC oLoanExportLC = new LoanExportLC();
            LoanInterestService oLoanInterestService = new LoanInterestService();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LoanExportLCDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLoanExportLC = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LoanExportLC", e);
                #endregion
            }
            return oLoanExportLC;
        }
        public List<LoanExportLC> Gets(int buid, Int64 nUserID)
        {
            List<LoanExportLC> oLoans = new List<LoanExportLC>();
            LoanInterestService oLoanInterestService = new LoanInterestService();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanExportLCDA.Gets(buid, tc);
                oLoans = CreateObjects(reader);
               reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                LoanExportLC oLoanExportLC = new LoanExportLC();
                oLoanExportLC.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oLoans;
        }
        public List<LoanExportLC> Gets(string sSQL, Int64 nUserID)
        {
            List<LoanExportLC> oLoans = new List<LoanExportLC>();
            LoanInterestService oLoanInterestService = new LoanInterestService();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = LoanExportLCDA.Gets(tc, sSQL);
                oLoans = CreateObjects(reader);

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LoanExportLC", e);
                #endregion
            }
            return oLoans;
        }

        #endregion
    }
}
