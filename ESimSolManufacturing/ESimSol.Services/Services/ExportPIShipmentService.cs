using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
    public class ExportPIShipmentService : MarshalByRefObject, IExportPIShipmentService
    {
        #region Private functions and declaration
        private ExportPIShipment MapObject(NullHandler oReader)
        {
            ExportPIShipment oExportPIShipment = new ExportPIShipment();
            oExportPIShipment.ExportPIShipmentID = oReader.GetInt32("ExportPIShipmentID");
            oExportPIShipment.ExportPIID = oReader.GetInt32("ExportPIID");
            oExportPIShipment.ExportBillID = oReader.GetInt32("ExportBillID");
            oExportPIShipment.ShipmentBy = oReader.GetString("ShipmentBy");
            oExportPIShipment.DestinationPort = oReader.GetString("DestinationPort");
            oExportPIShipment.LastUpdateBy = oReader.GetInt32("LastUpdateBy");
            oExportPIShipment.LastUpdateDateTime = Convert.ToDateTime(oReader.GetDateTime("LastUpdateDateTime"));
            oExportPIShipment.LastUpdateByName = oReader.GetString("LastUpdateByName");
            return oExportPIShipment;
        }
        private ExportPIShipment CreateObject(NullHandler oReader)
        {
            ExportPIShipment oExportPIShipment = new ExportPIShipment();
            oExportPIShipment = MapObject(oReader);
            return oExportPIShipment;
        }

        private List<ExportPIShipment> CreateObjects(IDataReader oReader)
        {
            List<ExportPIShipment> oExportPIShipment = new List<ExportPIShipment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ExportPIShipment oItem = CreateObject(oHandler);
                oExportPIShipment.Add(oItem);
            }
            return oExportPIShipment;
        }
        #endregion
        #region Interface implementation
        public ExportPIShipment Save(ExportPIShipment oExportPIShipment, Int64 nUserID)
        {
            string sRefChildIDs = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oExportPIShipment.ExportPIShipmentID <= 0)
                {

                    reader = ExportPIShipmentDA.InsertUpdate(tc, oExportPIShipment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ExportPIShipmentDA.InsertUpdate(tc, oExportPIShipment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIShipment = new ExportPIShipment();
                    oExportPIShipment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oExportPIShipment = new ExportPIShipment();
                    oExportPIShipment.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oExportPIShipment;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ExportPIShipment oExportPIShipment = new ExportPIShipment();
                oExportPIShipment.ExportPIShipmentID = id;
                DBTableReferenceDA.HasReference(tc, "ExportPIShipment", id);
                ExportPIShipmentDA.Delete(tc, oExportPIShipment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public ExportPIShipment Get(int id, Int64 nUserId)
        {
            ExportPIShipment oExportPIShipment = new ExportPIShipment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportPIShipmentDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIShipment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPIShipment", e);
                #endregion
            }
            return oExportPIShipment;
        }
        public ExportPIShipment GetByExportPIID(int ExportPIID, Int64 nUserId)
        {
            ExportPIShipment oExportPIShipment = new ExportPIShipment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = ExportPIShipmentDA.GetByExportPIID(tc, ExportPIID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oExportPIShipment = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ExportPIShipment", e);
                #endregion
            }
            return oExportPIShipment;
        }
        public List<ExportPIShipment> Gets(Int64 nUserID)
        {
            List<ExportPIShipment> oExportPIShipments = new List<ExportPIShipment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIShipmentDA.Gets(tc);
                oExportPIShipments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExportPIShipment oExportPIShipment = new ExportPIShipment();
                oExportPIShipment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oExportPIShipments;
        }
        public List<ExportPIShipment> Gets(string sSQL, Int64 nUserID)
        {
            List<ExportPIShipment> oExportPIShipments = new List<ExportPIShipment>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ExportPIShipmentDA.Gets(tc, sSQL);
                oExportPIShipments = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ExportPIShipment", e);
                #endregion
            }
            return oExportPIShipments;
        }

        #endregion
    }
}
