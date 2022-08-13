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

    public class DepartmentService : MarshalByRefObject, IDepartmentService
    {
        #region Private functions and declaration
        private Department MapObject(NullHandler oReader)
        {
            Department oDepartment = new Department();
            oDepartment.DepartmentID = oReader.GetInt32("DepartmentID");
            oDepartment.Code = oReader.GetString("Code");
            oDepartment.Name = oReader.GetString("Name");
            oDepartment.NameInBangla = oReader.GetString("NameInBangla");
            oDepartment.Description = oReader.GetString("Description");
            oDepartment.ParentID = oReader.GetInt32("ParentID");
            oDepartment.Sequence = oReader.GetInt32("Sequence");
            oDepartment.RequiredPerson = oReader.GetInt32("RequiredPerson");
            oDepartment.IsActive = oReader.GetBoolean("IsActive");
            return oDepartment;
        }

        private Department CreateObject(NullHandler oReader)
        {
            Department oDepartment = new Department();
            oDepartment = MapObject(oReader);
            return oDepartment;
        }

        private List<Department> CreateObjects(IDataReader oReader)
        {
            List<Department> oDepartment = new List<Department>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Department oItem = CreateObject(oHandler);
                oDepartment.Add(oItem);
            }
            return oDepartment;
        }

        #endregion

        #region Interface implementation
        public DepartmentService() { }

        public Department Save(Department oDepartment, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oDepartment.DepartmentID <= 0)
                {
                    reader = DepartmentDA.InsertUpdate(tc, oDepartment, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = DepartmentDA.InsertUpdate(tc, oDepartment, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oDepartment = new Department();
                    oDepartment = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oDepartment = new Department();
                oDepartment.ErrorMessage = e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save Department. Because of " + e.Message, e);
                #endregion
            }
            return oDepartment;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Department oDepartment = new Department();
                oDepartment.DepartmentID = id;
                DepartmentDA.Delete(tc, oDepartment, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                return e.Message;
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Department. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public Department Get(int id, Int64 nUserId)
        {
            Department oAccountHead = new Department();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = DepartmentDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<Department> Gets(Int64 nUserId)
        {
            List<Department> oDepartment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentDA.Gets(tc);
                oDepartment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }

            return oDepartment;
        }

        public List<Department> Gets(string sSQL, Int64 nUserId)
        {
            List<Department> oDepartment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentDA.Gets(tc, sSQL);
                oDepartment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }

            return oDepartment;
        }
        public List<Department> GetsDeptWithParent(string DeptName, Int64 nUserId)
        {
            List<Department> oDepartment = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentDA.GetsDeptWithParent(tc, DeptName);
                oDepartment = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }

            return oDepartment;
        }

        public List<Department> GetDepartmentHierarchy(string sDepartmentIDs, Int64 nUserID)
        {
            TransactionContext tc = null;
            List<Department> oDepartments = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = DepartmentDA.GetDepartmentHierarchy(tc, sDepartmentIDs);
                oDepartments = CreateObjects(reader);
                reader.Close();
              
                reader.Close();
                tc.End();
            }

            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Department", e);
                #endregion
            }
            return oDepartments;
        }

        public List<Department> GetsXL(string sSQL, Int64 nUserId)
        {
            List<Department> oDepartments = new List<Department>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DepartmentDA.GetsXL(tc, sSQL);
                NullHandler oreader = new NullHandler(reader);
                while (reader.Read())
                {
                    Department oItem = new Department();
                    oItem.Code = oreader.GetString("Code");
                    oItem.Name = oreader.GetString("Name");
                    oItem.PCode = oreader.GetString("PCode");
                    oDepartments.Add(oItem);
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
                throw new ServiceException(e.Message);
                #endregion
            }

            return oDepartments;
        }

        #endregion
    }
    
}
