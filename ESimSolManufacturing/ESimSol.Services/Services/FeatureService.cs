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
    public class FeatureService : MarshalByRefObject, IFeatureService
    {
        #region Private functions and declaration
        private Feature MapObject(NullHandler oReader)
        {
            Feature oFeature = new Feature();
            oFeature.FeatureID = oReader.GetInt32("FeatureID");
            oFeature.FeatureCode = oReader.GetString("FeatureCode");
            oFeature.FeatureName = oReader.GetString("FeatureName");
            oFeature.FeatureType = (EnumFeatureType)oReader.GetInt32("FeatureType");
            oFeature.FeatureTypeInInt = oReader.GetInt32("FeatureType");
            oFeature.CurrencyID = oReader.GetInt32("CurrencyID");
            oFeature.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oFeature.UnitPrice = oReader.GetDouble("UnitPrice");
            oFeature.VatAmount = oReader.GetDouble("VatAmount");
            oFeature.Price = oReader.GetDouble("Price");
            oFeature.Remarks = oReader.GetString("Remarks");
            oFeature.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oFeature.CurrencyName = oReader.GetString("CurrencyName");
            oFeature.ModelShortName = oReader.GetString("ModelShortName");
            oFeature.ModelNo = oReader.GetString("ModelNo");
            
            return oFeature;
        }

        private Feature CreateObject(NullHandler oReader)
        {
            Feature oFeature = new Feature();
            oFeature = MapObject(oReader);
            return oFeature;
        }

        private List<Feature> CreateObjects(IDataReader oReader)
        {
            List<Feature> oFeature = new List<Feature>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                Feature oItem = CreateObject(oHandler);
                oFeature.Add(oItem);
            }
            return oFeature;
        }

        #endregion

        #region Interface implementation
        public FeatureService() { }

        public Feature Save(Feature oFeature, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oFeature.FeatureID <= 0)
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Feature, EnumRoleOperationType.Add);
                    reader = FeatureDA.InsertUpdate(tc, oFeature, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    AuthorizationRoleDA.CheckUserPermission(tc, nUserID, EnumModuleName.Feature, EnumRoleOperationType.Edit);
                    reader = FeatureDA.InsertUpdate(tc, oFeature, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFeature = new Feature();
                    oFeature = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                oFeature.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFeature;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                Feature oFeature = new Feature();
                oFeature.FeatureID = id;
                AuthorizationRoleDA.CheckUserPermission(tc, nUserId, EnumModuleName.Feature, EnumRoleOperationType.Delete);
                DBTableReferenceDA.HasReference(tc, "Feature", id);
                FeatureDA.Delete(tc, oFeature, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }

        public Feature Get(int id, Int64 nUserId)
        {
            Feature oAccountHead = new Feature();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FeatureDA.Get(tc, id);
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
                throw new ServiceException("Failed to Get Feature", e);
                #endregion
            }

            return oAccountHead;
        }
        
     
        public List<Feature> Gets(Int64 nUserID)
        {
            List<Feature> oFeature = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FeatureDA.Gets(tc);
                oFeature = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Feature", e);
                #endregion
            }

            return oFeature;
        }

        public List<Feature> GetsbyFeatureNameWithType(string sFeatureName, string sFTypes, Int64 nUserID)
        {
            List<Feature> oFeature = new List<Feature>();

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FeatureDA.GetsbyFeatureNameWithType(tc, sFeatureName, sFTypes);
                oFeature = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Feature", e);
                #endregion
            }

            return oFeature;
        }


        public List<Feature> Gets(string sSQL, Int64 nUserID)
        {
            List<Feature> oFeature = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FeatureDA.Gets(tc, sSQL);
                oFeature = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Feature", e);
                #endregion
            }

            return oFeature;
        }
        #endregion
    }
}
