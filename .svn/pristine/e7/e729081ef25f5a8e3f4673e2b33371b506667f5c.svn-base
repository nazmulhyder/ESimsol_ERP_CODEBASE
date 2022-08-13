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
	public class LabdipChallanService : MarshalByRefObject, ILabdipChallanService
	{
		#region Private functions and declaration

		private LabdipChallan MapObject(NullHandler oReader)
		{
			LabdipChallan oLabdipChallan = new LabdipChallan();
			oLabdipChallan.LabdipChallanID = oReader.GetInt32("LabdipChallanID");
			oLabdipChallan.ChallanDate = oReader.GetDateTime("ChallanDate");
			oLabdipChallan.ChallanNo = oReader.GetString("ChallanNo");
			oLabdipChallan.ContractorID = oReader.GetInt32("ContractorID");
			oLabdipChallan.DeliveryZoneID  = oReader.GetInt32("DeliveryZoneID");
            oLabdipChallan.Status = (EnumLabDipChallanStatus)oReader.GetInt32("Status");
            oLabdipChallan.ContractorName = oReader.GetString("ContractorName");
            oLabdipChallan.Contractor_Address = oReader.GetString("ContractorAddress");
            oLabdipChallan.ChallanNoFull = oReader.GetString("ChallanNoFull");
            oLabdipChallan.DeliveryZoneName = oReader.GetString("DeliveryZoneName");
            oLabdipChallan.PrepareBy = oReader.GetString("PrepareBy");
            oLabdipChallan.ColorCount = oReader.GetInt32("ColorCount");
            oLabdipChallan.Remarks = oReader.GetString("Remarks");
			return oLabdipChallan;
		}

		private LabdipChallan CreateObject(NullHandler oReader)
		{
			LabdipChallan oLabdipChallan = new LabdipChallan();
			oLabdipChallan = MapObject(oReader);
			return oLabdipChallan;
		}

		private List<LabdipChallan> CreateObjects(IDataReader oReader)
		{
			List<LabdipChallan> oLabdipChallan = new List<LabdipChallan>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				LabdipChallan oItem = CreateObject(oHandler);
				oLabdipChallan.Add(oItem);
			}
			return oLabdipChallan;
		}

		#endregion

		#region Interface implementation
		public LabdipChallan Save(LabdipChallan oLabdipChallan, Int64 nUserID)
		{
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            oLabDipDetails = oLabdipChallan.LabDipDetails;

			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				IDataReader reader;
				if (oLabdipChallan.LabdipChallanID <= 0)
				{
					reader = LabdipChallanDA.InsertUpdate(tc, oLabdipChallan, EnumDBOperation.Insert, nUserID);
				}
				else{
					reader = LabdipChallanDA.InsertUpdate(tc, oLabdipChallan, EnumDBOperation.Update, nUserID);
				}
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
					oLabdipChallan = new LabdipChallan();
					oLabdipChallan = CreateObject(oReader);
				}
				reader.Close();

                #region LabDipDetail Part

                foreach (LabDipDetail oItem in oLabDipDetails)
                {
                    IDataReader readerdetail;
                    oItem.LabdipChallanID = oLabdipChallan.LabdipChallanID;
                    if (oItem.LabdipChallanID > 0)
                    {
                        readerdetail = LabdipChallanDA.UpdateLabDipDetails(tc, oItem);
                        readerdetail.Close();
                    }
                }
                #endregion

				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				{
					tc.HandleError();
					oLabdipChallan = new LabdipChallan();
					oLabdipChallan.ErrorMessage = e.Message.Split('!')[0];
				}
				#endregion
			}
			return oLabdipChallan;
		}
        public LabdipChallan UpdateStatus(LabdipChallan oLabdipChallan, Int64 nUserID)
        {
            List<LabDipDetail> oLabDipDetails = new List<LabDipDetail>();
            oLabDipDetails = oLabdipChallan.LabDipDetails;

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = LabdipChallanDA.UpdateStatus(tc, oLabdipChallan, EnumDBOperation.Insert, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oLabdipChallan = new LabdipChallan();
                    oLabdipChallan = CreateObject(oReader);
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
                    oLabdipChallan = new LabdipChallan();
                    oLabdipChallan.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oLabdipChallan;
        }    
        public string RemoveDetail(int id, Int64 nUserId)
            {
                TransactionContext tc = null;
                try
                {
                    tc = TransactionContext.Begin(true);
                    DBTableReferenceDA.HasReference(tc, "LabDipDetail", id);
                    LabdipChallanDA.RemoveLabDipDetail(tc, id);
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
		public string Delete(int id, Int64 nUserId)
		{
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin(true);
				LabdipChallan oLabdipChallan = new LabdipChallan();
				oLabdipChallan.LabdipChallanID = id;
				DBTableReferenceDA.HasReference(tc, "LabdipChallan", id);
				LabdipChallanDA.Delete(tc, oLabdipChallan, EnumDBOperation.Delete, nUserId);
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

		public LabdipChallan Get(int id, Int64 nUserId)
		{
			LabdipChallan oLabdipChallan = new LabdipChallan();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = LabdipChallanDA.Get(tc, id);
				NullHandler oReader = new NullHandler(reader);
				if (reader.Read())
				{
				oLabdipChallan = CreateObject(oReader);
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
				throw new ServiceException("Failed to Get LabdipChallan", e);
				#endregion
			}
			return oLabdipChallan;
		}

		public List<LabdipChallan> Gets(Int64 nUserID)
		{
			List<LabdipChallan> oLabdipChallans = new List<LabdipChallan>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LabdipChallanDA.Gets(tc);
				oLabdipChallans = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null)
				tc.HandleError();
				LabdipChallan oLabdipChallan = new LabdipChallan();
				oLabdipChallan.ErrorMessage =  e.Message.Split('!')[0];
				#endregion
			}
			return oLabdipChallans;
		}

		public List<LabdipChallan> Gets (string sSQL, Int64 nUserID)
		{
			List<LabdipChallan> oLabdipChallans = new List<LabdipChallan>();
			TransactionContext tc = null;
			try
			{
				tc = TransactionContext.Begin();
				IDataReader reader = null;
				reader = LabdipChallanDA.Gets(tc, sSQL);
				oLabdipChallans = CreateObjects(reader);
				reader.Close();
				tc.End();
			}
			catch (Exception e)
			{
				#region Handle Exception
				if (tc != null);
				tc.HandleError();
				ExceptionLog.Write(e);
				throw new ServiceException("Failed to Get LabdipChallan", e);
				#endregion
			}
			return oLabdipChallans;
		}

		#endregion
	}

}
