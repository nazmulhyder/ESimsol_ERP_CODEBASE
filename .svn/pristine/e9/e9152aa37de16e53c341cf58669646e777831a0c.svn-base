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
    public class MailSetUpService : MarshalByRefObject, IMailSetUpService
    {
        #region Private functions and declaration
        private MailSetUp MapObject(NullHandler oReader)
        {
            MailSetUp oMailSetUp = new MailSetUp();
            oMailSetUp.MSID = oReader.GetInt32("MSID");
            oMailSetUp.ReportID = oReader.GetInt32("ReportID");
            oMailSetUp.Subject = oReader.GetString("Subject");
            oMailSetUp.MailType = (MailReportingType)oReader.GetInt16("MailType");
            oMailSetUp.MailTime = oReader.GetDateTime("MailTime");
            oMailSetUp.NextTimeToMail = oReader.GetDateTime("NextTimeToMail");
            oMailSetUp.IsActive = oReader.GetBoolean("IsActive");
            oMailSetUp.ReportName = oReader.GetString("ReportName");
            oMailSetUp.ControllerName = oReader.GetString("ControllerName");
            oMailSetUp.FunctionName = oReader.GetString("FunctionName");
            return oMailSetUp;
        }

        public static MailSetUp CreateObject(NullHandler oReader)
        {
            MailSetUpService oMailSetUpService = new MailSetUpService();
            MailSetUp oMailSetUp = oMailSetUpService.MapObject(oReader);
            return oMailSetUp;
        }

        private List<MailSetUp> CreateObjects(IDataReader oReader)
        {
            List<MailSetUp> oMailSetUp = new List<MailSetUp>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MailSetUp oItem = CreateObject(oHandler);
                oMailSetUp.Add(oItem);
            }
            return oMailSetUp;
        }

        #endregion

        #region Interface implementation
        public MailSetUpService() { }

        public MailSetUp IUD(MailSetUp oMailSetUp, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = MailSetUpDA.IUD(tc, oMailSetUp, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailSetUp = new MailSetUp();
                    oMailSetUp = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oMailSetUp = new MailSetUp();
                    oMailSetUp.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oMailSetUp = new MailSetUp();
                oMailSetUp.ErrorMessage = ex.Message;
                #endregion
            }
            return oMailSetUp;
        }

        public MailSetUp Get(int nMSID, Int64 nUserId)
        {
            MailSetUp oMailSetUp = new MailSetUp();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MailSetUpDA.Get(tc, nMSID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailSetUp = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailSetUp = new MailSetUp();
                oMailSetUp.ErrorMessage = "Failed to get mail set up.";
                #endregion
            }

            return oMailSetUp;
        }

        public List<MailSetUp> Gets(string sSQL, Int64 nUserID)
        {
            List<MailSetUp> oMailSetUps = new List<MailSetUp>(); ;
            MailSetUp oMailSetUp = new MailSetUp();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MailSetUpDA.Gets(tc, sSQL);
                oMailSetUps = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailSetUp = new MailSetUp();
                oMailSetUp.ErrorMessage = "Failed to get mail setup";
                oMailSetUps.Add(oMailSetUp);
                #endregion
            }

            return oMailSetUps;
        }
        #endregion
    }
}