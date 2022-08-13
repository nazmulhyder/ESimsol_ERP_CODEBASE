using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;

namespace ESimSol.Services.Services
{
	public class ModelFeatureService : MarshalByRefObject, IModelFeatureService
	{
		#region Private functions and declaration

		private ModelFeature MapObject(NullHandler oReader)
		{
			ModelFeature oModelFeature = new ModelFeature();
			oModelFeature.ModelFeatureID = oReader.GetInt32("ModelFeatureID");
			oModelFeature.VehicleModelID = oReader.GetInt32("VehicleModelID");
            oModelFeature.FeatureID = oReader.GetInt32("FeatureID");
            oModelFeature.Sequence = oReader.GetInt32("Sequence");
            oModelFeature.FeatureType =  (EnumFeatureType)oReader.GetInt32("FeatureType");
            oModelFeature.FeatureTypeInInt = oReader.GetInt32("FeatureType");
            oModelFeature.FeatureCode = oReader.GetString("FeatureCode");
            oModelFeature.FeatureName = oReader.GetString("FeatureName");
            oModelFeature.Price = oReader.GetDouble("Price");
			return oModelFeature;
		}

		private ModelFeature CreateObject(NullHandler oReader)
		{
			ModelFeature oModelFeature = new ModelFeature();
			oModelFeature = MapObject(oReader);
			return oModelFeature;
		}

		private List<ModelFeature> CreateObjects(IDataReader oReader)
		{
			List<ModelFeature> oModelFeature = new List<ModelFeature>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				ModelFeature oItem = CreateObject(oHandler);
				oModelFeature.Add(oItem);
			}
			return oModelFeature;
		}

		#endregion

		#region Interface implementation
		
        public ModelFeature Get(int id, Int64 nUserId)
			{
				ModelFeature oModelFeature = new ModelFeature();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = ModelFeatureDA.Get(tc, id);
					NullHandler oReader = new NullHandler(reader);
					if (reader.Read())
					{
					oModelFeature = CreateObject(oReader);
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
					throw new ServiceException("Failed to Get ModelFeature", e);
					#endregion
				}
				return oModelFeature;
			}

		public List<ModelFeature> Gets(int id, Int64 nUserID)
		{
			List<ModelFeature> oModelFeatures = new List<ModelFeature>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = ModelFeatureDA.Gets(tc, id);
				oModelFeatures = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ModelFeature oModelFeature = new ModelFeature();
				oModelFeature.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oModelFeatures;
		}

	   public List<ModelFeature> Gets (string sSQL, Int64 nUserID)
			{
				List<ModelFeature> oModelFeatures = new List<ModelFeature>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = ModelFeatureDA.Gets(tc, sSQL);
					oModelFeatures = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get ModelFeature", e);
					#endregion
				}
				return oModelFeatures;
			}

		#endregion
	}

}
