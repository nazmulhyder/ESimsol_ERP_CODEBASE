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
    public class FNBatchQCService : MarshalByRefObject, IFNBatchQCService
    {
        #region Private functions and declaration
        private static FNBatchQC MapObject(NullHandler oReader)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            oFNBatchQC.FNBatchQCID = oReader.GetInt32("FNBatchQCID");
            oFNBatchQC.FNBatchID = oReader.GetInt32("FNBatchID");
            oFNBatchQC.Qty = oReader.GetDouble("Qty");
            oFNBatchQC.StartTime = oReader.GetDateTime("StartTime");
            oFNBatchQC.EndTime = oReader.GetDateTime("EndTime");
            oFNBatchQC.ActualWidth = oReader.GetDouble("ActualWidth");
            oFNBatchQC.QCInchargeID = oReader.GetInt32("QCInchargeID");
            oFNBatchQC.FNBatchNo = oReader.GetString("FNBatchNo");
            oFNBatchQC.FNExONo = oReader.GetString("FNExONo");
            oFNBatchQC.BatchQty = oReader.GetDouble("BatchQty");
            oFNBatchQC.OutQty = oReader.GetDouble("OutQty");
            oFNBatchQC.CountName = oReader.GetString("CountName");
            oFNBatchQC.BuyerName = oReader.GetString("BuyerName");
            oFNBatchQC.Construction = oReader.GetString("Construction");
            oFNBatchQC.ConstructionPI = oReader.GetString("ConstructionPI");
            oFNBatchQC.FinishWidth = oReader.GetString("FinishWidth");
            oFNBatchQC.FinishTypeName = oReader.GetString("FinishTypeName");
            oFNBatchQC.FNBatchStatus = (EnumFNBatchStatus)oReader.GetInt16("FNBatchStatus");
            oFNBatchQC.MUnit = oReader.GetString("MUnit");
            oFNBatchQC.Color = oReader.GetString("Color");
            oFNBatchQC.Composition = oReader.GetString("Composition");
            oFNBatchQC.FabricWeaveName = oReader.GetString("FabricWeaveName");
            oFNBatchQC.StyleNo = oReader.GetString("StyleNo");
            oFNBatchQC.BuyerRef = oReader.GetString("BuyerRef");
            oFNBatchQC.ExeQty = oReader.GetDouble("ExeQty");
            oFNBatchQC.CountYetNotRecv = oReader.GetInt32("CountYetNotRecv");
            oFNBatchQC.FNExOID = oReader.GetInt32("FNExOID");
            oFNBatchQC.QCInchargeName = oReader.GetString("QCInchargeName");
            oFNBatchQC.BuyerID = oReader.GetInt32("BuyerID");

            return oFNBatchQC;
        }

        public static FNBatchQC CreateObject(NullHandler oReader)
        {
            FNBatchQC oFNBatchQC = MapObject(oReader);
            return oFNBatchQC;
        }

        private List<FNBatchQC> CreateObjects(IDataReader oReader)
        {
            List<FNBatchQC> oFNBatchQC = new List<FNBatchQC>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNBatchQC oItem = CreateObject(oHandler);
                oFNBatchQC.Add(oItem);
            }
            return oFNBatchQC;
        }

        #endregion

        #region Interface implementation
        public FNBatchQCService() { }

        public FNBatchQC IUD(FNBatchQC oFNBatchQC, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    reader = FNBatchQCDA.IUD(tc, oFNBatchQC, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFNBatchQC = new FNBatchQC();
                        oFNBatchQC = CreateObject(oReader);
                    }
                    reader.Close();

                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FNBatchQCDA.IUD(tc, oFNBatchQC, nDBOperation, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    reader.Close();
                    oFNBatchQC.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFNBatchQC;
        }

        public FNBatchQC Get(int nFNBatchQCID, Int64 nUserId)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNBatchQCDA.Get(tc, nFNBatchQCID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchQC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNBatchQC;
        }

        public List<FNBatchQC> Gets(string sSQL, Int64 nUserID)
        {
            List<FNBatchQC> oFNBatchQCs = new List<FNBatchQC>();
            FNBatchQC oFNBatchQC = new FNBatchQC();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNBatchQCDA.Gets(tc, sSQL);
                oFNBatchQCs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFNBatchQCs.Add(oFNBatchQC);
                #endregion
            }

            return oFNBatchQCs;
        }

        public FNBatchQC FNBatchQCDone(FNBatchQC oFNBatchQC, Int64 nUserID)
        {
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                reader = FNBatchQCDA.IUD(tc, oFNBatchQC, (int)EnumDBOperation.Approval, nUserID); // As QC Done
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchQC = new FNBatchQC();
                    oFNBatchQC = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFNBatchQC;
        }

        public FNBatchQC SaveProcess(int nFNBatchID, string sRollNo, int nRollCountStart, int nQCLotItem, Int64 nUserID)
        {
            FNBatchQC oFNBatchQC = new FNBatchQC();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);

                reader = FNBatchQCDA.SaveProcess(tc, nFNBatchID, sRollNo, nRollCountStart, nQCLotItem, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNBatchQC = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNBatchQC = new FNBatchQC();
                oFNBatchQC.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            return oFNBatchQC;
        }

        
        #endregion
    }
}
