using System;
using ICS.Core.Framework;
using ICS.Core.Utility;
using ICS.Core;
using ESimSol.BusinessObjects;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.ServiceModel;
using ESimSol.BusinessObjects.ReportingObject;

namespace ESimSol.BusinessObjects
{
    public class KnitDyeingYarnBooking : BusinessObject
    {
        public KnitDyeingYarnBooking()
		{
            KnitDyeingYarnBookingID = 0;
            KnitDyeingPTUID = 0;
            CompositionID = 0;
            LotID = 0;
            MUnitID = 0;
            BookingQty = 0;
            Remarks = "";
            LotNo = "";
            StoreName = "";
            StoreID = 0;
            CompositionName = "";
			ErrorMessage = "";
            BookingByName = "";
            IDs = "";
            BookingBy = 0;
            MUSymbol = "";
		}

        #region Property
        public int KnitDyeingYarnBookingID { get; set; }
        public int KnitDyeingPTUID { get; set; }
        public int CompositionID { get; set; }
        public int BookingBy { get; set; }
        public int StoreID { get; set; }
        public int MUnitID { get; set; }
        public int LotID { get; set; }
        public double BookingQty { get; set; }
        public string BookingByName { get; set; }
        public string CompositionName { get; set; }
        public string LotNo { get; set; }
        public string StoreName { get; set; }
        public string Remarks { get; set; }
        public string ErrorMessage { get; set; }
        public string MUSymbol { get; set; }
        public string IDs { get; set; }
        #endregion 

  
        public KnitDyeingYarnBooking Save(long nUserID)
        {
            return KnitDyeingYarnBooking.Service.Save(this, nUserID);
        }
        public KnitDyeingYarnBooking Approve(long nUserID)
        {
            return KnitDyeingYarnBooking.Service.Approve(this, nUserID);
        }
        	public  string  Delete(int id, long nUserID)
		{
            return KnitDyeingYarnBooking.Service.Delete(id, nUserID);
		}
            public static List<KnitDyeingYarnBooking> Gets(string sSQL, long nUserID)
            {
                return KnitDyeingYarnBooking.Service.Gets(sSQL, nUserID);
            }
        #region ServiceFactory
        internal static IKnitDyeingYarnBookingService Service
        {
            get { return (IKnitDyeingYarnBookingService)Services.Factory.CreateService(typeof(IKnitDyeingYarnBookingService)); }
        }
        #endregion
    }

    #region IKnitDyeingYarnBookingService interface
    public interface IKnitDyeingYarnBookingService
    {
        KnitDyeingYarnBooking Save(KnitDyeingYarnBooking oKnitDyeingYarnBooking,long nUserID);
        KnitDyeingYarnBooking Approve(KnitDyeingYarnBooking oKnitDyeingYarnBooking, long nUserID);
        string Delete(int id, Int64 nUserID);
        List<KnitDyeingYarnBooking> Gets(string sSQL, Int64 nUserID);
    }
    #endregion
}
