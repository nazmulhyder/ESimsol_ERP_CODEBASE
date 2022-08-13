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
    public class MailReportingService : MarshalByRefObject, IMailReportingService
    {
        #region Private functions and declaration
        private MailReporting MapObject(NullHandler oReader)
        {
            MailReporting oMailReporting = new MailReporting();
            oMailReporting.ReportID = oReader.GetInt32("ReportID");
            oMailReporting.Name = oReader.GetString("Name");
            oMailReporting.ControllerName = oReader.GetString("ControllerName");
            oMailReporting.FunctionName = oReader.GetString("FunctionName");
            oMailReporting.IsActive = oReader.GetBoolean("IsActive");
            oMailReporting.IsMail = oReader.GetBoolean("IsMail");
            return oMailReporting;
        }

        private MailReporting CreateObject(NullHandler oReader)
        {
            MailReporting oMailReporting = MapObject(oReader);
            return oMailReporting;
        }

        private List<MailReporting> CreateObjects(IDataReader oReader)
        {
            List<MailReporting> oMailReporting = new List<MailReporting>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MailReporting oItem = CreateObject(oHandler);
                oMailReporting.Add(oItem);
            }
            return oMailReporting;
        }

        #endregion

        #region Interface implementation
        public MailReportingService() { }

        public MailReporting IUD(MailReporting oMailReporting, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = MailReportingDA.IUD(tc, oMailReporting, nDBOperation, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailReporting = new MailReporting();
                    oMailReporting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();

                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oMailReporting = new MailReporting();
                    oMailReporting.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception ex)
            {
                #region Handle Exception
                oMailReporting = new MailReporting();
                oMailReporting.ErrorMessage = ex.Message;
                #endregion
            }
            return oMailReporting;
        }

        public MailReporting Get(int nReportID, Int64 nUserId)
        {
            MailReporting oMailReporting = new MailReporting();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MailReportingDA.Get(tc, nReportID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailReporting = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailReporting = new MailReporting();
                oMailReporting.ErrorMessage = "Failed to get mailing report.";
                #endregion
            }

            return oMailReporting;
        }

        public List<MailReporting> Gets(string sSQL,Int64 nUserID)
        {
            List<MailReporting> oMailReportings = new List<MailReporting>(); ;
            MailReporting oMailReporting = new MailReporting();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MailReportingDA.Gets(tc, sSQL);
                oMailReportings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailReporting = new MailReporting();
                oMailReporting.ErrorMessage = "Failed to get mailing report.";
                oMailReportings.Add(oMailReporting);
                #endregion
            }

            return oMailReportings;
        }
        #endregion
    }
}