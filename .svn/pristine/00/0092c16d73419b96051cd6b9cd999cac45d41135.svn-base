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
    [Serializable]
    public class ImportPIGRNDetailService : MarshalByRefObject, IImportPIGRNDetailService
    {
        #region Private functions and declaration
        private ImportPIGRNDetail MapObject(NullHandler oReader)
        {
            ImportPIGRNDetail oPCD = new ImportPIGRNDetail();
            oPCD.ImportPIGRNDetailID = oReader.GetInt32("ImportPIGRNDetailID");
            oPCD.ImportPIID = oReader.GetInt32("ImportPIID");
            oPCD.ProductID = oReader.GetInt32("ProductID");
            oPCD.MUnitID = oReader.GetInt32("MUnitID");
            oPCD.UnitPrice = oReader.GetDouble("UnitPrice");
            oPCD.Qty = oReader.GetDouble("Qty");
            oPCD.Note = oReader.GetString("Note");
            oPCD.ProductCode = oReader.GetString("ProductCode");
            oPCD.ProductName = oReader.GetString("ProductName");
            oPCD.MUName = oReader.GetString("MUName");
            oPCD.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oPCD.SmallUnitValue = oReader.GetDouble("SmallUnitValue");
            oPCD.InvoiceQty = oReader.GetDouble("InvoiceQty");
            return oPCD;
        }

        private ImportPIGRNDetail CreateObject(NullHandler oReader)
        {
            ImportPIGRNDetail oPCD = new ImportPIGRNDetail();
            oPCD=MapObject( oReader);
            return oPCD;
        }

        private List<ImportPIGRNDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportPIGRNDetail> oPCDs = new List<ImportPIGRNDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPIGRNDetail oItem = CreateObject(oHandler);
                oPCDs.Add(oItem);
            }
            return oPCDs;
        }
        #endregion

        #region Interface implementation
        public ImportPIGRNDetail Get(int nImportInvoiceDetailID, Int64 nUserId)
        {
            ImportPIGRNDetail oPCD = new ImportPIGRNDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPIGRNDetailDA.Get(tc, nImportInvoiceDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPCD = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPIGRNDetail", e);
                #endregion
            }

            return oPCD;
        }
        public List<ImportPIGRNDetail> Gets(Int64 nUserId)
        {
            List<ImportPIGRNDetail> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIGRNDetailDA.Gets(tc);
                oPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProformaInvoices", e);
                #endregion
            }
            return oPCDs;
        }
        public List<ImportPIGRNDetail> Gets(int nImportPIGRNID, Int64 nUserId)
        {
            List<ImportPIGRNDetail> oPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIGRNDetailDA.Gets(tc, nImportPIGRNID);
                oPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Proforma Invoices", e);
                #endregion
            }
            return oPCDs;
        }
        public List<ImportPIGRNDetail> GetsByImportPIGRNID(int nImportPIGRNId, Int64 nUserId)
        {
            List<ImportPIGRNDetail> oImportPIGRNDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIGRNDetailDA.GetsByImportPIGRNID(tc, nImportPIGRNId);
                oImportPIGRNDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIGRNDetail", e);
                #endregion
            }

            return oImportPIGRNDetail;
        }
        public List<ImportPIGRNDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<ImportPIGRNDetail> oImportPIGRNDetail = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPIGRNDetailDA.Gets(tc, sSQL);
                oImportPIGRNDetail = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPIGRNDetail", e);
                #endregion
            }

            return oImportPIGRNDetail;
        }
        public List<ImportPIGRNDetail> SaveImportPIGRNDetail(ImportPIGRNDetail oImportPIGRNDetail, Int64 nUserId)
        {
            List<ImportPIGRNDetail> oImportPIGRNDetails = new List<ImportPIGRNDetail> ();
            oImportPIGRNDetails = oImportPIGRNDetail.ImportPIGRNDetails;
            string sImportPIGRNDetailIDs = "";
            ImportPIGRNDetail oTempImportPIGRNDetail = new ImportPIGRNDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                if(oImportPIGRNDetails.Count>0)
                {
                    foreach(ImportPIGRNDetail oItem in oImportPIGRNDetails)
                    {
                        IDataReader reader = null;
                        if(oItem.ImportPIGRNDetailID<=0)
                        {
                            reader = ImportPIGRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId);
                        }
                        else
                        {
                            reader = ImportPIGRNDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId);
                        }
                        NullHandler oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oTempImportPIGRNDetail = CreateObject(oReader);
                        }
                        sImportPIGRNDetailIDs += oTempImportPIGRNDetail.ImportPIGRNDetailID + ",";
                        reader.Close();
                    }
                    sImportPIGRNDetailIDs = sImportPIGRNDetailIDs.Substring(0, sImportPIGRNDetailIDs.Length - 1);
                    oTempImportPIGRNDetail = new ImportPIGRNDetail();
                    oTempImportPIGRNDetail.ImportPIID = oImportPIGRNDetail.ImportPIID;
                    ImportPIGRNDetailDA.Delete(tc, oTempImportPIGRNDetail, EnumDBOperation.Delete, nUserId,sImportPIGRNDetailIDs);
                }
                IDataReader readers = null;
                readers = ImportPIGRNDetailDA.GetsByImportPIGRNID(tc, oImportPIGRNDetail.ImportPIID);
                oImportPIGRNDetails = CreateObjects(readers);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Import PI GRN Detail", e);
                #endregion
            }

            return oImportPIGRNDetails;
        }
        public String Delete(ImportPIGRNDetail oImportPIGRNDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPIGRNDetailDA.Delete(tc, oImportPIGRNDetail, EnumDBOperation.Delete, nUserID,"");
                tc.End();

            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                #endregion
            }
            return Global.DeleteMessage;
        }
        #endregion
    }
}
