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
    public class DispoHWService : MarshalByRefObject, IDispoHWService
    {
        #region Private functions and declaration

        private DispoHW MapObject(NullHandler oReader)
        {
            DispoHW oDispoHW = new DispoHW();
            oDispoHW.FEOSID = oReader.GetInt32("FEOSID");
            oDispoHW.ProductID = oReader.GetInt32("ProductID");
            oDispoHW.ProductName = oReader.GetString("ProductName");
            oDispoHW.ColorName = oReader.GetString("ColorName");
            oDispoHW.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
            oDispoHW.QtyWarp = oReader.GetDouble("QtyWarp");
            oDispoHW.QtyWeft = oReader.GetDouble("QtyWeft");
            oDispoHW.QtyGREY = oReader.GetDouble("QtyGREY");
            oDispoHW.DyeingOrderDetailID = oReader.GetInt32("DyeingOrderDetailID");
            oDispoHW.QtyDyeing = oReader.GetDouble("QtyDyeing");
            oDispoHW.QtyPCS = oReader.GetDouble("QtyPCS");
            oDispoHW.WarpTF = oReader.GetDouble("WarpTF");
            oDispoHW.WeftTF = oReader.GetDouble("WeftTF");

            oDispoHW.FSCDetailID = oReader.GetInt32("FSCDetailID");
            oDispoHW.FSCID = oReader.GetInt32("FSCID");
            oDispoHW.ExeNo = oReader.GetString("ExeNo");
            oDispoHW.ExeDate = oReader.GetDateTime("ExeDate");
            oDispoHW.ReceiveDate = oReader.GetDateTime("ReceiveDate");
            oDispoHW.WarpLangth = oReader.GetDouble("WarpLangth");
            oDispoHW.CompLangth = oReader.GetDouble("CompLangth");
            oDispoHW.TFLangth = oReader.GetDouble("TFLangth");
            oDispoHW.BeamNo = oReader.GetString("BeamNo");
            oDispoHW.BuyerName = oReader.GetString("BuyerName");
            oDispoHW.ContractorName = oReader.GetString("ContractorName");
            oDispoHW.ContractorID = oReader.GetInt32("ContractorID");
            oDispoHW.MKTPersonID = oReader.GetInt32("MKTPersonID");
            oDispoHW.BuyerID = oReader.GetInt32("BuyerID");

            return oDispoHW;
        }

        private DispoHW CreateObject(NullHandler oReader)
        {
            DispoHW oDispoHW = new DispoHW();
            oDispoHW = MapObject(oReader);
            return oDispoHW;
        }

        private List<DispoHW> CreateObjects(IDataReader oReader)
        {
            List<DispoHW> oDispoHW = new List<DispoHW>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                DispoHW oItem = CreateObject(oHandler);
                oDispoHW.Add(oItem);
            }
            return oDispoHW;
        }

        #endregion

        #region Interface implementation
        public List<DispoHW> Gets(string sSQL, int nRptType, Int64 nUserID)
        {
            List<DispoHW> oDispoHWs = new List<DispoHW>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = DispoHWDA.Gets(tc, sSQL, nRptType);
                oDispoHWs = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) ;
                tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get DispoHW", e);
                #endregion
            }
            return oDispoHWs;
        }

        #endregion
    }

}
