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
    public class MailAssignedPersonService : MarshalByRefObject, IMailAssignedPersonService
    {
        #region Private functions and declaration
        private MailAssignedPerson MapObject(NullHandler oReader)
        {
            MailAssignedPerson oMailAssignedPerson = new MailAssignedPerson();
            oMailAssignedPerson.MAPID = oReader.GetInt32("MAPID");
            oMailAssignedPerson.MSID = oReader.GetInt32("MSID");
            oMailAssignedPerson.MailTo = oReader.GetString("MailTo");
            oMailAssignedPerson.IsCCMail = oReader.GetBoolean("IsCCMail");
            return oMailAssignedPerson;
        }

        private MailAssignedPerson CreateObject(NullHandler oReader)
        {
            MailAssignedPerson oMailAssignedPerson = MapObject(oReader);
            return oMailAssignedPerson;
        }

        private List<MailAssignedPerson> CreateObjects(IDataReader oReader)
        {
            List<MailAssignedPerson> oMailAssignedPerson = new List<MailAssignedPerson>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MailAssignedPerson oItem = CreateObject(oHandler);
                oMailAssignedPerson.Add(oItem);
            }
            return oMailAssignedPerson;
        }

        #endregion

        #region Interface implementation
        public MailAssignedPersonService() { }

        public MailAssignedPerson IUD(MailAssignedPerson oMailAssignedPerson, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            MailSetUp oMS = new MailSetUp();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oMailAssignedPerson.MSID == 0)
                {
                    if (oMailAssignedPerson.MS != null)
                    {
                        reader = MailSetUpDA.IUD(tc, oMailAssignedPerson.MS, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oMS = MailSetUpService.CreateObject(oReader);
                        }
                        reader.Close();
                        oMailAssignedPerson.MSID = oMS.MSID;
                    }
                    else { throw new Exception("No mail setup info found to save."); }
                }

                reader = MailAssignedPersonDA.IUD(tc, oMailAssignedPerson, nDBOperation, nUserID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailAssignedPerson = new MailAssignedPerson();
                    oMailAssignedPerson = CreateObject(oReader);
                }
                reader.Close();
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oMailAssignedPerson = new MailAssignedPerson();
                    oMailAssignedPerson.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oMailAssignedPerson = new MailAssignedPerson();
                oMailAssignedPerson.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                oMS = new MailSetUp();
                #endregion
            }
            oMailAssignedPerson.MS = oMS;
            return oMailAssignedPerson;
        }

        public MailAssignedPerson Get(int nMAPID, Int64 nUserId)
        {
            MailAssignedPerson oMailAssignedPerson = new MailAssignedPerson();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MailAssignedPersonDA.Get(tc, nMAPID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMailAssignedPerson = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailAssignedPerson = new MailAssignedPerson();
                oMailAssignedPerson.ErrorMessage = "Failed to get mail assigned person.";
                #endregion
            }

            return oMailAssignedPerson;
        }

        public List<MailAssignedPerson> Gets(string sSQL, Int64 nUserID)
        {
            List<MailAssignedPerson> oMailAssignedPersons = new List<MailAssignedPerson>(); ;
            MailAssignedPerson oMailAssignedPerson = new MailAssignedPerson();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MailAssignedPersonDA.Gets(tc, sSQL);
                oMailAssignedPersons = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oMailAssignedPerson = new MailAssignedPerson();
                oMailAssignedPerson.ErrorMessage = "Failed to get mail assigned person.";
                oMailAssignedPersons.Add(oMailAssignedPerson);
                #endregion
            }

            return oMailAssignedPersons;
        }
        #endregion
    }
}