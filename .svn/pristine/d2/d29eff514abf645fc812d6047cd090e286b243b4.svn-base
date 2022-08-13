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
    public class GUProductionOrderDetailService : MarshalByRefObject, IGUProductionOrderDetailService
    {
        #region Private functions and declaration
        private GUProductionOrderDetail MapObject(NullHandler oReader)
        {
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
            oGUProductionOrderDetail.GUProductionOrderDetailID = oReader.GetInt32("GUProductionOrderDetailID");
            oGUProductionOrderDetail.GUProductionOrderID = oReader.GetInt32("GUProductionOrderID");
            oGUProductionOrderDetail.ColorID = oReader.GetInt32("ColorID");
            oGUProductionOrderDetail.UnitID = oReader.GetInt32("UnitID");
            oGUProductionOrderDetail.Qty = oReader.GetDouble("Qty");
            oGUProductionOrderDetail.ColorName = oReader.GetString("ColorName");
            oGUProductionOrderDetail.UnitName = oReader.GetString("UnitName");
            oGUProductionOrderDetail.Symbol = oReader.GetString("Symbol");
            oGUProductionOrderDetail.SizeID = oReader.GetInt32("SizeID");
            oGUProductionOrderDetail.SizeName = oReader.GetString("SizeName");
            oGUProductionOrderDetail.OrderQty = oReader.GetDouble("OrderQty");
            oGUProductionOrderDetail.YetToProductionQty = oReader.GetDouble("YetToProductionQty");
            
            return oGUProductionOrderDetail;
        }

        private GUProductionOrderDetail CreateObject(NullHandler oReader)
        {
            GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
            oGUProductionOrderDetail = MapObject(oReader);
            return oGUProductionOrderDetail;
        }

        private List<GUProductionOrderDetail> CreateObjects(IDataReader oReader)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetail = new List<GUProductionOrderDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                GUProductionOrderDetail oItem = CreateObject(oHandler);
                oGUProductionOrderDetail.Add(oItem);
            }
            return oGUProductionOrderDetail;
        }

        #endregion

        #region Interface implementation
        public GUProductionOrderDetailService() { }

        public GUProductionOrderDetail Save(GUProductionOrderDetail oGUProductionOrderDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oGUProductionOrderDetail.GUProductionOrderDetailID <= 0)
                {
                    reader = GUProductionOrderDetailDA.InsertUpdate(tc, oGUProductionOrderDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = GUProductionOrderDetailDA.InsertUpdate(tc, oGUProductionOrderDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oGUProductionOrderDetail = new GUProductionOrderDetail();
                    oGUProductionOrderDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oGUProductionOrderDetail = new GUProductionOrderDetail();
                oGUProductionOrderDetail.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save GUProductionOrderDetail. Because of " + e.Message, e);
                #endregion
            }
            return oGUProductionOrderDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                GUProductionOrderDetail oGUProductionOrderDetail = new GUProductionOrderDetail();
                oGUProductionOrderDetail.GUProductionOrderDetailID = id;
                GUProductionOrderDetailDA.Delete(tc, oGUProductionOrderDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete GUProductionOrderDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public GUProductionOrderDetail Get(int id, Int64 nUserId)
        {
            GUProductionOrderDetail oAccountHead = new GUProductionOrderDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = GUProductionOrderDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get GUProductionOrderDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<GUProductionOrderDetail> Gets(Int64 nUserID)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDetailDA.Gets(tc);
                oGUProductionOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrderDetail", e);
                #endregion
            }

            return oGUProductionOrderDetail;
        }

        public List<GUProductionOrderDetail> Gets(int nid, Int64 nUserID)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDetailDA.Gets(nid, tc);
                oGUProductionOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrderDetail", e);
                #endregion
            }

            return oGUProductionOrderDetail;
        }
        public List<GUProductionOrderDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDetailDA.Gets(tc, sSQL);
                oGUProductionOrderDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrderDetail", e);
                #endregion
            }

            return oGUProductionOrderDetail;
        }

        public List<GUProductionOrderDetail> GetsByGUProductionOrder(int nid, Int64 nUserID)
        {
            List<GUProductionOrderDetail> oGUProductionOrderDetails = new List<GUProductionOrderDetail>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = GUProductionOrderDetailDA.GetsByGUProductionOrder(nid, tc);
                oGUProductionOrderDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get GUProductionOrderDetail", e);
                #endregion
            }

            return oGUProductionOrderDetails;
        }

        #endregion
    }  

}
