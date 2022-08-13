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
    public class MeasurementSpecService : MarshalByRefObject, IMeasurementSpecService
    {
        #region Private functions and declaration
        private MeasurementSpec MapObject(NullHandler oReader)
        {
            MeasurementSpec oMeasurementSpec = new MeasurementSpec();
            oMeasurementSpec.MeasurementSpecID = oReader.GetInt32("MeasurementSpecID");
            oMeasurementSpec.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
            oMeasurementSpec.SampleSizeID = oReader.GetInt32("SampleSizeID");
            oMeasurementSpec.SizeClassID = oReader.GetInt32("SizeClassID");
            oMeasurementSpec.GarmentsTypeID = oReader.GetInt32("GarmentsTypeID");
            oMeasurementSpec.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oMeasurementSpec.ShownAs = oReader.GetString("ShownAs");
            oMeasurementSpec.Note = oReader.GetString("Note");
            oMeasurementSpec.GarmentsTypeName = oReader.GetString("GarmentsTypeName");
            oMeasurementSpec.SampleSizeName = oReader.GetString("SampleSizeName");
            oMeasurementSpec.UnitName = oReader.GetString("UnitName");
            oMeasurementSpec.SizeClassName = oReader.GetString("SizeClassName");
            return oMeasurementSpec;
        }

        private MeasurementSpec CreateObject(NullHandler oReader)
        {
            MeasurementSpec oMeasurementSpec = new MeasurementSpec();
            oMeasurementSpec = MapObject(oReader);
            return oMeasurementSpec;
        }

        private List<MeasurementSpec> CreateObjects(IDataReader oReader)
        {
            List<MeasurementSpec> oMeasurementSpec = new List<MeasurementSpec>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                MeasurementSpec oItem = CreateObject(oHandler);
                oMeasurementSpec.Add(oItem);
            }
            return oMeasurementSpec;
        }

        #endregion

        #region Interface implementation
        public MeasurementSpecService() { }

        public MeasurementSpec Save(MeasurementSpec oMeasurementSpec, Int64 nUserID)
        {
            List<MeasurementSpecDetail> oMeasurementSpecDetails = new List<MeasurementSpecDetail>();
            MeasurementSpecDetail oMeasurementSpecDetail = new MeasurementSpecDetail();
            oMeasurementSpecDetails = oMeasurementSpec.MeasurementSpecDetails;
            
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                #region Measerment Spec Part
                IDataReader reader;
                if (oMeasurementSpec.MeasurementSpecID <= 0)
                {
                    reader = MeasurementSpecDA.InsertUpdate(tc, oMeasurementSpec, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = MeasurementSpecDA.InsertUpdate(tc, oMeasurementSpec, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oMeasurementSpec = new MeasurementSpec();
                    oMeasurementSpec = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                #region Measerument Spec Detail Part
                oMeasurementSpecDetail = new MeasurementSpecDetail();
                oMeasurementSpecDetail.MeasurementSpecID = oMeasurementSpec.MeasurementSpecID;
                MeasurementSpecDetailDA.Delete(tc, oMeasurementSpecDetail, EnumDBOperation.Delete, nUserID);
                if (oMeasurementSpecDetails!=null)
                {
                    foreach (MeasurementSpecDetail oItem in oMeasurementSpecDetails)
                    {
                        oItem.MeasurementSpecID = oMeasurementSpec.MeasurementSpecID;
                        reader = MeasurementSpecDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID);
                        NullHandler oReaderDetail = new NullHandler(reader);
                        if (reader.Read())
                        {
                        }
                        reader.Close();
                    }
                }
              
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save MeasurementSpec. Because of " + e.Message, e);
                #endregion
            }
            return oMeasurementSpec;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                MeasurementSpec oMeasurementSpec = new MeasurementSpec();
                oMeasurementSpec.MeasurementSpecID = id;
                MeasurementSpecDA.Delete(tc, oMeasurementSpec, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete MeasurementSpec. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public MeasurementSpec Get(int id, Int64 nUserId)
        {
            MeasurementSpec oAccountHead = new MeasurementSpec();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = MeasurementSpecDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get MeasurementSpec", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<MeasurementSpec> Gets(Int64 nUserID)
        {
            List<MeasurementSpec> oMeasurementSpec = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = MeasurementSpecDA.Gets(tc);
                oMeasurementSpec = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get MeasurementSpec", e);
                #endregion
            }

            return oMeasurementSpec;
        }

        public List<MeasurementSpec> Gets_Report(int id, Int64 nUserID)
        {
            List<MeasurementSpec> oMeasurementSpec = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = MeasurementSpecDA.Gets_Report(tc, id);
                oMeasurementSpec = CreateObjects(reader);
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

            return oMeasurementSpec;
        }
        #endregion
    }
}
