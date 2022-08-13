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
    public class ProductSortService : MarshalByRefObject, IProductSortService
    {
        #region Private functions and declaration
        private ProductSort MapObject(NullHandler oReader)
        {
            ProductSort oProductSort = new ProductSort();
            oProductSort.ProductSortID = oReader.GetInt32("ProductSortID");
            oProductSort.ProductID = oReader.GetInt32("ProductID");
            oProductSort.ProductID_Bulk = oReader.GetInt32("ProductID_Bulk");
            oProductSort.SortType = oReader.GetInt32("SortType");
            oProductSort.Qty_Grace = oReader.GetInt32("Qty_Grace");
            oProductSort.ProductNameBulk = oReader.GetString("ProductNameBulk");
            oProductSort.ProductName = oReader.GetString("ProductName");
            oProductSort.Value = oReader.GetDouble("Value");
            
            return oProductSort;
        }

        private ProductSort CreateObject(NullHandler oReader)
        {
            ProductSort oProductSort = new ProductSort();
            oProductSort = MapObject(oReader);
            return oProductSort;
        }

        private List<ProductSort> CreateObjects(IDataReader oReader)
        {
            List<ProductSort> oProductSorts = new List<ProductSort>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductSort oItem = CreateObject(oHandler);
                oProductSorts.Add(oItem);
            }
            return oProductSorts;
        }

        #endregion

        #region Interface implementation
        public ProductSortService() { }

      
        public ProductSort Save(ProductSort oProductSort, Int64 nUserId)
        {
            List<DUDyeingTypeMapping> oDUDyeingTypeMappings=new List<DUDyeingTypeMapping>();
            TransactionContext tc = null;
            oDUDyeingTypeMappings = oProductSort.DUDyeingTypeMappings;
          
            try
            {
                tc = TransactionContext.Begin(true);
                #region ProductSort
                IDataReader reader;
                if (oProductSort.ProductSortID <= 0)
                {
                    reader = ProductSortDA.InsertUpdate(tc, oProductSort, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ProductSortDA.InsertUpdate(tc, oProductSort, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSort = new ProductSort();
                    oProductSort = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                #region oDUDyeingTypeMappings
                if (oDUDyeingTypeMappings != null)
                {
                    foreach (DUDyeingTypeMapping oItem in oDUDyeingTypeMappings)
                    {
                      
                            IDataReader readertnc;
                            //oItem.ExportSCID = oExportSC.ExportSCID;
                            if (oItem.DyeingTypeMappingID <= 0)
                            {
                                readertnc = DUDyeingTypeMappingDA.IUD(tc, oItem, (int)EnumDBOperation.Insert, nUserId);
                            }
                            else
                            {
                                readertnc = DUDyeingTypeMappingDA.IUD(tc, oItem, (int)EnumDBOperation.Update, nUserId);
                            }
                            //NullHandler oReaderTNC = new NullHandler(readertnc);
                            //if (readertnc.Read())
                            //{
                            //    sExportSCDetaillIDs = sExportSCDetaillIDs + oReaderTNC.GetString("ExportSCDetailID") + ",";
                            //}
                            readertnc.Close();
                      
                    }
                    //if (sExportSCDetaillIDs.Length > 0)
                    //{
                    //    sExportSCDetaillIDs = sExportSCDetaillIDs.Remove(sExportSCDetaillIDs.Length - 1, 1);
                    //}
                    //ExportSCDetail oExportSCDetail = new ExportSCDetail();
                    //oExportSCDetail.ExportSCID = oExportSC.ExportSCID;
                    //ExportSCDetailDA.Delete(tc, oExportSCDetail, EnumDBOperation.Delete, nUserID, sExportSCDetaillIDs);
                    //sExportSCDetaillIDs = "";
                }
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductSort = new ProductSort();
                oProductSort.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oProductSort;
        }
      
        public String Delete(ProductSort oProductSort, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductSortDA.Delete(tc, oProductSort, EnumDBOperation.Delete, nUserID);
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
        public ProductSort Get(int id, Int64 nUserId)
        {
            ProductSort oProductSort = new ProductSort();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductSortDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSort = CreateObject(oReader);
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

            return oProductSort;
        }

        public ProductSort GetBy(int id, Int64 nUserId)
        {
            ProductSort oProductSort = new ProductSort();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductSortDA.GetBy(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductSort = CreateObject(oReader);
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

            return oProductSort;
        }
    

        public List<ProductSort> Gets(Int64 nUserId)
        {
            List<ProductSort> oProductSorts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductSortDA.Gets(tc);
                oProductSorts = CreateObjects(reader);
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

            return oProductSorts;
        }
        public List<ProductSort> GetsBy(int nProductID_Bulk,Int64 nUserId)
        {
            List<ProductSort> oProductSorts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductSortDA.GetsBy(tc, nProductID_Bulk);
                oProductSorts = CreateObjects(reader);
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

            return oProductSorts;
        }
     
    
    

        #endregion
    }
}