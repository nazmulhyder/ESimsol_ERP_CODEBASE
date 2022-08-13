using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class FabricYarnDeliveryChallanDetailService : MarshalByRefObject, IFabricYarnDeliveryChallanDetailService
    {
        #region Private functions and declaration
        private  FabricYarnDeliveryChallanDetail MapObject(NullHandler oReader)
        {
            FabricYarnDeliveryChallanDetail  oFabricYarnDeliveryChallanDetail = new  FabricYarnDeliveryChallanDetail();
            oFabricYarnDeliveryChallanDetail.FYDCDetailID = oReader.GetInt32("FYDCDetailID");
            oFabricYarnDeliveryChallanDetail.FYDChallanID = oReader.GetInt32("FYDChallanID");
            oFabricYarnDeliveryChallanDetail.FYDODetailID = oReader.GetInt32("FYDODetailID");
            oFabricYarnDeliveryChallanDetail.LotID = oReader.GetInt32("LotID");
            oFabricYarnDeliveryChallanDetail.Qty = oReader.GetDouble("Qty");
            oFabricYarnDeliveryChallanDetail.LotNo = oReader.GetString("LotNo");
            oFabricYarnDeliveryChallanDetail.MUName = oReader.GetString("MUName");
            oFabricYarnDeliveryChallanDetail.Balance = oReader.GetDouble("Balance");
            oFabricYarnDeliveryChallanDetail.ProductID = oReader.GetInt32("ProductID");
            oFabricYarnDeliveryChallanDetail.ProductCode = oReader.GetString("ProductCode");
            oFabricYarnDeliveryChallanDetail.ProductName = oReader.GetString("ProductName");
            oFabricYarnDeliveryChallanDetail.Remarks = oReader.GetString("Remarks");

            oFabricYarnDeliveryChallanDetail.DisburseDate = oReader.GetDateTime("DisburseDate");
            oFabricYarnDeliveryChallanDetail.DisburseByName = oReader.GetString("DisburseByName");
            oFabricYarnDeliveryChallanDetail.WUID = oReader.GetInt32("WUID");
            oFabricYarnDeliveryChallanDetail.Color = oReader.GetString("Color");
            oFabricYarnDeliveryChallanDetail.FYDChallanNo = oReader.GetString("FYDChallanNo");
            oFabricYarnDeliveryChallanDetail.ChallanDate = oReader.GetDateTime("ChallanDate");
            oFabricYarnDeliveryChallanDetail.FEOID = oReader.GetInt32("FEOID");
            oFabricYarnDeliveryChallanDetail.UnitPrice = oReader.GetDouble("UnitPrice");
            oFabricYarnDeliveryChallanDetail.FYDOID = oReader.GetInt32("FYDOID");
            oFabricYarnDeliveryChallanDetail.BagQty = oReader.GetString("BagQty");
            //oFabricYarnDeliveryChallanDetail.LCDate = oReader.GetDateTime("LCDate");
            
            return  oFabricYarnDeliveryChallanDetail;
       }

        private  FabricYarnDeliveryChallanDetail CreateObject(NullHandler oReader)
        {
             FabricYarnDeliveryChallanDetail  oFabricYarnDeliveryChallanDetail = MapObject(oReader);
            return  oFabricYarnDeliveryChallanDetail;
        }

        private List< FabricYarnDeliveryChallanDetail> CreateObjects(IDataReader oReader)
        {
            List< FabricYarnDeliveryChallanDetail>  oFabricYarnDeliveryChallanDetails = new List< FabricYarnDeliveryChallanDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                 FabricYarnDeliveryChallanDetail oItem = CreateObject(oHandler);
                 oFabricYarnDeliveryChallanDetails.Add(oItem);
            }
            return  oFabricYarnDeliveryChallanDetails;
        }

        #endregion

       #region Interface implementation
        public FabricYarnDeliveryChallanDetailService() { }

        public FabricYarnDeliveryChallanDetail IUD(FabricYarnDeliveryChallanDetail oFYDCDetail, int nDBOperation, Int64 nUserID)
        {
            FabricYarnDeliveryChallan oFYDChallan = new FabricYarnDeliveryChallan();
            TransactionContext tc = null;
            IDataReader reader;
            try
            {
                tc = TransactionContext.Begin(true);
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert || nDBOperation == (int)EnumDBOperation.Update || nDBOperation == (int)EnumDBOperation.Approval)
                {
                    if (oFYDCDetail.FYDCDetailID <= 0 && oFYDCDetail.FYDC != null)
                    {
                        reader = FabricYarnDeliveryChallanDA.IUD(tc, oFYDCDetail.FYDC, nDBOperation, nUserID);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFYDChallan = new FabricYarnDeliveryChallan();
                            oFYDChallan = FabricYarnDeliveryChallanService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    oFYDCDetail.FYDChallanID = (oFYDCDetail.FYDChallanID > 0) ? oFYDCDetail.FYDChallanID : oFYDChallan.FYDChallanID;
                    reader = FabricYarnDeliveryChallanDetailDA.IUD(tc, oFYDCDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    if (reader.Read())
                    {
                        oFYDCDetail = new FabricYarnDeliveryChallanDetail();
                        oFYDCDetail = CreateObject(oReader);
                    }
                    reader.Close();
                }
                else if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    reader = FabricYarnDeliveryChallanDetailDA.IUD(tc, oFYDCDetail, nDBOperation, nUserID);
                    oReader = new NullHandler(reader);
                    reader.Close();
                    oFYDCDetail.ErrorMessage = Global.DeleteMessage;
                }

                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFYDCDetail = new FabricYarnDeliveryChallanDetail();
                oFYDCDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }
            oFYDCDetail.FYDC = oFYDChallan;
            return oFYDCDetail;
        }

        public FabricYarnDeliveryChallanDetail Get(int nSULDDetailID, Int64 nUserId)
        {
            FabricYarnDeliveryChallanDetail oFabricYarnDeliveryChallanDetail = new FabricYarnDeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricYarnDeliveryChallanDetailDA.Get(tc, nSULDDetailID);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricYarnDeliveryChallanDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallanDetail = new FabricYarnDeliveryChallanDetail();
                oFabricYarnDeliveryChallanDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                #endregion
            }

            return oFabricYarnDeliveryChallanDetail;
        }

        public List<FabricYarnDeliveryChallanDetail> Gets(string sSQL, Int64 nUserID)
        {
            List<FabricYarnDeliveryChallanDetail> oFabricYarnDeliveryChallanDetails = new List<FabricYarnDeliveryChallanDetail>();
            FabricYarnDeliveryChallanDetail oFabricYarnDeliveryChallanDetail = new FabricYarnDeliveryChallanDetail();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricYarnDeliveryChallanDetailDA.Gets(tc, sSQL);
                oFabricYarnDeliveryChallanDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception ex)
            {
                #region Handle Exception
                tc.HandleError();
                oFabricYarnDeliveryChallanDetail.ErrorMessage = (ex.Message.Contains("~")) ? ex.Message.Split('~')[0] : ex.Message;
                oFabricYarnDeliveryChallanDetails.Add(oFabricYarnDeliveryChallanDetail);
                #endregion
            }

            return oFabricYarnDeliveryChallanDetails;
        }

      
        
        #endregion
    }
}
