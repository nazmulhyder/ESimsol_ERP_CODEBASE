using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.Services.Services
{
    public class ImportBLService : MarshalByRefObject, IImportBLService
    {
        #region Private functions and declaration
        private ImportBL MapObject(NullHandler oReader)
        {
            ImportBL oImportBL = new ImportBL();
            oImportBL.ImportBLID = oReader.GetInt32("ImportBLID");
            oImportBL.BLNo = oReader.GetString("BLNo");
            oImportBL.BLDate = oReader.GetDateTime("BLDate");
            oImportBL.ETA = oReader.GetDateTime("ETA");
            oImportBL.BLQuantity = oReader.GetInt32("BLQuantity");
            //oImportBL.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oImportBL.ShippingLine = oReader.GetInt32("ShippingLine");
            oImportBL.LandingPort = oReader.GetInt32("LandingPort");
            oImportBL.DestinationPort = oReader.GetInt32("DestinationPort");
            oImportBL.PlaceOfIssue = oReader.GetInt32("PlaceOfIssue");
            oImportBL.IssueDate = oReader.GetDateTime("IssueDate");
            oImportBL.ContainerCount = oReader.GetInt32("ContainerCount");
            oImportBL.ShipmentDate = oReader.GetDateTime("ShipmentDate");
            oImportBL.VesselInfo = oReader.GetString("VesselInfo");
            oImportBL.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            oImportBL.BLType = oReader.GetInt32("BLType");
            //oImportBL.InvoiceNo = oReader.GetInt32("InvoiceNo");
            //oImportBL.MeasurementUnitName = oReader.GetString("MeasurementUnitName");   

            oImportBL.ShippingLineName = oReader.GetString("ShippingLineName");
            oImportBL.LandingPortName = oReader.GetString("LandingPortName");
            oImportBL.DestinationPortName = oReader.GetString("DestinationPortName");
            return oImportBL;
        }

        private ImportBL CreateObject(NullHandler oReader)
        {
            ImportBL oImportBL = new ImportBL();
            oImportBL = MapObject(oReader);
            return oImportBL;
        }

        private List<ImportBL> CreateObjects(IDataReader oReader)
        {
            List<ImportBL> oImportBL = new List<ImportBL>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportBL oItem = CreateObject(oHandler);
                oImportBL.Add(oItem);
            }
            return oImportBL;
        }

        #endregion

        #region Interface implementation
        public ImportBLService() { }


        public ImportBL Save(ImportBL oImportBL, Int64 nUserID)
        {
           
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region  Part
                IDataReader reader;
                if (oImportBL.ImportBLID <= 0)
                {

                    reader = ImportBLDA.InsertUpdate(tc, oImportBL, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = ImportBLDA.InsertUpdate(tc, oImportBL, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportBL = new ImportBL();
                    oImportBL = CreateObject(oReader);
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

                oImportBL = new ImportBL();
                oImportBL.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportBL;
        }
        public string Delete(ImportBL oImportBL, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportBLDA.Delete(tc, oImportBL, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete RouteLocation. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ImportBL Get(int id, Int64 nUserId)
        {
            ImportBL oAccountHead = new ImportBL();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportBLDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ImportBL", e);
                #endregion
            }

            return oAccountHead;
        }
        public ImportBL GetByInvoice(int nInvoiceId, Int64 nUserId)
        {
            ImportBL oImportBL = new ImportBL();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportBLDA.GetByInvoiceID(tc, nInvoiceId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportBL = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportBL", e);
                #endregion
            }

            return oImportBL;
        }
        public List<ImportBL> Gets(Int64 nUserId)
        {
            List<ImportBL> oImportBL = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportBLDA.Gets(tc);
                oImportBL = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportBL", e);
                #endregion
            }

            return oImportBL;
        }

   
        #endregion
    }
}