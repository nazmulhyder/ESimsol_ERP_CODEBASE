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
    public class ProductPermissionService : MarshalByRefObject, IProductPermissionService
    {
        #region Private functions and declaration
        private ProductPermission MapObject(NullHandler oReader)
        {
            ProductPermission oProductPermission = new ProductPermission();
            oProductPermission.ProductPermissionID = oReader.GetInt32("ProductPermissionID");
            oProductPermission.UserID = oReader.GetInt32("UserID");
            oProductPermission.ModuleName = (EnumModuleName)oReader.GetInt32("ModuleName");
            oProductPermission.ModuleNameInt = oReader.GetInt32("ModuleName");
            oProductPermission.ProductUsages = (EnumProductUsages)oReader.GetInt32("ProductUsages");
            oProductPermission.ProductUsagesInt = oReader.GetInt32("ProductUsages");
            oProductPermission.ProductCategoryID = oReader.GetInt32("ProductCategoryID");
            oProductPermission.Remarks = oReader.GetString("Remarks");
            oProductPermission.ProductCategoryName = oReader.GetString("ProductCategoryName");
            return oProductPermission;
        }

        private ProductPermission CreateObject(NullHandler oReader)
        {
            ProductPermission oProductPermission = new ProductPermission();
            oProductPermission = MapObject(oReader);
            return oProductPermission;
        }

        private List<ProductPermission> CreateObjects(IDataReader oReader)
        {
            List<ProductPermission> oProductPermission = new List<ProductPermission>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductPermission oItem = CreateObject(oHandler);
                oProductPermission.Add(oItem);
            }
            return oProductPermission;
        }

        #endregion

        #region Interface implementation
        public ProductPermissionService() { }

        public ProductPermission Save(ProductPermission oProductPermission, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductPermission.ProductPermissionID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductPermission, EnumRoleOperationType.Add);
                    reader = ProductPermissionDA.InsertUpdate(tc, oProductPermission, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ProductPermission, EnumRoleOperationType.Edit);
                    reader = ProductPermissionDA.InsertUpdate(tc, oProductPermission, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductPermission = new ProductPermission();
                    oProductPermission = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductPermission = new ProductPermission();
                oProductPermission.ErrorMessage = e.Message.Split('~')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save ProductPermission. Because of " + e.Message, e);
                #endregion
            }
            return oProductPermission;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductPermission oProductPermission = new ProductPermission();
                oProductPermission.ProductPermissionID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductPermission, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProductPermission", id);
                ProductPermissionDA.Delete(tc, oProductPermission, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductPermission. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ProductPermission Get(int id, int nUserId)
        {
            ProductPermission oAccountHead = new ProductPermission();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductPermissionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductPermission", e);
                #endregion
            }

            return oAccountHead;
        }



        public List<ProductPermission> Gets(int nUserID)
        {
            List<ProductPermission> oProductPermission = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductPermissionDA.Gets(tc);
                oProductPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductPermission", e);
                #endregion
            }

            return oProductPermission;
        }
        public List<ProductPermission> Gets(string sSQL, int nUserID)
        {
            List<ProductPermission> oProductPermission = new List<ProductPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if (sSQL == "")
                {
                    sSQL = "Select * from View_ProductPermission where ProductPermissionID in (1,2,80,272,347,370,60,45)";
                }
                reader = ProductPermissionDA.Gets(tc, sSQL);
                oProductPermission = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductPermission", e);
                #endregion
            }

            return oProductPermission;
        }

        public List<ProductPermission> GetsByUser(int nPermittedUserID, int nUserID)
        {
            List<ProductPermission> oProductPermissions = new List<ProductPermission>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = ProductPermissionDA.GetsByUser(tc, nPermittedUserID);
                oProductPermissions = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oProductPermissions = new List<ProductPermission>();
                ProductPermission oProductPermission = new ProductPermission();
                oProductPermission.ErrorMessage = e.Message;
                oProductPermissions.Add(oProductPermission);
                #endregion
            }
            return oProductPermissions;
        }
        #endregion
    }
}
