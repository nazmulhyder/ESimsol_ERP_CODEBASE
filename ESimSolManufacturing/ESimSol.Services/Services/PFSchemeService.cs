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
    public class PFSchemeService : MarshalByRefObject, IPFSchemeService
    {
        #region Private functions and declaration
        private PFScheme MapObject(NullHandler oReader)
        {
            PFScheme oPFScheme = new PFScheme();
            oPFScheme.PFSchemeID = oReader.GetInt32("PFSchemeID");
            oPFScheme.Name = oReader.GetString("Name");
            oPFScheme.Description = oReader.GetString("Description");
            oPFScheme.IsRecognize = oReader.GetBoolean("IsRecognize");
            oPFScheme.RecommandedSalaryHeadID = oReader.GetInt32("RecommandedSalaryHeadID");
            oPFScheme.ApproveBy = oReader.GetInt32("ApproveBy");
            oPFScheme.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oPFScheme.IsActive = oReader.GetBoolean("IsActive");
            oPFScheme.InactiveDate = oReader.GetDateTime("InactiveDate");

            oPFScheme.ApproveByName = oReader.GetString("ApproveByName");
            return oPFScheme;
        }

        public static PFScheme CreateObject(NullHandler oReader)
        {
            PFSchemeService oPFSchemeService = new PFSchemeService();
            PFScheme oPFScheme = oPFSchemeService.MapObject(oReader);
            return oPFScheme;
        }

        private List<PFScheme> CreateObjects(IDataReader oReader)
        {
            List<PFScheme> oPFScheme = new List<PFScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PFScheme oItem = CreateObject(oHandler);
                oPFScheme.Add(oItem);
            }
            return oPFScheme;
        }

        #endregion

        #region Interface implementation
        public PFSchemeService() { }

        public PFScheme IUD(PFScheme oPFScheme, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = PFSchemeDA.IUD(tc, oPFScheme, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFScheme = new PFScheme();
                    oPFScheme = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oPFScheme = new PFScheme();
                    oPFScheme.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oPFScheme = new PFScheme();
                oPFScheme.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            return oPFScheme;
        }

        public PFScheme Get(int nPFSchemeID, Int64 nUserId)
        {
            PFScheme oPFScheme = new PFScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PFSchemeDA.Get(tc, nPFSchemeID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPFScheme = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFScheme = new PFScheme();
                oPFScheme.ErrorMessage = "Failed to get provident fund scheme.";
                #endregion
            }

            return oPFScheme;
        }

        public List<PFScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<PFScheme> oPFSchemes = new List<PFScheme>(); ;
            PFScheme oPFScheme = new PFScheme();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PFSchemeDA.Gets(tc, sSQL);
                oPFSchemes = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPFScheme = new PFScheme();
                oPFScheme.ErrorMessage = "Failed to get provident fund scheme.";
                oPFSchemes.Add(oPFScheme);
                #endregion
            }

            return oPFSchemes;
        }
        #endregion

      

    }
}