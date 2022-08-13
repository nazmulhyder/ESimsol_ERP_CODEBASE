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
    public class ShipmentScheduleService : MarshalByRefObject, IShipmentScheduleService
    {
        #region Private functions and declaration
        private ShipmentSchedule MapObject(NullHandler oReader)
        {
            ShipmentSchedule oShipmentSchedule = new ShipmentSchedule();
            oShipmentSchedule.ShipmentScheduleID = oReader.GetInt32("ShipmentScheduleID");
            oShipmentSchedule.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oShipmentSchedule.CountryID = oReader.GetInt32("CountryID");
            oShipmentSchedule.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oShipmentSchedule.CutOffType = (EnumCutOffType)oReader.GetInt32("CutOffType");
            oShipmentSchedule.ShipmentMode = (EnumShipmentBy)oReader.GetInt32("ShipmentMode");
            oShipmentSchedule.CutOffDate = oReader.GetDateTime("CutOffDate");
            oShipmentSchedule.CutOffWeek = oReader.GetInt32("CutOffWeek");
            oShipmentSchedule.TotalQty = oReader.GetDouble("TotalQty");
            oShipmentSchedule.OrderQty = oReader.GetDouble("OrderQty");
            oShipmentSchedule.YetToScheduleQty = oReader.GetDouble("YetToScheduleQty");
            oShipmentSchedule.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oShipmentSchedule.Remarks = oReader.GetString("Remarks");
            oShipmentSchedule.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oShipmentSchedule.BuyerName = oReader.GetString("BuyerName");
            oShipmentSchedule.ProductName = oReader.GetString("ProductName");
            oShipmentSchedule.CountryCode = oReader.GetString("CountryCode");
            oShipmentSchedule.CountryName = oReader.GetString("CountryName");
            oShipmentSchedule.CountryShortName = oReader.GetString("CountryShortName");
            oShipmentSchedule.UnitName = oReader.GetString("UnitName");
            oShipmentSchedule.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            
            return oShipmentSchedule;
        }

        private ShipmentSchedule CreateObject(NullHandler oReader)
        {
            ShipmentSchedule oShipmentSchedule = new ShipmentSchedule();
            oShipmentSchedule = MapObject(oReader);
            return oShipmentSchedule;
        }

        private List<ShipmentSchedule> CreateObjects(IDataReader oReader)
        {
            List<ShipmentSchedule> oShipmentSchedules = new List<ShipmentSchedule>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ShipmentSchedule oItem = CreateObject(oHandler);
                oShipmentSchedules.Add(oItem);
            }
            return oShipmentSchedules;
        }

        #endregion

        #region Interface implementation
        public ShipmentScheduleService() { }


        public ShipmentSchedule Save(ShipmentSchedule oShipmentSchedule, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                List<ShipmentScheduleDetail> oShipmentScheduleDetails = new List<ShipmentScheduleDetail>();
                oShipmentScheduleDetails = oShipmentSchedule.ShipmentScheduleDetails;
                string ShipmentScheduleDetailIDs = "";
                ShipmentScheduleDetail oShipmentScheduleDetail = new ShipmentScheduleDetail();
                tc = TransactionContext.Begin(true);
                #region ShipmentSchedule
                IDataReader reader;
                if (oShipmentSchedule.ShipmentScheduleID <= 0)
                {
                    reader = ShipmentScheduleDA.InsertUpdate(tc, oShipmentSchedule, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ShipmentScheduleDA.InsertUpdate(tc, oShipmentSchedule, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentSchedule = new ShipmentSchedule();
                    oShipmentSchedule = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region  Shcedule Detail Part
                foreach (ShipmentScheduleDetail oItem in oShipmentScheduleDetails)
                {
                    IDataReader readerShipmentScheduleDetail;
                    oItem.ShipmentScheduleID = oShipmentSchedule.ShipmentScheduleID;
                    if (oItem.ShipmentScheduleDetailID <= 0)
                    {
                        readerShipmentScheduleDetail = ShipmentScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId,"");
                    }
                    else
                    {
                        readerShipmentScheduleDetail = ShipmentScheduleDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId,"");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerShipmentScheduleDetail);
                    if (readerShipmentScheduleDetail.Read())
                    {
                        ShipmentScheduleDetailIDs = ShipmentScheduleDetailIDs + oReaderDetail.GetString("ShipmentScheduleDetailID") + ",";
                    }
                    readerShipmentScheduleDetail.Close();
                }
                if (ShipmentScheduleDetailIDs.Length > 0)
                {
                    ShipmentScheduleDetailIDs = ShipmentScheduleDetailIDs.Remove(ShipmentScheduleDetailIDs.Length - 1, 1);
                }
                oShipmentScheduleDetail = new ShipmentScheduleDetail();
                oShipmentScheduleDetail.ShipmentScheduleID = oShipmentSchedule.ShipmentScheduleID;                
                ShipmentScheduleDetailDA.Delete(tc, oShipmentScheduleDetail, EnumDBOperation.Delete, nUserId, ShipmentScheduleDetailIDs);
                #endregion

                #region Schedule Get
                reader = ShipmentScheduleDA.Get(tc, oShipmentSchedule.ShipmentScheduleID);
                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentSchedule = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oShipmentSchedule = new ShipmentSchedule();
                oShipmentSchedule.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oShipmentSchedule;
        }

   
        public String Delete(int id, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                ShipmentSchedule oShipmentSchedule = new ShipmentSchedule();
                oShipmentSchedule.ShipmentScheduleID = id;
                tc = TransactionContext.Begin(true);
                ShipmentScheduleDA.Delete(tc, oShipmentSchedule, EnumDBOperation.Delete, nUserID);
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
        public ShipmentSchedule Get(int id, Int64 nUserId)
        {
            ShipmentSchedule oShipmentSchedule = new ShipmentSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShipmentScheduleDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oShipmentSchedule;
        }

        public List<ShipmentSchedule> Gets(string sSQL, Int64 nUserID)
        {
            List<ShipmentSchedule> oShipmentSchedule = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDA.Gets(sSQL, tc);
                oShipmentSchedule = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ShipmentSchedule", e);
                #endregion
            }
            return oShipmentSchedule;
        }
        public ShipmentSchedule GetByType(int nRequisitionType, Int64 nUserId)
        {
            ShipmentSchedule oShipmentSchedule = new ShipmentSchedule();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ShipmentScheduleDA.GetByType(tc, nRequisitionType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oShipmentSchedule = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oShipmentSchedule;
        }


        public List<ShipmentSchedule> Gets(Int64 nUserId)
        {
            List<ShipmentSchedule> oShipmentSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDA.Gets(tc);
                oShipmentSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oShipmentSchedules;
        }
        public List<ShipmentSchedule> Gets(int nORID, Int64 nUserId)
        {
            List<ShipmentSchedule> oShipmentSchedules = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ShipmentScheduleDA.Gets(tc, nORID);
                oShipmentSchedules = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oShipmentSchedules;
        }

        #endregion
    }
}