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
    public class GUProductionProcedureService : MarshalByRefObject, IGUProductionProcedureService
    {
        #region Private functions and declaration
        private GUProductionProcedure MapObject(NullHandler oReader)
        {
            GUProductionProcedure oGUProductionProcedure = new GUProductionProcedure();
            oGUProductionProcedure.GUProductionProcedureID = oReader.GetInt32("GUProductionProcedureID");
            oGUProductionProcedure.GUProductionOrderID = oReader.GetInt32("GUProductionOrderID");
            oGUProductionProcedure.ProductionStepID = oReader.GetInt32("ProductionStepID");
            oGUProductionProcedure.Sequence = oReader.GetInt32("Sequence");
            oGUProductionProcedure.Remarks = oReader.GetString("Remarks");
            oGUProductionProcedure.StepName = oReader.GetString("StepName");
            return oGUProductionProcedure;
        }

        private GUProductionProcedure CreateObject(NullHandler oReader)
        {
            GUProductionProcedure oGUProductionProcedure = new GUProductionProcedure();
            oGUProductionProcedure = MapObject(oReader);
            return oGUProductionProcedure;
        }

        private List<GUProductionProcedure> CreateObjects(IDataReader oReader)
        {
            List<GUProductionProcedure> oGUProductionProcedure = new List<GUProductionProcedure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionProcedure oItem = CreateObject(oHandler);
                oGUProductionProcedure.Add(oItem);
            }
            return oGUProductionProcedure;
        }

        #endregion

        #region Interface implementation
        public GUProductionProcedureService() { }

        public List<GUProductionProcedure> Save(GUProductionOrder oGUProductionOrder, Int64 nUserID)
        {
            TransactionContext tc = null; string sGUProductionProcedureIDs = "";
            List<GUProductionProcedure> oGUProductionProcedures = new List<GUProductionProcedure>();
            GUProductionProcedure oGUProductionProcedure = new GUProductionProcedure();
            oGUProductionProcedures = oGUProductionOrder.GUProductionProcedures;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Insert GUProductionProcedures
                if (oGUProductionProcedures != null)
                {
                    foreach (GUProductionProcedure oItem in oGUProductionProcedures)
                    {
                        IDataReader readerdetail;
                        oItem.GUProductionOrderID = oGUProductionOrder.GUProductionOrderID;
                        if (oItem.GUProductionProcedureID <= 0)
                        {
                            readerdetail = GUProductionProcedureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        }
                        else
                        {
                            readerdetail = GUProductionProcedureDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sGUProductionProcedureIDs = sGUProductionProcedureIDs + oReaderDetail.GetString("GUProductionProcedureID") + ",";
                        }
                        readerdetail.Close();
                    }
                    if (sGUProductionProcedureIDs.Length > 0)
                    {
                        sGUProductionProcedureIDs = sGUProductionProcedureIDs.Remove(sGUProductionProcedureIDs.Length - 1, 1);
                    }

                }

                oGUProductionProcedure = new GUProductionProcedure();
                oGUProductionProcedure.GUProductionOrderID = oGUProductionOrder.GUProductionOrderID;
                GUProductionProcedureDA.Delete(tc, oGUProductionProcedure, EnumDBOperation.Delete, nUserID, sGUProductionProcedureIDs);
                #endregion

                #region Gets
                IDataReader reader;
                reader = GUProductionProcedureDA.Gets(tc, oGUProductionOrder.GUProductionOrderID);
                oGUProductionProcedures = CreateObjects(reader);
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionProcedures = new List<GUProductionProcedure>();
                oGUProductionProcedure = new GUProductionProcedure();
                oGUProductionProcedure.ErrorMessage = e.Message.Split('~')[0];
                oGUProductionProcedures.Add(oGUProductionProcedure);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GUProductionProcedure. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionProcedures;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUProductionProcedure oGUProductionProcedure = new GUProductionProcedure();
                oGUProductionProcedure.GUProductionProcedureID = id;
                GUProductionProcedureDA.Delete(tc, oGUProductionProcedure, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GUProductionProcedure. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GUProductionProcedure Get(int id, Int64 nUserId)
        {
            GUProductionProcedure oAccountHead = new GUProductionProcedure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionProcedureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GUProductionProcedure", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GUProductionProcedure> Gets(Int64 nUserID)
        {
            List<GUProductionProcedure> oGUProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionProcedureDA.Gets(tc);
                oGUProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionProcedure", e);
                #endregion
            }

            return oGUProductionProcedure;
        }
        

        public List<GUProductionProcedure> Gets(int nGUProductionOrderID, Int64 nUserID)
        {
            List<GUProductionProcedure> oGUProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionProcedureDA.Gets(tc, nGUProductionOrderID);
                oGUProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionProcedure", e);
                #endregion
            }

            return oGUProductionProcedure;
        }

        public List<GUProductionProcedure> GetsbyOrderRecap(int nOrderRecapID, Int64 nUserID)
        {
            List<GUProductionProcedure> oGUProductionProcedure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionProcedureDA.GetsbyOrderRecap(tc, nOrderRecapID);
                oGUProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionProcedure", e);
                #endregion
            }

            return oGUProductionProcedure;
        }

        public List<GUProductionProcedure> Gets_byPOIDs(string sPOIDs, Int64 nUserID)
        {
            List<GUProductionProcedure> oGUProductionProcedure = new List<GUProductionProcedure>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionProcedureDA.Gets_byPOIDs(tc, sPOIDs);
                oGUProductionProcedure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionProcedure", e);
                #endregion
            }

            return oGUProductionProcedure;
        }
        
        #endregion
    }
}
