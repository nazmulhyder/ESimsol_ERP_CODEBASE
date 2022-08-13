using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;


namespace ESimSol.Services.Services
{
    public class FabricBatchProductionBatchManService : MarshalByRefObject, IFabricBatchProductionBatchManService
    {
        #region Private functions and declaration
        public static FabricBatchProductionBatchMan MapObject(NullHandler oReader)
        {
            FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
            oFabricBatchProductionBatchMan.FBPBID = oReader.GetInt32("FBPBID");
            oFabricBatchProductionBatchMan.FBPID = oReader.GetInt32("FBPID");
            oFabricBatchProductionBatchMan.ShiftStartTime =oReader.GetDateTime("ShiftStartTime");
            oFabricBatchProductionBatchMan.ShiftEndTime = oReader.GetDateTime("ShiftEndTime");
            oFabricBatchProductionBatchMan.EmployeeID = oReader.GetInt32("EmployeeID");
            oFabricBatchProductionBatchMan.EmployeeName = oReader.GetString("EmployeeName");
            oFabricBatchProductionBatchMan.Note = oReader.GetString("Note");
            oFabricBatchProductionBatchMan.ShiftID = oReader.GetInt32("ShiftID");
            oFabricBatchProductionBatchMan.ShiftName = oReader.GetString("ShiftName");
            oFabricBatchProductionBatchMan.Qty = oReader.GetDouble("Qty");
            oFabricBatchProductionBatchMan.FinishDate = oReader.GetDateTime("FinishDate");
            oFabricBatchProductionBatchMan.TotalFBPBreakage = oReader.GetInt32("TotalFBPBreakage");
            oFabricBatchProductionBatchMan.TotalNoOfBreakage = oReader.GetInt32("TotalNoOfBreakage");
            oFabricBatchProductionBatchMan.TotalColor = oReader.GetInt32("TotalColor");
            oFabricBatchProductionBatchMan.Efficiency = oReader.GetInt32("Efficiency");
            oFabricBatchProductionBatchMan.RPM = oReader.GetInt32("RPM");
            oFabricBatchProductionBatchMan.TSUID = oReader.GetInt32("TSUID");
            oFabricBatchProductionBatchMan.BeamNo = oReader.GetString("BeamNo");
            oFabricBatchProductionBatchMan.ApproveBy = oReader.GetInt32("ApproveBy");
            oFabricBatchProductionBatchMan.ApproveName = oReader.GetString("ApproveName");
            oFabricBatchProductionBatchMan.ApproveDate = oReader.GetDateTime("ApproveDate");
            oFabricBatchProductionBatchMan.Construction = oReader.GetString("Construction");
            oFabricBatchProductionBatchMan.FEONo = oReader.GetString("FEONo");
            oFabricBatchProductionBatchMan.MachineCode = oReader.GetString("MachineCode");
            oFabricBatchProductionBatchMan.BuyerName = oReader.GetString("BuyerName");
            oFabricBatchProductionBatchMan.Color = oReader.GetString("Color");

            return oFabricBatchProductionBatchMan;
        }

        public static FabricBatchProductionBatchMan CreateObject(NullHandler oReader)
        {
            FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
            oFabricBatchProductionBatchMan = MapObject(oReader);
            return oFabricBatchProductionBatchMan;
        }

        private List<FabricBatchProductionBatchMan> CreateObjects(IDataReader oReader)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMan = new List<FabricBatchProductionBatchMan>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                FabricBatchProductionBatchMan oItem = CreateObject(oHandler);
                oFabricBatchProductionBatchMan.Add(oItem);
            }
            return oFabricBatchProductionBatchMan;
        }

        #endregion
        
        public List<FabricBatchProductionBatchMan> Gets(int nFBPID, Int64 nUserID)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionBatchManDA.Gets(tc, nFBPID);
                oFabricBatchProductionBatchMans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchProductionBatchMans;
        }

        public List<FabricBatchProductionBatchMan> GetsBySql(string sSql, Int64 nUserID)
        {
            List<FabricBatchProductionBatchMan> oFabricBatchProductionBatchMans = new List<FabricBatchProductionBatchMan>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = FabricBatchProductionBatchManDA.GetsBySql(tc, sSql);
                oFabricBatchProductionBatchMans = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get Fabric Yarn Out", e);
                #endregion
            }
            return oFabricBatchProductionBatchMans;
        }

        public FabricBatchProductionBatchMan Get(int id, Int64 nUserId)
        {
            FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = FabricBatchProductionBatchManDA.Get(tc, id);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBatchMan = CreateObject(oReader);
                }
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get FabricBatchProductionBatchMan", e);
                #endregion
            }
            return oFabricBatchProductionBatchMan;
        }
        public FabricBatchProductionBatchMan Save(FabricBatchProductionBatchMan oFabricBatchProductionBatchMan, Int64 nUserID)
        {
            TransactionContext tc = null;
            try
            {
               
                tc = TransactionContext.Begin(true);
               
                IDataReader reader;
                if (oFabricBatchProductionBatchMan.FBPBID <= 0)
                {
                    reader = FabricBatchProductionBatchManDA.IUD(tc, EnumDBOperation.Insert, oFabricBatchProductionBatchMan, nUserID);
                }
                else
                {
                    reader = FabricBatchProductionBatchManDA.IUD(tc, EnumDBOperation.Update, oFabricBatchProductionBatchMan, nUserID);
                }
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
                    oFabricBatchProductionBatchMan = CreateObject(oReader);
                }
                else {
                    oFabricBatchProductionBatchMan.ErrorMessage = "Invalid fabric batch production batch man !!!";
                }
                reader.Close();        
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
                oFabricBatchProductionBatchMan.ErrorMessage = e.Message.Split('!')[0];
                #endregion
            }
            return oFabricBatchProductionBatchMan;
        }

        public string Delete(int id, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBatchMan oFabricBatchProductionBatchMan = new FabricBatchProductionBatchMan();
                oFabricBatchProductionBatchMan.FBPBID = id;
                FabricBatchProductionBatchManDA.Delete(tc,EnumDBOperation.Delete, oFabricBatchProductionBatchMan,nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }
        public string MultipleApprove(String FBPBIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBatchManDA.MultipleApprove(tc, FBPBIDS, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return "Success";
        }
        public string MultipleDelete(String FBPBIDS, Int64 nUserId)
        {
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                FabricBatchProductionBatchManDA.MultipleDelete(tc, FBPBIDS, nUserId);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return e.Message.Split('~')[0];
                #endregion
            }
            return Global.DeleteMessage;
        }

    }
    
  
}
