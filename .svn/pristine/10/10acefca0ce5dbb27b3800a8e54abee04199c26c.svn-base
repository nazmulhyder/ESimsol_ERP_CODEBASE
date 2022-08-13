using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class TwistingDetailService : MarshalByRefObject, ITwistingDetailService
    {
        #region Private functions and declaration
        private TwistingDetail MapObject(NullHandler oReader)
        {
            TwistingDetail oTwistingDetail = new TwistingDetail();

            oTwistingDetail.TwistingDetailID = oReader.GetInt32("TwistingDetailID");
            oTwistingDetail.TwistingID = oReader.GetInt32("TwistingID");
            oTwistingDetail.LotID = oReader.GetInt32("LotID");
            oTwistingDetail.MUnitID = oReader.GetInt16("MUnitID");
            oTwistingDetail.InOutType = (EnumInOutType)oReader.GetInt32("InOutType");
            oTwistingDetail.ProductID = oReader.GetInt32("ProductID");
            oTwistingDetail.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oTwistingDetail.IsLotMendatory = oReader.GetBoolean("IsLotMendatory");
            oTwistingDetail.Qty = oReader.GetDouble("Qty");
            oTwistingDetail.Qty_Order = oReader.GetDouble("Qty_Order");
            oTwistingDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oTwistingDetail.LotNo = oReader.GetString("LotNo");
            oTwistingDetail.MUnit = oReader.GetString("MUnit");
            oTwistingDetail.Note = oReader.GetString("Note");
            oTwistingDetail.ProductCode = oReader.GetString("ProductCode");
            oTwistingDetail.ProductName = oReader.GetString("ProductName");
            oTwistingDetail.ColorName = oReader.GetString("ColorName");
            oTwistingDetail.YetQty = oReader.GetDouble("YetQty");

            return oTwistingDetail;           
        }

        private TwistingDetail CreateObject(NullHandler oReader)
        {
            TwistingDetail oTwistingDetail = MapObject(oReader);
            return oTwistingDetail;
        }

        private List<TwistingDetail> CreateObjects(IDataReader oReader)
        {
            List<TwistingDetail> oTwistingDetail = new List<TwistingDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                TwistingDetail oItem = CreateObject(oHandler);
                oTwistingDetail.Add(oItem);
            }
            return oTwistingDetail;
        }

        #endregion

        #region Interface implementation
        public TwistingDetailService() { }

        public TwistingDetail Save(TwistingDetail oTwistingDetail, Int64 nUserID)
        {
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oTwistingDetail.TwistingDetailID <= 0)
                {
                    reader = TwistingDetailDA.InsertUpdate(tc, oTwistingDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = TwistingDetailDA.InsertUpdate(tc, oTwistingDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwistingDetail = new TwistingDetail();
                    oTwistingDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to . Because of " + e.Message, e);
                oTwistingDetail = new TwistingDetail();
                oTwistingDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oTwistingDetail;
        }
        public string Delete(TwistingDetail oTwistingDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                //  TwistingDetailDA.Delete(tc, oTwistingDetail, EnumDBOperation.Delete, nUserId,"");
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
        public TwistingDetail Get(int nTwistingDetailID, Int64 nUserId)
        {
            TwistingDetail oTwistingDetail = new TwistingDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = TwistingDetailDA.Get(nTwistingDetailID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oTwistingDetail = CreateObject(oReader);
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
                oTwistingDetail.ErrorMessage = e.Message;
                #endregion
            }

            return oTwistingDetail;
        }
        public List<TwistingDetail> Gets(int nTwistingID, Int64 nUserID)
        {
            List<TwistingDetail> oTwistingDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TwistingDetailDA.Gets(tc, nTwistingID);
                oTwistingDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TwistingDetail", e);
                #endregion
            }
            return oTwistingDetail;
        }
        public List<TwistingDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<TwistingDetail> oTwistingDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = TwistingDetailDA.Gets(sSQL, tc);
                oTwistingDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TwistingDetail", e);
                #endregion
            }
            return oTwistingDetail;
        }

        #endregion
    }
}
