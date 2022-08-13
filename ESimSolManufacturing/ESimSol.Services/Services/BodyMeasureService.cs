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
    public class BodyMeasureService : MarshalByRefObject, IBodyMeasureService
    {
        #region Private functions and declaration
        private BodyMeasure MapObject(NullHandler oReader)
        {
            BodyMeasure oBodyMeasure = new BodyMeasure();
            oBodyMeasure.BodyMeasureID = oReader.GetInt32("BodyMeasureID");
            oBodyMeasure.CostSheetID = oReader.GetInt32("CostSheetID");
            oBodyMeasure.BodyPartID = oReader.GetInt32("BodyPartID");
            oBodyMeasure.MeasureInCM = oReader.GetDouble("MeasureInCM");
            oBodyMeasure.GSM = oReader.GetDouble("GSM");
            oBodyMeasure.Remarks = oReader.GetString("Remarks");
            oBodyMeasure.BodyPartCode = oReader.GetString("BodyPartCode");
            oBodyMeasure.BodyPartName = oReader.GetString("BodyPartName");
            return oBodyMeasure;
        }

        private BodyMeasure CreateObject(NullHandler oReader)
        {
            BodyMeasure oBodyMeasure = new BodyMeasure();
            oBodyMeasure = MapObject(oReader);
            return oBodyMeasure;
        }

        private List<BodyMeasure> CreateObjects(IDataReader oReader)
        {
            List<BodyMeasure> oBodyMeasure = new List<BodyMeasure>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BodyMeasure oItem = CreateObject(oHandler);
                oBodyMeasure.Add(oItem);
            }
            return oBodyMeasure;
        }

        #endregion

        #region Interface implementation
        public BodyMeasureService() { }
        public BodyMeasure Save(BodyMeasure oBodyMeasure, Int64 nUserID)
        {
            int nCostSheetID = 0;
            string sBodyMeasureIDs = ""; 
            TransactionContext tc = null;            
            List<BodyMeasure> oBodyMeasures = new List<BodyMeasure>();
            oBodyMeasures = oBodyMeasure.BodyMeasures;
            try
            {                
                tc = TransactionContext.Begin(true);
                foreach (BodyMeasure oItem in oBodyMeasures)
                {
                    nCostSheetID = oItem.CostSheetID;
                    IDataReader reader;
                    if (oItem.BodyMeasureID <= 0)
                    {
                        reader = BodyMeasureDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                    }
                    else
                    {
                        reader = BodyMeasureDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBodyMeasure = new BodyMeasure();
                        oBodyMeasure = CreateObject(oReader);
                        sBodyMeasureIDs = sBodyMeasureIDs + oBodyMeasure.BodyMeasureID.ToString() + ",";
                    }
                    reader.Close();                    
                }
                if (sBodyMeasureIDs.Length > 0)
                {
                    sBodyMeasureIDs = sBodyMeasureIDs.Remove(sBodyMeasureIDs.Length - 1, 1);
                }
                oBodyMeasure = new BodyMeasure();
                oBodyMeasure.CostSheetID = nCostSheetID;
                BodyMeasureDA.Delete(tc, oBodyMeasure, EnumDBOperation.Delete, nUserID, sBodyMeasureIDs);

                oBodyMeasures = new List<BodyMeasure>();
                IDataReader readers = null;
                readers = BodyMeasureDA.Gets(tc, nCostSheetID);
                oBodyMeasures = CreateObjects(readers);
                readers.Close();

                oBodyMeasure = new BodyMeasure();
                oBodyMeasure.BodyMeasures = oBodyMeasures;
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBodyMeasure = new BodyMeasure();
                oBodyMeasure.ErrorMessage = e.Message;
                #endregion
            }
            return oBodyMeasure;
        }    
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BodyMeasure oBodyMeasure = new BodyMeasure();
                oBodyMeasure.BodyMeasureID = id;
                BodyMeasureDA.Delete(tc, oBodyMeasure, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BodyMeasure. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }
        public BodyMeasure Get(int id, Int64 nUserId)
        {
            BodyMeasure oAccountHead = new BodyMeasure();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BodyMeasureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BodyMeasure", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<BodyMeasure> Gets(Int64 nUserID)
        {
            List<BodyMeasure> oBodyMeasure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BodyMeasureDA.Gets(tc);
                oBodyMeasure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BodyMeasure", e);
                #endregion
            }

            return oBodyMeasure;
        }
        public List<BodyMeasure> Gets(int nCostSheetID, Int64 nUserID)
        {
            List<BodyMeasure> oBodyMeasure = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BodyMeasureDA.Gets(tc, nCostSheetID);
                oBodyMeasure = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BodyMeasure", e);
                #endregion
            }

            return oBodyMeasure;
        }
        #endregion
    }
}

