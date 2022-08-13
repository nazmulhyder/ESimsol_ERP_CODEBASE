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
    public class FNBatchQCFaultService : MarshalByRefObject, IFNBatchQCFaultService
    {
        #region Private functions and declaration
        private FNBatchQCFault MapObject(NullHandler oReader)
        {
            FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
            oFNBatchQCFault.FNBQCFaultID = oReader.GetInt32("FNBQCFaultID");
            oFNBatchQCFault.FNBatchQCDetailID = oReader.GetInt32("FNBatchQCDetailID");
            oFNBatchQCFault.FPFID = oReader.GetInt32("FPFID");
            oFNBatchQCFault.FaultPoint = oReader.GetInt16("FaultPoint");
            oFNBatchQCFault.NoOfFault = oReader.GetInt32("NoOfFault");
            oFNBatchQCFault.FaultName = oReader.GetString("FaultName");
            return oFNBatchQCFault;
        }

        private FNBatchQCFault CreateObject(NullHandler oReader)
        {
            FNBatchQCFault oFNBatchQCFault = MapObject(oReader);
            return oFNBatchQCFault;
        }

        private List<FNBatchQCFault> CreateObjects(IDataReader oReader)
        {
            List<FNBatchQCFault> oFNBatchQCFault = new List<FNBatchQCFault>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchQCFault oItem = CreateObject(oHandler);
                oFNBatchQCFault.Add(oItem);
            }
            return oFNBatchQCFault;
        }

        #endregion

        #region Interface implementation
        public FNBatchQCFaultService() { }


        public FNBatchQCFault IUD(FNBatchQCFault oFNBatchQCFault, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = FNBatchQCFaultDA.IUD(tc, oFNBatchQCFault, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCFault = new FNBatchQCFault();
                        oFNBatchQCFault = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FNBatchQCFaultDA.IUD(tc, oFNBatchQCFault, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFNBatchQCFault.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFNBatchQCFault;
        }

        public List<FNBatchQCFault> SaveMultipleFNBatchQCFault(List<FNBatchQCFault> oFNBatchQCFaults, Int64 nUserID)
        {
            FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
            List<FNBatchQCFault> _oFNBatchQCFaults = new List<FNBatchQCFault>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                _oFNBatchQCFaults = new List<FNBatchQCFault>();
                tc = TransactionContext.Begin(true);
                foreach (FNBatchQCFault oItem in oFNBatchQCFaults)
                {
                    if (oItem.FNBQCFaultID <= 0)
                        reader = FNBatchQCFaultDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserID);
                    else
                        reader = FNBatchQCFaultDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserID);

                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQCFault = new FNBatchQCFault();
                        oFNBatchQCFault = CreateObject(oReader);
                        _oFNBatchQCFaults.Add(oFNBatchQCFault);
                    }
                    reader.Close();
                }
                

                tc.End();
            }
            catch (Exception ex)
            {
                _oFNBatchQCFaults = new List<FNBatchQCFault>();
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                _oFNBatchQCFaults.Add(oFNBatchQCFault);
                #endregion
            }
            return _oFNBatchQCFaults;
        }

        public FNBatchQCFault Get(int nFNBQCFaultID, Int64 nUserId)
        {
            FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchQCFaultDA.Get(tc, nFNBQCFaultID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchQCFault = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCFault = new FNBatchQCFault();
                oFNBatchQCFault.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQCFault;
        }

        public List<FNBatchQCFault> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatchQCFault> oFNBatchQCFaults = new List<FNBatchQCFault>();
            FNBatchQCFault oFNBatchQCFault = new FNBatchQCFault();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchQCFaultDA.Gets(tc, sSQL);
                oFNBatchQCFaults = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQCFault.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFNBatchQCFaults.Add(oFNBatchQCFault);
                #endregion
            }

            return oFNBatchQCFaults;
        }

        #endregion
    }
}
