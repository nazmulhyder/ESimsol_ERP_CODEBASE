using System;
using System.IO;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;

namespace ESimSol.BusinessObjects
{
    #region EmployeeProductionReceiveDetailSheet

    public class EmployeeProductionReceiveDetail : BusinessObject
    {
        public EmployeeProductionReceiveDetail()
        {
            EPSRDID = 0;
            EPSID = 0;
            RcvQty = 0;
            Rate = 0;
            CurrencyID = 0;
            RcvBy = 0;
            RcvByDate = DateTime.Now;
            ErrorMessage = "";
            GarmentPart = 0;
            GPName = "";
        }

        #region Properties
        public int EPSRDID { get; set; }
        public int EPSID { get; set; }
        public double RcvQty { get; set; }
        public double Rate { get; set; }
        public int CurrencyID { get; set; }
        public int RcvBy { get; set; }
        public DateTime RcvByDate { get; set; }

        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property

        public string StyleNo { get; set; }
        public string ColorName { get; set; }
        public string SizeCategoryName { get; set; }
        public int GarmentPart { get; set; }
        public double ProductionRate { get; set; }
        public string ProductionNote { get; set; }
        public double NewRate
        {
            get
            {
                if (this.Rate <= 0)
                {
                    return ProductionRate;
                }
                else
                {
                    return Rate;
                }
            }
        }

        public string RcvByDateInString
        {
            get
            {
                return RcvByDate.ToString("dd MMM yyyy");
            }
        }

        //public int GarmentPartInt { get; set; }
        //public string GarmentPartInString
        //{
        //    get
        //    {
        //        return GarmentPart.ToString();
        //    }
        //}

        public string GPName { get; set; }
        public string Color_Size_BodyPart
        {
            get
            {
                return ColorName + "[" + SizeCategoryName + "] - " + GPName;
            }


        }

        #endregion

        #region Functions
        public static EmployeeProductionReceiveDetail Get(int id, long nUserID)
        {
            return EmployeeProductionReceiveDetail.Service.Get(id, nUserID);
        }

        public static List<EmployeeProductionReceiveDetail> Gets(long nUserID)
        {
            return EmployeeProductionReceiveDetail.Service.Gets(nUserID);
        }

        public static List<EmployeeProductionReceiveDetail> Gets(string sSQL, long nUserID)
        {
            return EmployeeProductionReceiveDetail.Service.Gets(sSQL, nUserID);
        }

        public EmployeeProductionReceiveDetail IUD(int nDBOperation, long nUserID)
        {
            return EmployeeProductionReceiveDetail.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IEmployeeProductionReceiveDetailService Service
        {
            get { return (IEmployeeProductionReceiveDetailService)Services.Factory.CreateService(typeof(IEmployeeProductionReceiveDetailService)); }
        }

        #endregion
    }
    #endregion

    #region IEmployeeProductionReceiveDetail interface

    public interface IEmployeeProductionReceiveDetailService
    {
        EmployeeProductionReceiveDetail Get(int id, Int64 nUserID);
        List<EmployeeProductionReceiveDetail> Gets(Int64 nUserID);
        List<EmployeeProductionReceiveDetail> Gets(string sSQL, Int64 nUserID);
        EmployeeProductionReceiveDetail IUD(EmployeeProductionReceiveDetail oEmployeeProductionReceiveDetailSheet, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
