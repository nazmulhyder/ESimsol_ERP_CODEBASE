using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class FabricQCParNameService : MarshalByRefObject, IFabricQCParNameService
    {
        #region Private functions and declaration
        private FabricQCParName MapObject(NullHandler oReader)
        {
            FabricQCParName oFabricQCParName = new FabricQCParName();
            oFabricQCParName.FabricQCParNameID = oReader.GetInt32("FabricQCParNameID");
            oFabricQCParName.Name = oReader.GetString("Name");
            oFabricQCParName.Note = oReader.GetString("Note");
            oFabricQCParName.SL = oReader.GetInt32("SL");
            oFabricQCParName.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricQCParName.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricQCParName.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oFabricQCParName;
        }
        private FabricQCParName CreateObject(NullHandler oReader)
        {
            FabricQCParName oFabricQCParName = new FabricQCParName();
            oFabricQCParName = MapObject(oReader);
            return oFabricQCParName;
        }

        private List<FabricQCParName> CreateObjects(IDataReader oReader)
        {
            List<FabricQCParName> oFabricQCParName = new List<FabricQCParName>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricQCParName oItem = CreateObject(oHandler);
                oFabricQCParName.Add(oItem);
            }
            return oFabricQCParName;
        }
        #endregion
        #region Interface implementation
        public FabricQCParName Save(FabricQCParName oFabricQCParName, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricQCParName.FabricQCParNameID <= 0)
                {

                    reader = FabricQCParNameDA.InsertUpdate(tc, oFabricQCParName, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricQCParNameDA.InsertUpdate(tc, oFabricQCParName, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQCParName = new FabricQCParName();
                    oFabricQCParName = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricQCParName = new FabricQCParName();
                    oFabricQCParName.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricQCParName;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricQCParName oFabricQCParName = new FabricQCParName();
                oFabricQCParName.FabricQCParNameID = id;
                DBTableReferenceDA.HasReference(tc, "FabricQCParName", id);
                FabricQCParNameDA.Delete(tc, oFabricQCParName, EnumDBOperation.Delete, nUserId);
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
        public FabricQCParName Get(int id, Int64 nUserId)
        {
            FabricQCParName oFabricQCParName = new FabricQCParName();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricQCParNameDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricQCParName = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricQCParName", e);
                #endregion
            }
            return oFabricQCParName;
        }
        public List<FabricQCParName> Gets(Int64 nUserID)
        {
            List<FabricQCParName> oFabricQCParNames = new List<FabricQCParName>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricQCParNameDA.Gets(tc);
                oFabricQCParNames = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricQCParName oFabricQCParName = new FabricQCParName();
                oFabricQCParName.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricQCParNames;
        }
        public List<FabricQCParName> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricQCParName> oFabricQCParNames = new List<FabricQCParName>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricQCParNameDA.Gets(tc, sSQL);
                oFabricQCParNames = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricQCParName", e);
                #endregion
            }
            return oFabricQCParNames;
        }

        #endregion
    }
}
