using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;


using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class ProductionUnitService : MarshalByRefObject, IProductionUnitService
    {
        #region Private functions and declaration
        private ProductionUnit MapObject(NullHandler oReader)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            oProductionUnit.ProductionUnitID = oReader.GetInt32("ProductionUnitID");
            oProductionUnit.Name = oReader.GetString("Name");
            oProductionUnit.ShortName = oReader.GetString("ShortName");
            oProductionUnit.RefName = oReader.GetString("RefName");
            oProductionUnit.ProductionUnitType = (EnumProductionUnitType)(oReader.GetInt32("ProductionUnitType"));
            oProductionUnit.RefID = oReader.GetInt32("RefID");
            oProductionUnit.FBUID = oReader.GetInt32("FBUID");
            return oProductionUnit;
        }

        private ProductionUnit CreateObject(NullHandler oReader)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            oProductionUnit = MapObject(oReader);
            return oProductionUnit;
        }

        private List<ProductionUnit> CreateObjects(IDataReader oReader)
        {
            List<ProductionUnit> oProductionUnits = new List<ProductionUnit>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                ProductionUnit oItem = CreateObject(oHandler);
                oProductionUnits.Add(oItem);
            }
            return oProductionUnits;
        }

        #endregion

        #region Interface implementation
        public ProductionUnitService() { }


        public ProductionUnit Save(ProductionUnit oProductionUnit, Int64 nUserId)
        {

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin(true);
                #region ProductionUnit
                IDataReader reader;
                if (oProductionUnit.ProductionUnitID <= 0)
                {
                    reader = ProductionUnitDA.InsertUpdate(tc, oProductionUnit, EnumDBOperation.Insert, nUserId);
                }
                else
                {
                    reader = ProductionUnitDA.InsertUpdate(tc, oProductionUnit, EnumDBOperation.Update, nUserId);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionUnit = new ProductionUnit();
                    oProductionUnit = CreateObject(oReader);
                }
                reader.Close();
                #endregion

                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oProductionUnit = new ProductionUnit();
                oProductionUnit.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oProductionUnit;
        }

        public String Delete(ProductionUnit oProductionUnit, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                ProductionUnitDA.Delete(tc, oProductionUnit, EnumDBOperation.Delete, nUserID);
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
        public ProductionUnit Get(int id, Int64 nUserId)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionUnitDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductionUnit;
        }

        public List<ProductionUnit> Gets(string sSQL, Int64 nUserID)
        {
            List<ProductionUnit> oProductionUnit = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionUnitDA.Gets(sSQL, tc);
                oProductionUnit = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get ProductionUnit", e);
                #endregion
            }
            return oProductionUnit;
        }
        public ProductionUnit GetByType(int nRequisitionType, Int64 nUserId)
        {
            ProductionUnit oProductionUnit = new ProductionUnit();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = ProductionUnitDA.GetByType(tc, nRequisitionType);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oProductionUnit = CreateObject(oReader);
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
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductionUnit;
        }


        public List<ProductionUnit> Gets(Int64 nUserId)
        {
            List<ProductionUnit> oProductionUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionUnitDA.Gets(tc);
                oProductionUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductionUnits;
        }
        public List<ProductionUnit> Gets(int nBUID, Int64 nUserId)
        {
            List<ProductionUnit> oProductionUnits = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = ProductionUnitDA.Gets(tc, nBUID);
                oProductionUnits = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get LC Terms", e);
                #endregion
            }

            return oProductionUnits;
        }

        #endregion
    }
}