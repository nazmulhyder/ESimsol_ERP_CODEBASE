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
    public class FabricReturnChallanDetailService : MarshalByRefObject, IFabricReturnChallanDetailService
    {
        #region Private functions and declaration
        private FabricReturnChallanDetail MapObject(NullHandler oReader)
        {
            FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
            oFabricReturnChallanDetail.FabricReturnChallanDetailID = oReader.GetInt32("FabricReturnChallanDetailID");
            oFabricReturnChallanDetail.FabricReturnChallanID = oReader.GetInt32("FabricReturnChallanID");
            oFabricReturnChallanDetail.FDCDID = oReader.GetInt32("FDCDID");
            oFabricReturnChallanDetail.LotID = oReader.GetInt32("LotID");
            oFabricReturnChallanDetail.Qty = oReader.GetDouble("Qty");
            oFabricReturnChallanDetail.Qty_DC = oReader.GetDouble("Qty_DC");
            oFabricReturnChallanDetail.Qty_DO = oReader.GetDouble("Qty_DO");
            oFabricReturnChallanDetail.Qty_Return_Prv = oReader.GetDouble("Qty_Return_Prv");
            oFabricReturnChallanDetail.LotNo = oReader.GetString("LotNo");
            oFabricReturnChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oFabricReturnChallanDetail.FabricNo = oReader.GetString("FabricNo");
            oFabricReturnChallanDetail.ProductName = oReader.GetString("ProductName");
            oFabricReturnChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oFabricReturnChallanDetail.ChallanQty = oReader.GetDouble("ChallanQty");
            oFabricReturnChallanDetail.ChallanNo = oReader.GetString("ChallanNo");
            oFabricReturnChallanDetail.ExeNo = oReader.GetString("ExeNo");
            oFabricReturnChallanDetail.MUName = oReader.GetString("MUName");
            oFabricReturnChallanDetail.MUnitID = oReader.GetInt32("MUnitID");

            return oFabricReturnChallanDetail;
        }

        private FabricReturnChallanDetail CreateObject(NullHandler oReader)
        {
            FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
            oFabricReturnChallanDetail = MapObject(oReader);
            return oFabricReturnChallanDetail;
        }

        private List<FabricReturnChallanDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricReturnChallanDetail> oFabricReturnChallanDetail = new List<FabricReturnChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricReturnChallanDetail oItem = CreateObject(oHandler);
                oFabricReturnChallanDetail.Add(oItem);
            }
            return oFabricReturnChallanDetail;
        }
        #endregion

        #region Interface implementation
        public FabricReturnChallanDetail Save(FabricReturnChallanDetail oFabricReturnChallanDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
              
                #region New Code
                IDataReader readerdetail;
                if (oFabricReturnChallanDetail.FabricReturnChallanDetailID <= 0)
                {
                    readerdetail = FabricReturnChallanDetailDA.InsertUpdate(tc, oFabricReturnChallanDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    readerdetail = FabricReturnChallanDetailDA.InsertUpdate(tc, oFabricReturnChallanDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReaderDetail = new NullHandler(readerdetail);

                if (readerdetail.Read())
                {
                    oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                    oFabricReturnChallanDetail = CreateObject(oReaderDetail);
                   
                }

                readerdetail.Close();
                #endregion
             
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricReturnChallanDetail;
        }       
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.FabricReturnChallanDetailID = id;
                FabricReturnChallanDetailDA.Delete(tc, oFabricReturnChallanDetail, EnumDBOperation.Delete, nUserId, "");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

        public FabricReturnChallanDetail Get(int id, Int64 nUserId)
        {
            FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricReturnChallanDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricReturnChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.ErrorMessage = ex.Message.Split('!')[0];
                #endregion
            }
            return oFabricReturnChallanDetail;
        }
        public List<FabricReturnChallanDetail> Gets(Int64 nUserID)
        {
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricReturnChallanDetailDA.Gets(tc);
                oFabricReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.ErrorMessage = ex.Message.Split('!')[0];
                oFabricReturnChallanDetails.Add(oFabricReturnChallanDetail);
                #endregion
            }
            return oFabricReturnChallanDetails;
        }
        public List<FabricReturnChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricReturnChallanDetailDA.Gets(tc, sSQL);
                oFabricReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.ErrorMessage = ex.Message.Split('!')[0];
                oFabricReturnChallanDetails.Add(oFabricReturnChallanDetail);
                #endregion
            }
            return oFabricReturnChallanDetails;
        }
        public List<FabricReturnChallanDetail> Gets(int nFRCID, Int64 nUserID)
        {
            List<FabricReturnChallanDetail> oFabricReturnChallanDetails = new List<FabricReturnChallanDetail>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = FabricReturnChallanDetailDA.Gets(tc, nFRCID);
                oFabricReturnChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                FabricReturnChallanDetail oFabricReturnChallanDetail = new FabricReturnChallanDetail();
                oFabricReturnChallanDetail.ErrorMessage = ex.Message.Split('!')[0];
                oFabricReturnChallanDetails.Add(oFabricReturnChallanDetail);
                #endregion
            }
            return oFabricReturnChallanDetails;
        }
        #endregion
    }
}
