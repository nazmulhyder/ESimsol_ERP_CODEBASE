using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class ModelCategoryService : MarshalByRefObject, IModelCategoryService
    {
        #region Private functions and declaration
        private ModelCategory MapObject(NullHandler oReader)
        {
            ModelCategory oModelCategory = new ModelCategory();
            oModelCategory.ModelCategoryID = oReader.GetInt32("ModelCategoryID");
            oModelCategory.CategoryName = oReader.GetString("CategoryName");
            oModelCategory.Remarks = oReader.GetString("Remarks");
            return oModelCategory;
        }

        private ModelCategory CreateObject(NullHandler oReader)
        {
            ModelCategory oModelCategory = new ModelCategory();
            oModelCategory = MapObject(oReader);
            return oModelCategory;
        }

        private List<ModelCategory> CreateObjects(IDataReader oReader)
        {
            List<ModelCategory> oModelCategory = new List<ModelCategory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ModelCategory oItem = CreateObject(oHandler);
                oModelCategory.Add(oItem);
            }
            return oModelCategory;
        }

        #endregion

        #region Interface implementation
        public ModelCategoryService() { }

        public ModelCategory Save(ModelCategory oModelCategory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oModelCategory.ModelCategoryID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ModelCategory, EnumRoleOperationType.Add);
                    reader = ModelCategoryDA.InsertUpdate(tc, oModelCategory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ModelCategory, EnumRoleOperationType.Edit);
                    reader = ModelCategoryDA.InsertUpdate(tc, oModelCategory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oModelCategory = new ModelCategory();
                    oModelCategory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oModelCategory.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oModelCategory;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ModelCategory oModelCategory = new ModelCategory();
                oModelCategory.ModelCategoryID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ModelCategory, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ModelCategory", id);
                ModelCategoryDA.Delete(tc, oModelCategory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public ModelCategory Get(int id, Int64 nUserId)
        {
            ModelCategory oAccountHead = new ModelCategory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ModelCategoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<ModelCategory> GetsTSNotAssignColor(int nTechnicalSheetID, Int64 nUserID)
        {
            List<ModelCategory> oModelCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ModelCategoryDA.GetsTSNotAssignColor(tc, nTechnicalSheetID);
                oModelCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oModelCategory;
        }

        public List<ModelCategory> GetsColorPikerForQC(int nTechnicalSheetID, Int64 nUserID)
        {
            List<ModelCategory> oModelCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ModelCategoryDA.GetsColorPikerForQC(tc, nTechnicalSheetID);
                oModelCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oModelCategory;
        }
        public List<ModelCategory> Gets(Int64 nUserID)
        {
            List<ModelCategory> oModelCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ModelCategoryDA.Gets(tc);
                oModelCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oModelCategory;
        }

        public List<ModelCategory> GetsbyCategoryName(string sCategoryName, Int64 nUserID)
        {
            List<ModelCategory> oModelCategory = new List<ModelCategory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ModelCategoryDA.GetsbyCategoryName(tc, sCategoryName);
                oModelCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oModelCategory;
        }


        public List<ModelCategory> Gets(string sSQL, Int64 nUserID)
        {
            List<ModelCategory> oModelCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ModelCategoryDA.Gets(tc, sSQL);
                oModelCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ModelCategory", e);
                #endregion
            }

            return oModelCategory;
        }
        #endregion
    }
}
