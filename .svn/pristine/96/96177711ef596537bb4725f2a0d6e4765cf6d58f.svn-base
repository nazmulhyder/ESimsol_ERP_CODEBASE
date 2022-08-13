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
    #region DispoHW
    public class DispoHW : BusinessObject
    {
        public DispoHW()
        {
            FEOSID = 0;
            ProductID = 0;
            ProductName = "";
            ColorName = "";
            LabdipDetailID = 0;
            QtyWarp = 0;
            QtyWeft = 0;
            QtyGREY = 0;
            DyeingOrderDetailID = 0;
            QtyDyeing = 0;
            QtyPCS = 0;
            WarpTF = 0;
            WeftTF = 0;

            FSCDetailID = 0;
            FSCID = 0;
            //FEOSID = 0;
            ExeNo = "";
            ExeDate = DateTime.Now;
            ReceiveDate = DateTime.Now;
            WarpLangth = 0;
            CompLangth = 0;
            TFLangth = 0;
            BeamNo = "";
            BuyerName = "";
            ContractorName = "";
            ContractorID = 0;
            MKTPersonID = 0;
            BuyerID = 0;
            ErrorMessage = "";
            DispoHWs = new List<DispoHW>();
        }

        #region Property
        public int FEOSID { get; set; }
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public string ColorName { get; set; }
        public int LabdipDetailID { get; set; }
        public double QtyWarp { get; set; }
        public double QtyWeft { get; set; }
        public double QtyGREY { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public double QtyDyeing { get; set; }
        public double QtyPCS { get; set; }
        public double WarpTF { get; set; }
        public double WeftTF { get; set; }

        public int FSCDetailID { get; set; }
        public int FSCID { get; set; }
        //public int FEOSID { get; set; }
        public string ExeNo { get; set; }
        public DateTime ExeDate { get; set; }
        public DateTime ReceiveDate { get; set; }
        public double WarpLangth { get; set; }
        public double CompLangth { get; set; }
        public double TFLangth { get; set; }
        public string BeamNo { get; set; }
        public string BuyerName { get; set; }
        public string ContractorName { get; set; }
        public int ContractorID { get; set; }
        public int MKTPersonID { get; set; }
        public int BuyerID { get; set; }

        public string ErrorMessage { get; set; }
        #endregion

        #region Derived Property
        public double WarpKg { get; set; }
        public List<DispoHW> DispoHWs { get; set; }
        public string ExeDateInString
        {
            get
            {
                if (ExeDate == DateTime.MinValue) return "";
                return ExeDate.ToString("dd MMM yyyy");
            }
        }
        public string ReceiveDateInString
        {
            get
            {
                if (ReceiveDate == DateTime.MinValue) return "";
                return ReceiveDate.ToString("dd MMM yyyy");
            }
        }
        public double TFLangthDue
        {
            get
            {
              //  if (this.WarpLangth <this.TFLangth) return 0;
                return this.CompLangth - this.TFLangth;
            }
        }
        public double QtyTotal
        {
            get
            {
                return this.QtyWarp + this.QtyWeft;
            }
        }
        public double TotalTFKg
        {
            get
            {
                return this.WarpTF + this.WeftTF;
            }
        }
        public double DueTFKg
        {
            get
            {
                return this.QtyDyeing - (this.WarpTF + this.WeftTF);
            }
        }
        public double LossQty
        {
            get
            {
                if (this.QtyDyeing >(this.WarpTF + this.WeftTF)) return 0;
                return (this.WarpTF + this.WeftTF) - this.QtyDyeing;
            }
        }
        private double nLossP = 0;
        public double LossP
        {
            get
            {
                nLossP = (this.WarpTF + this.WeftTF) - this.QtyDyeing;
                if (nLossP < 0) nLossP = 0;
                if (this.QtyDyeing >0)
                {
                    nLossP = nLossP * 100 / this.QtyDyeing;
                }
                return nLossP;
            }
        }

        #endregion

        #region Functions
        public static List<DispoHW> Gets(string sSQL, int nRptType, long nUserID)
        {
            return DispoHW.Service.Gets(sSQL, nRptType, nUserID);
        }
        #endregion

        #region ServiceFactory
        internal static IDispoHWService Service
        {
            get { return (IDispoHWService)Services.Factory.CreateService(typeof(IDispoHWService)); }
        }
        #endregion
    }
    #endregion

    #region IDispoHW interface
    public interface IDispoHWService
    {
        List<DispoHW> Gets(string sSQL, int nRptType, Int64 nUserID);

    }
    #endregion
}
