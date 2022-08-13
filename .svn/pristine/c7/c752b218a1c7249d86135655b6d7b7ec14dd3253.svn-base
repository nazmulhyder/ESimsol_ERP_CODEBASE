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
    public class FabricProductionFaultService : MarshalByRefObject, IFabricProductionFaultService
    {
        #region Private functions and declaration
        private FabricProductionFault MapObject(NullHandler oReader)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            oFabricProductionFault.FPFID = oReader.GetInt32("FPFID");
            oFabricProductionFault.FabricFaultType = (EnumFabricFaultType)oReader.GetInt32("FabricFaultType");
            oFabricProductionFault.BUType = (EnumBusinessUnitType)oReader.GetInt32("BUType");
            oFabricProductionFault.Name = oReader.GetString("Name");
            oFabricProductionFault.IsActive = oReader.GetBoolean("IsActive");
            oFabricProductionFault.Sequence = oReader.GetInt32("Sequence");
            oFabricProductionFault.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oFabricProductionFault.LastUpdateDateTime = oReader.GetDateTime("LastUpdateDateTime");
            oFabricProductionFault.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oFabricProductionFault;
        }
        private FabricProductionFault CreateObject(NullHandler oReader)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            oFabricProductionFault = MapObject(oReader);
            return oFabricProductionFault;
        }
        private List<FabricProductionFault> CreateObjects(IDataReader oReader)
        {
            List<FabricProductionFault> oFabricProductionFault = new List<FabricProductionFault>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricProductionFault oItem = CreateObject(oHandler);
                oFabricProductionFault.Add(oItem);
            }
            return oFabricProductionFault;
        }

        #endregion

        #region Interface implementation
        public FabricProductionFault Save(FabricProductionFault oFabricProductionFault, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFabricProductionFault.FPFID <= 0)
                {
                    reader = FabricProductionFaultDA.InsertUpdate(tc, oFabricProductionFault, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = FabricProductionFaultDA.InsertUpdate(tc, oFabricProductionFault, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricProductionFault = new FabricProductionFault();
                    oFabricProductionFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProductionFault.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricProductionFault;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricProductionFault oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.FPFID = id;
                DBTableReferenceDA.HasReference(tc, "FabricProductionFault", id);
                FabricProductionFaultDA.Delete(tc, oFabricProductionFault, EnumDBOperation.Delete, nUserId);
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
        public List<FabricProductionFault> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricProductionFault> oFabricProductionFaults = new List<FabricProductionFault>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricProductionFaultDA.Gets(tc, sSQL);
                oFabricProductionFaults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProductionFaults = new List<FabricProductionFault>();
                FabricProductionFault oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.ErrorMessage = e.Message.Split('~')[0];
                oFabricProductionFaults.Add(oFabricProductionFault);
                #endregion
            }
            return oFabricProductionFaults;
        }
        public List<FabricProductionFault> Gets(Int64 nUserID)
        {
            List<FabricProductionFault> oFabricProductionFaults = new List<FabricProductionFault>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricProductionFaultDA.Gets(tc);
                oFabricProductionFaults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProductionFaults = new List<FabricProductionFault>();
                FabricProductionFault oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.ErrorMessage = e.Message.Split('~')[0];
                oFabricProductionFaults.Add(oFabricProductionFault);
                #endregion
            }
            return oFabricProductionFaults;
        }
        public FabricProductionFault Get(int id, Int64 nUserId)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricProductionFaultDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricProductionFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProductionFault.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricProductionFault;
        }
        public FabricProductionFault ActiveOrInactive(int nFPFID, bool bIsActive, Int64 nUserID)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricProductionFaultDA.ActiveOrInactive(tc, nFPFID, bIsActive);
                IDataReader reader = FabricProductionFaultDA.Get(tc, nFPFID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricProductionFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricProductionFault = new FabricProductionFault();
                oFabricProductionFault.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricProductionFault;
        }
        public List<FabricProductionFault> SaveList(List<FabricProductionFault> oFabricProductionFaults, Int64 nUserID)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            List<FabricProductionFault> _oFabricProductionFaults = new List<FabricProductionFault>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricProductionFault oTempFabricProductionFault in oFabricProductionFaults)
                {
                    #region FabricProductionFault Part
                    IDataReader reader;
                    if (oTempFabricProductionFault.FPFID <= 0)
                    {

                        reader = FabricProductionFaultDA.InsertUpdate(tc, oTempFabricProductionFault, EnumDBOperation.Insert, nUserID);
                    }
                    else
                    {

                        reader = FabricProductionFaultDA.InsertUpdate(tc, oTempFabricProductionFault, EnumDBOperation.Update, nUserID);

                    }
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFabricProductionFault = new FabricProductionFault();
                        oFabricProductionFault = CreateObject(oReader);
                    }
                    reader.Close();
                    #endregion
                    _oFabricProductionFaults.Add(oFabricProductionFault);
                }
                tc.End();

            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricProductionFault = new FabricProductionFault();
                    oFabricProductionFault.ErrorMessage = e.Message;
                    _oFabricProductionFaults = new List<FabricProductionFault>();
                    _oFabricProductionFaults.Add(oFabricProductionFault);
                }
            }
            return _oFabricProductionFaults;
        }

        public List<FabricProductionFault> RefreshSequence(List<FabricProductionFault> oFabricProductionFaults, Int64 nUserID)
        {
            FabricProductionFault oFabricProductionFault = new FabricProductionFault();
            List<FabricProductionFault> _oFabricProductionFaults = new List<FabricProductionFault>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oFabricProductionFaults.Count > 0)
                {
                    foreach (FabricProductionFault oTempFabricProductionFault in oFabricProductionFaults)
                    {
                        if (oTempFabricProductionFault.FPFID > 0 && oTempFabricProductionFault.Sequence > 0)
                        {
                            FabricProductionFaultDA.UpdateSequence(tc, oTempFabricProductionFault);
                        }
                    }
                }

                tc.End();
            }
            catch (Exception e)
            {
                if (tc != null)
                {
                    tc.HandleError();
                    oFabricProductionFault = new FabricProductionFault();
                    oFabricProductionFault.ErrorMessage = e.Message;
                    _oFabricProductionFaults = new List<FabricProductionFault>();
                    _oFabricProductionFaults.Add(oFabricProductionFault);
                }
            }
            return _oFabricProductionFaults;
        }
        

       
        #endregion
    }
}
