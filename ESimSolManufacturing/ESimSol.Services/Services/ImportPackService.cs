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
    public class ImportPackService : MarshalByRefObject, IImportPackService
    {
        #region Private functions and declaration
        private ImportPack MapObject(NullHandler oReader)
        {
            ImportPack oImportPack = new ImportPack();
            oImportPack.ImportPackID = oReader.GetInt32("ImportPackID");
            oImportPack.PackNo = oReader.GetString("PackNo");
            oImportPack.PackDate = oReader.GetDateTime("PackDate");
            oImportPack.ImportInvoiceID = oReader.GetInt32("ImportInvoiceID");
            //oImportPack.LoadingPortID = oReader.GetInt32("LoadingPortID");
            oImportPack.GrossWeight = oReader.GetDouble("GrossWeight");
            oImportPack.TotalPack = oReader.GetDouble("TotalPack");
            oImportPack.NetWeight = oReader.GetDouble("NetWeight");
            oImportPack.InvoiceQty = oReader.GetDouble("InvoiceQty");
            //oImportPack.DischargePortID = oReader.GetInt32("DischargePortID");
            oImportPack.PackCountBy = (EnumPackCountBy) oReader.GetInt32("PackCountBy");
            oImportPack.PackCountByInInt = oReader.GetInt32("PackCountBy");
            oImportPack.Remarks = oReader.GetString("Remarks");
            oImportPack.ImportInvoiceNo = oReader.GetString("ImportInvoiceNo");
            //oImportPack.LoadingPortName = oReader.GetString("LoadingPortName");
            oImportPack.ImportLCNo = oReader.GetString("ImportLCNo");
            oImportPack.Origin = oReader.GetString("Origin");
            oImportPack.Brand = oReader.GetString("Brand");
            oImportPack.ProductID = oReader.GetInt32("ProductID");
           
            oImportPack.MUnitID = oReader.GetInt32("MUnitID");
            oImportPack.ProductCode = oReader.GetString("ProductCode");
            oImportPack.ProductName = oReader.GetString("ProductName");
            oImportPack.MUName = oReader.GetString("MUName");
            oImportPack.LotNo = oReader.GetString("LotNo");
            oImportPack.Remarks = oReader.GetString("Remarks");
            return oImportPack;
        }

        private ImportPack CreateObject(NullHandler oReader)
        {
            ImportPack oImportPack = new ImportPack();
            oImportPack = MapObject(oReader);
            return oImportPack;
        }

        private List<ImportPack> CreateObjects(IDataReader oReader)
        {
            List<ImportPack> oImportPacks = new List<ImportPack>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportPack oItem = CreateObject(oHandler);
                oImportPacks.Add(oItem);
            }
            return oImportPacks;
        }
        #endregion

        #region Interface implementation
        public ImportPackService() { }

        #region Save Import Invoice & Import Invoice Product
        public ImportPack Save(ImportPack oImportPack, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                List<ImportPackDetail> oImportPackDetails = new List<ImportPackDetail>();
                oImportPackDetails = oImportPack.ImportPackDetails;
                string sImportPackDetailIDS = "";

                IDataReader reader;
                if (oImportPack.ImportPackID <= 0)
                {
                    reader = ImportPackDA.InsertUpdate(tc, oImportPack, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ImportPackDA.InsertUpdate(tc, oImportPack, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPack = CreateObject(oReader);
                }
                reader.Close();

                #region ImportPack Part
                oImportPack.TotalPack = 0;
                oImportPack.NetWeight = 0;
                foreach (ImportPackDetail oItem in oImportPackDetails)
                {
                    IDataReader readerdetail;
                    oItem.ImportPackID = oImportPack.ImportPackID;
                    oImportPack.TotalPack = oImportPack.TotalPack + oItem.NumberOfPack;
                    oImportPack.NetWeight = oImportPack.NetWeight + oItem.Qty;
                    //if (oItem.Qty > 0)
                    //{
                        if (oItem.ImportPackDetailID <= 0)
                        {
                            readerdetail = ImportPackDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserID, "");
                        }
                        else
                        {
                            readerdetail = ImportPackDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserID, "");
                        }
                        NullHandler oReaderDetail = new NullHandler(readerdetail);
                        if (readerdetail.Read())
                        {
                            sImportPackDetailIDS = sImportPackDetailIDS + oReaderDetail.GetString("ImportPackDetailID") + ",";
                        }
                        readerdetail.Close();
                    //}
                }
                ImportPackDetail oImportPackDetail = new ImportPackDetail();
                oImportPackDetail.ImportPackID = oImportPack.ImportPackID;
                if (sImportPackDetailIDS.Length > 0)
                {
                    sImportPackDetailIDS = sImportPackDetailIDS.Remove(sImportPackDetailIDS.Length - 1, 1);
                }
                ImportPackDetailDA.Delete(tc, oImportPackDetail, EnumDBOperation.Delete, nUserID, sImportPackDetailIDS);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportPack.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }

            return oImportPack;

        }
        public String Save_FromDO(ImportPack oImportPack, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPackDA.Save_FromDO(tc, oImportPack, EnumDBOperation.Delete, nUserID);
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
            return "";
        }
        public List<ImportPack> Gets(string sSQL, Int64 nUserID)
        {
            List<ImportPack> oImportPack = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPackDA.Gets(tc, sSQL);
                oImportPack = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPack", e);
                #endregion
            }

            return oImportPack;
        }

        #endregion

        #region Delete
        public String Delete(ImportPack oImportPack, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportPackDA.Delete(tc, oImportPack, EnumDBOperation.Delete, nUserID);
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

        #region Retrive Information
        public ImportPack Get(int nImportPackID, Int64 nUserID)
        {
            ImportPack oImportPack = new ImportPack();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPackDA.Get(nImportPackID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPack = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get ImportPack", e);
                #endregion
            }

            return oImportPack;
        }
        public ImportPack GetByInvoice(int nInvoiceID, Int64 nUserID)
        {
            ImportPack oImportPack = new ImportPack();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportPackDA.GetByInvoice(nInvoiceID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportPack = CreateObject(oReader);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPacks", e);
                #endregion
            }

            return oImportPack;
        }
        public List<ImportPack> Gets(int nInvoiceID,Int64 nUserID)
        {
            List<ImportPack> oImportPacks = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportPackDA.Gets(tc, nInvoiceID);
                oImportPacks = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ImportPacks", e);
                #endregion
            }

            return oImportPacks;
        }
        #endregion

        #endregion


    }

}
