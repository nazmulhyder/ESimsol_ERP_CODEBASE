using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class LCTermService : MarshalByRefObject, ILCTermService
    {
        #region Private functions and declaration
        private LCTerm MapObject(NullHandler oReader)
        {
            LCTerm oLCTerm = new LCTerm();
            oLCTerm.LCTermID = oReader.GetInt32("LCTermID");
            oLCTerm.Name = oReader.GetString("Name");
            oLCTerm.Description = oReader.GetString("Description");
            oLCTerm.Days = oReader.GetInt32("Days");

            return oLCTerm;
        }

        private LCTerm CreateObject(NullHandler oReader)
        {
            LCTerm oLCTerm = new LCTerm();
            oLCTerm = MapObject(oReader);
            return oLCTerm;
        }

        private List<LCTerm> CreateObjects(IDataReader oReader)
        {
            List<LCTerm> oLCTerms = new List<LCTerm>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LCTerm oItem = CreateObject(oHandler);
                oLCTerms.Add(oItem);
            }
            return oLCTerms;
        }

        #endregion

        #region Interface implementation
        public LCTermService() { }

        public LCTerm Save(LCTerm oLCTerm, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLCTerm.LCTermID <= 0)
                {
                    reader = LCTermDA.InsertUpdate(tc, oLCTerm, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LCTermDA.InsertUpdate(tc, oLCTerm, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTerm = new LCTerm();
                    oLCTerm = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLCTerm = new LCTerm();
                oLCTerm.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLCTerm;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LCTerm oLCTerm = new LCTerm();
                oLCTerm.LCTermID = id;
                LCTermDA.Delete(tc, oLCTerm, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];                
                #endregion
            }
            return Global.DeleteMessage;
        }
        public LCTerm Get(int id, Int64 nUserId)
        {
            LCTerm oLCTerm = new LCTerm();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = LCTermDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLCTerm = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oLCTerm;
        }

        public List<LCTerm> Gets(Int64 nUserId)
        {
            List<LCTerm> oLCTerms = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LCTermDA.Gets(tc);
                oLCTerms = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oLCTerms;
        }


        #endregion
    }
}