using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;
namespace ESimSol.Services.Services
{
    public class FNProductionBatchTransferDetailService : MarshalByRefObject, IFNProductionBatchTransferDetailService
    {
        #region Private functions and declaration
        private FNProductionBatchTransferDetail MapObject(NullHandler oReader)
        {
            FNProductionBatchTransferDetail oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
            oFNProductionBatchTransferDetail.FNPBTransferDetailID = oReader.GetInt32("FNPBTransferDetailID");
            oFNProductionBatchTransferDetail.FNPBTransferID = oReader.GetInt32("FNPBTransferID");
            oFNProductionBatchTransferDetail.FNPBatchID = oReader.GetInt32("FNPBatchID");
            oFNProductionBatchTransferDetail.StartQty = oReader.GetDouble("StartQty");
            oFNProductionBatchTransferDetail.EndQty = oReader.GetDouble("EndQty");
            oFNProductionBatchTransferDetail.StartWidth = oReader.GetDouble("StartWidth");
            oFNProductionBatchTransferDetail.EndWidth = oReader.GetDouble("EndWidth");
            oFNProductionBatchTransferDetail.ShiftName = oReader.GetString("ShiftName");
            oFNProductionBatchTransferDetail.BatchNo = oReader.GetString("BatchNo");
            return oFNProductionBatchTransferDetail;
        }

        private FNProductionBatchTransferDetail CreateObject(NullHandler oReader)
        {
            FNProductionBatchTransferDetail oFNProductionBatchTransferDetail = MapObject(oReader);
            return oFNProductionBatchTransferDetail;
        }

        private List<FNProductionBatchTransferDetail> CreateObjects(IDataReader oReader)
        {
            List<FNProductionBatchTransferDetail> oFNProductionBatchTransferDetail = new List<FNProductionBatchTransferDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FNProductionBatchTransferDetail oItem = CreateObject(oHandler);
                oFNProductionBatchTransferDetail.Add(oItem);
            }
            return oFNProductionBatchTransferDetail;
        }

        #endregion

        #region Interface implementation
        public FNProductionBatchTransferDetailService() { }

        public FNProductionBatchTransferDetail IUD(FNProductionBatchTransferDetail oFNProductionBatchTransferDetail, int nDBOperation, Int64 nUserID)
        {
            FNProductionBatchTransfer oFNPBT = new FNProductionBatchTransfer();
            List<FNProductionBatchTransferDetail> _oFNProductionBatchTransferDetails = new List<FNProductionBatchTransferDetail>();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update)
                {

                    if (oFNProductionBatchTransferDetail.FNPBTransferID <= 0)
                    {
                        reader = FNProductionBatchTransferDA.IUD(tc, oFNProductionBatchTransferDetail.TempFNProductionBatchTransfer, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFNPBT = new FNProductionBatchTransfer();
                            oFNPBT = FNProductionBatchTransferService.CreateObject(oReader);
                        }
                        reader.Close();
                    }

                    oFNProductionBatchTransferDetail.FNPBTransferID = (oFNProductionBatchTransferDetail.FNPBTransferID > 0) ? oFNProductionBatchTransferDetail.FNPBTransferID : oFNPBT.FNPBTransferID;
                    int FNPBTransferID = oFNProductionBatchTransferDetail.FNPBTransferID;
                    foreach (FNProductionBatchTransferDetail obj in oFNProductionBatchTransferDetail.FNProductionBatchTransferDetails)
                    {

                        obj.FNPBTransferID = oFNProductionBatchTransferDetail.FNPBTransferID;
                        reader = FNProductionBatchTransferDetailDA.IUD(tc, obj, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            var oFNBTD = new FNProductionBatchTransferDetail();
                            oFNBTD = CreateObject(oReader);
                            _oFNProductionBatchTransferDetails.Add(oFNBTD);

                        }
                        reader.Close();

                    }
                    oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
                    oFNProductionBatchTransferDetail.FNProductionBatchTransferDetails = _oFNProductionBatchTransferDetails;
                    if (_oFNProductionBatchTransferDetails.Count > 0)
                    {
                        oFNProductionBatchTransferDetail.FNPBTransferID = FNPBTransferID;
                    }
                       

                    
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FNProductionBatchTransferDetailDA.IUD(tc, oFNProductionBatchTransferDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oFNProductionBatchTransferDetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                _oFNProductionBatchTransferDetails = new List<FNProductionBatchTransferDetail>();
                oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
                oFNProductionBatchTransferDetail.ErrorMessage = (ex.Message.Contains("!")) ? ex.Message.Split('!')[0] : ex.Message;
                #endregion
            }
            //oFNProductionBatchTransferDetail.TempSULayDown = oSULayDown;
            return oFNProductionBatchTransferDetail;
        }

        public FNProductionBatchTransferDetail Get(int nSULDDetailID, Int64 nUserId)
        {
            FNProductionBatchTransferDetail oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FNProductionBatchTransferDetailDA.Get(tc, nSULDDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFNProductionBatchTransferDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
                oFNProductionBatchTransferDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFNProductionBatchTransferDetail;
        }

        public List<FNProductionBatchTransferDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FNProductionBatchTransferDetail> oFNProductionBatchTransferDetails = new List<FNProductionBatchTransferDetail>();
            FNProductionBatchTransferDetail oFNProductionBatchTransferDetail = new FNProductionBatchTransferDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FNProductionBatchTransferDetailDA.Gets(tc, sSQL);
                oFNProductionBatchTransferDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFNProductionBatchTransferDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFNProductionBatchTransferDetails.Add(oFNProductionBatchTransferDetail);
                #endregion
            }

            return oFNProductionBatchTransferDetails;
        }

      
        
        #endregion
    }
}
