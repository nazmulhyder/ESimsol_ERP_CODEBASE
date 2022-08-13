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
	public class FabricPlanningService : MarshalByRefObject, IFabricPlanningService
	{
		#region Private functions and declaration

		private FabricPlanning MapObject(NullHandler oReader)
		{
			FabricPlanning oFabricPlanning = new FabricPlanning();
			oFabricPlanning.FabricPlanningID = oReader.GetInt32("FabricPlanningID");
			oFabricPlanning.FabricID = oReader.GetInt32("FabricID");
			oFabricPlanning.ProductID = oReader.GetInt32("ProductID");
            oFabricPlanning.IsWarp = oReader.GetBoolean("IsWarp");
            oFabricPlanning.ProductName = oReader.GetString("ProductName");
            oFabricPlanning.Color = oReader.GetString("Color");
            oFabricPlanning.ColorNo = oReader.GetString("ColorNo");
            oFabricPlanning.PantonNo = oReader.GetString("PantonNo");
            oFabricPlanning.RGB = oReader.GetString("RGB");
            oFabricPlanning.Value = oReader.GetInt32("Value");
            oFabricPlanning.ComboNo = oReader.GetInt32("ComboNo");
            oFabricPlanning.RepeatNo = oReader.GetInt32("RepeatNo");
            oFabricPlanning.Count1 = oReader.GetInt32("Count1");
            oFabricPlanning.Count2 = oReader.GetInt32("Count2");
            oFabricPlanning.Count3 = oReader.GetInt32("Count3");
            oFabricPlanning.Count4 = oReader.GetInt32("Count4");
            oFabricPlanning.Count5 = oReader.GetInt32("Count5");
            oFabricPlanning.Count6 = oReader.GetInt32("Count6");
            oFabricPlanning.Count7 = oReader.GetInt32("Count7");
            oFabricPlanning.Count8 = oReader.GetInt32("Count8");
            oFabricPlanning.Count9 = oReader.GetInt32("Count9");
            oFabricPlanning.Count10 = oReader.GetInt32("Count10");
            oFabricPlanning.Count11 = oReader.GetInt32("Count11");
            oFabricPlanning.Count12 = oReader.GetInt32("Count12");
            oFabricPlanning.Count13 = oReader.GetInt32("Count13");
            oFabricPlanning.Count14 = oReader.GetInt32("Count14");
            oFabricPlanning.Count15 = oReader.GetInt32("Count15");
            oFabricPlanning.SLNo = oReader.GetInt32("SLNo");
            oFabricPlanning.EndsCount = oReader.GetInt32("EndsCount");
			return oFabricPlanning;
		}

		private FabricPlanning CreateObject(NullHandler oReader)
		{
			FabricPlanning oFabricPlanning = new FabricPlanning();
			oFabricPlanning = MapObject(oReader);
			return oFabricPlanning;
		}

		private List<FabricPlanning> CreateObjects(IDataReader oReader)
		{
			List<FabricPlanning> oFabricPlanning = new List<FabricPlanning>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricPlanning oItem = CreateObject(oHandler);
				oFabricPlanning.Add(oItem);
			}
			return oFabricPlanning;
		}

		#endregion

		#region Interface implementation
		public FabricPlanning Save(FabricPlanning oFabricPlanning, Int64 nUserID)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oFabricPlanning.FabricPlanningID <= 0)
				{
					reader = FabricPlanningDA.InsertUpdate(tc, oFabricPlanning, EnumDBOperation.Insert, nUserID);
				}
				else{
					reader = FabricPlanningDA.InsertUpdate(tc, oFabricPlanning, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oFabricPlanning = new FabricPlanning();
					oFabricPlanning = CreateObject(oReader);
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
					oFabricPlanning = new FabricPlanning();
					oFabricPlanning.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oFabricPlanning;
		}

		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				FabricPlanning oFabricPlanning = new FabricPlanning();
				oFabricPlanning.FabricPlanningID = id;
				DBTableReferenceDA.HasReference(tc, "FabricPlanning", id);
				FabricPlanningDA.Delete(tc, oFabricPlanning, EnumDBOperation.Delete, nUserId);
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exceptionif (tc != null)
				tc.HandleError();
				return e.Message.Split('!')[0];
				#endregion
			}
			return Global.DeleteMessage;
		}

		public FabricPlanning Get(int id, Int64 nUserId)
		{
			FabricPlanning oFabricPlanning = new FabricPlanning();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = FabricPlanningDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oFabricPlanning = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get FabricPlanning", e);
				#endregion
			}
			return oFabricPlanning;
		}

		public List<FabricPlanning> Gets(Int64 nUserID)
		{
			List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricPlanningDA.Gets(tc);
				oFabricPlannings = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FabricPlanning oFabricPlanning = new FabricPlanning();
				oFabricPlanning.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFabricPlannings;
		}

		public List<FabricPlanning> Gets (string sSQL, Int64 nUserID)
		{
			List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricPlanningDA.Gets(tc, sSQL);
				oFabricPlannings = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FabricPlanning", e);
				#endregion
			}
			return oFabricPlannings;
		}
        public List<FabricPlanning> MakeCombo(string sFabricPlanningID, int nFabricID, int nComboNo, int nDBOperation, int nUserID)
        {
            List<FabricPlanning> oFabricPlannings = new List<FabricPlanning>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPlanningDA.MakeCombo(tc, sFabricPlanningID, nFabricID, nComboNo, nDBOperation, nUserID);
                oFabricPlannings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlanning oFabricPlanning = new FabricPlanning();
                oFabricPlanning.ErrorMessage = e.Message;
                oFabricPlannings.Add(oFabricPlanning);
                #endregion
            }

            return oFabricPlannings;
        }
		#endregion
	}

}
