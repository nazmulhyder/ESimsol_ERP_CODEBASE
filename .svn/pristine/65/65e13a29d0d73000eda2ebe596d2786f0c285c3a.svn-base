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
    public class LightSourceService : MarshalByRefObject, ILightSourceService
    {
        #region Private functions and declaration
        private LightSource MapObject(NullHandler oReader)
        {
            LightSource oLightSource = new LightSource();
            oLightSource.LightSourceID = oReader.GetInt32("LightSourceID");
            oLightSource.Descriptions = oReader.GetString("Descriptions");
            return oLightSource;
        }
        private LightSource CreateObject(NullHandler oReader)
        {
            LightSource oLightSource = new LightSource();
            oLightSource = MapObject(oReader);
            return oLightSource;
        }
        private List<LightSource> CreateObjects(IDataReader oReader)
        {
            List<LightSource> oLightSource = new List<LightSource>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                LightSource oItem = CreateObject(oHandler);
                oLightSource.Add(oItem);
            }
            return oLightSource;
        }

        #endregion

        #region Interface implementation
        public LightSource Save(LightSource oLightSource, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oLightSource.LightSourceID <= 0)
                {
                    reader = LightSourceDA.InsertUpdate(tc, oLightSource, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = LightSourceDA.InsertUpdate(tc, oLightSource, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLightSource = new LightSource();
                    oLightSource = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLightSource.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLightSource;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                LightSource oLightSource = new LightSource();
                oLightSource.LightSourceID = id;
                //DBTableReferenceDA.HasReference(tc, "LightSource", id);
                LightSourceDA.Delete(tc, oLightSource, EnumDBOperation.Delete, nUserId);
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
        public List<LightSource> Gets(Int64 nUserID)
        {
            List<LightSource> oLightSources = new List<LightSource>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = LightSourceDA.Gets(tc);
                oLightSources = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLightSources = new List<LightSource>();
                LightSource oLightSource = new LightSource();
                oLightSource.ErrorMessage = e.Message.Split('~')[0];
                oLightSources.Add(oLightSource);
                #endregion
            }
            return oLightSources;
        }
        public LightSource Get(int id, Int64 nUserId)
        {
            LightSource oLightSource = new LightSource();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = LightSourceDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLightSource = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oLightSource.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oLightSource;
        }

        #endregion
    }
}
