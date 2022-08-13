using System;
using System.Data;
using System.Data.SqlClient;
using ESimSol.BusinessObjects;
using System.Data.OleDb;
using ICS.Core.DataAccess;
using ICS.Core.Utility;

namespace ESimSol.Services.DataAccess
{
    public class LocationDA
    {
        public LocationDA() { }

        #region Insert Update Delete Function
        public static IDataReader InsertUpdate(TransactionContext tc, Location oLocation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            return tc.ExecuteReader("EXEC [SP_IUD_Location]"
                                    + "%n, %s, %s,%s, %s,%n, %b, %n, %n, %n, %u ",
                                    oLocation.LocationID, oLocation.LocCode, oLocation.Name,oLocation.ShortName, oLocation.Description, oLocation.ParentID, oLocation.IsActive, oLocation.LocationType, nUserId, (int)eEnumDBOperation, oLocation.NameInBangla);
        }

        public static void Delete(TransactionContext tc, Location oLocation, EnumDBOperation eEnumDBOperation, Int64 nUserId)
        {
            tc.ExecuteNonQuery("EXEC [SP_IUD_Location]"
                                    + "%n, %s, %s,%s, %s,%n, %b, %n, %n, %n, %u ",
                                    oLocation.LocationID, oLocation.LocCode, oLocation.Name, oLocation.ShortName, oLocation.Description, oLocation.ParentID, oLocation.IsActive, oLocation.LocationType, nUserId, (int)eEnumDBOperation, oLocation.NameInBangla);
        }
        #endregion

        #region Get & Exist Function
        public static IDataReader Get(TransactionContext tc, long nID)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location WHERE LocationID=%n", nID);
        }

        public static IDataReader Gets(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location where ParentID !=0 and IsActive =1");
        }
        public static IDataReader GetsByType(TransactionContext tc, EnumLocationType eLocationType)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location WHERE LocationID != 1 AND LocationType = " + (int)eLocationType);
        }

        public static IDataReader GetsByCodeOrName(TransactionContext tc, Location oLocation)
        {
            if (oLocation.LocationNameCode == "00")
            {
                return tc.ExecuteReader("SELECT * FROM View_Location AS TT WHERE TT.ParentID=%n AND TT.IsActive=1 AND TT.LocationID IN (SELECT BL.LocationID FROM BusinessLocation AS BL WHERE BL.BusinessUnitID=%n)", oLocation.ParentID, oLocation.BusinessUnitID);
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_Location AS TT WHERE TT.LocationNameCode LIKE '%" + oLocation.LocationNameCode + "%' AND TT.ParentID="+oLocation.ParentID.ToString()+" AND TT.IsActive=1 AND TT.LocationID IN (SELECT BL.LocationID FROM BusinessLocation AS BL WHERE BL.BusinessUnitID="+oLocation.BusinessUnitID.ToString()+")");
            }
        }



        public static IDataReader GetsByCodeOrNamePick(TransactionContext tc, Location oLocation)
        {
            if (oLocation.LocationNameCode == "00")
            {
                return tc.ExecuteReader("SELECT * FROM View_Location ORDER BY LocCode");
            }
            else
            {
                return tc.ExecuteReader("SELECT * FROM View_Location AS TT WHERE TT.LocationNameCode LIKE '%" + oLocation.LocationNameCode + "%' ORDER BY LocCode");
            }
        }

        public static IDataReader GetsByCode(TransactionContext tc, Location oLocation)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location AS TT WHERE TT.IsActive=1 AND TT.LocationType=" + (int)oLocation.LocationType + " AND TT.LocationNameCode LIKE '%" + oLocation.LocationNameCode + "%'");
        }

        public static IDataReader Gets(TransactionContext tc, string sSQL)
        {
            return tc.ExecuteReader(sSQL);
        }

        public static IDataReader GetsAll(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location Order By LocationID");
        }

        public static IDataReader GetsIncludingStore(TransactionContext tc)
        {
            return tc.ExecuteReader("SELECT * FROM View_Location WHERE IsActive=1 and LocationID in (select LocationID from workingunit where IsActive=1 and operationunitId in (select operationunitID from operationunit where isstore=1 )) Order By [LocCode]");
        }
        #endregion
    }
}
