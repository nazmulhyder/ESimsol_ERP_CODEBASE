using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class VProductService : MarshalByRefObject, IVProductService
    {
        #region Private functions and declaration
        private VProduct MapObject(NullHandler oReader)
        {
            VProduct oVProduct = new VProduct();
            oVProduct.VProductID = oReader.GetInt32("VProductID");
            oVProduct.ProductCode = oReader.GetString("ProductCode");
            oVProduct.ProductName = oReader.GetString("ProductName");
            oVProduct.ShortName = oReader.GetString("ShortName");
            oVProduct.BrandName = oReader.GetString("BrandName");
            oVProduct.Remarks = oReader.GetString("Remarks");
            oVProduct.NameCode = oReader.GetString("NameCode");
            return oVProduct;
        }

        private VProduct CreateObject(NullHandler oReader)
        {
            VProduct oVProduct = new VProduct();
            oVProduct = MapObject(oReader);
            return oVProduct;
        }

        private List<VProduct> CreateObjects(IDataReader oReader)
        {
            List<VProduct> oVProducts = new List<VProduct>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VProduct oItem = CreateObject(oHandler);
                oVProducts.Add(oItem);
            }
            return oVProducts;
        }

        #endregion

        #region Interface implementation
        public VProductService() { }

        public VProduct Save(VProduct oVProduct, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVProduct.VProductID <= 0)
                {
                    reader = VProductDA.InsertUpdate(tc, oVProduct, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VProductDA.InsertUpdate(tc, oVProduct, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVProduct = new VProduct();
                    oVProduct = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oVProduct = new VProduct();
                oVProduct.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oVProduct;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VProduct oVProduct = new VProduct();
                oVProduct.VProductID = id;
                VProductDA.Delete(tc, oVProduct, EnumDBOperation.Delete, nUserId);
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
        public VProduct Get(int id, Int64 nUserId)
        {
            VProduct oVProduct = new VProduct();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVProduct = CreateObject(oReader);
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

            return oVProduct;
        }
       public List<VProduct> Gets(string sSQL, Int64 nUserID)
        {
            List<VProduct> oVProducts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VProductDA.Gets(tc,sSQL);
                oVProducts = CreateObjects(reader);
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

            return oVProducts;
        }
        public List<VProduct> Gets(Int64 nUserId)
        {
            List<VProduct> oVProducts = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VProductDA.Gets(tc);
                oVProducts = CreateObjects(reader);
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

            return oVProducts;
        }

        public List<VProduct> GetsByCodeOrName(VProduct oVProduct, int nUserID)
        {
            List<VProduct> oVProducts = new List<VProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VProductDA.GetsByCodeOrName(tc, oVProduct);
                oVProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVProducts = new List<VProduct>();
                oVProduct = new VProduct();
                oVProduct.ErrorMessage = e.Message;
                oVProducts.Add(oVProduct);
                //ExceptionLog.Write(e);
                //throw new ServiceException(e.Message);
                #endregion
            }
            return oVProducts;
        }
        #endregion
    }
}