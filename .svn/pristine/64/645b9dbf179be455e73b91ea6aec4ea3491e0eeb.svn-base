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

namespace ESimSol.Services
{
    public class KnitDyeingYarnBookingService : MarshalByRefObject, IKnitDyeingYarnBookingService
    {
        private KnitDyeingYarnBooking MapObject(NullHandler oReader)
        {
            KnitDyeingYarnBooking oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
            oKnitDyeingYarnBooking.KnitDyeingYarnBookingID = oReader.GetInt32("KnitDyeingYarnBookingID");
            oKnitDyeingYarnBooking.KnitDyeingPTUID = oReader.GetInt32("KnitDyeingPTUID");
            oKnitDyeingYarnBooking.CompositionID = oReader.GetInt32("CompositionID");
            oKnitDyeingYarnBooking.LotID = oReader.GetInt32("LotID");
            oKnitDyeingYarnBooking.StoreID = oReader.GetInt32("StoreID");
            oKnitDyeingYarnBooking.BookingBy = oReader.GetInt32("BookingBy");
            oKnitDyeingYarnBooking.MUnitID = oReader.GetInt32("MUnitID");
            oKnitDyeingYarnBooking.Remarks = oReader.GetString("Remarks");
            oKnitDyeingYarnBooking.CompositionName = oReader.GetString("CompositionName");
            oKnitDyeingYarnBooking.LotNo = oReader.GetString("LotNo");
            oKnitDyeingYarnBooking.StoreName = oReader.GetString("StoreName");
            oKnitDyeingYarnBooking.BookingQty = oReader.GetDouble("BookingQty");
            oKnitDyeingYarnBooking.BookingByName = oReader.GetString("BookingByName");
            oKnitDyeingYarnBooking.MUSymbol = oReader.GetString("MUSymbol");
            return oKnitDyeingYarnBooking;
        }
        private KnitDyeingYarnBooking CreateObject(NullHandler oReader)
		{
			KnitDyeingYarnBooking oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
			oKnitDyeingYarnBooking = MapObject(oReader);
			return oKnitDyeingYarnBooking;
		}

		private List<KnitDyeingYarnBooking> CreateObjects(IDataReader oReader)
		{
			List<KnitDyeingYarnBooking> oKnitDyeingYarnBooking = new List<KnitDyeingYarnBooking>();
			NullHandler oHandler = new NullHandler(oReader);
			while (oReader.Read())
			{
				KnitDyeingYarnBooking oItem = CreateObject(oHandler);
				oKnitDyeingYarnBooking.Add(oItem);
			}
			return oKnitDyeingYarnBooking;
		}

        public KnitDyeingYarnBooking Save(KnitDyeingYarnBooking oKnitDyeingYarnBooking, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                if (oKnitDyeingYarnBooking.KnitDyeingYarnBookingID <= 0)
                {
                    reader = KnitDyeingYarnBookingDA.InsertUpdate(tc, oKnitDyeingYarnBooking, EnumDBOperation.Insert, nUserID);
                }
                else
                {
                    reader = KnitDyeingYarnBookingDA.InsertUpdate(tc, oKnitDyeingYarnBooking, EnumDBOperation.Update, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                    oKnitDyeingYarnBooking = CreateObject(oReader);
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
                    oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                    oKnitDyeingYarnBooking.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingYarnBooking;
        }

        public KnitDyeingYarnBooking Approve(KnitDyeingYarnBooking oKnitDyeingYarnBooking, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;

                reader = KnitDyeingYarnBookingDA.InsertUpdate(tc, oKnitDyeingYarnBooking, EnumDBOperation.Approval, nUserID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                    oKnitDyeingYarnBooking = CreateObject(oReader);
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
                    oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                    oKnitDyeingYarnBooking.ErrorMessage = e.Message.Split('!')[0];
                }
                #endregion
            }
            return oKnitDyeingYarnBooking;
        }
 
        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                KnitDyeingYarnBooking oKnitDyeingYarnBooking = new KnitDyeingYarnBooking();
                oKnitDyeingYarnBooking.KnitDyeingYarnBookingID = id;
                KnitDyeingYarnBookingDA.Delete(tc, oKnitDyeingYarnBooking, EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exceptionif (tc != null)
                tc.HandleError();
                return e.Message.Split('!')[0];
                #endregion
            }
            return "Data delete successfully";
        }
        public List<KnitDyeingYarnBooking> Gets(string sSQL, Int64 nUserID)
        {
            List<KnitDyeingYarnBooking> oKnitDyeingYarnBookings = new List<KnitDyeingYarnBooking>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = KnitDyeingYarnBookingDA.Gets(tc, sSQL);
                oKnitDyeingYarnBookings = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get KnitDyeingYarnBooking", e);
                #endregion
            }
            return oKnitDyeingYarnBookings;
        }
		
    }
}
