using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ESimSol.BusinessObjects;
using ICS.Core.Utility;
using ICS.Core.DataAccess;
using System.Data;

namespace ESimSol.Services.DataAccess
{
    public class DyeingOrderDetailDA
    {
        public DyeingOrderDetailDA() { }

        #region Insert Update Delete Function


        public static IDataReader InsertUpdate(TransactionContext tc, DyeingOrderDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDyeingOrderDetaillIDs)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%D,%n,%n,%n,%s",
                     oDOD.DyeingOrderDetailID, oDOD.DyeingOrderID, oDOD.LabDipDetailID, oDOD.ExportSCDetailID, oDOD.ProductID, oDOD.LabDipType, oDOD.Shade, oDOD.Qty, oDOD.UnitPrice, oDOD.HankorCone, oDOD.BuyerCombo, oDOD.ColorName, oDOD.ColorNo, oDOD.PantonNo, oDOD.RGB, oDOD.ApproveLotNo, oDOD.BuyerRef, oDOD.NoOfCone, oDOD.LengthOfCone, oDOD.NoOfCone_Weft, oDOD.LengthOfCone_Weft, oDOD.Note,  NullHandler.GetNullValue(oDOD.DeliveryDate),oDOD.SL, nUserID, (int)eEnumDBOperation, sDyeingOrderDetaillIDs);
        }

         public static void Delete(TransactionContext tc, DyeingOrderDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID, string sDyeingOrderDetaillIDs)
          {
              tc.ExecuteNonQuery("EXEC [SP_IUD_DyeingOrderDetail]" + "%n, %n, %n, %n, %n, %n, %n, %n, %n, %n, %n,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%s,%D,%n,%n,%n,%s",
                                        oDOD.DyeingOrderDetailID, oDOD.DyeingOrderID, oDOD.LabDipDetailID, oDOD.ExportSCDetailID, oDOD.ProductID, oDOD.LabDipType, oDOD.Shade, oDOD.Qty, oDOD.UnitPrice, oDOD.HankorCone, oDOD.BuyerCombo, oDOD.ColorName, oDOD.ColorNo, oDOD.PantonNo, oDOD.RGB, oDOD.ApproveLotNo, oDOD.BuyerRef, oDOD.NoOfCone, oDOD.LengthOfCone, oDOD.NoOfCone_Weft, oDOD.LengthOfCone_Weft, oDOD.Note, NullHandler.GetNullValue(oDOD.DeliveryDate),oDOD.SL, nUserID, (int)eEnumDBOperation, sDyeingOrderDetaillIDs);
          }

          public static IDataReader InsertUpdate_PTU_DO(TransactionContext tc, DyeingOrderDetail oDOD, EnumDBOperation eEnumDBOperation, Int64 nUserID)
          {
              return tc.ExecuteReader("EXEC [SP_IUD_PTU_DyeingOrderDetail]" + "%n, %n,%n, %n,%n,%s,%s,%s,%s,%s,%s,%s,%s,%D,%n",
                       oDOD.DyeingOrderDetailID, oDOD.Qty, oDOD.Status, oDOD.HankorCone, oDOD.BuyerCombo,oDOD.RGB, oDOD.ApproveLotNo, oDOD.BuyerRef, oDOD.NoOfCone, oDOD.LengthOfCone, oDOD.NoOfCone_Weft, oDOD.LengthOfCone_Weft, oDOD.Note, NullHandler.GetNullValue(oDOD.DeliveryDate), nUserID);
          }
          public static IDataReader MakeTwistedGroup(TransactionContext tc, string sDyeingOrderDetailID, int nDyeingOrderIDID, int nTwistedGroup, int nDBOperation, int nUserID)
          {
              return tc.ExecuteReader("EXEC [SP_IUD_DyeingOrderDetail_Twisted] %s, %n, %n, %n, %n", sDyeingOrderDetailID, nDyeingOrderIDID, nTwistedGroup, nUserID, nDBOperation);
          }
        #endregion

        #region SetColor
          public static IDataReader OrderHold(TransactionContext tc, DyeingOrderDetail oDOD)
          {
              string sSQL1 = SQLParser.MakeSQL("Update DyeingOrderDetail Set [Status]=%n WHERE DyeingOrderDetailID=%n", oDOD.Status, oDOD.DyeingOrderDetailID);
              tc.ExecuteNonQuery(sSQL1);
              return tc.ExecuteReader("SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderDetailID=%n", oDOD.DyeingOrderDetailID);
          }
          public static void Update_Rate(TransactionContext tc, DyeingOrderDetail oDOD)
          {
              tc.ExecuteNonQuery("Update DyeingOrderDetail Set UnitPrice=%n,RGB=%s WHERE DyeingOrderDetailID=%n", oDOD.UnitPrice,oDOD.RGB, oDOD.DyeingOrderDetailID);

          }
        #endregion

        #region Get & Exist Function

        public static IDataReader Get(int nDEODID, TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderDetailID=%n", nDEODID);
        }
        public static IDataReader Gets(TransactionContext tc, int nDyeingOrderID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrderDetail WHERE DyeingOrderID=%n order by SL,DyeingOrderDetailID", nDyeingOrderID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nSampleInvoiceID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DyeingOrderDetail where DyeingOrderID in (Select DyeingOrderID from DyeingOrder where PaymentType  in (" + (int)EnumOrderPaymentType.AdjWithNextLC + "," + (int)EnumOrderPaymentType.AdjWithPI + "," + (int)EnumOrderPaymentType.CashOrCheque + ") and  DyeingOrder.SampleInvoiceID=%n)", nSampleInvoiceID);
        }
        public static IDataReader GetsBy(TransactionContext tc, int nLabDipDetailID, int nOrderID, int nOrderType)
        {
            string sSQL = "";
            if (nOrderType == (int)EnumOrderType.BulkOrder)
            {
                sSQL = "SELECT * FROM View_DyeingOrderDetail where LabDipDetailID=" + nLabDipDetailID + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType=3 and  DyeingOrder.ExportPIID in (Select ExportSC.ExportPIID from ExportSC where ExportSC.ExportSCID=" + nOrderID.ToString() + "))";
            }
            else
            {
                sSQL = "SELECT * FROM View_DyeingOrderDetail where LabDipDetailID=" + nLabDipDetailID + " and DyeingOrderID in (Select DyeingOrderID from DyeingOrder where DyeingOrderType =" + nOrderType + " and  DyeingOrder.DyeingOrderID=" + nOrderID.ToString()+")";
            }
             return tc.ExecuteReader(sSQL);
        }
        public static IDataReader Gets(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader(sSQL);
        }
        public static IDataReader GetsLoanOrder(string sSQL, TransactionContext tc)
        {
            return tc.ExecuteReader("EXEC [SP_RPT_LoanOrder] " + "'" + sSQL + "'");
        }
        #endregion
    }
}
