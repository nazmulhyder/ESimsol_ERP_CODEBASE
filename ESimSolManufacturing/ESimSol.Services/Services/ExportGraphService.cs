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
    public class ExportGraphService : MarshalByRefObject, IExportGraphService
    {
        #region Private functions and declaration
        private ExportGraph MapObject(NullHandler oReader)
        {
            ExportGraph oExportGraph = new ExportGraph();

            oExportGraph.ExportLCNo = oReader.GetString("ExportLCNo");
            oExportGraph.ExportBillNo = oReader.GetString("ExportBillNo");
            oExportGraph.ContractorName = oReader.GetString("ContractorName");
            oExportGraph.BankName = oReader.GetString("BankName_Nego");
            oExportGraph.BankNickName = oReader.GetString("Bank_Nego");
            oExportGraph.Amount_LC = oReader.GetDouble("Amount_LC");
            oExportGraph.Amount = oReader.GetDouble("Amount");
            oExportGraph.MaturityDate = oReader.GetDateTime("MaturityDate");
            oExportGraph.RelizationDate = oReader.GetDateTime("RelizationDate");
            oExportGraph.ContractorName = oReader.GetString("ContractorName");
            oExportGraph.Currency = oReader.GetString("Currency");
            oExportGraph.PINo = oReader.GetString("PINo");
            oExportGraph.BranchName_Issue = oReader.GetString("BankName_Issue");
            oExportGraph.MKTPName = oReader.GetString("MKTPName");
            oExportGraph.LDBCNo = oReader.GetString("LDBCNo");
            oExportGraph.ExportLCDate = oReader.GetDateTime("MaturityReceivedDate");
            oExportGraph.BUName = oReader.GetString("BUName");
            oExportGraph.LDBPNo = oReader.GetString("LDBPNo");
            oExportGraph.BBranchName_Issue = oReader.GetString("BBranchName_Issue");
            oExportGraph.Tenor = oReader.GetString("LCTermsName");
            oExportGraph.DateofAcceptance = oReader.GetDateTime("AcceptanceDate");
            oExportGraph.CurrencyID = oReader.GetInt32("CurrencyID");
            oExportGraph.BankBranchID = oReader.GetInt32("BankBranchID_Negotiation");
            oExportGraph.State = (EnumLCBillEvent)oReader.GetInt32("State");
            oExportGraph.AcceptanceRate = 1;
            oExportGraph.CurrencyName = oReader.GetString("CurrencyName");
            oExportGraph.ApplicantName = oReader.GetString("ApplicantName");

            

            return oExportGraph;
        }

        private ExportGraph CreateObject(NullHandler oReader)
        {
            ExportGraph oExportGraph = new ExportGraph();
            oExportGraph = MapObject(oReader);
            return oExportGraph;
        }

        private List<ExportGraph> CreateObjects(IDataReader oReader)
        {
            List<ExportGraph> oExportGraph = new List<ExportGraph>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportGraph oItem = CreateObject(oHandler);
                oExportGraph.Add(oItem);
            }
            return oExportGraph;
        }

        #endregion

        #region Interface implementation
        public ExportGraphService() { }



        public List<ExportGraph> Gets(int nBUID, Int64 nUserID)
        {
            List<ExportGraph> oExportGraph = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportGraphDA.Gets(tc, nBUID);
                oExportGraph = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oExportGraph;
        }

        public List<ExportGraph> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportGraph> oExportGraph = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportGraphDA.Gets(tc, sSQL);
                oExportGraph = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get vwForABP", e);
                #endregion
            }

            return oExportGraph;
        }

        public List<ExportGraph> GetsForGraph(string sYear, int nBankBranchID, int nBUID,string sDateCriteria, Int64 nUserId)
        {
            List<ExportGraph> oExportGraphs = new List<ExportGraph>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ExportGraphDA.GetsForGraph(tc, sYear, nBankBranchID, nBUID, sDateCriteria);
                oExportGraphs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                //#region Handle Exception
                //if (tc != null)
                //    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get View_PurchaseLCReport", e);
                //#endregion
                oExportGraphs = new List<ExportGraph>();
                ExportGraph oExportGraph = new ExportGraph();
                oExportGraph.ErrorMessage = e.Message;
                oExportGraphs.Add(oExportGraph);
            }
            return oExportGraphs;
        }

        #endregion
    }
}