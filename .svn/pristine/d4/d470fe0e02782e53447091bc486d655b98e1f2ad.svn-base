using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class FabricBeamFinishService : MarshalByRefObject, IFabricBeamFinishService
    {
        #region Private functions and declaration
        private FabricBeamFinish MapObject(NullHandler oReader)
        {
            FabricBeamFinish oFabricBeamFinish = new FabricBeamFinish();
            oFabricBeamFinish.FabricBeamFinishID = oReader.GetInt32("FabricBeamFinishID");
            oFabricBeamFinish.FESDID = oReader.GetInt32("FESDID");
            oFabricBeamFinish.ReadyDate = oReader.GetDateTime("ReadyDate");
            oFabricBeamFinish.Note = oReader.GetString("Note");
            oFabricBeamFinish.Qty = oReader.GetDouble("Qty");
            oFabricBeamFinish.LengthFinish = oReader.GetDouble("LengthFinish");
            oFabricBeamFinish.DyeingOrderID = oReader.GetInt32("DyeingOrderID");
            oFabricBeamFinish.FEOSID = oReader.GetInt32("FEOSID");
            oFabricBeamFinish.BeamNo = oReader.GetString("BeamNo");
            oFabricBeamFinish.IsFinish = oReader.GetBoolean("IsFinish");
            oFabricBeamFinish.FSCDID = oReader.GetInt32("FSCDID");
            oFabricBeamFinish.ExeNo = oReader.GetString("ExeNo");
            oFabricBeamFinish.ContractorID = oReader.GetInt32("ContractorID");
            oFabricBeamFinish.CustomerName = oReader.GetString("CustomerName");
            oFabricBeamFinish.TFlength = oReader.GetDouble("TFlength");
            return oFabricBeamFinish;
        }

        private FabricBeamFinish CreateObject(NullHandler oReader)
        {
            FabricBeamFinish oFabricBeamFinish = new FabricBeamFinish();
            oFabricBeamFinish = MapObject(oReader);
            return oFabricBeamFinish;
        }

        private List<FabricBeamFinish> CreateObjects(IDataReader oReader)
        {
            List<FabricBeamFinish> oFabricBeamFinish = new List<FabricBeamFinish>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBeamFinish oItem = CreateObject(oHandler);
                oFabricBeamFinish.Add(oItem);
            }
            return oFabricBeamFinish;
        }

        #endregion

        #region Interface implementation
        public FabricBeamFinishService() { }

        public FabricBeamFinish Save(FabricBeamFinish oFabricBeamFinish, Int64 nUserID)
        {
            TransactionContext tc = null;
            oFabricBeamFinish.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBeamFinish.FabricBeamFinishID <= 0)
                {
                    reader = FabricBeamFinishDA.InsertUpdate(tc, oFabricBeamFinish, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBeamFinishDA.InsertUpdate(tc, oFabricBeamFinish, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBeamFinish = new FabricBeamFinish();
                    oFabricBeamFinish = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFabricBeamFinish.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save FabricBeamFinish. Because of " + e.Message, e);
                #endregion
            }
            return oFabricBeamFinish;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBeamFinish oFabricBeamFinish = new FabricBeamFinish();
                DBTableReferenceDA.HasReference(tc, "FabricBeamFinish", id);
                oFabricBeamFinish.FabricBeamFinishID = id;
                FabricBeamFinishDA.Delete(tc, oFabricBeamFinish, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete FabricBeamFinish. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public FabricBeamFinish Get(int id, Int64 nUserId)
        {
            FabricBeamFinish oAccountHead = new FabricBeamFinish();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricBeamFinishDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get FabricBeamFinish", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<FabricBeamFinish> Gets(Int64 nUserID)
        {
            List<FabricBeamFinish> oFabricBeamFinish = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBeamFinishDA.Gets(tc);
                oFabricBeamFinish = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBeamFinish", e);
                #endregion
            }

            return oFabricBeamFinish;
        }

        public List<FabricBeamFinish> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBeamFinish> oFabricBeamFinishs = new List<FabricBeamFinish>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricBeamFinishDA.Gets(tc, sSQL);
                oFabricBeamFinishs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBeamFinish", e);
                #endregion
            }

            return oFabricBeamFinishs;
        }


        #endregion
    }
}
