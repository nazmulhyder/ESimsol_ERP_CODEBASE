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
    public class EmployeeTypeService : MarshalByRefObject, IEmployeeTypeService
    {
        #region Private functions and declaration
        private EmployeeType MapObject(NullHandler oReader)
        {
            EmployeeType oEmployeeType = new EmployeeType();
            oEmployeeType.EmployeeTypeID = oReader.GetInt32("EmployeeTypeID");
            oEmployeeType.Code = oReader.GetInt32("Code");
            oEmployeeType.Name = oReader.GetString("Name");
            oEmployeeType.NameInBangla = oReader.GetString("NameInBangla");
            oEmployeeType.Description = oReader.GetString("Description");
            oEmployeeType.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeType.EncryptEmpTypeID = Global.Encrypt(oEmployeeType.EmployeeTypeID.ToString());
            oEmployeeType.EmployeeGrouping = (EnumEmployeeGrouping)oReader.GetInt16("EmployeeGrouping");
            return oEmployeeType;
        }

        private EmployeeType CreateObject(NullHandler oReader)
        {
            EmployeeType oEmployeeType = MapObject(oReader);
            return oEmployeeType;
        }

        private List<EmployeeType> CreateObjects(IDataReader oReader)
        {
            List<EmployeeType> oEmployeeType = new List<EmployeeType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeType oItem = CreateObject(oHandler);
                oEmployeeType.Add(oItem);
            }
            return oEmployeeType;
        }

        #endregion

        #region Interface implementation
        public EmployeeTypeService() { }

        public EmployeeType Save(EmployeeType oEmployeeType, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oEmployeeType.EmployeeTypeID <= 0)
                {
                    reader = EmployeeTypeDA.InsertUpdate(tc, oEmployeeType, EnumDBOperation.Insert, nUserID);
                }
                else
                {

                    reader = EmployeeTypeDA.InsertUpdate(tc, oEmployeeType, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeType = new EmployeeType();
                    oEmployeeType = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save EmployeeType. Because of " + e.Message, e);
                #endregion
            }
            return oEmployeeType;
        }

        public List<EmployeeType> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeType> oEmployeeType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeTypeDA.Gets(sSQL, tc);
                oEmployeeType = CreateObjects(reader);
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
            return oEmployeeType;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeType oEmployeeType = new EmployeeType();
                oEmployeeType.EmployeeTypeID = id;
                EmployeeTypeDA.Delete(tc, oEmployeeType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete EmployeeType. Because of " + e.Message, e);
                #endregion
            }
            return "Data delete successfully";
        }

        public EmployeeType Get(int id, Int64 nUserId)
        {
            EmployeeType aEmployeeType = new EmployeeType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeTypeDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    aEmployeeType = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get EmployeeType", e);
                #endregion
            }

            return aEmployeeType;
        }

        public List<EmployeeType> Gets(Int64 nUserID)
        {
            List<EmployeeType> oEmployeeType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeTypeDA.Gets(tc);
                oEmployeeType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeType", e);
                #endregion
            }

            return oEmployeeType;
        }

        public string ChangeActiveStatus(EmployeeType oEmployeeType, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeTypeDA.ChangeActiveStatus(tc, oEmployeeType);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return "Update sucessfully";
        }
        #endregion
    }
}
