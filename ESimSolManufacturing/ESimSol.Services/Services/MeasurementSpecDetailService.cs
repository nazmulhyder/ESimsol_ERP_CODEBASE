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
    public class MeasurementSpecDetailService : MarshalByRefObject, IMeasurementSpecDetailService
    {
        #region Private functions and declaration
        private MeasurementSpecDetail MapObject(NullHandler oReader)
        {
            MeasurementSpecDetail oMeasurementSpecDetail = new MeasurementSpecDetail();
            oMeasurementSpecDetail.MeasurementSpecDetailID = oReader.GetInt32("MeasurementSpecDetailID");
            oMeasurementSpecDetail.MeasurementSpecID = oReader.GetInt32("MeasurementSpecID");
            oMeasurementSpecDetail.POM = oReader.GetString("POM");
            oMeasurementSpecDetail.DescriptionNote = oReader.GetString("DescriptionNote");
            oMeasurementSpecDetail.Addition = oReader.GetDouble("Addition");
            oMeasurementSpecDetail.Deduction = oReader.GetDouble("Deduction");
            oMeasurementSpecDetail.SizeID = oReader.GetInt32("SizeID");
            oMeasurementSpecDetail.SizeValue = oReader.GetDouble("SizeValue");
            oMeasurementSpecDetail.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oMeasurementSpecDetail.Sequence = oReader.GetInt32("Sequence");
            return oMeasurementSpecDetail;
        }

        private MeasurementSpecDetail CreateObject(NullHandler oReader)
        {
            MeasurementSpecDetail oMeasurementSpecDetail = new MeasurementSpecDetail();
            oMeasurementSpecDetail = MapObject(oReader);
            return oMeasurementSpecDetail;
        }

        private List<MeasurementSpecDetail> CreateObjects(IDataReader oReader)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetail = new List<MeasurementSpecDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeasurementSpecDetail oItem = CreateObject(oHandler);
                oMeasurementSpecDetail.Add(oItem);
            }
            return oMeasurementSpecDetail;
        }

        #endregion

        #region Interface implementation
        public MeasurementSpecDetailService() { }

        public MeasurementSpecDetail Save(MeasurementSpecDetail oMeasurementSpecDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oMeasurementSpecDetail.MeasurementSpecDetailID <= 0)
                {
                    reader = MeasurementSpecDetailDA.InsertUpdate(tc, oMeasurementSpecDetail, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MeasurementSpecDetailDA.InsertUpdate(tc, oMeasurementSpecDetail, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementSpecDetail = new MeasurementSpecDetail();
                    oMeasurementSpecDetail = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save MeasurementSpecDetail. Because of " + e.Message, e);
                #endregion
            }
            return oMeasurementSpecDetail;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeasurementSpecDetail oMeasurementSpecDetail = new MeasurementSpecDetail();
                oMeasurementSpecDetail.MeasurementSpecDetailID = id;
                MeasurementSpecDetailDA.Delete(tc, oMeasurementSpecDetail, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete MeasurementSpecDetail. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public MeasurementSpecDetail Get(int id, Int64 nUserId)
        {
            MeasurementSpecDetail oAccountHead = new MeasurementSpecDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementSpecDetailDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MeasurementSpecDetail", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MeasurementSpecDetail> Gets(Int64 nUserID)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecDetailDA.Gets(tc);
                oMeasurementSpecDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementSpecDetail", e);
                #endregion
            }

            return oMeasurementSpecDetail;
        }

        public List<MeasurementSpecDetail> Gets(int nMSID, Int64 nUserID)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecDetailDA.Gets(tc, nMSID);
                oMeasurementSpecDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementSpecDetail", e);
                #endregion
            }

            return oMeasurementSpecDetail;
        }

        public List<MeasurementSpecDetail> GetsByTechnicalSheet(int nTSID, Int64 nUserID)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecDetailDA.GetsByTechnicalSheet(tc, nTSID);
                oMeasurementSpecDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementSpecDetail", e);
                #endregion
            }

            return oMeasurementSpecDetail;
        }

        public List<MeasurementSpecDetail> Gets_Report(int id, Int64 nUserID)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MeasurementSpecDetailDA.Gets_Report(tc, id);
                oMeasurementSpecDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get TechnicalSheet", e);
                #endregion
            }

            return oMeasurementSpecDetail;
        }

        #endregion
    }
}
