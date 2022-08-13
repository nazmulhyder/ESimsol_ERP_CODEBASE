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
    public class FabricPlanRepeatService : MarshalByRefObject, IFabricPlanRepeatService
    {
        #region Private functions and declaration
        private FabricPlanRepeat MapObject(NullHandler oReader)
        {
            FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
            oFabricPlanRepeat.FabricPlanRepeatID = oReader.GetInt32("FabricPlanRepeatID");
            oFabricPlanRepeat.FabricPlanOrderID = oReader.GetInt32("FabricPlanOrderID");
            oFabricPlanRepeat.WarpWeftType = (EnumWarpWeft)oReader.GetInt32("WarpWeftType");
            oFabricPlanRepeat.RepeatNo = oReader.GetInt32("RepeatNo");
            oFabricPlanRepeat.SLNo = oReader.GetInt32("SLNo");
            oFabricPlanRepeat.StartCol = oReader.GetInt32("StartCol");
            oFabricPlanRepeat.EndCol = oReader.GetInt32("EndCol");
            oFabricPlanRepeat.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricPlanRepeat.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oFabricPlanRepeat.LastUpdateByName = oReader.GetString("LastUpdateByName");    
            return oFabricPlanRepeat;
        }
        private FabricPlanRepeat CreateObject(NullHandler oReader)
        {
            FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
            oFabricPlanRepeat = MapObject(oReader);
            return oFabricPlanRepeat;
        }

        private List<FabricPlanRepeat> CreateObjects(IDataReader oReader)
        {
            List<FabricPlanRepeat> oFabricPlanRepeat = new List<FabricPlanRepeat>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricPlanRepeat oItem = CreateObject(oHandler);
                oFabricPlanRepeat.Add(oItem);
            }
            return oFabricPlanRepeat;
        }
        #endregion
        #region Interface implementation
        public FabricPlanRepeat Save(FabricPlanRepeat oFabricPlanRepeat, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricPlanRepeat.FabricPlanRepeatID <= 0)
                {

                    reader = FabricPlanRepeatDA.InsertUpdate(tc, oFabricPlanRepeat, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricPlanRepeatDA.InsertUpdate(tc, oFabricPlanRepeat, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanRepeat = new FabricPlanRepeat();
                    oFabricPlanRepeat = CreateObject(oReader);
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
                    oFabricPlanRepeat = new FabricPlanRepeat();
                    oFabricPlanRepeat.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oFabricPlanRepeat;
        }
        public List<FabricPlanRepeat> SaveFabricPlanRepeats(List<FabricPlanRepeat> oFabricPlanRepeats, Int64 nUserID)
        {
            FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
            List<FabricPlanRepeat> _oFabricPlanRepeats = new List<FabricPlanRepeat>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricPlanRepeat oTempFabricPlanRepeat in oFabricPlanRepeats)
                {
                    IDataReader reader;
                    if (oTempFabricPlanRepeat.FabricPlanRepeatID <= 0)
                    {
                        reader = FabricPlanRepeatDA.InsertUpdate(tc, oTempFabricPlanRepeat, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {
                        reader = FabricPlanRepeatDA.InsertUpdate(tc, oTempFabricPlanRepeat, EnumDBOperation.Update, nUserID);
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricPlanRepeat = new FabricPlanRepeat();
                        oFabricPlanRepeat = CreateObject(oReader);
                    }
                    reader.Close();
                    _oFabricPlanRepeats.Add(oFabricPlanRepeat);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricPlanRepeat = new FabricPlanRepeat();
                    oFabricPlanRepeat.ErrorMessage = e.Message;
                    _oFabricPlanRepeats = new List<FabricPlanRepeat>();
                    _oFabricPlanRepeats.Add(oFabricPlanRepeat);
                }
            }
            return _oFabricPlanRepeats;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
                oFabricPlanRepeat.FabricPlanRepeatID = id;
                DBTableReferenceDA.HasReference(tc, "FabricPlanRepeat", id);
                FabricPlanRepeatDA.Delete(tc, oFabricPlanRepeat, EnumDBOperation.Delete, nUserId);
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
        public FabricPlanRepeat Get(int id, Int64 nUserId)
        {
            FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricPlanRepeatDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlanRepeat = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get FabricPlanRepeat", e);
                #endregion
            }
            return oFabricPlanRepeat;
        }
        public List<FabricPlanRepeat> Gets(Int64 nUserID)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanRepeatDA.Gets(tc);
                oFabricPlanRepeats = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanRepeat oFabricPlanRepeat = new FabricPlanRepeat();
                oFabricPlanRepeat.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricPlanRepeats;
        }
        public List<FabricPlanRepeat> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricPlanRepeat> oFabricPlanRepeats = new List<FabricPlanRepeat>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricPlanRepeatDA.Gets(tc, sSQL);
                oFabricPlanRepeats = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricPlanRepeat", e);
                #endregion
            }
            return oFabricPlanRepeats;
        }

        #endregion
    }
}
