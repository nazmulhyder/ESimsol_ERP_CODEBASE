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
    public class YarnRequisitionProductService : MarshalByRefObject, IYarnRequisitionProductService
    {
        #region Private functions and declaration

        private YarnRequisitionProduct MapObject(NullHandler oReader)
        {
            YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
            oYarnRequisitionProduct.YarnRequisitionProductID = oReader.GetInt32("YarnRequisitionProductID");
            oYarnRequisitionProduct.YarnRequisitionID = oReader.GetInt32("YarnRequisitionID");
            oYarnRequisitionProduct.YarnID = oReader.GetInt32("YarnID");
            oYarnRequisitionProduct.YarnCount = oReader.GetString("YarnCount");
            oYarnRequisitionProduct.RequisitionQty = oReader.GetDouble("RequisitionQty");
            oYarnRequisitionProduct.MUnitID = oReader.GetInt32("MUnitID");
            oYarnRequisitionProduct.YarnCode = oReader.GetString("YarnCode");
            oYarnRequisitionProduct.YarnName = oReader.GetString("YarnName");
            oYarnRequisitionProduct.MUSymbol = oReader.GetString("MUSymbol");
            return oYarnRequisitionProduct;
        }

        private YarnRequisitionProduct CreateObject(NullHandler oReader)
        {
            YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
            oYarnRequisitionProduct = MapObject(oReader);
            return oYarnRequisitionProduct;
        }

        private List<YarnRequisitionProduct> CreateObjects(IDataReader oReader)
        {
            List<YarnRequisitionProduct> oYarnRequisitionProduct = new List<YarnRequisitionProduct>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                YarnRequisitionProduct oItem = CreateObject(oHandler);
                oYarnRequisitionProduct.Add(oItem);
            }
            return oYarnRequisitionProduct;
        }

        #endregion

        #region Interface implementation
        public YarnRequisitionProduct Save(YarnRequisitionProduct oYarnRequisitionProduct, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);

                #region YarnRequisitionProduct
                IDataReader reader;
                if (oYarnRequisitionProduct.YarnRequisitionProductID <= 0)
                {
                    reader = YarnRequisitionProductDA.InsertUpdate(tc, oYarnRequisitionProduct, EnumDBOperation.Insert, "", nUserID);
                }
                else
                {
                    reader = YarnRequisitionProductDA.InsertUpdate(tc, oYarnRequisitionProduct, EnumDBOperation.Update, "", nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisitionProduct = new YarnRequisitionProduct();
                    oYarnRequisitionProduct = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oYarnRequisitionProduct = new YarnRequisitionProduct();
                    oYarnRequisitionProduct.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oYarnRequisitionProduct;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
                oYarnRequisitionProduct.YarnRequisitionProductID = id;
                DBTableReferenceDA.HasReference(tc, "YarnRequisitionProduct", id);
                YarnRequisitionProductDA.Delete(tc, oYarnRequisitionProduct, EnumDBOperation.Delete, "", nUserId);
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

        public YarnRequisitionProduct Get(int id, Int64 nUserId)
        {
            YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = YarnRequisitionProductDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oYarnRequisitionProduct = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get YarnRequisitionProduct", e);
                #endregion
            }
            return oYarnRequisitionProduct;
        }

        public List<YarnRequisitionProduct> Gets(Int64 nUserID)
        {
            List<YarnRequisitionProduct> oYarnRequisitionProducts = new List<YarnRequisitionProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionProductDA.Gets(tc);
                oYarnRequisitionProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                YarnRequisitionProduct oYarnRequisitionProduct = new YarnRequisitionProduct();
                oYarnRequisitionProduct.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oYarnRequisitionProducts;
        }

        public List<YarnRequisitionProduct> Gets(string sSQL, Int64 nUserID)
        {
            List<YarnRequisitionProduct> oYarnRequisitionProducts = new List<YarnRequisitionProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionProductDA.Gets(tc, sSQL);
                oYarnRequisitionProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get YarnRequisitionProduct", e);
                #endregion
            }
            return oYarnRequisitionProducts;
        }

        public List<YarnRequisitionProduct> Gets(int nYarnRequisitionID, Int64 nUserID)
        {
            List<YarnRequisitionProduct> oYarnRequisitionProducts = new List<YarnRequisitionProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = YarnRequisitionProductDA.Gets(tc, nYarnRequisitionID);
                oYarnRequisitionProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get YarnRequisitionProduct", e);
                #endregion
            }
            return oYarnRequisitionProducts;
        }
        #endregion
    }

}
