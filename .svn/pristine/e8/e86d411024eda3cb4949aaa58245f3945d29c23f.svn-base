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
    public class EmployeeCommissionMaterialService : MarshalByRefObject, IEmployeeCommissionMaterialService
    {
        #region Private functions and declaration
        private EmployeeCommissionMaterial MapObject(NullHandler oReader)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();

            oEmployeeCommissionMaterial.ECMID = oReader.GetInt32("ECMID");
            oEmployeeCommissionMaterial.CMID = oReader.GetInt32("CMID");
            oEmployeeCommissionMaterial.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeCommissionMaterial.SearchWhatValue = oReader.GetString("SearchWhatValue");
            oEmployeeCommissionMaterial.Note = oReader.GetString("Note");
            oEmployeeCommissionMaterial.EffectDate = oReader.GetDateTime("EffectDate");
            oEmployeeCommissionMaterial.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeCommissionMaterial.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oEmployeeCommissionMaterial.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeCommissionMaterial.InactiveDate = oReader.GetDateTime("InactiveDate");
            //DERIVE
            oEmployeeCommissionMaterial.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeCommissionMaterial.MaterialName = oReader.GetString("MaterialName");
            oEmployeeCommissionMaterial.EncryptedID = Global.Encrypt(oEmployeeCommissionMaterial.ECMID.ToString());
            return oEmployeeCommissionMaterial;
        }

        private EmployeeCommissionMaterial CreateObject(NullHandler oReader)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = MapObject(oReader);
            return oEmployeeCommissionMaterial;
        }

        private List<EmployeeCommissionMaterial> CreateObjects(IDataReader oReader)
        {
            List<EmployeeCommissionMaterial> oEmployeeCommissionMaterials = new List<EmployeeCommissionMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeCommissionMaterial oItem = CreateObject(oHandler);
                oEmployeeCommissionMaterials.Add(oItem);
            }
            return oEmployeeCommissionMaterials;
        }

        #endregion

        #region Interface implementation
        public EmployeeCommissionMaterialService() { }

        public EmployeeCommissionMaterial IUD(EmployeeCommissionMaterial oEmployeeCommissionMaterial, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeCommissionMaterialDA.IUD(tc, oEmployeeCommissionMaterial, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCommissionMaterial = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oEmployeeCommissionMaterial.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeCommissionMaterial.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeCommissionMaterial.ECMID = 0;
                #endregion
            }
            return oEmployeeCommissionMaterial;
        }

        public EmployeeCommissionMaterial Get(int nECMID, Int64 nUserId)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCommissionMaterialDA.Get(nECMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCommissionMaterial = CreateObject(oReader);
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

                oEmployeeCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCommissionMaterial;
        }

        public EmployeeCommissionMaterial Get(string sSQL, Int64 nUserId)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCommissionMaterialDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCommissionMaterial = CreateObject(oReader);
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

                oEmployeeCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCommissionMaterial;
        }

        public List<EmployeeCommissionMaterial> Gets(Int64 nUserID)
        {
            List<EmployeeCommissionMaterial> oEmployeeCommissionMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCommissionMaterialDA.Gets(tc);
                oEmployeeCommissionMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCommissionMaterial", e);
                #endregion
            }
            return oEmployeeCommissionMaterial;
        }

        public List<EmployeeCommissionMaterial> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeCommissionMaterial> oEmployeeCommissionMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeCommissionMaterialDA.Gets(sSQL, tc);
                oEmployeeCommissionMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get EmployeeCommissionMaterial", e);
                #endregion
            }
            return oEmployeeCommissionMaterial;
        }

        #endregion

        #region Activity
        public EmployeeCommissionMaterial Activite(int nECMID, Int64 nUserId)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeCommissionMaterialDA.Activity(nECMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCommissionMaterial = CreateObject(oReader);
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
                oEmployeeCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCommissionMaterial;
        }


        #endregion

        #region Approve
        public EmployeeCommissionMaterial Approve(string sSql, Int64 nUserId)
        {
            EmployeeCommissionMaterial oEmployeeCommissionMaterial = new EmployeeCommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = EmployeeCommissionMaterialDA.Approve(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeCommissionMaterial = CreateObject(oReader);
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
                oEmployeeCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeCommissionMaterial;
        }


        #endregion

    }
}
