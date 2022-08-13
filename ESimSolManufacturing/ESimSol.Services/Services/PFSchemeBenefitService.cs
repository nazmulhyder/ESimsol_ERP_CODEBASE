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
    public class PFSchemeBenefitService : MarshalByRefObject, IPFSchemeBenefitService
    {
        #region Private functions and declaration
        private PFSchemeBenefit MapObject(NullHandler oReader)
        {
            PFSchemeBenefit oPFSchemeBenefit = new PFSchemeBenefit();
            oPFSchemeBenefit.PFSBID = oReader.GetInt32("PFSBID");
            oPFSchemeBenefit.PFSchemeID = oReader.GetInt32("PFSchemeID");
            oPFSchemeBenefit.MaturityYear = oReader.GetInt32("MaturityYear");
            oPFSchemeBenefit.MaturityYrCalculateAfter = (EnumRecruitmentEvent)oReader.GetInt16("MaturityYrCalculateAfter");
            oPFSchemeBenefit.IsActive = oReader.GetBoolean("IsActive");
            oPFSchemeBenefit.InactiveDate = oReader.GetDateTime("InactiveDate");
            oPFSchemeBenefit.ContributionPercentage = oReader.GetDouble("ContributionPercentage");
            oPFSchemeBenefit.IsProfitShare = oReader.GetBoolean("IsProfitShare");
            return oPFSchemeBenefit;
        }

        public static PFSchemeBenefit CreateObject(NullHandler oReader)
        {
            PFSchemeBenefitService oPFSchemeBenefitService = new PFSchemeBenefitService();
            PFSchemeBenefit oPFSchemeBenefit = oPFSchemeBenefitService.MapObject(oReader);
            return oPFSchemeBenefit;
        }

        private List<PFSchemeBenefit> CreateObjects(IDataReader oReader)
        {
            List<PFSchemeBenefit> oPFSchemeBenefit = new List<PFSchemeBenefit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFSchemeBenefit oItem = CreateObject(oHandler);
                oPFSchemeBenefit.Add(oItem);
            }
            return oPFSchemeBenefit;
        }

        #endregion

        #region Interface implementation
        public PFSchemeBenefitService() { }


        public PFSchemeBenefit IUD(PFSchemeBenefit oPFSchemeBenefit, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            PFScheme oPFS = new PFScheme();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oPFSchemeBenefit.PFSchemeID == 0)
                {
                    if (oPFSchemeBenefit.PFS != null)
                    {
                        reader = PFSchemeDA.IUD(tc, oPFSchemeBenefit.PFS, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oPFS = PFSchemeService.CreateObject(oReader);
                        }
                        reader.Close();
                        oPFSchemeBenefit.PFSchemeID = oPFS.PFSchemeID;
                    }
                    else { throw new Exception("No PF scheme found to save."); }
                }

                reader = PFSchemeBenefitDA.IUD(tc, oPFSchemeBenefit, nDBOperation, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFSchemeBenefit = new PFSchemeBenefit();
                    oPFSchemeBenefit = CreateObject(oReader);
                }
                reader.Close();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPFSchemeBenefit = new PFSchemeBenefit();
                    oPFSchemeBenefit.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oPFSchemeBenefit = new PFSchemeBenefit();
                oPFSchemeBenefit.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                oPFS = new PFScheme();
                #endregion
            }
            oPFSchemeBenefit.PFS = oPFS;
            return oPFSchemeBenefit;
        }


        public PFSchemeBenefit Get(int nPFSBID, Int64 nUserId)
        {
            PFSchemeBenefit oPFSchemeBenefit = new PFSchemeBenefit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFSchemeBenefitDA.Get(tc, nPFSBID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFSchemeBenefit = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFSchemeBenefit = new PFSchemeBenefit();
                oPFSchemeBenefit.ErrorMessage = "Failed to get provident fund benefits.";
                #endregion
            }

            return oPFSchemeBenefit;
        }

        public List<PFSchemeBenefit> Gets(string sSQL, Int64 nUserID)
        {
            List<PFSchemeBenefit> oPFSchemeBenefits = new List<PFSchemeBenefit>(); ;
            PFSchemeBenefit oPFSchemeBenefit = new PFSchemeBenefit();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFSchemeBenefitDA.Gets(tc, sSQL);
                oPFSchemeBenefits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFSchemeBenefit = new PFSchemeBenefit();
                oPFSchemeBenefit.ErrorMessage = "Failed to get provident fund benefits.";
                oPFSchemeBenefits.Add(oPFSchemeBenefit);
                #endregion
            }

            return oPFSchemeBenefits;
        }
        #endregion
    }
}