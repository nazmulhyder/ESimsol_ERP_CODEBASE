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
	public class TechnicalSheetShipmentService : MarshalByRefObject, ITechnicalSheetShipmentService
	{
		#region Private functions and declaration

        public static TechnicalSheetShipment MapObject(NullHandler oReader)
		{
			TechnicalSheetShipment oTechnicalSheetShipment = new TechnicalSheetShipment();
			oTechnicalSheetShipment.TechnicalSheetShipmentID = oReader.GetInt32("TechnicalSheetShipmentID");
			oTechnicalSheetShipment.TechnicalSheetID = oReader.GetInt32("TechnicalSheetID");
			oTechnicalSheetShipment.DeliveryDate = oReader.GetString("DeliveryDate");
			oTechnicalSheetShipment.StyleNo = oReader.GetString("StyleNo");
			oTechnicalSheetShipment.Qty = oReader.GetDouble("Qty");
			oTechnicalSheetShipment.Remarks = oReader.GetString("Remarks");
			return oTechnicalSheetShipment;
		}

        public static TechnicalSheetShipment CreateObject(NullHandler oReader)
		{
			TechnicalSheetShipment oTechnicalSheetShipment = new TechnicalSheetShipment();
			oTechnicalSheetShipment = MapObject(oReader);
			return oTechnicalSheetShipment;
		}

		private List<TechnicalSheetShipment> CreateObjects(IDataReader oReader)
		{
			List<TechnicalSheetShipment> oTechnicalSheetShipment = new List<TechnicalSheetShipment>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				TechnicalSheetShipment oItem = CreateObject(oHandler);
				oTechnicalSheetShipment.Add(oItem);
			}
			return oTechnicalSheetShipment;
		}

		#endregion

		#region Interface implementation
			public List<TechnicalSheetShipment> Gets(int TechnicalSheetID, Int64 nUserID)
			{
				List<TechnicalSheetShipment> oTechnicalSheetShipments = new List<TechnicalSheetShipment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = TechnicalSheetShipmentDA.Gets(TechnicalSheetID, tc);
					oTechnicalSheetShipments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null)
					tc.HandleError();
					TechnicalSheetShipment oTechnicalSheetShipment = new TechnicalSheetShipment();
					oTechnicalSheetShipment.ErrorMessage =  e.Message.Split('!')[0];
					#endregion
				}
				return oTechnicalSheetShipments;
			}

			public List<TechnicalSheetShipment> Gets (string sSQL, Int64 nUserID)
			{
				List<TechnicalSheetShipment> oTechnicalSheetShipments = new List<TechnicalSheetShipment>();
				TransactionContext tc = null;
				try
				{
					tc = TransactionContext.Begin();
					IDataReader reader = null;
					reader = TechnicalSheetShipmentDA.Gets(tc, sSQL);
					oTechnicalSheetShipments = CreateObjects(reader);
					reader.Close();
					tc.End();
				}
				catch (Exception e)
				{
					#region Handle Exception
					if (tc != null);
					tc.HandleError();
					ExceptionLog.Write(e);
					throw new ServiceException("Failed to Get TechnicalSheetShipment", e);
					#endregion
				}
				return oTechnicalSheetShipments;
			}

		#endregion
	}

}
