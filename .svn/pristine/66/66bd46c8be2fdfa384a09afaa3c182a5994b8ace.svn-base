using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class CharitySlabService : MarshalByRefObject, ICharitySlabService
    {
        private CharitySlab MapObject(NullHandler oReader)
        {
            CharitySlab oCharitySlab = new CharitySlab();
            oCharitySlab.CharitySlabID = oReader.GetInt32("CharitySlabID");
            oCharitySlab.SalaryHeadID = oReader.GetInt16("SalaryHeadID");
            oCharitySlab.SalaryHeadName = oReader.GetString("SalaryHeadName");
            oCharitySlab.UserName = oReader.GetString("UserName");
            oCharitySlab.StartSalaryRange = oReader.GetDouble("StartSalaryRange");
            oCharitySlab.EndSalaryRange = oReader.GetDouble("EndSalaryRange");
            oCharitySlab.CharityAmount = oReader.GetDouble("CharityAmount");
            return oCharitySlab;
        }
        private CharitySlab CreateObject(NullHandler oReader)
        {
            CharitySlab oCharitySlab = new CharitySlab();
            oCharitySlab = MapObject(oReader);
            return oCharitySlab;
        }

        private List<CharitySlab> CreateObjects(IDataReader oReader)
        {
            List<CharitySlab> oCharitySlab = new List<CharitySlab>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CharitySlab oItem = CreateObject(oHandler);
                oCharitySlab.Add(oItem);
            }
            return oCharitySlab;
        }

        public CharitySlab Save(CharitySlab oCharitySlab, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oCharitySlab.CharitySlabID <= 0)
                {
                    reader = CharitySlabDA.InsertUpdate(tc, oCharitySlab, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = CharitySlabDA.InsertUpdate(tc, oCharitySlab, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCharitySlab = new CharitySlab();
                    oCharitySlab = CreateObject(oReader);
                }
                reader.Close();

                #region Get Employee Batch
                reader = CharitySlabDA.Get(tc, oCharitySlab.CharitySlabID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCharitySlab = new CharitySlab();
                    oCharitySlab = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oCharitySlab = new CharitySlab();
                    oCharitySlab.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oCharitySlab;
        }

        public CharitySlab Get(int id, Int64 nUserId)
        {
            CharitySlab oCharitySlab = new CharitySlab();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CharitySlabDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCharitySlab = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get CharitySlab", e);
                #endregion
            }

            return oCharitySlab;
        }
        public List<CharitySlab> Gets(string sSQL, Int64 nUserID)
        {
            List<CharitySlab> oCharitySlab = new List<CharitySlab>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = CharitySlabDA.Gets(tc, sSQL);
                oCharitySlab = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CharitySlab", e);
                #endregion
            }
            return oCharitySlab;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                CharitySlab oCharitySlab = new CharitySlab();
                oCharitySlab.CharitySlabID = id;
                CharitySlabDA.Delete(tc, oCharitySlab, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
    }
}
