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
    public class BusinessUnitService : MarshalByRefObject, IBusinessUnitService
    {
        #region Private functions and declaration
        private BusinessUnit MapObject(NullHandler oReader)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit.BusinessUnitID = oReader.GetInt32("BusinessUnitID");
            oBusinessUnit.Code = oReader.GetString("Code");
            oBusinessUnit.NameInBangla = oReader.GetString("NameInBangla");
            oBusinessUnit.AddressInBangla = oReader.GetString("AddressInBangla");
            oBusinessUnit.FaxNo = oReader.GetString("FaxNo");
            oBusinessUnit.Name = oReader.GetString("Name");
            oBusinessUnit.ShortName = oReader.GetString("ShortName");
            oBusinessUnit.RegistrationNo = oReader.GetString("RegistrationNo");
            oBusinessUnit.TINNo = oReader.GetString("TINNo");
            oBusinessUnit.VatNo = oReader.GetString("VatNo");
            oBusinessUnit.BusinessNature = (EnumBusinessNature)oReader.GetInt16("BusinessNature");
            oBusinessUnit.LegalFormat = (EnumLegalFormat)oReader.GetInt16("LegalFormat");
            oBusinessUnit.Address = oReader.GetString("Address");
            oBusinessUnit.Phone = oReader.GetString("Phone");
            oBusinessUnit.Email = oReader.GetString("Email");
            oBusinessUnit.WebAddress = oReader.GetString("WebAddress");
            oBusinessUnit.Note = oReader.GetString("Note");
            oBusinessUnit.IsAreaEffect = oReader.GetBoolean("IsAreaEffect");
            oBusinessUnit.IsZoneEffect = oReader.GetBoolean("IsZoneEffect");
            oBusinessUnit.IsSiteEffect = oReader.GetBoolean("IsSiteEffect");
            oBusinessUnit.NameCode = oReader.GetString("NameCode");
            oBusinessUnit.BusinessUnitType = (EnumBusinessUnitType)oReader.GetInt32("BusinessUnitType");
            oBusinessUnit.BusinessUnitTypeInInt = oReader.GetInt32("BusinessUnitType");
            oBusinessUnit.BUImage = oReader.GetBytes("BUImage");
            return oBusinessUnit;
        }

        private BusinessUnit CreateObject(NullHandler oReader)
        {
            BusinessUnit oBusinessUnit = new BusinessUnit();
            oBusinessUnit = MapObject(oReader);
            return oBusinessUnit;
        }

        private List<BusinessUnit> CreateObjects(IDataReader oReader)
        {
            List<BusinessUnit> oBusinessUnit = new List<BusinessUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                BusinessUnit oItem = CreateObject(oHandler);
                oBusinessUnit.Add(oItem);
            }
            return oBusinessUnit;
        }

        #endregion

        #region Interface implementation
        public BusinessUnitService() { }
        public BusinessUnit Save(BusinessUnit oBusinessUnit, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oBusinessUnit.BusinessUnitID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BusinessUnit, EnumRoleOperationType.Add);
                    reader = BusinessUnitDA.InsertUpdate(tc, oBusinessUnit, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.BusinessUnit, EnumRoleOperationType.Edit);
                    reader = BusinessUnitDA.InsertUpdate(tc, oBusinessUnit, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oBusinessUnit = new BusinessUnit();
                    oBusinessUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save BusinessUnit. Because of " + e.Message.Split('!')[0], e);
                #endregion
            }
            return oBusinessUnit;
        }
        
        public BusinessUnit UpdateImage(BusinessUnit oBusinessUnit, int nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                if (oBusinessUnit.BusinessUnitID > 0 && oBusinessUnit.BUImage != null)
                {
                    BusinessUnitDA.UpdateImage(tc, oBusinessUnit.BUImage, oBusinessUnit.BusinessUnitID, nUserID);
                    IDataReader reader = BusinessUnitDA.GetWithImage(tc, oBusinessUnit.BusinessUnitID);
                    NullHandler oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oBusinessUnit = CreateObject(oReader);
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

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Save BusinessUnit. Because of " + e.Message, e);
                #endregion
            }
            return oBusinessUnit;
        }
        public string Delete(int id, int nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit.BusinessUnitID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.BusinessUnit, EnumRoleOperationType.Delete);
                BusinessUnitDA.Delete(tc, oBusinessUnit, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete BusinessUnit. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        public BusinessUnit Get(int id, int nUserId)
        {
            BusinessUnit oAccountHead = new BusinessUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessUnitDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oAccountHead;
        }
        public BusinessUnit GetWithImage(int id, int nUserId)
        {
            BusinessUnit oAccountHead = new BusinessUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessUnitDA.GetWithImage(tc, id);
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
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oAccountHead;
        }
        public BusinessUnit GetByType(int nBUType, int nUserId)
        {
            BusinessUnit oAccountHead = new BusinessUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = BusinessUnitDA.GetByType(tc, nBUType);
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
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<BusinessUnit> GetsBUByCodeOrNameAndAccountHead(string sCodeOrName, int nAccountHeadID, int nUserID)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BusinessUnitDA.GetsBUByCodeOrNameAndAccountHead(tc, sCodeOrName, nAccountHeadID);
                oBusinessUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBusinessUnits = new List<BusinessUnit>();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit.ErrorMessage = e.Message;
                oBusinessUnits.Add(oBusinessUnit);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oBusinessUnits;
        }
        public List<BusinessUnit> GetsBUByCodeOrName(string sCodeOrName, int nUserID)
        {
            List<BusinessUnit> oBusinessUnits = new List<BusinessUnit>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BusinessUnitDA.GetsBUByCodeOrName(tc, sCodeOrName);
                oBusinessUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oBusinessUnits = new List<BusinessUnit>();
                BusinessUnit oBusinessUnit = new BusinessUnit();
                oBusinessUnit.ErrorMessage = e.Message;
                oBusinessUnits.Add(oBusinessUnit);

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oBusinessUnits;
        }
        public List<BusinessUnit> Gets(int nUserID)
        {
            List<BusinessUnit> oBusinessUnit = new List<BusinessUnit>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = BusinessUnitDA.Gets(tc);
                oBusinessUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oBusinessUnit;
        }
        public List<BusinessUnit> GetsPermittedBU(int nUserID)
        {
            List<BusinessUnit> oBusinessUnit = new List<BusinessUnit>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = BusinessUnitDA.GetsPermittedBU(tc, nUserID);
                oBusinessUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oBusinessUnit;
        }
        public List<BusinessUnit> Gets(string sSQL,int nUserID)
        {
            List<BusinessUnit> oBusinessUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                if(sSQL=="")
                {
                sSQL = "Select * from BusinessUnit where BusinessUnitID in (1,2,80,272,347,370,60,45)";
                    }
                reader = BusinessUnitDA.Gets(tc, sSQL);
                oBusinessUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get BusinessUnit", e);
                #endregion
            }

            return oBusinessUnit;
        }
        public bool IsPermittedBU(int nBUID, int nUserId)
        {
            bool bIsPermitted = false;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader = BusinessUnitDA.IsPermittedBU(tc, nBUID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    bIsPermitted = oReader.GetBoolean("IsPermitted");
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
                throw new ServiceException("Failed to Get . Because of " + e.Message, e);
                #endregion
            }
            return bIsPermitted;
        }       
        #endregion
    }   
}