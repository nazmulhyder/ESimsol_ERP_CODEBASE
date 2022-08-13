using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    [Serializable]
    public class FabricExecutionOrderSpecificationDetailService : MarshalByRefObject, IFabricExecutionOrderSpecificationDetailService
    {
        #region Private functions and declaration
        private FabricExecutionOrderSpecificationDetail MapObject(NullHandler oReader)
        {
            FabricExecutionOrderSpecificationDetail oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
            oFEOSDetail.FEOSDID = oReader.GetInt32("FEOSDID");
            oFEOSDetail.FEOSID = oReader.GetInt32("FEOSID");
            oFEOSDetail.IsWarp = oReader.GetBoolean("IsWarp");
            oFEOSDetail.ProductID = oReader.GetInt32("ProductID");
            oFEOSDetail.LabdipDetailID = oReader.GetInt32("LabdipDetailID");
            oFEOSDetail.ColorName = oReader.GetString("ColorNameLD");
            oFEOSDetail.YarnType = oReader.GetString("YarnType");
            oFEOSDetail.EndsCount = oReader.GetInt32("EndsCount");
            oFEOSDetail.ProductCode = oReader.GetString("ProductCode");
            oFEOSDetail.ProductName = oReader.GetString("ProductName");
            oFEOSDetail.ProductShortName = oReader.GetString("ProductShortName");
            oFEOSDetail.Value = oReader.GetDouble("Value");
            oFEOSDetail.ValueMin = oReader.GetDouble("ValueMin");
            oFEOSDetail.Qty = oReader.GetDouble("Qty");
            oFEOSDetail.Length = oReader.GetDouble("Length");
            oFEOSDetail.ProductName = oReader.GetString("ProductName");
            oFEOSDetail.ColorNo = oReader.GetString("ColorNo");
            oFEOSDetail.PantonNo = oReader.GetString("PantonNo");
            oFEOSDetail.LDNo = oReader.GetString("LDNo");
            oFEOSDetail.Allowance = oReader.GetDouble("Allowance");
            oFEOSDetail.BatchNo = oReader.GetString("BatchNo");
            oFEOSDetail.AllowanceWarp = oReader.GetDouble("AllowanceWarp");
            oFEOSDetail.TotalEnd = oReader.GetDouble("TotalEnd");
            oFEOSDetail.TotalEndActual = oReader.GetDouble("TotalEndActual");
            oFEOSDetail.SLNo = oReader.GetInt32("SLNo");
            oFEOSDetail.TwistedGroup = oReader.GetInt32("TwistedGroupTwo");
            oFEOSDetail.TwistedGroupInt = oReader.GetInt32("TwistedGroup");
            oFEOSDetail.IsYarnExist = oReader.GetBoolean("IsYarnExist");
            oFEOSDetail.BeamType = (EnumBeamType)oReader.GetInt16("BeamType");

            oFEOSDetail.FSCDID = oReader.GetInt32("FSCDID");
            oFEOSDetail.SCNoFull = oReader.GetString("SCNoFull");
            oFEOSDetail.IsInHouse = oReader.GetBoolean("IsInHouse");
            oFEOSDetail.PINo = oReader.GetString("PINo");
            oFEOSDetail.ExeNo = oReader.GetString("ExeNo");

            if (string.IsNullOrEmpty(oFEOSDetail.ColorNo))
            { oFEOSDetail.ColorNo = oFEOSDetail.LDNo; }
            oFEOSDetail.ProductShortName = (oFEOSDetail.ProductShortName == string.Empty) ? oFEOSDetail.ProductName : oFEOSDetail.ProductShortName;
            //if (oFEOSDetail.Value<30)
            //{
            //    if (oFEOSDetail.ValueMin<=0)
            //    {
            //        oFEOSDetail.ValueMin = 0.5;
            //    }
            //}

            return oFEOSDetail;
        }

        private FabricExecutionOrderSpecificationDetail CreateObject(NullHandler oReader)
        {
            FabricExecutionOrderSpecificationDetail oFabricExecutionOrderSpecificationDetail = new FabricExecutionOrderSpecificationDetail();
            oFabricExecutionOrderSpecificationDetail = MapObject(oReader);
            return oFabricExecutionOrderSpecificationDetail;
        }
        private List<FabricExecutionOrderSpecificationDetail> CreateObjects(IDataReader oReader)
        {
            List<FabricExecutionOrderSpecificationDetail> oFabricExecutionOrderSpecificationDetails = new List<FabricExecutionOrderSpecificationDetail>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricExecutionOrderSpecificationDetail oItem = CreateObject(oHandler);
                oFabricExecutionOrderSpecificationDetails.Add(oItem);
            }
            return oFabricExecutionOrderSpecificationDetails;
        }
        #endregion

        #region Interface implementatio
        public FabricExecutionOrderSpecificationDetailService() { }

        public FabricExecutionOrderSpecificationDetail IUD(FabricExecutionOrderSpecificationDetail oFEOSDetail, int nDBOperation, Int64 nUserId)
        {
            TransactionContext tc = null;
            FabricExecutionOrderSpecification oFEOS = new FabricExecutionOrderSpecification();
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                NullHandler oReader;
                if (nDBOperation == (int)EnumDBOperation.Insert && oFEOSDetail.FEOSID == 0)
                {

                    if (oFEOSDetail.FEOS != null)
                    {
                        reader = FabricExecutionOrderSpecificationDA.IUD(tc, oFEOSDetail.FEOS, nDBOperation, nUserId);
                        oReader = new NullHandler(reader);
                        if (reader.Read())
                        {
                            oFEOS = FabricExecutionOrderSpecificationService.CreateObject(oReader);
                        }
                        reader.Close();
                    }
                    else { throw new Exception("No fabric execution order specification information found to save."); }
                    oFEOSDetail.FEOSID = oFEOS.FEOSID;
                }


                reader = FabricExecutionOrderSpecificationDetailDA.IUD(tc, oFEOSDetail, nDBOperation, nUserId);

                oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                    oFEOSDetail = CreateObject(oReader);
                }
                if (nDBOperation == (int)EnumDBOperation.Delete)
                {
                    oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                    oFEOSDetail.ErrorMessage = Global.DeleteMessage;
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                oFEOSDetail.ErrorMessage = (e.Message.Contains("!")) ? e.Message.Split('!')[0] : e.Message;
                #endregion
            }
            oFEOSDetail.FEOS = oFEOS;
            return oFEOSDetail;
        }
        public FabricExecutionOrderSpecificationDetail Get(int nFEOSDID, Int64 nUserId)
        {
            FabricExecutionOrderSpecificationDetail oFEOSDetail = new FabricExecutionOrderSpecificationDetail();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderSpecificationDetailDA.Get(tc, nFEOSDID, nUserId);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFEOSDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOSDetail = new FabricExecutionOrderSpecificationDetail();
                oFEOSDetail.ErrorMessage = "Failed to get information.";
                #endregion
            }

            return oFEOSDetail;
        }
        public List<FabricExecutionOrderSpecificationDetail> Gets(int nFEOSID, Int64 nUserId)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderSpecificationDetailDA.Gets(tc, nFEOSID, nUserId);
                oFEOSDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                #endregion
            }

            return oFEOSDetails;
        }
        public List<FabricExecutionOrderSpecificationDetail> Gets(string sSQL, Int64 nUserId)
        {
            List<FabricExecutionOrderSpecificationDetail> oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricExecutionOrderSpecificationDetailDA.Gets(tc, sSQL, nUserId);
                oFEOSDetails = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null) tc.HandleError();
                oFEOSDetails = new List<FabricExecutionOrderSpecificationDetail>();
                #endregion
            }

            return oFEOSDetails;
        }
        public FabricExecutionOrderSpecificationDetail UpdateAllowance(FabricExecutionOrderSpecificationDetail oFESDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = FabricExecutionOrderSpecificationDetailDA.UpdateAllowance(tc, oFESDetail);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFESDetail = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                oFESDetail = new FabricExecutionOrderSpecificationDetail();
                oFESDetail.ErrorMessage = e.Message.Split('~')[0];
                #endregion
            }
            return oFESDetail;
        }
    
        public string DeleteAll(FabricExecutionOrderSpecificationDetail oFabricExecutionOrderSpecificationDetail, Int64 nUserId)
        {
            TransactionContext tc = null;
            int nCount = 0;
            try
            {
                tc = TransactionContext.Begin(true);

                nCount = FabricExecutionOrderSpecificationDetailDA.GetDUPS(tc, oFabricExecutionOrderSpecificationDetail.FEOSID);

                if (nCount > 0)
                {
                    throw new Exception("Production Schedule already create");
                }

                FabricExecutionOrderSpecificationDetailDA.DeleteAll(tc, oFabricExecutionOrderSpecificationDetail, (int)EnumDBOperation.Delete, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('!')[0];

                //ExceptionLog.Write(e);
                //throw new ServiceException("Failed to Delete Product. Because of " + e.Message, e);
                #endregion
            }
            return Global.DeleteMessage;
        }
        #endregion
    }
}