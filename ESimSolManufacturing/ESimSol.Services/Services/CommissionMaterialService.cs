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
    public class CommissionMaterialService : MarshalByRefObject, ICommissionMaterialService
    {
        #region Private functions and declaration
        private CommissionMaterial MapObject(NullHandler oReader)
        {
            CommissionMaterial oCommissionMaterial = new CommissionMaterial();

            oCommissionMaterial.CMID = oReader.GetInt32("CMID");
            oCommissionMaterial.Name = oReader.GetString("Name");
            oCommissionMaterial.SearchWhat = oReader.GetString("SearchWhat");
            oCommissionMaterial.IsActive = oReader.GetBoolean("IsActive");
            oCommissionMaterial.EncryptedID = Global.Encrypt(oCommissionMaterial.CMID.ToString());
            return oCommissionMaterial;
        }

        private CommissionMaterial CreateObject(NullHandler oReader)
        {
            CommissionMaterial oCommissionMaterial = MapObject(oReader);
            return oCommissionMaterial;
        }

        private List<CommissionMaterial> CreateObjects(IDataReader oReader)
        {
            List<CommissionMaterial> oCommissionMaterials = new List<CommissionMaterial>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                CommissionMaterial oItem = CreateObject(oHandler);
                oCommissionMaterials.Add(oItem);
            }
            return oCommissionMaterials;
        }

        #endregion

        #region Interface implementation
        public CommissionMaterialService() { }

        public CommissionMaterial IUD(CommissionMaterial oCommissionMaterial, int nDBOperation, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = CommissionMaterialDA.IUD(tc, oCommissionMaterial, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommissionMaterial = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
                if (nDBOperation == 3)
                {
                    oCommissionMaterial.ErrorMessage = Global.DeleteMessage;
                }
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oCommissionMaterial.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oCommissionMaterial.CMID = 0;
                #endregion
            }
            return oCommissionMaterial;
        }

        public CommissionMaterial Get(int nCMID, Int64 nUserId)
        {
            CommissionMaterial oCommissionMaterial = new CommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommissionMaterialDA.Get(nCMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommissionMaterial = CreateObject(oReader);
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

                oCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oCommissionMaterial;
        }

        public CommissionMaterial Get(string sSQL, Int64 nUserId)
        {
            CommissionMaterial oCommissionMaterial = new CommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommissionMaterialDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommissionMaterial = CreateObject(oReader);
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

                oCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oCommissionMaterial;
        }

        public List<CommissionMaterial> Gets(Int64 nUserID)
        {
            List<CommissionMaterial> oCommissionMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommissionMaterialDA.Gets(tc);
                oCommissionMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommissionMaterial", e);
                #endregion
            }
            return oCommissionMaterial;
        }

        public List<CommissionMaterial> Gets(string sSQL, Int64 nUserID)
        {
            List<CommissionMaterial> oCommissionMaterial = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = CommissionMaterialDA.Gets(sSQL, tc);
                oCommissionMaterial = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get CommissionMaterial", e);
                #endregion
            }
            return oCommissionMaterial;
        }

        #endregion

        #region Activity
        public CommissionMaterial Activite(int nCMID, Int64 nUserId)
        {
            CommissionMaterial oCommissionMaterial = new CommissionMaterial();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = CommissionMaterialDA.Activity(nCMID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oCommissionMaterial = CreateObject(oReader);
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
                oCommissionMaterial.ErrorMessage = e.Message;
                #endregion
            }

            return oCommissionMaterial;
        }


        #endregion

    }
}
