using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core.Framework;
using ICS.Core.Utility;


namespace ESimSol.BusinessObjects
{
    #region VoucherBillAgeSlab
    public class VoucherBillAgeSlab : BusinessObject
    {
        public VoucherBillAgeSlab()
        {
            Start = 0;
            End = 0;
            Separator = " To ";
        }

        #region Properties
        public int Start { get; set; }
        public string Separator { get; set; }
        public int End { get; set; }
       
        #region Derive Property
        public string Range 
        { 
            get 
            {
                if (this.End <= 0)
                {
                    return (this.Start + " Days " + this.Separator);
                }
                else
                {
                    return (this.Start + this.Separator + this.End + " Days");
                }
            } 
        }
        #endregion

        #endregion

       
    }
    #endregion
    
    
}
