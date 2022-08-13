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
    public class SizeCategoryService : MarshalByRefObject, ISizeCategoryService
    {
        #region Private functions and declaration
        private SizeCategory MapObject(NullHandler oReader)
        {
            SizeCategory oSizeCategory = new SizeCategory();
            oSizeCategory.SizeCategoryID = oReader.GetInt32("SizeCategoryID");            
            oSizeCategory.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oSizeCategory.Sequence = oReader.GetInt32("Sequence");
            oSizeCategory.Note = oReader.GetString("Note");            
            return oSizeCategory;
        }

        private SizeCategory CreateObject(NullHandler oReader)
        {
            SizeCategory oSizeCategory = new SizeCategory();
            oSizeCategory = MapObject(oReader);
            return oSizeCategory;
        }

        private List<SizeCategory> CreateObjects(IDataReader oReader)
        {
            List<SizeCategory> oSizeCategory = new List<SizeCategory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                SizeCategory oItem = CreateObject(oHandler);
                oSizeCategory.Add(oItem);
            }
            return oSizeCategory;
        }

        #endregion

        #region Interface implementation
        public SizeCategoryService() { }

        public SizeCategory Save(SizeCategory oSizeCategory, Int64 nUserID)
        {
            TransactionContext tc = null;
            oSizeCategory.ErrorMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oSizeCategory.SizeCategoryID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ColorCategory, EnumRoleOperationType.Add);
                    reader = SizeCategoryDA.InsertUpdate(tc, oSizeCategory, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.ColorCategory, EnumRoleOperationType.Edit);
                    reader = SizeCategoryDA.InsertUpdate(tc, oSizeCategory, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oSizeCategory = new SizeCategory();
                    oSizeCategory = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oSizeCategory.ErrorMessage = e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save SizeCategory. Because of " + e.Message, e);
                #endregion
            }
            return oSizeCategory;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                SizeCategory oSizeCategory = new SizeCategory();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.SizeCategory, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "SizeCategory", id);
                oSizeCategory.SizeCategoryID = id;
                SizeCategoryDA.Delete(tc, oSizeCategory, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete SizeCategory. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public SizeCategory Get(int id, Int64 nUserId)
        {
            SizeCategory oAccountHead = new SizeCategory();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = SizeCategoryDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get SizeCategory", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<SizeCategory> Gets(Int64 nUserID)
        {
            List<SizeCategory> oSizeCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SizeCategoryDA.Gets(tc);
                oSizeCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SizeCategory", e);
                #endregion
            }

            return oSizeCategory;
        }
        public List<SizeCategory> ResetSequence(SizeCategory oSizeCategory, Int64 nUserID)
        {
            SizeCategory oTempSizeCategorie = new SizeCategory();
            List<SizeCategory> oSizeCategorys = new List<SizeCategory>();
            List<SizeCategory> oTempSizeCategorys = new List<SizeCategory>();
            oTempSizeCategorys = oSizeCategory.SizeCategories;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                foreach (SizeCategory oItem in oTempSizeCategorys)
                {
                    IDataReader reader;
                    reader = SizeCategoryDA.ResetSequence(tc, oItem, nUserID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oTempSizeCategorie = new SizeCategory();
                        oTempSizeCategorie = CreateObject(oReader);
                        oSizeCategorys.Add(oTempSizeCategorie);
                    }
                    reader.Close();
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oTempSizeCategorie = new SizeCategory();
                oSizeCategorys = new List<SizeCategory>();
                oTempSizeCategorie.ErrorMessage = e.Message;
                oSizeCategorys.Add(oTempSizeCategorie);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get SizeCategory", e);
                #endregion
            }

            return oSizeCategorys;
        }

        public List<SizeCategory> Gets(string sSQL, Int64 nUserID)
        {
            List<SizeCategory> oSizeCategory = null;
            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = SizeCategoryDA.Gets(tc, sSQL);
                oSizeCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SizeCategory", e);
                #endregion
            }

            return oSizeCategory;
        }
        
        public List<SizeCategory> GetsBySizeCategory(string sSizeCategory, Int64 nUserID)
        {
            List<SizeCategory> oSizeCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SizeCategoryDA.GetsBySizeCategory(tc, sSizeCategory);
                oSizeCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get SizeCategory", e);
                #endregion
            }

            return oSizeCategory;
        }


        public List<SizeCategory> GetsSizeForQC(int nTechnicalSheetID, Int64 nUserID)
        {
            List<SizeCategory> oSizeCategory = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = SizeCategoryDA.GetsSizeForQC(tc, nTechnicalSheetID);
                oSizeCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Size Category", e);
                #endregion
            }

            return oSizeCategory;
        }
        #endregion
    }
}
