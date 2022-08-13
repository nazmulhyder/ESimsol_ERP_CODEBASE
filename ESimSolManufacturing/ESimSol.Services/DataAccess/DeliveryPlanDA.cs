using System;
using System.Data;
using System.Data.SqlClient;
using ICS.Core.DataAccess;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class DeliveryPlanDA
    {
        public DeliveryPlanDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, DeliveryPlan oDeliveryPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_DeliveryPlan]"
                                    + "%n,%s,%d,%n,%n,%n,%n,%s,%n,%n",
                                    oDeliveryPlan.DeliveryPlanID, oDeliveryPlan.PlanNo, oDeliveryPlan.DeliveryPlanDate, oDeliveryPlan.DeliveryOrderID, oDeliveryPlan.Sequence, oDeliveryPlan.BUID, (int)oDeliveryPlan.ProductNatureInInt, oDeliveryPlan.Remarks, nUserID, (int)eEnumDBOperation);
        }

        public static void Delete(TransactionContext tc, DeliveryPlan oDeliveryPlan, EnumDBOperation eEnumDBOperation, Int64 nUserID)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_DeliveryPlan]"
                                    + "%n,%s,%d,%n,%n,%n,%n,%s,%n,%n",
                                    oDeliveryPlan.DeliveryPlanID, oDeliveryPlan.PlanNo, oDeliveryPlan.DeliveryPlanDate, oDeliveryPlan.DeliveryOrderID, oDeliveryPlan.Sequence, oDeliveryPlan.BUID, (int)oDeliveryPlan.ProductNatureInInt, oDeliveryPlan.Remarks, nUserID, (int)eEnumDBOperation);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_DeliveryPlan WHERE DeliveryPlanID=%n", nID);
        }

        public static IDataReader Gets(int BUID, int nProductNature, DateTime dPlanDate,int nContractorID, TransactionContext tc)
        {
            
            if(nContractorID!=0)
            {
                return tc.ExecuteReader("SELECT * FROM View_DeliveryPlan WHERE BUID = %n AND ProductNature = %n AND Convert(Date, DeliveryPlanDate,106) = CONVERT(Date,%d,106) AND ContractorID = %n Order By [DeliveryPlanDate], Sequence", BUID, nProductNature, dPlanDate,nContractorID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_DeliveryPlan WHERE BUID = %n AND ProductNature = %n AND Convert(Date, DeliveryPlanDate,106) = CONVERT(Date,%d,106) Order By [DeliveryPlanDate], Sequence", BUID, nProductNature, dPlanDate);
            }
            

        }
        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);//use View_DeliveryPlan
        }

       
        #endregion
    }  
}
