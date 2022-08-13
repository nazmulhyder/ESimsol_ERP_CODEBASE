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
    public class UnitConversionService : MarshalByRefObject, IUnitConversionService
    {
        #region Private functions and declaration
        private UnitConversion MapObject(NullHandler oReader)
        {
            UnitConversion oUnitConversion = new UnitConversion();
            oUnitConversion.UnitConversionID = oReader.GetInt32("UnitConversionID");
            oUnitConversion.MeasurementUnitID = oReader.GetInt32("MeasurementUnitID");
            oUnitConversion.MeasureUnitValue = oReader.GetDouble("MeasureUnitValue");
            oUnitConversion.ConvertedUnitID = oReader.GetInt32("ConvertedUnitID");
            oUnitConversion.ConvertedUnitValue = oReader.GetDouble("ConvertedUnitValue");
            oUnitConversion.MeasurementUnitName = oReader.GetString("MeasurementUnitName");
            oUnitConversion.MeasurementUnitSymbol = oReader.GetString("MeasurementUnitSymbol");
            oUnitConversion.ConvertedUnitName = oReader.GetString("ConvertedUnitName");
            oUnitConversion.ConvertedUnitSymbol = oReader.GetString("ConvertedUnitSymbol");
            oUnitConversion.ProductID = oReader.GetInt32("ProductID");
            oUnitConversion.ProductName = oReader.GetString("ProductName");
            oUnitConversion.ProductCode = oReader.GetString("ProductCode");
            return oUnitConversion;
        }

        private UnitConversion CreateObject(NullHandler oReader)
        {
            UnitConversion oUnitConversion = new UnitConversion();
            oUnitConversion = MapObject(oReader);
            return oUnitConversion;
        }

        private List<UnitConversion> CreateObjects(IDataReader oReader)
        {
            List<UnitConversion> oUnitConversion = new List<UnitConversion>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                UnitConversion oItem = CreateObject(oHandler);
                oUnitConversion.Add(oItem);
            }
            return oUnitConversion;
        }

        #endregion

        #region Interface implementation
        public UnitConversionService() { }

        public UnitConversion Save(UnitConversion oUnitConversion, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
           
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.UnitConversion, EnumRoleOperationType.Add);
                reader = UnitConversionDA.InsertUpdate(tc, oUnitConversion, EnumDBOperation.Insert, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                  oUnitConversion = new UnitConversion();
                  oUnitConversion = CreateObject(oReader);
                          
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
                throw new ServiceException("Failed to Save UnitConversion. Because of " + e.Message, e);
                #endregion
            }
            return oUnitConversion;
        }
        public string Delete(UnitConversion oUnitConversion, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.UnitConversion, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "UnitConversion", oUnitConversion.UnitConversionID);
                UnitConversionDA.Delete(tc, oUnitConversion, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete UnitConversion. Because of " + e.Message.Split('!')[0], e);
                #endregion
            }
            return "Deleted";
        }
        public UnitConversion Get(int id, Int64 nUserId)
        {
            UnitConversion oAccountHead = new UnitConversion();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = UnitConversionDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get UnitConversion", e);
                #endregion
            }

            return oAccountHead;
        }

        public List<UnitConversion> Gets(Int64 nUserId)
        {
            List<UnitConversion> oUnitConversion = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UnitConversionDA.Gets(tc);
                oUnitConversion = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UnitConversion", e);
                #endregion
            }

            return oUnitConversion;
        }
        public List<UnitConversion> Gets(int nProductID, Int64 nUserId)
        {
            List<UnitConversion> oUnitConversion = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UnitConversionDA.Gets(tc, nProductID);
                oUnitConversion = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UnitConversion", e);
                #endregion
            }

            return oUnitConversion;
        }
        public List<UnitConversion> Gets(string sSQL, Int64 nUserId)
        {
            List<UnitConversion> oUnitConversion = new List<UnitConversion>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = UnitConversionDA.Gets(tc, sSQL);
                oUnitConversion = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get UnitConversion", e);
                #endregion
            }

            return oUnitConversion;
        }
        public string CommitUnitConversion(int nProductId, string sCopyProductIDs, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                UnitConversion oUnitConversion = new UnitConversion();
                UnitConversionDA.CommitUnitConversion(tc, nProductId, sCopyProductIDs, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Delete UnitConversion. Because of " + e.Message.Split('!')[0], e);
                #endregion
            }
            return "Successfully Commited";
        }

        #endregion
    }
}