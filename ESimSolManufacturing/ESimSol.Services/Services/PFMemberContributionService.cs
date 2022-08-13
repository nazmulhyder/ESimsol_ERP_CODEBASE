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
    public class PFMemberContributionService : MarshalByRefObject, IPFMemberContributionService
    {
        #region Private functions and declaration
        private PFMemberContribution MapObject(NullHandler oReader)
        {
            PFMemberContribution oPFMemberContribution = new PFMemberContribution();
            oPFMemberContribution.PFMCID = oReader.GetInt32("PFMCID");
            oPFMemberContribution.PFSchemeID = oReader.GetInt32("PFSchemeID");
            oPFMemberContribution.MinAmount = oReader.GetDouble("MinAmount");
            oPFMemberContribution.MaxAmount = oReader.GetDouble("MaxAmount");
            oPFMemberContribution.IsPercent = oReader.GetBoolean("IsPercent");
            oPFMemberContribution.Value = oReader.GetDouble("Value");
            oPFMemberContribution.CalculationOn = (EnumPayrollApplyOn)oReader.GetInt16("CalculationOn");
            oPFMemberContribution.IsActive = oReader.GetBoolean("IsActive");
            return oPFMemberContribution;
        }

        public static PFMemberContribution CreateObject(NullHandler oReader)
        {
            PFMemberContributionService oPFMemberContributionService = new PFMemberContributionService();
            PFMemberContribution oPFMemberContribution = oPFMemberContributionService.MapObject(oReader);
            return oPFMemberContribution;
        }

        private List<PFMemberContribution> CreateObjects(IDataReader oReader)
        {
            List<PFMemberContribution> oPFMemberContribution = new List<PFMemberContribution>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFMemberContribution oItem = CreateObject(oHandler);
                oPFMemberContribution.Add(oItem);
            }
            return oPFMemberContribution;
        }

        #endregion

        #region Interface implementation
        public PFMemberContributionService() { }

        public PFMemberContribution IUD(PFMemberContribution oPFMemberContribution, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            PFScheme oPFS = new PFScheme();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oPFMemberContribution.PFSchemeID == 0)
                {
                    if (oPFMemberContribution.PFS != null)
                    {
                        reader = PFSchemeDA.IUD(tc, oPFMemberContribution.PFS, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oPFS = PFSchemeService.CreateObject(oReader);
                        }
                        reader.Close();
                        oPFMemberContribution.PFSchemeID = oPFS.PFSchemeID;
                    }
                    else { throw new Exception("No PF scheme found to save."); }
                }

                reader = PFMemberContributionDA.IUD(tc, oPFMemberContribution, nDBOperation, nUserID);
               oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFMemberContribution = new PFMemberContribution();
                    oPFMemberContribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPFMemberContribution = new PFMemberContribution();
                    oPFMemberContribution.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oPFMemberContribution = new PFMemberContribution();
                oPFMemberContribution.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            oPFMemberContribution.PFS = oPFS;
            return oPFMemberContribution;
        }


        public PFMemberContribution Get(int nPFMCID, Int64 nUserId)
        {
            PFMemberContribution oPFMemberContribution = new PFMemberContribution();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFMemberContributionDA.Get(tc, nPFMCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFMemberContribution = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFMemberContribution = new PFMemberContribution();
                oPFMemberContribution.ErrorMessage = "Failed to get provident fund scheme.";
                #endregion
            }

            return oPFMemberContribution;
        }

        public List<PFMemberContribution> Gets(string sSQL, Int64 nUserID)
        {
            List<PFMemberContribution> oPFMemberContributions = new List<PFMemberContribution>(); ;
            PFMemberContribution oPFMemberContribution = new PFMemberContribution();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFMemberContributionDA.Gets(tc, sSQL);
                oPFMemberContributions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFMemberContribution = new PFMemberContribution();
                oPFMemberContribution.ErrorMessage = "Failed to get provident fund scheme.";
                oPFMemberContributions.Add(oPFMemberContribution);
                #endregion
            }

            return oPFMemberContributions;
        }
        #endregion
    }
}