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
    #region ITaxRateSlab

    public class ITaxRateSlab : BusinessObject
    {
        public ITaxRateSlab()
        {

            ITaxRateSlabID = 0;
            ITaxRateSchemeID = 0;
            SequenceType = EnumSequenceType.None;
            Amount = 0;
            Percents = 0;
            ErrorMessage = "";

        }

        #region Properties

        public int ITaxRateSlabID { get; set; }
        public int ITaxRateSchemeID { get; set; }
        public EnumSequenceType SequenceType { get; set; }
        public double Amount { get; set; }
        public int Percents { get; set; }
        public string ErrorMessage { get; set; }

        #endregion

        #region Derived Property
        public int SequenceTypeInint { get; set; }
        public string SequenceTypeString
        {
            get
            {
                return SequenceType.ToString();
            }
        }

        #endregion

        #region Functions
        public static ITaxRateSlab Get(int Id, long nUserID)
        {
            return ITaxRateSlab.Service.Get(Id, nUserID);
        }
        public static ITaxRateSlab Get(string sSQL, long nUserID)
        {
            return ITaxRateSlab.Service.Get(sSQL, nUserID);
        }
        public static List<ITaxRateSlab> Gets(long nUserID)
        {
            return ITaxRateSlab.Service.Gets(nUserID);
        }

        public static List<ITaxRateSlab> Gets(string sSQL, long nUserID)
        {
            return ITaxRateSlab.Service.Gets(sSQL, nUserID);
        }

        public ITaxRateSlab IUD(int nDBOperation, long nUserID)
        {
            return ITaxRateSlab.Service.IUD(this, nDBOperation, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IITaxRateSlabService Service
        {
            get { return (IITaxRateSlabService)Services.Factory.CreateService(typeof(IITaxRateSlabService)); }
        }

        #endregion
    }
    #endregion

    #region IITaxRateSlab interface

    public interface IITaxRateSlabService
    {

        ITaxRateSlab Get(int id, Int64 nUserID);


        ITaxRateSlab Get(string sSQL, Int64 nUserID);


        List<ITaxRateSlab> Gets(Int64 nUserID);


        List<ITaxRateSlab> Gets(string sSQL, Int64 nUserID);


        ITaxRateSlab IUD(ITaxRateSlab oITaxRateSlab, int nDBOperation, Int64 nUserID);

    }
    #endregion
}
