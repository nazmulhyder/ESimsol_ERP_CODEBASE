

using System;
using System.IO;
using ESimSol.BusinessObjects;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Collections.Generic;
using ICS.Core;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.BusinessObjects
{
    #region DUColorCombo

    public class DUColorCombo : BusinessObject
    {
        public DUColorCombo()
        {
           DUColorComboID=0;
           DyeingOrderID=0;
           DyeingOrderDetailID=0;
           ComboID=0;
           SLNo = 0;
           ColorName = "";
        }

        #region Properties
        public int DUColorComboID { get; set; }
        public int DyeingOrderID { get; set; }
        public int DyeingOrderDetailID { get; set; }
        public EnumNumericOrder ComboID { get; set; }
        public int SLNo { get; set; }
        public string ColorName { get; set; }
        public string ErrorMessage { get; set; }
        public string ComboIDSt
        {
            get
            {
                return this.ComboID.ToString();
            }
        }
        #endregion

        #region Functions
        public static List<DUColorCombo> Gets(int nDODID, int nComboID, long nUserID)
        {
            return DUColorCombo.Service.Gets( nDODID, nComboID,nUserID);
        }
        public static List<DUColorCombo> Gets(int nDODID, Int64 nUserID)
        {
            return DUColorCombo.Service.Gets(nDODID, nUserID);
        }
        public DUColorCombo Get(int nId, long nUserID)
        {
            return DUColorCombo.Service.Get(nId, nUserID);
        }
        public static List<DUColorCombo> Save(List<DUColorCombo> oDUColorCombos, long nUserID)
        {
            return DUColorCombo.Service.Save(oDUColorCombos, nUserID);
        }
        public string Delete( long nUserID)
        {
            return DUColorCombo.Service.Delete(this, nUserID);
        }

        #endregion

        #region ServiceFactory
        internal static IDUColorComboService Service
        {
            get { return (IDUColorComboService)Services.Factory.CreateService(typeof(IDUColorComboService)); }
        }
        #endregion
    }
    #endregion

    #region IDUColorCombo interface
    public interface IDUColorComboService
    {
        DUColorCombo Get(int id, long nUserID);
        List<DUColorCombo> Gets(int nDODID, int nComboID, long nUserID);
        List<DUColorCombo> Gets(int nDODID, Int64 nUserID);
        string Delete(DUColorCombo oDUColorCombo, long nUserID);
        List<DUColorCombo> Save(List<DUColorCombo> oDUColorCombos, long nUserID);
    }
    #endregion
}
