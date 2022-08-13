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
    public class LCContactService : MarshalByRefObject, ILCContactService
    {
        #region Private functions and declaration
        private LCContact MapObject(NullHandler oReader)
        {
            LCContact oLCContact = new LCContact();
            oLCContact.LCContactID = oReader.GetInt32("LCContactID");
            oLCContact.BUID = oReader.GetInt32("BUID");
            oLCContact.BalanceDate = oReader.GetDateTime("BalanceDate");
            oLCContact.LCInHand = oReader.GetDouble("LCInHand");
            oLCContact.ContactInHand = oReader.GetDouble("ContactInHand");
            oLCContact.Remarks = oReader.GetString("Remarks");
            oLCContact.BUName = oReader.GetString("BUName");
            oLCContact.BUSName = oReader.GetString("BUSName");
            oLCContact.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            return oLCContact;
        }

        private LCContact CreateObject(NullHandler oReader)
        {
            LCContact oLCContact = new LCContact();
            oLCContact = MapObject(oReader);
            return oLCContact;
        }

        private List<LCContact> CreateObjects(IDataReader oReader)
        {
            List<LCContact> oLCContact = new List<LCContact>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LCContact oItem = CreateObject(oHandler);
                oLCContact.Add(oItem);
            }
            return oLCContact;
        }

        #endregion

        #region Interface implementation
        public LCContactService() { }

        public LCContact Save(LCContact oLCContact, int nUserID)
        {
            TransactionContext tc = null;
            oLCContact.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLCContact.LCContactID <= 0)
                {
                    reader = LCContactDA.InsertUpdate(tc, oLCContact, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LCContactDA.InsertUpdate(tc, oLCContact, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCContact = new LCContact();
                    oLCContact = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oLCContact.ErrorMessage = e.Message.Split('!')[0];

                #endregion
            }
            return oLCContact;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LCContact oLCContact = new LCContact();

                oLCContact.LCContactID = id;
                LCContactDA.Delete(tc, oLCContact, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                #endregion
            }
            return "Deleted";
        }

        public LCContact Get(int id, int nUserId)
        {
            LCContact oAccountHead = new LCContact();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LCContactDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oAccountHead = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LCContact", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<LCContact> Gets(int nUserID)
        {
            List<LCContact> oLCContact = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCContactDA.Gets(tc);
                oLCContact = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCContact", e);
                #endregion
            }

            return oLCContact;
        }
        public List<LCContact> Gets(string sSQL, int nUserID)
        {
            List<LCContact> oLCContact = new List<LCContact>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_LCContact where LCContactID in (1,2,80,272,347,370,60,45)";
                }
                reader = LCContactDA.Gets(tc, sSQL);
                oLCContact = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCContact", e);
                #endregion
            }

            return oLCContact;
        }


        public List<LCContact> GetsLCContacts(LCContact oLCContact, int nUserID)
        {
            List<LCContact> oLCContacts = new List<LCContact>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null; 
                //reader = LCContactDA.Gets(tc);
                reader = LCContactDA.GetsLcContacts(tc, oLCContact, nUserID);
                oLCContacts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LCContact", e);
                #endregion
            }

            return oLCContacts;
        }

        #endregion
    }
}
