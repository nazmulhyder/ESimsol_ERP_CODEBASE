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
    public class ImportProductService : MarshalByRefObject, IImportProductService
    {
        #region Private functions and declaration
        private ImportProduct MapObject(NullHandler oReader)
        {
            ImportProduct oImportProduct = new ImportProduct();
            oImportProduct.ImportProductID = oReader.GetInt32("ImportProductID");
            oImportProduct.BUID = oReader.GetInt32("BUID");
            oImportProduct.BUName = oReader.GetString("BUName");
            oImportProduct.FileName = oReader.GetString("FileName");
            oImportProduct.Name = oReader.GetString("Name");
            oImportProduct.PrintName = oReader.GetString("PrintName");
            oImportProduct.ProductType = (EnumProductNature)oReader.GetInt32("ProductType");
            oImportProduct.ProductTypeInt = oReader.GetInt32("ProductType");
            return oImportProduct;
        }

        private ImportProduct CreateObject(NullHandler oReader)
        {
            ImportProduct oImportProduct = new ImportProduct();
            oImportProduct = MapObject(oReader);
            return oImportProduct;
        }

        private List<ImportProduct> CreateObjects(IDataReader oReader)
        {
            List<ImportProduct> oImportProducts = new List<ImportProduct>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportProduct oItem = CreateObject(oHandler);
                oImportProducts.Add(oItem);
            }
            return oImportProducts;
        }

        #endregion

        #region Interface implementation
        public ImportProductService() { }


        public ImportProduct Save(ImportProduct oImportProduct, Int64 nUserId)
        {
            TransactionContext tc = null;
            string sDetailIDs = "";
            ImportProductDetail oImportProductDetail = new ImportProductDetail();
            List<ImportProductDetail> oImportProductDetails = new List<ImportProductDetail>();
            oImportProductDetails = oImportProduct.ImportProductDetails;
            try
            {
                tc = TransactionContext.Begin(true);
                #region ImportProduct
                IDataReader reader;
                if (oImportProduct.ImportProductID <= 0)
                {
                    reader = ImportProductDA.InsertUpdate(tc, oImportProduct, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ImportProductDA.InsertUpdate(tc, oImportProduct, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportProduct = new ImportProduct();
                    oImportProduct = CreateObject(oReader);
                }
                reader.Close();
                #endregion
                
                reader.Close();
                #region  Detail Part
                foreach (ImportProductDetail oItem in oImportProductDetails)
                {
                    IDataReader readerdetail;
                    oItem.ImportProductID = oImportProduct.ImportProductID;
                    if (oItem.ImportProductDetailID <= 0)
                    {
                        readerdetail = ImportProductDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Insert, nUserId, "");
                    }
                    else
                    {
                        readerdetail = ImportProductDetailDA.InsertUpdate(tc, oItem, EnumDBOperation.Update, nUserId, "");
                    }
                    NullHandler oReaderDetail = new NullHandler(readerdetail);
                    if (readerdetail.Read())
                    {
                        sDetailIDs = sDetailIDs + oReaderDetail.GetString("ImportProductDetailID") + ",";
                    }
                    readerdetail.Close();
                }
                if (sDetailIDs.Length > 0)
                {
                    sDetailIDs = sDetailIDs.Remove(sDetailIDs.Length - 1, 1);
                }
                oImportProductDetail = new ImportProductDetail();
                oImportProductDetail.ImportProductID = oImportProduct.ImportProductID;
                ImportProductDetailDA.Delete(tc, oImportProductDetail, EnumDBOperation.Delete, nUserId, sDetailIDs);
                #endregion
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oImportProduct = new ImportProduct();
                oImportProduct.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportProduct;
        }
    
        public String Delete(ImportProduct oImportProduct, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportProductDA.Delete(tc, oImportProduct, EnumDBOperation.Delete, nUserID);
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
        public ImportProduct Get(int id, Int64 nUserId)
        {
            ImportProduct oImportProduct = new ImportProduct();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportProduct = CreateObject(oReader);
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

            return oImportProduct;
        }
     
        public List<ImportProduct> GetByBU(int nBUID, Int64 nUserId)
        {
            List<ImportProduct> oImportProducts = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportProductDA.GetByBU(tc, nBUID);
                oImportProducts = CreateObjects(reader);
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

            return oImportProducts;
        }
        public List<ImportProduct> Gets(Int64 nUserId)
        {
            List<ImportProduct> oImportProducts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportProductDA.Gets(tc);
                oImportProducts = CreateObjects(reader);
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

            return oImportProducts;
        }

        public ImportProduct Activate(ImportProduct oImportProduct, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportProductDA.Activate(tc, oImportProduct);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportProduct = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oImportProduct = new ImportProduct();
                oImportProduct.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oImportProduct;
        }
    

        #endregion
    }
}