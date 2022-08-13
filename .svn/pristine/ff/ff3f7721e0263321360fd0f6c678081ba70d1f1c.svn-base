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
    public class MeasurementUnitConService : MarshalByRefObject, IMeasurementUnitConService
    {
        #region Private functions and declaration
        private MeasurementUnitCon MapObject(NullHandler oReader)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon.MeasurementUnitConID = oReader.GetInt32("MeasurementUnitConID");
            oMeasurementUnitCon.ToMUnitID = oReader.GetInt32("ToMUnitID");
            oMeasurementUnitCon.Value = oReader.GetDouble("Value");
            oMeasurementUnitCon.FromMUnitID = oReader.GetInt32("FromMUnitID");
            oMeasurementUnitCon.FromMUnit = oReader.GetString("FromMUnit");
            oMeasurementUnitCon.ToMUnit = oReader.GetString("ToMUnit");
            
            return oMeasurementUnitCon;
        }

        private MeasurementUnitCon CreateObject(NullHandler oReader)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            oMeasurementUnitCon = MapObject(oReader);
            return oMeasurementUnitCon;
        }

        private List<MeasurementUnitCon> CreateObjects(IDataReader oReader)
        {
            List<MeasurementUnitCon> oMeasurementUnitCons = new List<MeasurementUnitCon>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeasurementUnitCon oItem = CreateObject(oHandler);
                oMeasurementUnitCons.Add(oItem);
            }
            return oMeasurementUnitCons;
        }

        #endregion

        #region Interface implementation
        public MeasurementUnitConService() { }
        public MeasurementUnitCon Save(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserId)
        {

            TransactionContext tc = null;
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();
          
            try
            {
                oBusinessUnits = oMeasurementUnitCon.BusinessUnits;
                tc = TransactionContext.Begin(true);
                #region MeasurementUnitCon
                IDataReader reader;
                if (oMeasurementUnitCon.MeasurementUnitConID <= 0)
                {
                    reader = MeasurementUnitConDA.InsertUpdate(tc, oMeasurementUnitCon, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = MeasurementUnitConDA.InsertUpdate(tc, oMeasurementUnitCon, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = new MeasurementUnitCon();
                    oMeasurementUnitCon = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                #region
               
                if (oBusinessUnits != null)
                {
                    MeasurementUnitBU oMeasurementUnitBU = new MeasurementUnitBU();
                    oMeasurementUnitBU.MeasurementUnitConID = oMeasurementUnitCon.MeasurementUnitConID;
                    MeasurementUnitBUDA.Delete(tc, oMeasurementUnitBU, EnumDBOperation.Delete, nUserId);

                    foreach (BusinessUnit oItem in oBusinessUnits)
                    {
                        IDataReader readerMeasurementCOnWithBU;
                        oMeasurementUnitBU = new MeasurementUnitBU();
                        oMeasurementUnitBU.MeasurementUnitConID = oMeasurementUnitCon.MeasurementUnitConID;
                        oMeasurementUnitBU.BUID = oItem.BusinessUnitID;
                        readerMeasurementCOnWithBU = MeasurementUnitBUDA.InsertUpdate(tc, oMeasurementUnitBU, EnumDBOperation.Insert, nUserId);
                        NullHandler oReaderTNC = new NullHandler(readerMeasurementCOnWithBU);
                        readerMeasurementCOnWithBU.Close();
                    }
                }
                #endregion
                
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oMeasurementUnitCon = new MeasurementUnitCon();
                oMeasurementUnitCon.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMeasurementUnitCon;
        }
        public String Delete(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeasurementUnitConDA.Delete(tc, oMeasurementUnitCon, EnumDBOperation.Delete, nUserID);
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
        public MeasurementUnitCon Get(int id, Int64 nUserId)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitConDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = CreateObject(oReader);
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

            return oMeasurementUnitCon;
        }
        public MeasurementUnitCon GetByBU(int nBUID, Int64 nUserId)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitConDA.GetByBU(tc, nBUID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = CreateObject(oReader);
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

            return oMeasurementUnitCon;
        }
        public MeasurementUnitCon GetBy(int nFromMUnitID, int ToMUnitID, Int64 nUserId)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitConDA.GetBy(tc,  nFromMUnitID,  ToMUnitID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = CreateObject(oReader);
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

            return oMeasurementUnitCon;
        }
        public List<MeasurementUnitCon> Gets(Int64 nUserId)
        {
            List<MeasurementUnitCon> oMeasurementUnitCons = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementUnitConDA.Gets(tc);
                oMeasurementUnitCons = CreateObjects(reader);
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

            return oMeasurementUnitCons;
        }
        public List<MeasurementUnitCon> Gets(int nBUID,Int64 nUserId)
        {
            List<MeasurementUnitCon> oMeasurementUnitCons = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementUnitConDA.Gets(tc, nBUID);
                oMeasurementUnitCons = CreateObjects(reader);
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

            return oMeasurementUnitCons;
        }
        public MeasurementUnitCon Activate(MeasurementUnitCon oMeasurementUnitCon, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitConDA.Activate(tc, oMeasurementUnitCon);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oMeasurementUnitCon = new MeasurementUnitCon();
                oMeasurementUnitCon.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oMeasurementUnitCon;
        }
        public MeasurementUnitCon GetByMUnit(int nFromMUnitID,  Int64 nUserId)
        {
            MeasurementUnitCon oMeasurementUnitCon = new MeasurementUnitCon();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementUnitConDA.GetByMUnit(tc, nFromMUnitID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementUnitCon = CreateObject(oReader);
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

            return oMeasurementUnitCon;
        }


        #endregion
    }
}