using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;

namespace ESimSol.Services.Services
{
    public class VehicleRegistrationService : MarshalByRefObject, IVehicleRegistrationService
    {
        #region Private functions and declaration
        private VehicleRegistration MapObject(NullHandler oReader)
        {
            VehicleRegistration oVehicleRegistration = new VehicleRegistration();
            oVehicleRegistration.VehicleRegistrationID = oReader.GetInt32("VehicleRegistrationID");
            oVehicleRegistration.FileNo = oReader.GetString("FileNo");
            oVehicleRegistration.VehicleRegNo = oReader.GetString("VehicleRegNo");
            oVehicleRegistration.VehicleRegDate = oReader.GetDateTime("VehicleRegDate");
            oVehicleRegistration.VehicleTypeID = oReader.GetInt32("VehicleTypeID");
            oVehicleRegistration.CustomerID = oReader.GetInt32("CustomerID");
            oVehicleRegistration.ContactPersonID = oReader.GetInt32("ContactPersonID");
            oVehicleRegistration.VehicleChassisID = oReader.GetInt32("VehicleChassisID");
            oVehicleRegistration.VehicleEngineID = oReader.GetInt32("VehicleEngineID");
            oVehicleRegistration.VehicleColorID = oReader.GetInt32("VehicleColorID");
            oVehicleRegistration.ContactPerson = oReader.GetString("ContactPerson");
            oVehicleRegistration.CustomerName = oReader.GetString("CustomerName");
            oVehicleRegistration.VehicleTypeName = oReader.GetString("VehicleTypeName");
            oVehicleRegistration.ChassisNo = oReader.GetString("ChassisNo");
            oVehicleRegistration.EngineNo = oReader.GetString("EngineNo");
            oVehicleRegistration.VehicleModelNo = oReader.GetString("VehicleModelNo");
            oVehicleRegistration.ColorName = oReader.GetString("ColorName");
            oVehicleRegistration.VehicleRegistrationType = (EnumVehicleRegistrationType)oReader.GetInt32("VehicleRegistrationType");
            oVehicleRegistration.DeliveryDate = oReader.GetString("DeliveryDate");
            oVehicleRegistration.Remarks = oReader.GetString("Remarks");
            oVehicleRegistration.NoShowStatus = oReader.GetString("NoShowStatus");
            oVehicleRegistration.ServicePlan = oReader.GetString("ServicePlan");
            oVehicleRegistration.RemainingFreeService = oReader.GetString("RemainingFreeService");
            return oVehicleRegistration;
        }

        private VehicleRegistration CreateObject(NullHandler oReader)
        {
            VehicleRegistration oVehicleRegistration = new VehicleRegistration();
            oVehicleRegistration = MapObject(oReader);
            return oVehicleRegistration;
        }

        private List<VehicleRegistration> CreateObjects(IDataReader oReader)
        {
            List<VehicleRegistration> oVehicleRegistration = new List<VehicleRegistration>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                VehicleRegistration oItem = CreateObject(oHandler);
                oVehicleRegistration.Add(oItem);
            }
            return oVehicleRegistration;
        }

        #endregion

        #region Interface implementation
        public VehicleRegistrationService() { }
        public VehicleRegistration Save(VehicleRegistration oVehicleRegistration, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oVehicleRegistration.VehicleRegistrationID <= 0)
                {
                    reader = VehicleRegistrationDA.InsertUpdate(tc, oVehicleRegistration, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = VehicleRegistrationDA.InsertUpdate(tc, oVehicleRegistration, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleRegistration = new VehicleRegistration();
                    oVehicleRegistration = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save VehicleRegistration. Because of " + e.Message, e);
                #endregion
            }
            return oVehicleRegistration;
        }
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                VehicleRegistration oVehicleRegistration = new VehicleRegistration();
                oVehicleRegistration.VehicleRegistrationID = id;
                VehicleRegistrationDA.Delete(tc, oVehicleRegistration, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public VehicleRegistration Get(int id, Int64 nUserId)
        {
            VehicleRegistration oVehicleRegistration = new VehicleRegistration();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = VehicleRegistrationDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleRegistration = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get VehicleRegistration", e);
                #endregion
            }
            return oVehicleRegistration;
        }
        public List<VehicleRegistration> Gets(Int64 nUserID)
        {
            List<VehicleRegistration> oVehicleRegistrations = new List<VehicleRegistration>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleRegistrationDA.Gets(tc);
                oVehicleRegistrations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleRegistration", e);
                #endregion
            }
            return oVehicleRegistrations;
        }
        public List<VehicleRegistration> Gets(string sSQL, Int64 nUserID)
        {
            List<VehicleRegistration> oVehicleRegistrations = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = VehicleRegistrationDA.Gets(tc, sSQL);
                oVehicleRegistrations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleRegistration", e);
                #endregion
            }
            return oVehicleRegistrations;
        }
        public List<VehicleRegistration> GetsByChassisNo(string sChassis, Int64 nUserID)
        {
            List<VehicleRegistration> oVehicleRegistrations = null;
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = VehicleRegistrationDA.GetsByChassisNo(tc, sChassis);
                oVehicleRegistrations = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get VehicleRegistration", e);
                #endregion
            }
            return oVehicleRegistrations;
        }

        public VehicleRegistration Approve(VehicleRegistration oVehicleRegistration, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.VehicleRegistration, EnumRoleOperationType.Approved);
                reader = VehicleRegistrationDA.InsertUpdate(tc, oVehicleRegistration, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oVehicleRegistration = new VehicleRegistration();
                    oVehicleRegistration = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                {
                    tc.HandleError();
                    oVehicleRegistration = new VehicleRegistration();
                    oVehicleRegistration.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oVehicleRegistration;
        }

        #endregion
    }
}
