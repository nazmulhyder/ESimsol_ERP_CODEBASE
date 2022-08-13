using System;
using System.Collections.Generic;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core.DataAccess;
using ICS.Core.Framework;
using ICS.Core.Utility;

namespace ESimSol.Services.Services
{
    public class EmployeeProductionService : MarshalByRefObject, IEmployeeProductionService
    {
        #region Private functions and declaration
        private EmployeeProduction MapObject(NullHandler oReader)
        {
            EmployeeProduction oEmployeeProduction = new EmployeeProduction();
            oEmployeeProduction.EPSID = oReader.GetInt32("EPSID");
            oEmployeeProduction.EPSNO = oReader.GetString("EPSNO");
            oEmployeeProduction.EmployeeID = oReader.GetInt32("EmployeeID");
            oEmployeeProduction.OrderRecapDetailID = oReader.GetInt32("OrderRecapDetailID");
            oEmployeeProduction.ProductionProcess = (EnumProductionProcess)oReader.GetInt16("ProductionProcess");
            oEmployeeProduction.GPID = oReader.GetInt32("GarmentPart");
            oEmployeeProduction.MachineNo = oReader.GetString("MachineNo");
            oEmployeeProduction.TSPID = oReader.GetInt32("TSPID");
            oEmployeeProduction.IssueQty = oReader.GetDouble("IssueQty");
            oEmployeeProduction.IssueBy = oReader.GetInt32("IssueBy");
            oEmployeeProduction.IssueDate = oReader.GetDateTime("IssueDate");
            oEmployeeProduction.RcvQty = oReader.GetDouble("RcvQty");
            oEmployeeProduction.YarnRcvQty = oReader.GetDouble("YarnRcvQty");
            oEmployeeProduction.ApproveBy = oReader.GetInt32("ApproveBy");
            oEmployeeProduction.ApproveByDate = oReader.GetDateTime("ApproveByDate");
            oEmployeeProduction.EPSLotNo = oReader.GetString("EPSLotNo");
            oEmployeeProduction.IsActive = oReader.GetBoolean("IsActive");
            oEmployeeProduction.ReferenceEPSID = oReader.GetInt32("ReferenceEPSID");
            oEmployeeProduction.SLNO = oReader.GetString("SLNO");
            oEmployeeProduction.DepartmentID = oReader.GetInt32("DepartmentID");
            //derive
            oEmployeeProduction.EmployeeName = oReader.GetString("EmployeeName");
            oEmployeeProduction.Code = oReader.GetString("Code");
            oEmployeeProduction.EmpOfficial = oReader.GetString("EmpOfficial");
            oEmployeeProduction.ColorName = oReader.GetString("ColorName");
            oEmployeeProduction.SizeCategoryName = oReader.GetString("SizeCategoryName");
            oEmployeeProduction.StyleNo = oReader.GetString("StyleNo");
            oEmployeeProduction.LotNo = oReader.GetString("LotNo");
            oEmployeeProduction.ApproveByName = oReader.GetString("ApproveByName");
            oEmployeeProduction.RcvByDate = oReader.GetDateTime("RcvByDate");
            oEmployeeProduction.OrderRecapNo = oReader.GetString("OrderRecapNo");
            oEmployeeProduction.OrderRecapID = oReader.GetInt32("OrderRecapID");
            oEmployeeProduction.ReferenceLotNo = oReader.GetString("ReferenceLotNo");
            oEmployeeProduction.ReferenceMachineNo = oReader.GetString("ReferenceMachineNo");
            oEmployeeProduction.ReferenceEPSNo = oReader.GetString("ReferenceEPSNo");
            oEmployeeProduction.GPName = oReader.GetString("GPName");

            return oEmployeeProduction;

        }

        private EmployeeProduction CreateObject(NullHandler oReader)
        {
            EmployeeProduction oEmployeeProduction = MapObject(oReader);
            return oEmployeeProduction;
        }

        private List<EmployeeProduction> CreateObjects(IDataReader oReader)
        {
            List<EmployeeProduction> oEmployeeProduction = new List<EmployeeProduction>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                EmployeeProduction oItem = CreateObject(oHandler);
                oEmployeeProduction.Add(oItem);
            }
            return oEmployeeProduction;
        }

        #endregion

        #region Interface implementation
        public EmployeeProductionService() { }

