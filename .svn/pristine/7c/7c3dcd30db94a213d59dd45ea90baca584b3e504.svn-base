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
    public class ProductionTimeSetupService : MarshalByRefObject, IProductionTimeSetupService
    {
        #region Private functions and declaration
        private ProductionTimeSetup MapObject(NullHandler oReader)
        {
            ProductionTimeSetup oProductionTimeSetup = new ProductionTimeSetup();
            oProductionTimeSetup.ProductionTimeSetupID = oReader.GetInt32("ProductionTimeSetupID");
            oProductionTimeSetup.BUName = oReader.GetString("BUName");
            oProductionTimeSetup.BUShortName = oReader.GetString("ShortName");
            oProductionTimeSetup.OffDay = oReader.GetString("OffDay");
            oProductionTimeSetup.RegularTime = oReader.GetDouble("RegularTime");
            oProductionTimeSetup.OverTime = oReader.GetDouble("OverTime");
            oProductionTimeSetup.BUID = oReader.GetInt32("BUID");
            return oProductionTimeSetup;
        }
        private ProductionTimeSetup CreateObject(NullHandler oReader)
        {
            ProductionTimeSetup oProductionTimeSetup = new ProductionTimeSetup();
            oProductionTimeSetup = MapObject(oReader);
            return oProductionTimeSetup;
        }
        private List<ProductionTimeSetup> CreateObjects(IDataReader oReader)
        {
            List<ProductionTimeSetup> oProductionTimeSetup = new List<ProductionTimeSetup>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionTimeSetup oItem = CreateObject(oHandler);
                oProductionTimeSetup.Add(oItem);
            }
            return oProductionTimeSetup;
        }
        #endregion

        #region Interface implementation
        public ProductionTimeSetupService() { }

        public ProductionTimeSetup Save(ProductionTimeSetup oProductionTimeSetup, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oProductionTimeSetup.ProductionTimeSetupID <= 0)
                {
                    reader = ProductionTimeSetupDA.InsertUpdate(tc, oProductionTimeSetup, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = ProductionTimeSetupDA.InsertUpdate(tc, oProductionTimeSetup, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionTimeSetup = new ProductionTimeSetup();
                    oProductionTimeSetup = CreateObject(oReader);
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
                throw new ServiceException("Failed to Save ProductionTimeSetup. Because of " + e.Message.Split('!')[0], e);
                #endregion
            }
            return oProductionTimeSetup;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionTimeSetup oProductionTimeSetup = new ProductionTimeSetup();
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.ProductionTimeSetup, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "ProductionTimeSetup", id);
                oProductionTimeSetup.ProductionTimeSetupID = id;
                ProductionTimeSetupDA.Delete(tc, oProductionTimeSetup, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete ProductionTimeSetup. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }

        public ProductionTimeSetup Get(int id, Int64 nUserId)
        {
            ProductionTimeSetup oAccountHead = new ProductionTimeSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionTimeSetupDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get ProductionTimeSetup", e);
                #endregion
            }

            return oAccountHead;
        }

        //

        public ProductionTimeSetup GetByBU(int buid, Int64 nUserId)
        {
            ProductionTimeSetup oAccountHead = new ProductionTimeSetup();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionTimeSetupDA.GetByBU(tc, buid);
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
                throw new ServiceException("Failed to Get ProductionTimeSetup", e);
                #endregion
            }

            return oAccountHead;
        }
        public List<ProductionTimeSetup> Gets(Int64 nUserID)
        {
            List<ProductionTimeSetup> oProductionTimeSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionTimeSetupDA.Gets(tc);
                oProductionTimeSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionTimeSetup", e);
                #endregion
            }

            return oProductionTimeSetup;
        }

        public List<ProductionTimeSetup> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionTimeSetup> oProductionTimeSetup = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionTimeSetupDA.Gets(tc, sSQL);
                oProductionTimeSetup = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionTimeSetup", e);
                #endregion
            }

            return oProductionTimeSetup;
        }
        #endregion
    }
}
