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
    public class DevelopmentRecapDetailService : MarshalByRefObject, IDevelopmentRecapDetailService
    {
        #region Private functions and declaration
        private DevelopmentRecapDetail MapObject(NullHandler oReader)
        {
            DevelopmentRecapDetail oDevelopmentRecapDetail = new DevelopmentRecapDetail();
            oDevelopmentRecapDetail.DevelopmentRecapDetailID = oReader.GetInt32("DevelopmentRecapDetailID");
            oDevelopmentRecapDetail.DevelopmentRecapID = oReader.GetInt32("DevelopmentRecapID");
            oDevelopmentRecapDetail.FactoryID = oReader.GetInt32("FactoryID");
            oDevelopmentRecapDetail.FactoryPersonID = oReader.GetInt32("FactoryPersonID");
            oDevelopmentRecapDetail.SeekingDate = oReader.GetDateTime("SeekingDate");
            oDevelopmentRecapDetail.ReceivedBy = oReader.GetInt32("ReceivedBy");
            oDevelopmentRecapDetail.UnitID = oReader.GetInt32("UnitID");
            oDevelopmentRecapDetail.SampleQty = oReader.GetDouble("SampleQty");
            oDevelopmentRecapDetail.IsRawmaterialProvide = oReader.GetBoolean("IsRawmaterialProvide");
            oDevelopmentRecapDetail.FactoryName = oReader.GetString("FactoryName");
            oDevelopmentRecapDetail.FactoryPersonName = oReader.GetString("FactoryPersonName");
            oDevelopmentRecapDetail.UnitName = oReader.GetString("UnitName");
            oDevelopmentRecapDetail.ReceivedByName = oReader.GetString("ReceivedByName");
            oDevelopmentRecapDetail.Note = oReader.GetString("Note");
            return oDevelopmentRecapDetail;
        }

        private DevelopmentRecapDetail CreateObject(NullHandler oReader)
        {
            DevelopmentRecapDetail oDevelopmentRecapDetail = new DevelopmentRecapDetail();
            oDevelopmentRecapDetail = MapObject(oReader);
            return oDevelopmentRecapDetail;
        }

        private List<DevelopmentRecapDetail> CreateObjects(IDataReader oReader)
        {
            List<DevelopmentRecapDetail> oDevelopmentRecapDetail = new List<DevelopmentRecapDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DevelopmentRecapDetail oItem = CreateObject(oHandler);
                oDevelopmentRecapDetail.Add(oItem);
            }
            return oDevelopmentRecapDetail;
        }

        #endregion

        #region Interface implementation
        public DevelopmentRecapDetailService() { }

        public DevelopmentRecapDetail Save(DevelopmentRecapDetail oDevelopmentRecapDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDevelopmentRecapDetail.DevelopmentRecapDetailID <= 0)
                {
                    reader = DevelopmentRecapDetailDA.InsertUpdate(tc, oDevelopmentRecapDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = DevelopmentRecapDetailDA.InsertUpdate(tc, oDevelopmentRecapDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDevelopmentRecapDetail = new DevelopmentRecapDetail();
                    oDevelopmentRecapDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save DevelopmentRecapDetail. Because of " + e.Message, e);
                #endregion
            }
            return oDevelopmentRecapDetail;
        }

        public string Delete(int id, Int64 nUserId, string sDevelopmentRecapDetailIDs)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                DevelopmentRecapDetail oDevelopmentRecapDetail = new DevelopmentRecapDetail();
                oDevelopmentRecapDetail.DevelopmentRecapDetailID = id;
                DevelopmentRecapDetailDA.Delete(tc, oDevelopmentRecapDetail, EnumDBOperation.Delete, nUserId, sDevelopmentRecapDetailIDs);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete DevelopmentRecapDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public DevelopmentRecapDetail Get(int id, Int64 nUserId)
        {
            DevelopmentRecapDetail oAccountHead = new DevelopmentRecapDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DevelopmentRecapDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get DevelopmentRecapDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<DevelopmentRecapDetail> Gets(Int64 nUserID)
        {
            List<DevelopmentRecapDetail> oDevelopmentRecapDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDetailDA.Gets(tc);
                oDevelopmentRecapDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapDetail", e);
                #endregion
            }

            return oDevelopmentRecapDetail;
        }

        public List<DevelopmentRecapDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<DevelopmentRecapDetail> oDevelopmentRecapDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDetailDA.Gets(tc, sSQL);
                oDevelopmentRecapDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapDetail", e);
                #endregion
            }

            return oDevelopmentRecapDetail;
        }

        public List<DevelopmentRecapDetail> Gets(int nDRID, Int64 nUserID)
        {
            List<DevelopmentRecapDetail> oDevelopmentRecapDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DevelopmentRecapDetailDA.Gets(tc, nDRID);
                oDevelopmentRecapDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DevelopmentRecapDetail", e);
                #endregion
            }

            return oDevelopmentRecapDetail;
        }

        public List<DevelopmentRecapDetail> Gets_Report(int id, Int64 nUserID)
        {
            List<DevelopmentRecapDetail> oDevelopmentRecapDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DevelopmentRecapDetailDA.Gets_Report(tc, id);
                oDevelopmentRecapDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Development Recap", e);
                #endregion
            }

            return oDevelopmentRecapDetail;
        }
        #endregion
    }
}
