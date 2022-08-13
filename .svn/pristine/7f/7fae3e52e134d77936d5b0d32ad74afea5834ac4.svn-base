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
    public class PFDistributionService : MarshalByRefObject, IPFDistributionService
    {
        #region Private functions and declaration
        private PFDistribution MapObject(NullHandler oReader)
        {
            PFDistribution oPFDistribution = new PFDistribution();
            oPFDistribution.PFPDID = oReader.GetInt32("PFPDID");
            oPFDistribution.PETID = oReader.GetInt32("PETID");
            oPFDistribution.EmployeeID = oReader.GetInt32("EmployeeID");
            oPFDistribution.PFMemberContribution = oReader.GetDouble("PFMemberContribution");
            oPFDistribution.PFAllMemberContribution = oReader.GetDouble("PFAllMemberContribution");
            oPFDistribution.ProfitAmount = oReader.GetDouble("ProfitAmount");
            oPFDistribution.DBUserID = oReader.GetInt32("DBUserID");
            oPFDistribution.DBServerDateTime = oReader.GetDateTime("DBServerDateTime");
            oPFDistribution.EmployeeCode = oReader.GetString("EmployeeCode");
            oPFDistribution.Name = oReader.GetString("Name");
            oPFDistribution.SelfContribution = oReader.GetDouble("SelfContribution");
            oPFDistribution.AllMemberContribution = oReader.GetDouble("AllMemberContribution");
            oPFDistribution.ProfitAmountX = oReader.GetDouble("PFProfitAmount");
            return oPFDistribution;
        }

        public static PFDistribution CreateObject(NullHandler oReader)
        {
            PFDistributionService oPFDistributionService = new PFDistributionService();
            PFDistribution oPFDistribution = oPFDistributionService.MapObject(oReader);
            return oPFDistribution;
        }

        private List<PFDistribution> CreateObjects(IDataReader oReader)
        {
            List<PFDistribution> oPFDistribution = new List<PFDistribution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFDistribution oItem = CreateObject(oHandler);
                oPFDistribution.Add(oItem);
            }
            return oPFDistribution;
        }

        #endregion

        #region Interface implementation
        public PFDistributionService() { }

        //distribute
        public PFDistribution Distribute(int EnumDBOperation, int nPETID, Int64 nUserId)
        {
            PFDistribution oPFDistribution = new PFDistribution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFDistributionDA.Distribute(tc, EnumDBOperation, nPETID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFDistribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFDistribution = new PFDistribution();
                oPFDistribution.ErrorMessage = "Failed to distribute.";
                #endregion
            }

            return oPFDistribution;
        }

        public List<PFDistribution> Distributes(int EnumDBOperation, int nPETID, Int64 nUserId)
        {
            List<PFDistribution> oPFDistributions = new List<PFDistribution>(); ;
            PFDistribution oPFDistribution = new PFDistribution();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFDistributionDA.Distributes(tc, EnumDBOperation, nPETID, nUserId);
                oPFDistributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFDistribution = new PFDistribution();
                oPFDistribution.ErrorMessage = "Failed to distribute.";
                oPFDistributions.Add(oPFDistribution);
                #endregion
            }

            return oPFDistributions;
        }

        //Rollback
        public PFDistribution Rollback(int EnumDBOperation, int nPETID, Int64 nUserId)
        {
            PFDistribution oPFDistribution = new PFDistribution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                PFDistributionDA.Rollback(tc, EnumDBOperation, nPETID, nUserId);

                if (EnumDBOperation == 3)
                {
                    oPFDistribution = new PFDistribution();
                    oPFDistribution.ErrorMessage = Global.DeleteMessage;
                }
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oPFDistribution = new PFDistribution();
                oPFDistribution.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oPFDistribution;
        }


        public List<PFDistribution> Gets(string sSql, Int64 nUserId)
        {
            List<PFDistribution> oPFGets = new List<PFDistribution>(); ;
            PFDistribution oPFGet = new PFDistribution();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFDistributionDA.Gets(tc, sSql);
                oPFGets = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFGet = new PFDistribution();
                oPFGet.ErrorMessage = "Failed to distribute.";
                oPFGets.Add(oPFGet);
                #endregion
            }

            return oPFGets;
        }


        #endregion


    }
}