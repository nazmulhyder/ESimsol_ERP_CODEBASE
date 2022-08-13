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
    public class ImportProductDetailService : MarshalByRefObject, IImportProductDetailService
    {
        #region Private functions and declaration
        private ImportProductDetail MapObject(NullHandler oReader)
        {
            ImportProductDetail oImportProductDetail = new ImportProductDetail();
            oImportProductDetail.ImportProductDetailID = oReader.GetInt32("ImportProductDetailID");
            oImportProductDetail.ImportProductID = oReader.GetInt32("ImportProductID");
            oImportProductDetail.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oImportProductDetail.ProductCategoryName = oReader.GetString("ProductCategoryName");
            return oImportProductDetail;
        }

        private ImportProductDetail CreateObject(NullHandler oReader)
        {
            ImportProductDetail oImportProductDetail = new ImportProductDetail();
            oImportProductDetail = MapObject(oReader);
            return oImportProductDetail;
        }

        private List<ImportProductDetail> CreateObjects(IDataReader oReader)
        {
            List<ImportProductDetail> oImportProductDetails = new List<ImportProductDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportProductDetail oItem = CreateObject(oHandler);
                oImportProductDetails.Add(oItem);
            }
            return oImportProductDetails;
        }

        #endregion

        #region Interface implementation
        public ImportProductDetailService() { }


  
      
        public String Delete(ImportProductDetail oImportProductDetail, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ImportProductDetailDA.Delete(tc, oImportProductDetail, EnumDBOperation.Delete, nUserID,"");
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
        public ImportProductDetail Get(int id, Int64 nUserId)
        {
            ImportProductDetail oImportProductDetail = new ImportProductDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportProductDetailDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oImportProductDetail = CreateObject(oReader);
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

            return oImportProductDetail;
        }
        public List<ImportProductDetail> Gets(int nIPID, Int64 nUserId)
        {
            List<ImportProductDetail> oImportProductDetails = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportProductDetailDA.Gets(tc, nIPID);
                oImportProductDetails = CreateObjects(reader);
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

            return oImportProductDetails;
        }

   
        #endregion
    }
}