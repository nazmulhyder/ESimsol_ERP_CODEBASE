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
    public class ContainingProductService : MarshalByRefObject, IContainingProductService
    {
        #region Private functions and declaration
        private ContainingProduct MapObject(NullHandler oReader)
        {
            ContainingProduct oContainingProduct = new ContainingProduct();
            oContainingProduct.ContainingProductID = oReader.GetInt32("ContainingProductID");
            oContainingProduct.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oContainingProduct.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oContainingProduct.Remarks = oReader.GetString("Remarks");
            oContainingProduct.LocationName = oReader.GetString("LocationName");
            oContainingProduct.OperationUnitName = oReader.GetString("OperationUnitName");
            oContainingProduct.WorkingUnitCode = oReader.GetString("WorkingUnitCode");
            oContainingProduct.ProductCategoryName = oReader.GetString("ProductCategoryName");
            return oContainingProduct;
        }

        private ContainingProduct CreateObject(NullHandler oReader)
        {
            ContainingProduct oContainingProduct = new ContainingProduct();
            oContainingProduct = MapObject(oReader);
            return oContainingProduct;
        }

        private List<ContainingProduct> CreateObjects(IDataReader oReader)
        {
            List<ContainingProduct> oContainingProduct = new List<ContainingProduct>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ContainingProduct oItem = CreateObject(oHandler);
                oContainingProduct.Add(oItem);
            }
            return oContainingProduct;
        }

        #endregion

        #region Interface implementation
        public ContainingProductService() { }

        public ContainingProduct Save(ContainingProduct oContainingProduct, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oContainingProduct.ContainingProductID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ContainingProduct, EnumRoleOperationType.Add);
                    reader = ContainingProductDA.InsertUpdate(tc, oContainingProduct, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ContainingProduct, EnumRoleOperationType.Edit);
                    reader = ContainingProductDA.InsertUpdate(tc, oContainingProduct, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oContainingProduct = new ContainingProduct();
                    oContainingProduct = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContainingProduct = new ContainingProduct();
                oContainingProduct.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ContainingProduct. Because of " + e.Message, e);
                #endregion
            }
            return oContainingProduct;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ContainingProduct oContainingProduct = new ContainingProduct();
                oContainingProduct.ContainingProductID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ContainingProduct, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ContainingProduct", id);
                ContainingProductDA.Delete(tc, oContainingProduct, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ContainingProduct. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ContainingProduct Get(int id, int nUserId)
        {
            ContainingProduct oAccountHead = new ContainingProduct();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ContainingProductDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ContainingProduct", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<ContainingProduct> Gets(int nUserID)
        {
            List<ContainingProduct> oContainingProduct = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ContainingProductDA.Gets(tc);
                oContainingProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContainingProduct", e);
                #endregion
            }

            return oContainingProduct;
        }
        public List<ContainingProduct> Gets(string sSQL, int nUserID)
        {
            List<ContainingProduct> oContainingProduct = new List<ContainingProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_ContainingProduct where ContainingProductID in (1,2,80,272,347,370,60,45)";
                }
                reader = ContainingProductDA.Gets(tc, sSQL);
                oContainingProduct = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ContainingProduct", e);
                #endregion
            }

            return oContainingProduct;
        }

        public List<ContainingProduct> GetsByWU(int nWUID, int nUserID)
        {
            List<ContainingProduct> oContainingProducts = new List<ContainingProduct>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ContainingProductDA.GetsByWU(tc, nWUID);
                oContainingProducts = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oContainingProducts = new List<ContainingProduct>();
                ContainingProduct oContainingProduct = new ContainingProduct();
                oContainingProduct.ErrorMessage = e.Message;
                oContainingProducts.Add(oContainingProduct);
                #endregion
            }
            return oContainingProducts;
        }
        #endregion
    }
}
