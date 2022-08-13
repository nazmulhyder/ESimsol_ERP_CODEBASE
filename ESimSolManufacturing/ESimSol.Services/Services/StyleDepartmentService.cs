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

    public class StyleDepartmentService : MarshalByRefObject, IStyleDepartmentService
    {
        #region Private functions and declaration
        private StyleDepartment MapObject(NullHandler oReader)
        {
            StyleDepartment oStyleDepartment = new StyleDepartment();
            oStyleDepartment.StyleDepartmentID = oReader.GetInt32("StyleDepartmentID");
            oStyleDepartment.Name = oReader.GetString("Name");
            oStyleDepartment.Note = oReader.GetString("Note");
            return oStyleDepartment;
        }

        private StyleDepartment CreateObject(NullHandler oReader)
        {
            StyleDepartment oStyleDepartment = new StyleDepartment();
            oStyleDepartment = MapObject(oReader);
            return oStyleDepartment;
        }

        private List<StyleDepartment> CreateObjects(IDataReader oReader)
        {
            List<StyleDepartment> oStyleDepartment = new List<StyleDepartment>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                StyleDepartment oItem = CreateObject(oHandler);
                oStyleDepartment.Add(oItem);
            }
            return oStyleDepartment;
        }

        #endregion

        #region Interface implementation
        public StyleDepartmentService() { }

        public StyleDepartment Save(StyleDepartment oStyleDepartment, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oStyleDepartment.StyleDepartmentID <= 0)
                {

                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleDepartment, EnumRoleOperationType.Add);
                    reader = StyleDepartmentDA.InsertUpdate(tc, oStyleDepartment, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.StyleDepartment, EnumRoleOperationType.Edit);
                    reader = StyleDepartmentDA.InsertUpdate(tc, oStyleDepartment, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oStyleDepartment = new StyleDepartment();
                    oStyleDepartment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oStyleDepartment = new StyleDepartment();
                oStyleDepartment.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oStyleDepartment;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                StyleDepartment oStyleDepartment = new StyleDepartment();
                oStyleDepartment.StyleDepartmentID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.StyleDepartment, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "StyleDepartment", id);
                StyleDepartmentDA.Delete(tc, oStyleDepartment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete StyleDepartment. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public StyleDepartment Get(int id, Int64 nUserId)
        {
            StyleDepartment oAccountHead = new StyleDepartment();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = StyleDepartmentDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get StyleDepartment", e);
                #endregion
            }

            return oAccountHead;
        }


        public List<StyleDepartment> Gets(Int64 nUserID)
        {
            List<StyleDepartment> oStyleDepartment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleDepartmentDA.Gets(tc);
                oStyleDepartment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleDepartment", e);
                #endregion
            }

            return oStyleDepartment;
        }

        public List<StyleDepartment> Gets(string sSQL, Int64 nUserID)
        {
            List<StyleDepartment> oStyleDepartment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = StyleDepartmentDA.Gets(tc, sSQL);
                oStyleDepartment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get StyleDepartment", e);
                #endregion
            }

            return oStyleDepartment;
        }

        #endregion
    }   

}
