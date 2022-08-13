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
    public class ShipmentScheduleDetailService : MarshalByRefObject, IShipmentScheduleDetailService
    {
        #region Private functions and declaration
        private ShipmentScheduleDetail MapObject(NullHandler oReader)
        {
            ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
            oShipmentScheduleDetail.ShipmentScheduleDetailID = oReader.GetInt32("ShipmentScheduleDetailID");
            oShipmentScheduleDetail.ShipmentScheduleID = oReader.GetInt32("ShipmentScheduleID");
            oShipmentScheduleDetail.ColorID = oReader.GetInt32("ColorID");
            oShipmentScheduleDetail.UnitID = oReader.GetInt32("UnitID");
            oShipmentScheduleDetail.Qty = oReader.GetDouble("Qty");
            oShipmentScheduleDetail.ColorName = oReader.GetString("ColorName");
            oShipmentScheduleDetail.UnitName = oReader.GetString("UnitName");
            oShipmentScheduleDetail.Symbol = oReader.GetString("Symbol");
            oShipmentScheduleDetail.SizeID = oReader.GetInt32("SizeID");
            oShipmentScheduleDetail.SizeName = oReader.GetString("SizeName");
            oShipmentScheduleDetail.OrderQty = oReader.GetDouble("OrderQty");
            oShipmentScheduleDetail.YetToPoductionOrderQty = oReader.GetDouble("YetToPoductionOrderQty");           
            return oShipmentScheduleDetail;
        }

        private ShipmentScheduleDetail CreateObject(NullHandler oReader)
        {
            ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
            oShipmentScheduleDetail = MapObject(oReader);
            return oShipmentScheduleDetail;
        }

        private List<ShipmentScheduleDetail> CreateObjects(IDataReader oReader)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetail = new List<ShipmentScheduleDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShipmentScheduleDetail oItem = CreateObject(oHandler);
                oShipmentScheduleDetail.Add(oItem);
            }
            return oShipmentScheduleDetail;
        }

        #endregion

        #region Interface implementation
        public ShipmentScheduleDetailService() { }

        public ShipmentScheduleDetail Save(ShipmentScheduleDetail oShipmentScheduleDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oShipmentScheduleDetail.ShipmentScheduleDetailID <= 0)
                {
                    reader = ShipmentScheduleDetailDA.InsertUpdate(tc, oShipmentScheduleDetail, EnumDBOperation.Insert, nUserID,"");
                }
                else
                {
                    reader = ShipmentScheduleDetailDA.InsertUpdate(tc, oShipmentScheduleDetail, EnumDBOperation.Update, nUserID,"");
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentScheduleDetail = new ShipmentScheduleDetail();
                    oShipmentScheduleDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oShipmentScheduleDetail = new ShipmentScheduleDetail();
                oShipmentScheduleDetail.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ShipmentScheduleDetail. Because of " + e.Message, e);
                #endregion
            }
            return oShipmentScheduleDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
                oShipmentScheduleDetail.ShipmentScheduleDetailID = id;
                ShipmentScheduleDetailDA.Delete(tc, oShipmentScheduleDetail, EnumDBOperation.Delete, nUserId,"");
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ShipmentScheduleDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public ShipmentScheduleDetail Get(int id, Int64 nUserId)
        {
            ShipmentScheduleDetail oAccountHead = new ShipmentScheduleDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShipmentScheduleDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ShipmentScheduleDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<ShipmentScheduleDetail> Gets(Int64 nUserID)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDetailDA.Gets(tc);
                oShipmentScheduleDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentScheduleDetail", e);
                #endregion
            }

            return oShipmentScheduleDetail;
        }

        public List<ShipmentScheduleDetail> Gets(int nid, Int64 nUserID)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDetailDA.Gets(nid, tc);
                oShipmentScheduleDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentScheduleDetail", e);
                #endregion
            }

            return oShipmentScheduleDetail;
        }
        public List<ShipmentScheduleDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDetailDA.Gets(tc, sSQL);
                oShipmentScheduleDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentScheduleDetail", e);
                #endregion
            }

            return oShipmentScheduleDetail;
        }

        public List<ShipmentScheduleDetail> GetsByShipmentSchedule(int nid, Int64 nUserID)
        {
            List<ShipmentScheduleDetail> oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDetailDA.GetsByShipmentSchedule(nid, tc);
                oShipmentScheduleDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentScheduleDetail", e);
                #endregion
            }

            return oShipmentScheduleDetails;
        }

        #endregion
    }  

}
