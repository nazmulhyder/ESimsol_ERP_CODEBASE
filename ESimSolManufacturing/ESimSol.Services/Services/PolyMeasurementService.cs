
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
    public class PolyMeasurementService : MarshalByRefObject, IPolyMeasurementService
    {
        #region Private functions and declaration
        private PolyMeasurement MapObject(NullHandler oReader)
        {
            PolyMeasurement oPolyMeasurement = new PolyMeasurement();
            oPolyMeasurement.PolyMeasurementID = oReader.GetInt32("PolyMeasurementID");
            oPolyMeasurement.Measurement = oReader.GetString("Measurement");
            oPolyMeasurement.Note = oReader.GetString("Note");

            oPolyMeasurement.PolyMeasurementType = (EnumPolyMeasurementType)oReader.GetInt32("PolyMeasurementType");
            oPolyMeasurement.PolyMeasurementTypeInt = oReader.GetInt32("PolyMeasurementType");

            oPolyMeasurement.Length = oReader.GetInt32("Length");
            oPolyMeasurement.LengthUnit = oReader.GetString("LengthUnit");
            oPolyMeasurement.Width = oReader.GetInt32("Width");
            oPolyMeasurement.WidthUnit = oReader.GetString("WidthUnit");
            oPolyMeasurement.Thickness = oReader.GetInt32("Thickness");
            oPolyMeasurement.ThicknessUnit = oReader.GetString("ThicknessUnit");
            oPolyMeasurement.Flap = oReader.GetInt32("Flap");
            oPolyMeasurement.FlapUnit = oReader.GetString("FlapUnit");
            oPolyMeasurement.Lip = oReader.GetInt32("Lip");
            oPolyMeasurement.LipUnit = oReader.GetString("LipUnit");
            oPolyMeasurement.Gusset = oReader.GetInt32("Gusset");
            oPolyMeasurement.GussetUnit = oReader.GetString("GussetUnit");
            oPolyMeasurement.Gusset1 = oReader.GetInt32("Gusset1");
            oPolyMeasurement.GussetUnit1 = oReader.GetString("GussetUnit1");

            return oPolyMeasurement;
        }

        private PolyMeasurement CreateObject(NullHandler oReader)
        {
            PolyMeasurement oPolyMeasurement = new PolyMeasurement();
            oPolyMeasurement = MapObject(oReader);
            return oPolyMeasurement;
        }

        private List<PolyMeasurement> CreateObjects(IDataReader oReader)
        {
            List<PolyMeasurement> oPolyMeasurement = new List<PolyMeasurement>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                PolyMeasurement oItem = CreateObject(oHandler);
                oPolyMeasurement.Add(oItem);
            }
            return oPolyMeasurement;
        }

        #endregion

        #region Interface implementation
        public PolyMeasurementService() { }

        public PolyMeasurement Save(PolyMeasurement oPolyMeasurement, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oPolyMeasurement.PolyMeasurementID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PolyMeasurement, EnumRoleOperationType.Add);
                    reader = PolyMeasurementDA.InsertUpdate(tc, oPolyMeasurement, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.PolyMeasurement, EnumRoleOperationType.Edit);
                    reader = PolyMeasurementDA.InsertUpdate(tc, oPolyMeasurement, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPolyMeasurement = new PolyMeasurement();
                    oPolyMeasurement = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oPolyMeasurement.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oPolyMeasurement;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                PolyMeasurement oPolyMeasurement = new PolyMeasurement();
                oPolyMeasurement.PolyMeasurementID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.PolyMeasurement, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "PolyMeasurement", id);
                PolyMeasurementDA.Delete(tc, oPolyMeasurement, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public PolyMeasurement Get(int id, Int64 nUserId)
        {
            PolyMeasurement oAccountHead = new PolyMeasurement();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = PolyMeasurementDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get PolyMeasurement", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<PolyMeasurement> Gets(Int64 nUserID)
        {
            List<PolyMeasurement> oPolyMeasurement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PolyMeasurementDA.Gets(tc);
                oPolyMeasurement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PolyMeasurement", e);
                #endregion
            }

            return oPolyMeasurement;
        }

        public List<PolyMeasurement> GetsbyMeasurement(string sMeasurement, Int64 nUserID)
        {
            List<PolyMeasurement> oPolyMeasurement = new List<PolyMeasurement>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PolyMeasurementDA.GetsbyMeasurement(tc, sMeasurement);
                oPolyMeasurement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PolyMeasurement", e);
                #endregion
            }

            return oPolyMeasurement;
        }


        public List<PolyMeasurement> Gets(string sSQL, Int64 nUserID)
        {
            List<PolyMeasurement> oPolyMeasurement = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = PolyMeasurementDA.Gets(tc, sSQL);
                oPolyMeasurement = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PolyMeasurement", e);
                #endregion
            }

            return oPolyMeasurement;
        }
        #endregion
    }
}
