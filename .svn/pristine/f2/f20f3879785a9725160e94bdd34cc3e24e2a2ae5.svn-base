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
    public class ImportLCDetailProductService : MarshalByRefObject, IPurchasePaymentContractDetailService
    {
        #region Private functions and declaration
        private ImportLCDetailProduct MapObject( NullHandler oReader)
        {
            ImportLCDetailProduct oImportLCDetailProduct = new ImportLCDetailProduct();
            oImportLCDetailProduct.ImportLCDetailProductID = oReader.GetInt32("ImportLCDetailProductID");
            oImportLCDetailProduct.ImportLCDetailID = oReader.GetInt32("ImportLCDetailID");
            oImportLCDetailProduct.ProductID = oReader.GetInt32("ProductID");
            oImportLCDetailProduct.Quantity = oReader.GetDouble("Quantity");
            oImportLCDetailProduct.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");           
            oImportLCDetailProduct.GrossOrNetWeight = (EnumGrossOrNetWeight)oReader.GetInt16("GrossOrNetWeight");            
            oImportLCDetailProduct.UnitPrice = oReader.GetDouble("UnitPrice");
            oImportLCDetailProduct.PackingQty = oReader.GetDouble("PackingQty");
            oImportLCDetailProduct.Note = oReader.GetString("Note");
            oImportLCDetailProduct.ProductCode = oReader.GetString("ProductCode");
            oImportLCDetailProduct.ProductName = oReader.GetString("ProductName");
            oImportLCDetailProduct.MUnitName = oReader.GetString("MUnitName");
            oImportLCDetailProduct.InvoiceQty = oReader.GetDouble("InvoiceQty");

            return oImportLCDetailProduct;
        }

        private ImportLCDetailProduct CreateObject(NullHandler oReader)
        {
            ImportLCDetailProduct oPPCD = new ImportLCDetailProduct();
            oPPCD=MapObject(oReader);
            return oPPCD;
        }

        private List<ImportLCDetailProduct> CreateObjects(IDataReader oReader)
        {
            List<ImportLCDetailProduct> oPPCDs = new List<ImportLCDetailProduct>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ImportLCDetailProduct oItem = CreateObject(oHandler);
                oPPCDs.Add(oItem);
            }
            return oPPCDs;
        }
        #endregion

        #region Interface implementation
        public ImportLCDetailProductService() { }

        #region Old Version

        //public PurchasePaymentContractDetail Save(PurchasePaymentContractDetail oPPCD, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
        //        double nAmount = 0;
        //        if (oPPCD.PPCDetailID<=0)
        //        {
        //            oPPCD.PPCDetailID= PurchasePaymentContractDetailDA.GetNewID(tc);
        //            PurchasePaymentContractDetailDA.Insert(tc, oPPCD);
        //        }
        //        else
        //        {
        //            nAmount = PurchasePaymentContractDetailDA.GetValue(tc, oPPCD.PPCDetailID);
        //            PurchasePaymentContractDetailDA.Update(tc, oPPCD);
        //            nAmount = (-1)*nAmount + oPPCD.Quantity * oPPCD.UnitPrice;
        //            PurchasePaymentContractDA.UpdateAmount(tc,nAmount, oPPCD.PPCID);

        //        }
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        ExceptionLog.Write(e);
        //        throw new ServiceException("Failed to Save PurchasePaymentContractDetail", e);
        //        #endregion
        //    }
        //    return oPPCD;
        //}
        //public bool Delete(int oID, Int64 nUserId)
        //{
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin(true);
                
        //        PurchasePaymentContractDetailDA.Delete(tc, oID);
        //        tc.End();
        //        return true;
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null)
        //            tc.HandleError();

        //        throw new ServiceException(e.Message, e);
        //        #endregion
        //    }
        //}

        #endregion 

        public ImportLCDetailProduct Get(int id, Int64 nUserId)
        {
            ImportLCDetailProduct oPPCD = new ImportLCDetailProduct();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ImportLCDetailProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oPPCD = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get PurchasePaymentContractDetail", e);
                #endregion
            }

            return oPPCD;
        }

        public List<ImportLCDetailProduct> Gets(Int64 nUserId)
        {
            List<ImportLCDetailProduct> oPPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailProductDA.Gets(tc);
                oPPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContractDetails", e);
                #endregion
            }

            return oPPCDs;
        }

        public List<ImportLCDetailProduct> Gets(int nPPCID, Int64 nUserId)
        {
            List<ImportLCDetailProduct> oPPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailProductDA.Gets(tc, nPPCID);
                oPPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContractDetails", e);
                #endregion
            }

            return oPPCDs;
        }

        public List<ImportLCDetailProduct> GetsByLCID(int nLCID, Int64 nUserId)
        {
            List<ImportLCDetailProduct> oPPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailProductDA.GetsByLCID(tc, nLCID);
                oPPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContractDetails", e);
                #endregion
            }

            return oPPCDs;
        }

        public List<ImportLCDetailProduct> GetsBySQL(string sSQL, Int64 nUserId)
        {
            List<ImportLCDetailProduct> oPPCDs = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ImportLCDetailProductDA.GetsBySQL(tc, sSQL);
                oPPCDs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get PurchasePaymentContractDetails", e);
                #endregion
            }

            return oPPCDs;
        }

        #endregion
    }
   
  
}
