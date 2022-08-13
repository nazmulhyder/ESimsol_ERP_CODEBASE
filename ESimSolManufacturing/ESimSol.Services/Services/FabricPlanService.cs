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
	public class FabricPlanService : MarshalByRefObject, IFabricPlanService
	{
		#region Private functions and declaration

		private FabricPlan MapObject(NullHandler oReader)
		{
			FabricPlan oFabricPlan = new FabricPlan();
			oFabricPlan.FabricPlanID = oReader.GetInt32("FabricPlanID");
			oFabricPlan.FabricID = oReader.GetInt32("FabricID");
			oFabricPlan.ProductID = oReader.GetInt32("ProductID");
            oFabricPlan.WarpWeftType = (EnumWarpWeft)oReader.GetInt16("WarpWeftType");
            oFabricPlan.ProductName = oReader.GetString("ProductName");
            oFabricPlan.Color = oReader.GetString("Color");
            oFabricPlan.ColorNo = oReader.GetString("ColorNo");
            oFabricPlan.PantonNo = oReader.GetString("PantonNo");
            oFabricPlan.RGB = oReader.GetString("RGB");
            oFabricPlan.Value = oReader.GetInt32("Value");
            oFabricPlan.TwistedGroup = oReader.GetInt32("TwistedGroup");
            //oFabricPlan.RepeatNo = oReader.GetInt32("RepeatNo");
            oFabricPlan.SLNo = oReader.GetInt32("SLNo");
            oFabricPlan.EndsCount = oReader.GetInt32("EndsCount");
            oFabricPlan.FabricPlanOrderID = oReader.GetInt32("FabricPlanOrderID");
            oFabricPlan.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
			return oFabricPlan;
		}

		private FabricPlan CreateObject(NullHandler oReader)
		{
			FabricPlan oFabricPlan = new FabricPlan();
			oFabricPlan = MapObject(oReader);
			return oFabricPlan;
		}

		private List<FabricPlan> CreateObjects(IDataReader oReader)
		{
			List<FabricPlan> oFabricPlan = new List<FabricPlan>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				FabricPlan oItem = CreateObject(oHandler);
				oFabricPlan.Add(oItem);
			}
			return oFabricPlan;
		}

		#endregion

		#region Interface implementation
		public FabricPlan Save(FabricPlan oFabricPlan, Int64 nUserID)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oFabricPlan.FabricPlanID <= 0)
				{
					reader = FabricPlanDA.InsertUpdate(tc, oFabricPlan, EnumDBOperation.Insert, nUserID);
				}
				else{
					reader = FabricPlanDA.InsertUpdate(tc, oFabricPlan, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oFabricPlan = new FabricPlan();
					oFabricPlan = CreateObject(oReader);
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
					oFabricPlan = new FabricPlan();
					oFabricPlan.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oFabricPlan;
		}

        public string Delete(List<FabricPlan> oFabricPlans, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
                foreach (FabricPlan oitem in oFabricPlans)
                {
                    //DBTableReferenceDA.HasReference(tc, "FabricPlan", id);
                    FabricPlanDA.Delete(tc, oitem, EnumDBOperation.Delete, nUserId);
                }
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
        public string UpdateYarn(List<FabricPlan> oFabricPlans, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                foreach (FabricPlan oitem in oFabricPlans)
                {
                    //DBTableReferenceDA.HasReference(tc, "FabricPlan", id);
                    FabricPlanDA.UpdateYarn(tc, oitem.FabricPlanID, oitem.ProductID, nUserId);
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Saved";
        }

		public FabricPlan Get(int id, Int64 nUserId)
		{
			FabricPlan oFabricPlan = new FabricPlan();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = FabricPlanDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oFabricPlan = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get FabricPlan", e);
				#endregion
			}
			return oFabricPlan;
		}

		public List<FabricPlan> Gets(Int64 nUserID)
		{
			List<FabricPlan> oFabricPlans = new List<FabricPlan>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricPlanDA.Gets(tc);
				oFabricPlans = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				FabricPlan oFabricPlan = new FabricPlan();
				oFabricPlan.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oFabricPlans;
		}

		public List<FabricPlan> Gets (string sSQL, Int64 nUserID)
		{
			List<FabricPlan> oFabricPlans = new List<FabricPlan>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = FabricPlanDA.Gets(tc, sSQL);
				oFabricPlans = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get FabricPlan", e);
				#endregion
			}
			return oFabricPlans;
		}
        public List<FabricPlan> MakeCombo(string sFabricPlanID, int nFabricPlanOrderID, int nComboNo, int nDBOperation, int nUserID)
        {
            List<FabricPlan> oFabricPlans = new List<FabricPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricPlanDA.MakeCombo(tc, sFabricPlanID, nFabricPlanOrderID, nComboNo, nDBOperation, nUserID);
                oFabricPlans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlan oFabricPlan = new FabricPlan();
                oFabricPlan.ErrorMessage = e.Message;
                oFabricPlans.Add(oFabricPlan);
                #endregion
            }

            return oFabricPlans;
        }
   
        public List<FabricPlan> SaveSequence(List<FabricPlan> FabricPlans,  Int64 nUserId)
        {
            List<FabricPlan> oFabricPlansRet = new List<FabricPlan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                if (FabricPlans != null)
                {
                    foreach (FabricPlan oItem in FabricPlans)
                    {
                        if (oItem.FabricPlanID > 0 && oItem.SLNo > 0)
                        {
                            FabricPlanDA.UpdateSequence(tc, oItem);
                        }
                        oFabricPlansRet.Add(oItem);
                    }
                }
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                FabricPlan oFabricPlan = new FabricPlan();
                oFabricPlan.ErrorMessage = e.Message;
                oFabricPlansRet.Add(oFabricPlan);
                #endregion
            }

            return oFabricPlansRet;
        }

        public FabricPlan UpdateLDDetailID(FabricPlan oFabricPlan, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricPlanDA.UpdateLDDetailID(tc, oFabricPlan);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricPlan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricPlan = new FabricPlan();
                oFabricPlan.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFabricPlan;
        }

        //public List<FabricPlan> CopyFabricPlans(FabricPlan oFabricPlan, Int64 nUserID)
        //{
        //    List<FabricPlan> oFabricPlans = new List<FabricPlan>();
        //    TransactionContext tc = null;
        //    try
        //    {
        //        tc = TransactionContext.Begin();
        //        IDataReader reader = null;
        //        reader = FabricPlanDA.CopyFabricPlan(tc, oFabricPlan, EnumDBOperation.Update, nUserID);
        //        oFabricPlans = CreateObjects(reader);
        //        reader.Close();
        //        tc.End();
        //    }
        //    catch (Exception e)
        //    {
        //        #region Handle Exception
        //        if (tc != null) ;
        //        tc.HandleError();
        //        oFabricPlans = new List<FabricPlan>();
        //        oFabricPlan = new FabricPlan();
        //        oFabricPlan.ErrorMessage = e.Message.Split('~')[0];
        //        oFabricPlans.Add(oFabricPlan);
        //        //ExceptionLog.Write(e);
        //        //throw new ServiceException("Failed to Get FabricPlan", e);
        //        #endregion
        //    }
        //    return oFabricPlans;
        //}

		#endregion
	}

}