        public EmployeeProduction IUD(EmployeeProduction oEmployeeProduction, int nDBOperation, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeProductionDA.IUD(tc, oEmployeeProduction, nUserID, nDBOperation);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeProduction = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeProduction.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeProduction.EPSID = 0;
                #endregion
            }
            return oEmployeeProduction;
        }


        public EmployeeProduction Get(int nEPSID, Int64 nUserId)
        {
            EmployeeProduction oEmployeeProduction = new EmployeeProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeProductionDA.Get(nEPSID, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProduction = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeProduction", e);
                oEmployeeProduction.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeProduction;
        }

        public EmployeeProduction Get(string sSql, Int64 nUserId)
        {
            EmployeeProduction oEmployeeProduction = new EmployeeProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeProductionDA.Get(sSql, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProduction = CreateObject(oReader);
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
                //throw new ServiceException("Failed to Get EmployeeProduction", e);
                oEmployeeProduction.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeProduction;
        }

        public List<EmployeeProduction> Gets(Int64 nUserID)
        {
            List<EmployeeProduction> oEmployeeProduction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProductionDA.Gets(tc);
                oEmployeeProduction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeProduction", e);
                #endregion
            }
            return oEmployeeProduction;
        }

        public List<EmployeeProduction> Gets(string sSQL, Int64 nUserID)
        {
            List<EmployeeProduction> oEmployeeProduction = null;

            TransactionContext tc = null;

            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = null;
                reader = EmployeeProductionDA.Gets(sSQL, tc);
                oEmployeeProduction = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get View_EmployeeProduction", e);
                #endregion
            }
            return oEmployeeProduction;
        }

        public EmployeeProduction TransferEmployeeProduction(EmployeeProduction oEmployeeProduction, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin(true);
                IDataReader reader;
                reader = EmployeeProductionDA.TransferEmployeeProduction(tc, oEmployeeProduction);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {

                    oEmployeeProduction = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeProduction.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeProduction.EPSID = 0;
                #endregion
            }
            return oEmployeeProduction;
        }


        #region Activity
        public EmployeeProduction Activity(int nEmployeeProductionID, bool Active, Int64 nUserId)
        {
            EmployeeProduction oEmployeeProduction = new EmployeeProduction();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();

                IDataReader reader = EmployeeProductionDA.Activity(nEmployeeProductionID, Active, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProduction = CreateObject(oReader);
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
                oEmployeeProduction.ErrorMessage = e.Message;
                #endregion
            }

            return oEmployeeProduction;
        }


        #endregion

        public EmployeeProduction AdvanceEdit(EmployeeProduction oEmployeeProduction, Int64 nUserID)
        {

            TransactionContext tc = null;
            try
            {
                double nTotalRcvQty = 0;

                tc = TransactionContext.Begin(true);
                foreach (EmployeeProductionReceiveDetail EPRD in oEmployeeProduction.EmployeeProductionReceiveDetails)
                {
                    nTotalRcvQty += EPRD.RcvQty;
                    IDataReader reaDeretail;
                    if (oEmployeeProduction.IssueDate > EPRD.RcvByDate)
                    {
                        throw new Exception("Issue date must not be greater than the receive date !");
                    }
                    reaDeretail = EmployeeProductionReceiveDetailDA.AdvanceEdit(EPRD, tc);
                    NullHandler oreaDeretail = new NullHandler(reaDeretail);
                    reaDeretail.Close();
                }

                if (oEmployeeProduction.IssueQty < nTotalRcvQty)
                {
                    throw new Exception("Receive qty must not be greater than Issue qty !");
                }

                IDataReader reader;
                string sSQL = "UPDATE EmployeeProduction SET IssueQty=" + oEmployeeProduction.IssueQty + ",IssueDate='" + oEmployeeProduction.IssueDate + "',RcvQty =" + oEmployeeProduction.RcvQty + "WHERE EPSID=" + oEmployeeProduction.EPSID + " SELECT * FROM View_EmployeeProduction WHERE EPSID=" + oEmployeeProduction.EPSID;
                reader = EmployeeProductionDA.Get(sSQL, tc);
                NullHandler oReader = new NullHandler(reader);
                if (reader.Read())
                {
                    oEmployeeProduction = CreateObject(oReader);
                }
                reader.Close();

                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                oEmployeeProduction.ErrorMessage = e.Message.Split('!')[0]; // Optional if required to pass validation message to View                  
                oEmployeeProduction.EPSID = 0;
                #endregion
            }
            return oEmployeeProduction;
        }

        public string GetBalance(string Ssql, Int64 nUserId)
        {
            TransactionContext tc = null;
            string sFeedbackMessage = "";
            try
            {
                tc = TransactionContext.Begin(true);
                EmployeeProduction oEmployeeProduction = new EmployeeProduction();
                sFeedbackMessage = EmployeeProductionDA.GetBalance(Ssql, tc);
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();
                return sFeedbackMessage = e.Message.Split('~')[0];
                #endregion
            }
            return sFeedbackMessage;
        }

        #endregion

    }
}
