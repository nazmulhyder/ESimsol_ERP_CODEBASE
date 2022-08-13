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
    public class VoucherTypeService : MarshalByRefObject, IVoucherTypeService
    {
        #region Private functions and declaration
        private VoucherType MapObject(NullHandler oReader)
        {
            VoucherType oVoucherType = new VoucherType();
            oVoucherType.VoucherTypeID = oReader.GetInt32("VoucherTypeID");
            oVoucherType.VoucherName= oReader.GetString("VoucherName");            
            oVoucherType.VoucherCategory= (EnumVoucherCategory)oReader.GetInt32("VoucherCategory");
            oVoucherType.NumberMethod= (EnumNumberMethod)oReader.GetInt32("NumberMethod");            
            oVoucherType.PrintAfterSave= oReader.GetBoolean("PrintAfterSave");
            oVoucherType.MustNarrationEntry= oReader.GetBoolean("MustNarrationEntry");
            oVoucherType.IsProductRequired = oReader.GetBoolean("IsProductRequired");
            oVoucherType.IsDepartmentRequired = oReader.GetBoolean("IsDepartmentRequired");
            oVoucherType.IsPaymentCheque = oReader.GetBoolean("IsPaymentCheque");
            oVoucherType.VoucherNumberFormat = oReader.GetString("VoucherNumberFormat");           
            return oVoucherType;
        }

        private VoucherType CreateObject(NullHandler oReader)
        {
            VoucherType oVoucherType = new VoucherType();
            oVoucherType = MapObject(oReader);
            return oVoucherType;
        }

        private List<VoucherType> CreateObjects(IDataReader oReader)
        {
            List<VoucherType> oVoucherType = new List<VoucherType>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VoucherType oItem = CreateObject(oHandler);
                oVoucherType.Add(oItem);
            }
            return oVoucherType;
        }

        #endregion

        #region Interface implementation
        public VoucherTypeService() { }

        public VoucherType Save(VoucherType oVoucherType, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVoucherType.VoucherTypeID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VoucherType, EnumRoleOperationType.Add);
                    reader = VoucherTypeDA.InsertUpdate(tc, oVoucherType, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VoucherType, EnumRoleOperationType.Edit);
                    reader = VoucherTypeDA.InsertUpdate(tc, oVoucherType, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVoucherType = new VoucherType();
                    oVoucherType = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oVoucherType.ErrorMessage = e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Save VoucherType. Because of " + e.Message, e);
                #endregion
            }
            return oVoucherType;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VoucherType oVoucherType = new VoucherType();
                oVoucherType.VoucherTypeID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.VoucherType, EnumRoleOperationType.Delete);
                VoucherTypeDA.Delete(tc, oVoucherType, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete VoucherType. Because of " + e.Message, e);
                return e.Message;
                #endregion
            }
            return "Data delete successfully";
        }

        public VoucherType Get(int id, int nUserId)
        {
            VoucherType oAccountHead = new VoucherType();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = VoucherTypeDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get VoucherType", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<VoucherType> Gets(int nUserId)
        {
            List<VoucherType> oVoucherType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherTypeDA.Gets(tc);
                oVoucherType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherType", e);
                #endregion
            }

            return oVoucherType;
        }

        public List<VoucherType> Gets(string sSQL, int nUserId)
        {
            List<VoucherType> oVoucherType = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VoucherTypeDA.Gets(tc, sSQL);
                oVoucherType = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VoucherType", e);
                #endregion
            }

            return oVoucherType;
        }
        #endregion
    }
}
