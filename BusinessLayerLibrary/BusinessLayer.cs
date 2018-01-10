using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;
using System.Reflection;

namespace BusinessLayerLibrary
{


    public class StoredProcedureBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public bool ExecuteStoredProcedure(Object obj, string CRUDAction, string AuditUser)
        {
            //all encompassing executing action method (well it returns a boolean) that uses reflection to edit/create/delete an object
            //you must have the stored procedure parameters as ALL the properties of a class, including the get-only key ones (e.g. ID)
            string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
            int returnVal = -1;
            string StoredProcedureName = "FrontEnd."+ obj.GetType().Name + "_" + CRUDAction;
            try
            {
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(StoredProcedureName, con);
                try
                {
                    //calls the reflection tool, which pulls the properties/values of a class into a collection of sql parameters
                    cmd.CommandType = CommandType.StoredProcedure;

                    ReflectionTool r = new ReflectionTool();
                    List<SqlParameter> ListOfSqlParams = r.SqlParameters(obj.GetType(), obj, AuditUser).ToList();

                    //cmd.paramters is read only...so we have to do a slightly daft loop
                    foreach (SqlParameter sp in ListOfSqlParams)
                    {
                        cmd.Parameters.Add(sp);
                    }

                    SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
                    Param.Direction = System.Data.ParameterDirection.ReturnValue;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
                    return Convert.ToBoolean(returnVal);
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    cmd.Parameters.Clear();
                    con.Close();
                }
            }
            catch
            {
                return false;
            }
        }
    }

    public class SeasonBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Season> Seasons
        {
            //provides a list of Seasons
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Season> SeasonList = new List<Season>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Season_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Season LoopSeason = new Season()
                        {
                            SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                            SeasonDesc = (string)rdr["SeasonDesc"],
                            FirstLaunchDate = Convert.ToDateTime(rdr["FirstLaunchDate"]),
                            SeasonActive = Convert.ToInt32(rdr["SeasonActive"]),
                            VAT = Convert.ToInt32(rdr["VAT"]),
                            GBPUSD = Convert.ToInt32(rdr["GBPUSD"]),
                            GBPEUR = Convert.ToInt32(rdr["GBPEUR"]),
                        };
                        SeasonList.Add(LoopSeason);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return SeasonList;
            }
        }
    }

    public class TeamBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Team> Teams
        {
            //provides a list of Teams
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Team> TeamList = new List<Team>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Team_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Team LoopTeam = new Team()
                        {
                            TeamID = Convert.ToInt32(rdr["TeamID"]),
                            TeamDesc = (string)rdr["TeamDescription"],
                            TeamEmail = (string)rdr["TeamEmail"]
                        };
                        TeamList.Add(LoopTeam);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return TeamList;
            }

        }

    }

    public class AttributeValuePivotBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<AttributeValuePivot> AttributeValuePivots(int DeptID,int ViewID, int SeasonID)
        {
            //provides a list of AttributeValuePivots

            string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
            List<AttributeValuePivot> AttributeValuePivotList = new List<AttributeValuePivot>();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("FrontEnd.AttributeValuePivot_Get", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter parDeptID = new SqlParameter()
                {
                    ParameterName = "@DeptID",
                    Value = DeptID
                };
                cmd.Parameters.Add(parDeptID);
                SqlParameter parViewID = new SqlParameter()
                {
                    ParameterName = "@ViewID",
                    Value = ViewID
                };
                cmd.Parameters.Add(parViewID);

                SqlParameter parSeasonID = new SqlParameter()
                {
                    ParameterName = "@SeasonID",
                    Value = SeasonID
                };
                cmd.Parameters.Add(parSeasonID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    AttributeValuePivot LoopAttributeValuePivot = new AttributeValuePivot()
                    {
                        SFID = Convert.ToInt32(rdr["sfID"]),
                        SunflowerDesc = (string)rdr["SunflowerDesc"],
                        SapArticleID = (string)rdr["SapArticleID"],
                        SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                        MerchCatID = Convert.ToInt32(rdr["MerchCatID"]),
                        VendorID = (string)rdr["VendorID"],
                        DeptID = Convert.ToInt32(rdr["DeptID"]),
                        DeptName = (string)rdr["DeptName"],
                        ConfirmedPreArticleGradeID = Convert.ToInt32(rdr["ConfirmedPreArticleGradeID"]),
                        ConfirmedPreArticleGradeDesc = (string)rdr["ConfirmedPreArticleGradeDesc"],
                        ProposedPreArticleGradeID = Convert.ToInt32(rdr["ProposedPreArticleGradeID"]),
                        ProposedPreArticleGradeDesc = (string)rdr["ProposedPreArticleGradeDesc"],
                        CatDesc = (string)rdr["CatDesc"],
                        MerchCatDesc = (string)rdr["MerchCatDesc"],
                        MasterDescription = (string)rdr["MasterDescription"],
                        VendorDesc = (string)rdr["VendorDesc"],
                        ContinuationStatus = (string)rdr["ContinuationStatus"],
                        ReplacementStatus = (string)rdr["ReplacementStatus"],
                        Concepts = (string)rdr["Concepts"],
                        //surely a way to loop :)
                        Att01 = (string)rdr["Att01"],
                        Att02 = (string)rdr["Att02"],
                        Att03 = (string)rdr["Att03"],
                        Att04 = (string)rdr["Att04"],
                        Att05 = (string)rdr["Att05"],
                        Att06 = (string)rdr["Att06"],
                        Att07 = (string)rdr["Att07"],
                        Att08 = (string)rdr["Att08"],
                        Att09 = (string)rdr["Att09"],
                        Att10 = (string)rdr["Att10"],
                        Att11 = (string)rdr["Att11"],
                        Att12 = (string)rdr["Att12"],
                        Att13 = (string)rdr["Att13"],
                        Att14 = (string)rdr["Att14"],
                        Att15 = (string)rdr["Att15"],
                        Att16 = (string)rdr["Att16"],
                        Att17 = (string)rdr["Att17"],
                        Att18 = (string)rdr["Att18"]
                    };
                    AttributeValuePivotList.Add(LoopAttributeValuePivot);
                }
                cmd.Parameters.Clear();
                con.Close();
            }
            return AttributeValuePivotList;
        }

        public IEnumerable<AttributeValuePivot> AttributeValuePivotWhereQuery(IEnumerable<AttributeValuePivot> source, string columnName, string propertyValue)
        {
            //filters dynamically by the property name using reflection
            return source.Where(m => { return m.GetType().GetProperty(columnName).GetValue(m, null).ToString().ToLower().Contains(propertyValue.ToLower()); });
        }

        //public string ColumnFormat(int AttributeID)
        //{
        //    return "#,##0.00";
        //}
    }

    public class TeamAttributePermissionBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<TeamAttributePermission> TeamAttributePermissions
        {
            //provides a list of TeamAttributePermissions
            //i toyed with having the user/team as a parameter, but didn't
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<TeamAttributePermission> TeamAttributePermissionList = new List<TeamAttributePermission>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.TeamAttributePermission_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        TeamAttributePermission LoopTeamAttributePermission = new TeamAttributePermission()
                        {
                            TeamID = Convert.ToInt32(rdr["TeamID"]),
                            AttributeID = Convert.ToInt32(rdr["AttributeID"]),
                            AttributeDescription = (string)rdr["AttributeDescription"],
                            ReadPermission = Convert.ToInt32(rdr["ReadPermission"]),
                            WritePermission = Convert.ToInt32(rdr["WritePermission"]),
                            Notify = Convert.ToInt32(rdr["Notify"]),
                            ViewChange = Convert.ToInt32(rdr["ViewChange"]),
                            ParkForQuarantine = Convert.ToInt32(rdr["ParkForQuarantine"])
                        };
                        //LoopTeamAttributePermission.PivotOrder = Convert.ToInt32(rdr["PivotOrder"]);
                        TeamAttributePermissionList.Add(LoopTeamAttributePermission);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return TeamAttributePermissionList;
            }
        }

        //public bool InsertTeamAttributePermission(TeamAttributePermission SomeTeamAttributePermission, string AuditUser)
        //{
        //    //adds a TeamAttributePermission
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.TeamAttributePermission_Create", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parDesc = new SqlParameter();
        //            parDesc.ParameterName = "@TeamAttributePermissionDesc";
        //            parDesc.Value = SomeTeamAttributePermission.TeamAttributePermissionDesc;
        //            cmd.Parameters.Add(parDesc);

        //            SqlParameter parFY = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeTeamAttributePermission.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parStart = new SqlParameter();
        //            parStart.ParameterName = "@StartDate";
        //            parStart.Value = SomeTeamAttributePermission.StartDate;
        //            cmd.Parameters.Add(parStart);

        //            SqlParameter parEnd = new SqlParameter();
        //            parEnd.ParameterName = "@EndDate";
        //            parEnd.Value = SomeTeamAttributePermission.EndDate;
        //            cmd.Parameters.Add(parEnd);

        //            SqlParameter TeamAttributePermissionActive = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeTeamAttributePermission.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parUser = new SqlParameter();
        //            parUser.ParameterName = "@User";
        //            parUser.Value = AuditUser;
        //            cmd.Parameters.Add(parUser);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool EditTeamAttributePermission(TeamAttributePermission SomeTeamAttributePermission, string AuditUser)
        //{
        //    //adds a TeamAttributePermission
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.TeamAttributePermission_Edit", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parTeamAttributePermissionID = new SqlParameter();
        //            parTeamAttributePermissionID.ParameterName = "@TeamAttributePermissionID";
        //            parTeamAttributePermissionID.Value = SomeTeamAttributePermission.TeamAttributePermissionID;
        //            cmd.Parameters.Add(parTeamAttributePermissionID);

        //            SqlParameter parDesc = new SqlParameter();
        //            parDesc.ParameterName = "@TeamAttributePermissionDesc";
        //            parDesc.Value = SomeTeamAttributePermission.TeamAttributePermissionDesc;
        //            cmd.Parameters.Add(parDesc);

        //            SqlParameter parFY = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeTeamAttributePermission.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parStart = new SqlParameter();
        //            parStart.ParameterName = "@StartDate";
        //            parStart.Value = SomeTeamAttributePermission.StartDate;
        //            cmd.Parameters.Add(parStart);

        //            SqlParameter parEnd = new SqlParameter();
        //            parEnd.ParameterName = "@EndDate";
        //            parEnd.Value = SomeTeamAttributePermission.EndDate;
        //            cmd.Parameters.Add(parEnd);

        //            SqlParameter TeamAttributePermissionActive = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeTeamAttributePermission.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parUser = new SqlParameter();
        //            parUser.ParameterName = "@User";
        //            parUser.Value = AuditUser;
        //            cmd.Parameters.Add(parUser);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteTeamAttributePermission(TeamAttributePermission SomeTeamAttributePermission, string AuditUser)
        //{
        //    //adds a TeamAttributePermission
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.TeamAttributePermission_Delete", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parTeamAttributePermissionID = new SqlParameter();
        //            parTeamAttributePermissionID.ParameterName = "@TeamAttributePermissionID";
        //            parTeamAttributePermissionID.Value = SomeTeamAttributePermission.TeamAttributePermissionID;
        //            cmd.Parameters.Add(parTeamAttributePermissionID);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }

    public class AttributeValueBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<AttributeValue> AttributeValues
        {
            //provides a list of AttributeValues
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<AttributeValue> AttributeValueList = new List<AttributeValue>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.AttributeValue_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AttributeValue LoopAttributeValue = new AttributeValue()
                        {
                            SFID = Convert.ToInt32(rdr["SFID"]),
                            AttributeID = Convert.ToInt32(rdr["AttributeID"]),
                            AttributeOrder = Convert.ToInt32(rdr["AttributeOrder"]),
                            AttributeDescription = (string)rdr["AttributeDescription"],
                            SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                            AttributeValueEntry = (string)rdr["AttributeValue"]
                        };
                        AttributeValueList.Add(LoopAttributeValue);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return AttributeValueList;
            }
        }

    }

    public class AttributeBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Attribute> Attributes
        {
            //provides a list of Attributes
            get
            {
            
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Attribute> AttributeList = new List<Attribute>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Attribute_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Attribute LoopAttribute = new Attribute()
                        {
                            AttributeID = Convert.ToInt32(rdr["AttributeID"]),
                            AttributeDesc = (string)rdr["AttributeDesc"],
                            Seasonal = Convert.ToInt32(rdr["Seasonal"]),
                            AttributeOrder = Convert.ToInt32(rdr["AttributeOrder"]),
                            SapName = (string)rdr["SapName"],
                            PlmName = (string)rdr["PlmName"],
                            DataTypeID = Convert.ToInt32(rdr["DataTypeID"]),
                            FutureSeasonCascade = Convert.ToInt32(rdr["FutureSeasonCascade"]),
                            Calculated = Convert.ToInt32(rdr["Calculated"])
                        };
                        AttributeList.Add(LoopAttribute);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return AttributeList;
            }
        }
    }

    public class PivotViewBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<PivotView> PivotViews(string AuditUser)
        {
            //provides a list of PivotViews

            string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
            List<PivotView> PivotViewList = new List<PivotView>();

            using (SqlConnection con = new SqlConnection(connString))
            {
                SqlCommand cmd = new SqlCommand("FrontEnd.PivotView_Get", con)
                {
                    CommandType = CommandType.StoredProcedure
                };
                
                SqlParameter parNetworkID = new SqlParameter()
                {
                    ParameterName = "@NetworkID",
                    Value = AuditUser
                };
                cmd.Parameters.Add(parNetworkID);

                con.Open();
                SqlDataReader rdr = cmd.ExecuteReader();
                while (rdr.Read())
                {
                    PivotView LoopPivotView = new PivotView()
                    {
                        ViewID = Convert.ToInt32(rdr["ViewID"]),
                        ViewDesc = (string)rdr["ViewDesc"],
                    };
                    PivotViewList.Add(LoopPivotView);
                }
                cmd.Parameters.Clear();
                con.Close();
            }
            return PivotViewList;
        }

    }

    public class UserBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<User> Users
        {
            //provides a list of Users
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<User> UserList = new List<User>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.User_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        User LoopUser = new User()
                        {
                            TeamID = Convert.ToInt32(rdr["TeamID"]),
                            NetworkID = (string)rdr["NetworkID"]
                        };
                        UserList.Add(LoopUser);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return UserList;
            }

        }

    }

    public class ViewAttributePivotBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<ViewAttributePivot> ViewAttributePivots
        {
            //provides a list of ViewAttributePivots
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<ViewAttributePivot> ViewAttributePivotList = new List<ViewAttributePivot>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.ViewAttributePivot_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        ViewAttributePivot LoopViewAttributePivot = new ViewAttributePivot()
                        {
                            ViewID = Convert.ToInt32(rdr["ViewID"]),
                            AttributeID = Convert.ToInt32(rdr["AttributeID"]),
                            PivotOrder = Convert.ToInt32(rdr["PivotOrder"]),
                            AttributeDescription=(string)rdr["AttributeDescription"]
                        };
                        //LoopViewAttributePivot.PivotOrder = Convert.ToInt32(rdr["PivotOrder"]);
                        ViewAttributePivotList.Add(LoopViewAttributePivot);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return ViewAttributePivotList;
            }
        }

        //public bool InsertViewAttributePivot(ViewAttributePivot SomeViewAttributePivot, string AuditUser)
        //{
        //    //adds a ViewAttributePivot
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.ViewAttributePivot_Create", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parDesc = new SqlParameter();
        //            parDesc.ParameterName = "@ViewAttributePivotDesc";
        //            parDesc.Value = SomeViewAttributePivot.ViewAttributePivotDesc;
        //            cmd.Parameters.Add(parDesc);

        //            SqlParameter parFY = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeViewAttributePivot.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parStart = new SqlParameter();
        //            parStart.ParameterName = "@StartDate";
        //            parStart.Value = SomeViewAttributePivot.StartDate;
        //            cmd.Parameters.Add(parStart);

        //            SqlParameter parEnd = new SqlParameter();
        //            parEnd.ParameterName = "@EndDate";
        //            parEnd.Value = SomeViewAttributePivot.EndDate;
        //            cmd.Parameters.Add(parEnd);

        //            SqlParameter ViewAttributePivotActive = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeViewAttributePivot.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parUser = new SqlParameter();
        //            parUser.ParameterName = "@User";
        //            parUser.Value = AuditUser;
        //            cmd.Parameters.Add(parUser);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool EditViewAttributePivot(ViewAttributePivot SomeViewAttributePivot, string AuditUser)
        //{
        //    //adds a ViewAttributePivot
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.ViewAttributePivot_Edit", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parViewAttributePivotID = new SqlParameter();
        //            parViewAttributePivotID.ParameterName = "@ViewAttributePivotID";
        //            parViewAttributePivotID.Value = SomeViewAttributePivot.ViewAttributePivotID;
        //            cmd.Parameters.Add(parViewAttributePivotID);

        //            SqlParameter parDesc = new SqlParameter();
        //            parDesc.ParameterName = "@ViewAttributePivotDesc";
        //            parDesc.Value = SomeViewAttributePivot.ViewAttributePivotDesc;
        //            cmd.Parameters.Add(parDesc);

        //            SqlParameter parFY = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeViewAttributePivot.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parStart = new SqlParameter();
        //            parStart.ParameterName = "@StartDate";
        //            parStart.Value = SomeViewAttributePivot.StartDate;
        //            cmd.Parameters.Add(parStart);

        //            SqlParameter parEnd = new SqlParameter();
        //            parEnd.ParameterName = "@EndDate";
        //            parEnd.Value = SomeViewAttributePivot.EndDate;
        //            cmd.Parameters.Add(parEnd);

        //            SqlParameter ViewAttributePivotActive = new SqlParameter();
        //            parFY.ParameterName = "@FinancialYear";
        //            parFY.Value = SomeViewAttributePivot.FinancialYear;
        //            cmd.Parameters.Add(parFY);

        //            SqlParameter parUser = new SqlParameter();
        //            parUser.ParameterName = "@User";
        //            parUser.Value = AuditUser;
        //            cmd.Parameters.Add(parUser);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        //public bool DeleteViewAttributePivot(ViewAttributePivot SomeViewAttributePivot, string AuditUser)
        //{
        //    //adds a ViewAttributePivot
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand("FrontEnd.ViewAttributePivot_Delete", con);
        //        try
        //        {
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            SqlParameter parViewAttributePivotID = new SqlParameter();
        //            parViewAttributePivotID.ParameterName = "@ViewAttributePivotID";
        //            parViewAttributePivotID.Value = SomeViewAttributePivot.ViewAttributePivotID;
        //            cmd.Parameters.Add(parViewAttributePivotID);

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

    }

    public class PreArticleSeasonalBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<PreArticleSeasonal> PreArticleSeasonals
        {
            //provides a list of PreArticleSeasonals
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<PreArticleSeasonal> PreArticleSeasonalList = new List<PreArticleSeasonal>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.PreArticleSeasonal_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        PreArticleSeasonal LoopPreArticleSeasonal = new PreArticleSeasonal()
                        {
                            SFID = Convert.ToInt32(rdr["SFID"]),
                            SunflowerDesc = (string)rdr["SunflowerDesc"],
                            PlmDescription = (string)rdr["PlmDescription"],
                            PlmID = (string)rdr["PlmID"],
                            SapArticleID = (string)rdr["SapArticleID"],
                            SapDescription = (string)rdr["SapDescription"],
                            SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                            ProposedPreArticleGradeID = Convert.ToInt32(rdr["ProposedPreArticleGradeID"]),
                            ConfirmedPreArticleGradeID = Convert.ToInt32(rdr["ConfirmedPreArticleGradeID"]),
                            MerchCatID = Convert.ToInt32(rdr["MerchCatID"]),
                            VendorID = (string)rdr["VendorID"],
                            ContinuationStatusID = Convert.ToInt32(rdr["ContinuationStatusID"]),
                            ReplacementStatusID = Convert.ToInt32(rdr["ReplacementStatusID"]),
                            SpaceUseID = (string)rdr["SpaceUseID"],
                        };
                        PreArticleSeasonalList.Add(LoopPreArticleSeasonal);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return PreArticleSeasonalList;
            }
        }

    }

    public class GradeBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Grade> Grades
        {
            //provides a list of Grades
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Grade> GradeList = new List<Grade>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Grade_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Grade LoopGrade = new Grade()
                        {
                            GradeID = Convert.ToInt32(rdr["GradeID"]),
                            GradeDescription = (string)rdr["GradeDescription"],
                            GradeOrder = Convert.ToInt32(rdr["GradeOrder"]),
                            PreArticleGrade = Convert.ToInt32(rdr["PreArticleGrade"]),
                            WholeGrade = Convert.ToInt32(rdr["WholeGrade"]),
                            SubCatGrade = Convert.ToInt32(rdr["PreArticleGrade"])
                        };
                        GradeList.Add(LoopGrade);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return GradeList;
            }
        }

    }

    public class MerchCatBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<MerchCat> MerchCats
        {
            //provides a list of MerchCats
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<MerchCat> MerchCatList = new List<MerchCat>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.MerchCat_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MerchCat LoopMerchCat = new MerchCat()
                        {
                            MerchCatID = Convert.ToInt32(rdr["MerchCatID"]),
                            MerchCatDesc = (string)rdr["MerchCatDesc"],
                            DeptID = Convert.ToInt32(rdr["DeptID"]),
                            CatID = Convert.ToInt32(rdr["CatID"]),
                            MerchCatActive = Convert.ToInt32(rdr["MerchCatActive"]),
                        };
                        MerchCatList.Add(LoopMerchCat);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return MerchCatList;
            }
        }

    }

    public class VendorBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Vendor> Vendors
        {
            //provides a list of Vendors
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Vendor> VendorList = new List<Vendor>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Vendor_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Vendor LoopVendor = new Vendor()
                        {
                            VendorID = (string)rdr["VendorID"],
                            VendorDesc = (string)rdr["VendorDesc"],
                        };
                        VendorList.Add(LoopVendor);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return VendorList;
            }
        }

    }

    public class SpaceBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Space> Spaces
        {
            //provides a list of Spaces
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Space> SpaceList = new List<Space>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Space_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Space LoopSpace = new Space()
                        {
                            SpaceID = Convert.ToInt32(rdr["SpaceID"]),
                            SpaceDesc = (string)rdr["SpaceDesc"],
                            SpaceOrder = Convert.ToInt32(rdr["SpaceOrder"])
                        };
                        SpaceList.Add(LoopSpace);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return SpaceList;
            }
        }

    }

    public class MeTypeBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<MeType> MeTypes
        {
            //provides a list of MeTypes
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<MeType> MeTypeList = new List<MeType>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.MeType_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        MeType LoopMeType = new MeType()
                        {
                            MeTypeID = Convert.ToInt32(rdr["MeTypeID"]),
                            MeTypeDesc = (string)rdr["MeTypeDesc"],
                        };
                        MeTypeList.Add(LoopMeType);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return MeTypeList;
            }
        }

    }

    public class SpaceAllocationBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<SpaceAllocation> SpaceAllocations
        {
            //provides a list of SpaceAllocations
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<SpaceAllocation> SpaceAllocationList = new List<SpaceAllocation>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.SpaceAllocation_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SpaceAllocation LoopSpaceAllocation = new SpaceAllocation()
                        {
                            AutoID = Convert.ToInt32(rdr["AutoID"]),
                            SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                            SpaceID = Convert.ToInt32(rdr["SpaceID"]),
                            CatID = Convert.ToInt32(rdr["CatID"]),
                            SpaceGradeID = Convert.ToInt32(rdr["SpaceGradeID"]),
                            FixtureOrdinal = Convert.ToInt32(rdr["FixtureOrdinal"]),
                            FixtureName = (string)rdr["FixtureName"],
                            OrdinalSequence = Convert.ToInt32(rdr["OrdinalSequence"]),
                            SFID = Convert.ToInt32(rdr["SFID"]),
                            Fill = Convert.ToInt32(rdr["Fill"]),
                            MeTypeID = Convert.ToInt32(rdr["MeTypeID"]),
                            MeTypeDesc = (string)rdr["MeTypeDesc"],
                            SapDescription = (string)rdr["SapDescription"],
                            ProposedGrade = (string)rdr["ProposedGrade"],
                            ConfirmedGrade = (string)rdr["ConfirmedGrade"],
                            DisplayQuantity = Convert.ToInt32(rdr["DisplayQuantity"]),
                            PosSequence = Convert.ToInt32(rdr["PosSequence"]),
                            PosOccurances = Convert.ToInt32(rdr["PosOccurances"]),
                            Facings = Convert.ToInt32(rdr["Facings"]),
                        };
                        SpaceAllocationList.Add(LoopSpaceAllocation);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return SpaceAllocationList;
            }
        }

        //public bool ActionSpaceAllocation(SpaceAllocation SomeSpaceAllocation, string CRUDAction, string AuditUser)
        //{
        //    //all encompassing executing action method (well it returns a boolean) that uses reflection to edit/create/delete an object
        //    //you must have the stored procedure parameters as ALL the properties of a class, including the get-only key ones (e.g. ID)
        //    string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
        //    int returnVal = -1;
        //    string StoredProcedureName = "FrontEnd.SpaceAllocation_" + CRUDAction;
        //    try
        //    {
        //        SqlConnection con = new SqlConnection(connString);
        //        SqlCommand cmd = new SqlCommand(StoredProcedureName, con);
        //        try
        //        {
        //            //calls the reflection tool, which pulls the properties/values of a class into a collection of sql parameters
        //            cmd.CommandType = CommandType.StoredProcedure;

        //            ReflectionTool r = new ReflectionTool();
        //            List<SqlParameter> ListOfSqlParams = r.SqlParameters(typeof(SpaceAllocation), SomeSpaceAllocation, AuditUser).ToList();

        //            //cmd.paramters is read only...so we have to do a slightly daft loop
        //            foreach (SqlParameter sp in ListOfSqlParams)
        //            {
        //                cmd.Parameters.Add(sp);
        //            }

        //            SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
        //            Param.Direction = System.Data.ParameterDirection.ReturnValue;

        //            con.Open();
        //            cmd.ExecuteNonQuery();

        //            returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
        //            return Convert.ToBoolean(returnVal);
        //        }
        //        catch (Exception e)
        //        {
        //            return false;
        //        }
        //        finally
        //        {
        //            cmd.Parameters.Clear();
        //            con.Close();
        //        }
        //    }
        //    catch
        //    {
        //        return false;
        //    }
        //}

        public bool RetazzSpaceAllocation()
        {
            //adds a SpaceAllocation
            string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
            int returnVal = -1;
            try
            {
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand("FrontEnd.SpaceAllocation_Retazz", con);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    
                    SqlParameter Param = cmd.Parameters.Add("Return Value", SqlDbType.Int);
                    Param.Direction = System.Data.ParameterDirection.ReturnValue;

                    con.Open();
                    cmd.ExecuteNonQuery();

                    returnVal = Convert.ToInt32(cmd.Parameters["Return Value"].Value);
                    return Convert.ToBoolean(returnVal);
                }
                catch (Exception e)
                {
                    return false;
                }
                finally
                {
                    cmd.Parameters.Clear();
                    con.Close();
                }
            }
            catch
            {
                return false;
            }
        }

    }

    public class DeptBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Dept> Depts
        {
            //provides a list of Depts
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Dept> DeptList = new List<Dept>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Dept_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Dept LoopDept = new Dept()
                        {
                            DeptID = Convert.ToInt32(rdr["DeptID"]),
                            DeptName = (string)rdr["DeptName"],
                            DeptOrder = Convert.ToInt32(rdr["DeptOrder"]),
                            DeptActive = Convert.ToInt32(rdr["DeptActive"]),
                        };
                        DeptList.Add(LoopDept);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return DeptList;
            }
        }

    }

    public class CatBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Cat> Cats
        {
            //provides a list of Cats
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Cat> CatList = new List<Cat>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Cat_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Cat LoopCat = new Cat()
                        {
                            CatID = Convert.ToInt32(rdr["CatID"]),
                            CatDesc = (string)rdr["CatDesc"],
                            CatActive = Convert.ToInt32(rdr["CatActive"]),
                            CatOrder = Convert.ToInt32(rdr["CatOrder"]),
                            Concept = Convert.ToInt32(rdr["Concept"]),
                        };
                        CatList.Add(LoopCat);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return CatList;
            }
        }


    }

    public class AvailableSpaceBusinessLayer
    {
        string cnPromoPipeline = "sunflower";
        public IEnumerable<AvailableSpace> AvailableSpaces
        {
            //provides a list of AvailableSpaces
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<AvailableSpace> AvailableSpaceList = new List<AvailableSpace>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.AvailableSpace_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        AvailableSpace LoopAvailableSpace = new AvailableSpace()
                        {
                            SeasonID = Convert.ToInt32(rdr["SeasonID"]),
                            SpaceID = Convert.ToInt32(rdr["SpaceID"]),
                            CatID = Convert.ToInt32(rdr["CatID"]),
                            SpaceGradeID = Convert.ToInt32(rdr["SpaceGradeID"]),
                            FixtureOrdinal = Convert.ToInt32(rdr["FixtureOrdinal"]),
                            FixtureName = (string)rdr["FixtureName"],
                            SpaceGradeDescription = (string)rdr["SpaceGradeDescription"],
                        };
                        AvailableSpaceList.Add(LoopAvailableSpace);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return AvailableSpaceList;
            }
        }
    }

    public class DataTypeBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<SFDataType> DataTypes
        {
            //provides a list of DataType
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<SFDataType> DataTypeList = new List<SFDataType>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.SFDataType_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SFDataType LoopDataType = new SFDataType()
                        {
                            DataTypeID = Convert.ToInt32(rdr["DataTypeID"]),
                            DataTypeDescription = (string)rdr["DataTypeDescription"],
                        };
                        DataTypeList.Add(LoopDataType);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return DataTypeList;
            }
        }

    }

    public class SpaceUseBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<SpaceUse> SpaceUses
        {
            //provides a list of SpaceUses
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<SpaceUse> SpaceUseList = new List<SpaceUse>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.SpaceUse_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SpaceUse LoopSpaceUse = new SpaceUse()
                        {
                            SpaceUseID = (string)rdr["SpaceUseID"],
                            SpaceUseDesc = (string)rdr["SpaceUseDesc"],
                        };
                        SpaceUseList.Add(LoopSpaceUse);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return SpaceUseList;
            }
        }

    }

    public class StatusBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<Status> Statuses
        {
            //provides a list of Statuss
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<Status> StatusList = new List<Status>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.Status_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        Status LoopStatus = new Status()
                        {
                            StatusID = Convert.ToInt32(rdr["StatusID"]),
                            StatusDesc = (string)rdr["StatusDesc"],
                            StatusType = (string)rdr["StatusType"],
                            CarryForward = Convert.ToInt32(rdr["CarryForward"]),
                            Default = Convert.ToInt32(rdr["Default"]),
                        };
                        StatusList.Add(LoopStatus);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return StatusList;
            }
        }

    }

    public class TeamViewBusinessLayer
    {
        string cnPromoPipeline = "sunflower";

        public IEnumerable<TeamView> TeamViews
        {
            //provides a list of TeamViews
            get
            {
                string connString = ConfigurationManager.ConnectionStrings[cnPromoPipeline].ConnectionString;
                List<TeamView> TeamViewList = new List<TeamView>();

                using (SqlConnection con = new SqlConnection(connString))
                {
                    SqlCommand cmd = new SqlCommand("FrontEnd.TeamView_Get", con)
                    {
                        CommandType = CommandType.StoredProcedure
                    };
                    con.Open();
                    SqlDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        TeamView LoopTeamView = new TeamView()
                        {
                            TeamID = Convert.ToInt32(rdr["TeamID"]),
                            TeamDescription = (string)rdr["TeamDescription"],
                            ViewID = Convert.ToInt32(rdr["ViewID"]),
                            ViewDescription = (string)rdr["ViewDescription"],
                        };
                        TeamViewList.Add(LoopTeamView);
                    }
                    cmd.Parameters.Clear();
                    con.Close();
                }
                return TeamViewList;
            }
        }

    }
}
