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
    public class FabricBatchQCFaultService : MarshalByRefObject, IFabricBatchQCFaultService
    {
        #region Private functions and declaration
        private FabricBatchQCFault MapObject(NullHandler oReader)
        {
            FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
            oFabricBatchQCFault.FBQCFaultID = oReader.GetInt32("FBQCFaultID");
            oFabricBatchQCFault.FBQCDetailID = oReader.GetInt32("FBQCDetailID");
            oFabricBatchQCFault.FPFID = oReader.GetInt32("FPFID");
            oFabricBatchQCFault.FaultPoint = oReader.GetInt32("FaultPoint");
            oFabricBatchQCFault.NoOfFault = oReader.GetInt32("NoOfFault");
            oFabricBatchQCFault.FPFName = oReader.GetString("FPFName");
            oFabricBatchQCFault.FaultDate = oReader.GetDateTime("FaultDate");
            oFabricBatchQCFault.FabricFaultType = (EnumFabricFaultType)oReader.GetInt32("FabricFaultType");
            oFabricBatchQCFault.Remarks = oReader.GetString("Remarks");
            return oFabricBatchQCFault;
        }
        private FabricBatchQCFault CreateObject(NullHandler oReader)
        {
            FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
            oFabricBatchQCFault = MapObject(oReader);
            return oFabricBatchQCFault;
        }
        private List<FabricBatchQCFault> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchQCFault> oFabricBatchQCFault = new List<FabricBatchQCFault>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchQCFault oItem = CreateObject(oHandler);
                oFabricBatchQCFault.Add(oItem);
            }
            return oFabricBatchQCFault;
        }

        #endregion

        #region Interface implementation
        public FabricBatchQCFault Save(FabricBatchQCFault oFabricBatchQCFault, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricBatchQCFault.FBQCFaultID <= 0)
                {
                    reader = FabricBatchQCFaultDA.InsertUpdate(tc, oFabricBatchQCFault, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricBatchQCFaultDA.InsertUpdate(tc, oFabricBatchQCFault, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQCFault = new FabricBatchQCFault();
                    oFabricBatchQCFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchQCFault;
        }

        public List<FabricBatchQCFault> SaveMultipleFabricBatchQCFault(List<FabricBatchQCFault> oFabricBatchQCFaults, Int64 nUserID)
        {
            FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
            List<FabricBatchQCFault> _oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                _oFabricBatchQCFaults = new List<FabricBatchQCFault>();
                tc = TransactionContext.Begin(true);
                foreach (FabricBatchQCFault oItem in oFabricBatchQCFaults)
                {
                    if (oItem.FBQCFaultID <= 0)
                        reader = FabricBatchQCFaultDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                    else
                        reader = FabricBatchQCFaultDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricBatchQCFault = new FabricBatchQCFault();
                        oFabricBatchQCFault = CreateObject(oReader);
                        _oFabricBatchQCFaults.Add(oFabricBatchQCFault);
                    }
                    reader.Close();
                }


                tc.End();
            }
            catch (Exception ex)
            {
                _oFabricBatchQCFaults = new List<FabricBatchQCFault>();
                #region Handle Exception
                tc.HandleError();
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                _oFabricBatchQCFaults.Add(oFabricBatchQCFault);
                #endregion
            }
            return _oFabricBatchQCFaults;
        }


        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.FBQCFaultID = id;
                DBTableReferenceDA.HasReference(tc, "FabricBatchQCFault", id);
                FabricBatchQCFaultDA.Delete(tc, oFabricBatchQCFault, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public List<FabricBatchQCFault> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricBatchQCFault> oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCFaultDA.Gets(tc, sSQL);
                oFabricBatchQCFaults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCFaults = new List<FabricBatchQCFault>();
                FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchQCFaults.Add(oFabricBatchQCFault);
                #endregion
            }
            return oFabricBatchQCFaults;
        }
        public List<FabricBatchQCFault> Gets(Int64 nUserID)
        {
            List<FabricBatchQCFault> oFabricBatchQCFaults = new List<FabricBatchQCFault>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchQCFaultDA.Gets(tc);
                oFabricBatchQCFaults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCFaults = new List<FabricBatchQCFault>();
                FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = e.Message.Split('!')[0];
                oFabricBatchQCFaults.Add(oFabricBatchQCFault);
                #endregion
            }
            return oFabricBatchQCFaults;
        }
        public FabricBatchQCFault Get(int id, Int64 nUserId)
        {
            FabricBatchQCFault oFabricBatchQCFault = new FabricBatchQCFault();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchQCFaultDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchQCFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchQCFault = new FabricBatchQCFault();
                oFabricBatchQCFault.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchQCFault;
        }

        #endregion
    }
}
