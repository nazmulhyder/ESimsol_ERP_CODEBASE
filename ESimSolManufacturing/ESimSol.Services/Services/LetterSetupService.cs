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
    public class LetterSetupService : MarshalByRefObject, ILetterSetupService
    {
        #region Private functions and declaration
        private LetterSetup MapObject(NullHandler oReader)
        {
            LetterSetup oLetterSetup = new LetterSetup();
            oLetterSetup.LSID = oReader.GetInt32("LSID");
            oLetterSetup.Code = oReader.GetString("Code");
            oLetterSetup.Name = oReader.GetString("Name");
            oLetterSetup.Body = oReader.GetString("Body");
            oLetterSetup.Remark = oReader.GetString("Remark");
            oLetterSetup.PageSize = oReader.GetString("PageSize");
            oLetterSetup.IsEnglish = oReader.GetBoolean("IsEnglish");
            oLetterSetup.IsPadFormat = oReader.GetBoolean("IsPadFormat");
            oLetterSetup.LSID = oReader.GetInt32("LSID");
            oLetterSetup.MarginTop = oReader.GetDouble("MarginTop");
            oLetterSetup.MarginBottom = oReader.GetDouble("MarginBottom");
            oLetterSetup.MarginLeft = oReader.GetDouble("MarginLeft");
            oLetterSetup.MarginRight = oReader.GetDouble("MarginRight");
            oLetterSetup.HeaderFontSize = oReader.GetInt32("HeaderFontSize");
            oLetterSetup.HeaderTextAlign = oReader.GetInt32("HeaderTextAlign");
            oLetterSetup.HeaderName = oReader.GetString("HeaderName");
            return oLetterSetup;
        }

        private LetterSetup CreateObject(NullHandler oReader)
        {
            LetterSetup oLetterSetup = MapObject(oReader);
            return oLetterSetup;
        }

        private List<LetterSetup> CreateObjects(IDataReader oReader)
        {
            List<LetterSetup> oLetterSetup = new List<LetterSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LetterSetup oItem = CreateObject(oHandler);
                oLetterSetup.Add(oItem);
            }
            return oLetterSetup;
        }


        #endregion

        #region Interface implementation
        public LetterSetupService() { }
        public LetterSetup IUD(LetterSetup oLetterSetup, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {
                    reader = LetterSetupDA.IUD(tc, oLetterSetup, nUserID, nDBOperation);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oLetterSetup = new LetterSetup();
                        oLetterSetup = CreateObject(oReader);
                    }
                    reader.Close();
                }
                //else if (nDBOperation == (int)EnumDBOperation.Delete)
                //{
                //    reader = LetterSetupDA.IUD(tc, oLetterSetup, nUserID, nDBOperation);
                //    NullHandler oReader = new NullHandler(reader);
                //    reader.Close();
                //    oLetterSetup.ErrorMessage = Global.DeleteMessage;
                //    reader.Close();
                //}

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oLetterSetup = new LetterSetup();
                oLetterSetup.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion

            }
            return oLetterSetup;
        }


        public LetterSetup Get(string sSQL, Int64 nUserId)
        {
            LetterSetup oLetterSetup = new LetterSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LetterSetupDA.Get(tc, sSQL);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetup = CreateObject(oReader);
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
                throw new ServiceException(e.Message);;
                #endregion
            }

            return oLetterSetup;
        }

        public String Delete(LetterSetup oLetterSetup, Int64 nUserID)
        {
            TransactionContext tc1 = null;
            try
            {
                tc1 = TransactionContext.Begin(true);
                //LetterSetupDA.Delete(tc1, oLetterSetup, nUserID, (int)EnumDBOperation.Delete);
              
                IDataReader reader = LetterSetupDA.Delete(tc1, oLetterSetup, nUserID, (int)EnumDBOperation.Delete);
                NullHandler oReader = new NullHandler(reader);
                reader.Close();
                oLetterSetup.ErrorMessage = Global.DeleteMessage;
                tc1.End();


            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc1 != null)
                    tc1.HandleError();

                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public LetterSetup Get(int id, Int64 nUserId)
        {
            LetterSetup oLetterSetup = new LetterSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LetterSetupDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLetterSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LetterSetup", e);
                #endregion
            }
            return oLetterSetup;
        }
        public List<LetterSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<LetterSetup> oLetterSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LetterSetupDA.Gets(sSQL, tc);
                oLetterSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException(e.Message);
                #endregion
            }
            return oLetterSetup;
        }
        #endregion
    }
}


