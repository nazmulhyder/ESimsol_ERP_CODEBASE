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
    public class GratuitySchemeService : MarshalByRefObject, IGratuitySchemeService
    {
        #region Private functions and declaration
        private GratuityScheme MapObject(NullHandler oReader)
        {
            GratuityScheme oGratuityScheme = new GratuityScheme();

            oGratuityScheme.GSID = oReader.GetInt32("GSID");
            oGratuityScheme.Name = oReader.GetString("Name");
            oGratuityScheme.Description = oReader.GetString("Description");
            oGratuityScheme.ApproveBy = oReader.GetInt32("ApproveBy");
            oGratuityScheme.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oGratuityScheme.InactiveDate = oReader.GetDateTime("InactiveDate");
            oGratuityScheme.EncryptedID = Global.Encrypt(oGratuityScheme.GSID.ToString());
            oGratuityScheme.ApproveByNameCode = oReader.GetString("ApproveByNameCode");
            return oGratuityScheme;

        }

        private GratuityScheme CreateObject(NullHandler oReader)
        {
            GratuityScheme oGratuityScheme = MapObject(oReader);
            return oGratuityScheme;
        }

        private List<GratuityScheme> CreateObjects(IDataReader oReader)
        {
            List<GratuityScheme> oGratuitySchemes = new List<GratuityScheme>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GratuityScheme oItem = CreateObject(oHandler);
                oGratuitySchemes.Add(oItem);
            }
            return oGratuitySchemes;
        }

        #endregion

        #region Interface implementation
        public GratuitySchemeService() { }

        public GratuityScheme IUD(GratuityScheme oGratuityScheme, int nDBOperation, Int64 nUserID)
        {
            GratuityScheme oGS = new GratuityScheme();
            TransactionContext tc = null;
            try
            {

                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = GratuitySchemeDA.IUD(tc, oGratuityScheme, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oGS = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oGS.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oGS = new GratuityScheme();
                oGS.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                #endregion
            }
            return oGS;
        }


        public GratuityScheme Get(int nGSID, Int64 nUserId)
        {
            GratuityScheme oGratuityScheme = new GratuityScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDA.Get(nGSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuityScheme = CreateObject(oReader);
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

                oGratuityScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuityScheme;
        }

        public GratuityScheme Get(string sSQL, Int64 nUserId)
        {
            GratuityScheme oGratuityScheme = new GratuityScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuityScheme = CreateObject(oReader);
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

                oGratuityScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuityScheme;
        }

        public List<GratuityScheme> Gets(Int64 nUserID)
        {
            List<GratuityScheme> oGratuityScheme =  new List<GratuityScheme>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GratuitySchemeDA.Gets(tc);
                oGratuityScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GratuityScheme", e);
                #endregion
            }
            return oGratuityScheme;
        }

        public List<GratuityScheme> Gets(string sSQL, Int64 nUserID)
        {
            List<GratuityScheme> oGratuityScheme = new List<GratuityScheme>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GratuitySchemeDA.Gets(sSQL, tc);
                oGratuityScheme = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GratuityScheme", e);
                #endregion
            }
            return oGratuityScheme;
        }

        #region Activity
        public GratuityScheme Activity(GratuityScheme oGS, Int64 nUserId)
        {
            GratuityScheme oGratuityScheme = new GratuityScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDA.Activity(oGS, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuityScheme = CreateObject(oReader);
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
                oGratuityScheme = new GratuityScheme();
                oGratuityScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuityScheme;
        }


        #endregion

        #region Approve
        public GratuityScheme Approve(GratuityScheme oGS, Int64 nUserId)
        {
            GratuityScheme oGratuityScheme = new GratuityScheme();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GratuitySchemeDA.Approve(oGS, nUserId, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGratuityScheme = CreateObject(oReader);
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
                oGratuityScheme = new GratuityScheme();
                oGratuityScheme.ErrorMessage = e.Message;
                #endregion
            }

            return oGratuityScheme;
        }

        #endregion

        #endregion


    }
}
