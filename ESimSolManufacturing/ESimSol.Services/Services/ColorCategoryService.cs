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
    public class ColorCategoryService : MarshalByRefObject, IColorCategoryService
    {
        #region Private functions and declaration
        private ColorCategory MapObject(NullHandler oReader)
        {
            ColorCategory oColorCategory = new ColorCategory();
            oColorCategory.ColorCategoryID = oReader.GetInt32("ColorCategoryID");
            oColorCategory.ColorName = oReader.GetString("ColorName");
            oColorCategory.Note = oReader.GetString("Note");
            return oColorCategory;
        }

        private ColorCategory CreateObject(NullHandler oReader)
        {
            ColorCategory oColorCategory = new ColorCategory();
            oColorCategory = MapObject(oReader);
            return oColorCategory;
        }

        private List<ColorCategory> CreateObjects(IDataReader oReader)
        {
            List<ColorCategory> oColorCategory = new List<ColorCategory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ColorCategory oItem = CreateObject(oHandler);
                oColorCategory.Add(oItem);
            }
            return oColorCategory;
        }

        #endregion

        #region Interface implementation
        public ColorCategoryService() { }

        public ColorCategory Save(ColorCategory oColorCategory, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oColorCategory.ColorCategoryID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ColorCategory, EnumRoleOperationType.Add);
                    reader = ColorCategoryDA.InsertUpdate(tc, oColorCategory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ColorCategory, EnumRoleOperationType.Edit);
                    reader = ColorCategoryDA.InsertUpdate(tc, oColorCategory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oColorCategory = new ColorCategory();
                    oColorCategory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oColorCategory.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oColorCategory;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ColorCategory oColorCategory = new ColorCategory();
                oColorCategory.ColorCategoryID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ColorCategory, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ColorCategory", id);
                ColorCategoryDA.Delete(tc, oColorCategory, EnumDBOperation.Delete, nUserId);
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

        public ColorCategory Get(int id, Int64 nUserId)
        {
            ColorCategory oAccountHead = new ColorCategory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ColorCategoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oAccountHead;
        }
        
        public List<ColorCategory> GetsTSNotAssignColor(int nTechnicalSheetID, Int64 nUserID)
        {
            List<ColorCategory> oColorCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ColorCategoryDA.GetsTSNotAssignColor(tc, nTechnicalSheetID);
                oColorCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oColorCategory;
        }

        public List<ColorCategory> GetsColorPikerForQC(int nTechnicalSheetID, Int64 nUserID)
        {
            List<ColorCategory> oColorCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ColorCategoryDA.GetsColorPikerForQC(tc, nTechnicalSheetID);
                oColorCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oColorCategory;
        }
        public List<ColorCategory> Gets(Int64 nUserID)
        {
            List<ColorCategory> oColorCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ColorCategoryDA.Gets(tc);
                oColorCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oColorCategory;
        }

        public List<ColorCategory> GetsbyColorName(string sColorName, Int64 nUserID)
        {
            List<ColorCategory> oColorCategory = new List<ColorCategory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ColorCategoryDA.GetsbyColorName(tc, sColorName);
                oColorCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oColorCategory;
        }


        public List<ColorCategory> Gets(string sSQL, Int64 nUserID)
        {
            List<ColorCategory> oColorCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ColorCategoryDA.Gets(tc, sSQL);
                oColorCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ColorCategory", e);
                #endregion
            }

            return oColorCategory;
        }
        #endregion
    }
}
