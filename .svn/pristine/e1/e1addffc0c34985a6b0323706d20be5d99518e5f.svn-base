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
    public class EmployeeActivityCategoryService : MarshalByRefObject, ESimSol.BusinessObjects.EmployeeActivityCategory.IEmployeeActivityCategoryService
    {
        #region Private functions and declaration
        private EmployeeActivityCategory MapObject(NullHandler oReader)
        {
            EmployeeActivityCategory oEmployeeActivityCategory = new EmployeeActivityCategory();
            oEmployeeActivityCategory.EACID = oReader.GetInt32("EACID");
            oEmployeeActivityCategory.Description = oReader.GetString("Description");

            return oEmployeeActivityCategory;
        }
        private EmployeeActivityCategory CreateObject(NullHandler oReader)
        {
            EmployeeActivityCategory oEmployeeActivityCategory = new EmployeeActivityCategory();
            oEmployeeActivityCategory = MapObject(oReader);
            return oEmployeeActivityCategory;
        }
        private List<EmployeeActivityCategory> CreateObjects(IDataReader oReader)
        {
            List<EmployeeActivityCategory> oEmployeeActivityCategory = new List<EmployeeActivityCategory>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeActivityCategory oItem = CreateObject(oHandler);
                oEmployeeActivityCategory.Add(oItem);
            }
            return oEmployeeActivityCategory;
        }
        #endregion

        #region Interface implementation
        public List<EmployeeActivityCategory> Gets(Int64 nUserID)
        {
            List<EmployeeActivityCategory> oEmployeeActivityCategory = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = EmployeeActivityCategoryDA.Gets(tc);
                oEmployeeActivityCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Error", e);
                #endregion
            }

            return oEmployeeActivityCategory;
        }

        public EmployeeActivityCategory Save(EmployeeActivityCategory oEmployeeActivityCategory, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeActivityCategory.EACID <= 0)
                {
                    reader = EmployeeActivityCategoryDA.InsertUpdate(tc, oEmployeeActivityCategory, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = EmployeeActivityCategoryDA.InsertUpdate(tc, oEmployeeActivityCategory, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeActivityCategory = new EmployeeActivityCategory();
                    oEmployeeActivityCategory = CreateObject(oReader);
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
                throw new ServiceException("Failed to save category description " + e.Message, e);
                #endregion
            }
            return oEmployeeActivityCategory;
        }


        public string Delete(int id, Int64 nUserId)
        {
            string message = "";
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeActivityCategory oEmployeeActivityCategory = new EmployeeActivityCategory();
                oEmployeeActivityCategory.EACID = id;
                EmployeeActivityCategoryDA.Delete(tc, oEmployeeActivityCategory, EnumDBOperation.Delete, nUserId);
                tc.End();
                message = Global.DeleteMessage;
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                message = (e.Message.Contains("~")) ? e.Message.Split('~')[0] : e.Message;
                #endregion
            }
            return message;
        }


        public List<EmployeeActivityCategory> Gets(string sSQL, Int64 nUserId)
        {
            List<EmployeeActivityCategory> oEmployeeActivityCategory = new List<EmployeeActivityCategory>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeActivityCategoryDA.Gets(sSQL, tc);
                oEmployeeActivityCategory = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Category", e);
                #endregion
            }

            return oEmployeeActivityCategory;
        }




        #endregion
    }
}
