using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using ESimSol.BusinessObjects;
using ESimSol.Services.DataAccess;
using ICS.Core;
using ICS.Core.DataAccess;
using ICS.Core.Utility;
using ICS.Core.Framework;
 

namespace ESimSol.Services.Services
{
    public class QCRegisterService : MarshalByRefObject, IQCRegisterService
    {
        #region Private functions and declaration
        private QCRegister MapObject(NullHandler oReader)
        {
            QCRegister oQCRegister = new QCRegister();
            oQCRegister.QCID = oReader.GetInt32("QCID");
            oQCRegister.WorkingUnitID = oReader.GetInt32("WorkingUnitID");
            oQCRegister.ProductionSheetID = oReader.GetInt32("ProductionSheetID");
            oQCRegister.PassQuantity = oReader.GetDouble("PassQuantity");
            oQCRegister.RejectQuantity = oReader.GetDouble("RejectQuantity");
            oQCRegister.ProductID = oReader.GetInt32("ProductID");
            oQCRegister.QCPerson = oReader.GetInt32("QCPerson");
            oQCRegister.OperationTime = oReader.GetDateTime("OperationTime");
            oQCRegister.SheetNo = oReader.GetString("SheetNo");
            oQCRegister.ProductCode = oReader.GetString("ProductCode");
            oQCRegister.ProductName = oReader.GetString("ProductName");
            oQCRegister.QCPersonName = oReader.GetString("QCPersonName");
            oQCRegister.StoreName = oReader.GetString("StoreName");
            oQCRegister.CartonQty = oReader.GetInt32("CartonQty");
            oQCRegister.PerCartonFGQty = oReader.GetDouble("PerCartonFGQty");
            oQCRegister.LotID = oReader.GetInt32("LotID");
            oQCRegister.IsExist = oReader.GetBoolean("IsExist");
            oQCRegister.LotNo = oReader.GetString("LotNo");
            oQCRegister.MUName = oReader.GetString("MUName");
            oQCRegister.QCStatus = oReader.GetInt32("QCStatus");
            oQCRegister.BUID = oReader.GetInt32("BUID");
            oQCRegister.BuyerID = oReader.GetInt32("BuyerID");
            oQCRegister.BuyerName = oReader.GetString("BuyerName");
            oQCRegister.ExportPINo = oReader.GetString("ExportPINo");
            oQCRegister.CurrencySymbol = oReader.GetString("CurrencySymbol");
            oQCRegister.UnitPrice = oReader.GetDouble("UnitPrice");

            oQCRegister.PSIssueDate = oReader.GetDateTime("PSIssueDate");
            oQCRegister.ColorName = oReader.GetString("ColorName");
            oQCRegister.SheetQty = oReader.GetDouble("SheetQty");
            oQCRegister.ConsumptionQty = oReader.GetDouble("ConsumptionQty");
            oQCRegister.QCPassQty = oReader.GetDouble("QCPassQty");
            oQCRegister.RejectQty = oReader.GetDouble("RejectQty");
            oQCRegister.QCQty = oReader.GetDouble("QCQty");
            oQCRegister.YetToQCQty = oReader.GetDouble("YetToQCQty");

            return oQCRegister;
        }
        private QCRegister CreateObject(NullHandler oReader)
        {
            QCRegister oQCRegister = new QCRegister();
            oQCRegister = MapObject(oReader);
            return oQCRegister;
        }

        private List<QCRegister> CreateObjects(IDataReader oReader)
        {
            List<QCRegister> oQCRegister = new List<QCRegister>();
            NullHandler oHandler = new NullHandler(oReader);
            while (oReader.Read())
            {
                QCRegister oItem = CreateObject(oHandler);
                oQCRegister.Add(oItem);
            }
            return oQCRegister;
        }

        #endregion

        #region Interface implementation
        public QCRegisterService() { }        
        public List<QCRegister> Gets(string sSQL, Int64 nUserID)
        {
            List<QCRegister> oQCRegister = new List<QCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = QCRegisterDA.Gets(tc, sSQL);
                oQCRegister = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCRegister", e);
                #endregion
            }

            return oQCRegister;
        }

        public List<QCRegister> GetsByQCFollowUp(QCRegister oQCRegister, Int64 nUserID)
        {
            List<QCRegister> oQCRegisters = new List<QCRegister>();
            TransactionContext tc = null;
            try
            {
                tc = TransactionContext.Begin();
                IDataReader reader = null;
                reader = QCRegisterDA.GetsByQCFollowUp(tc, oQCRegister);
                oQCRegisters = CreateObjects(reader);
                reader.Close();
                tc.End();
            }
            catch (Exception e)
            {
                #region Handle Exception
                if (tc != null)
                    tc.HandleError();

                ExceptionLog.Write(e);
                throw new ServiceException("Failed to Get QCRegister", e);
                #endregion
            }

            return oQCRegisters;
        }
        #endregion
    }
}
