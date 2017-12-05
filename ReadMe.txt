--to do list

1) concepts 
	add ConceptProduct table
	add Concepts into Cat table
	ensure concepts do/don't flow in all uses
	create aggregate concept field for Total Lines view

2) data type validation

3) clone season to season

4) drop down data







not in Linq


List<Flattribute> z = ListOfSDHFlattributes.Except(ListOfFlattributes, new ClassLibrary.Compare.LambdaComparer<Flattribute>((x, y) => x.SAPStoreID == y.SAPStoreID)).ToList();
List<Flattribute> UnknownFlattributes = ListOfFlattributesFromTextFile.Except(ListOfFlattributes, new ClassLibrary.Compare.LambdaComparer<Flattribute>((x, y) => x.SAPStoreID == y.SAPStoreID)).ToList();

join
List<Event> ListOfEventsForStores = ListOfEvents.Join(StoreEventDayTypes, a => a.DayID, b => b.DayTypeID, (a, b) => new { a, b }).Select(z => z.a).ToList();

turn list into a smaller one
ListOfEvents.OrderByDescending(m => m.EventStartDate).Select(m => new SelectListItem { Value = m.EventID.ToString(), Text = m.EventDescription + " (" + m.EventID.ToString() + ")" });




--dumping ground, for reflection code--------------------------------------------------------------------------------------------------------------------------------------------------

try
            {
                SqlConnection con = new SqlConnection(connString);
                SqlCommand cmd = new SqlCommand(StoredProcedureName, con);
                try
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    //use BigParameter, a bespoke class to form a list of parameters
                    List<BigParameter> ListOfBigParameters = new List<BigParameter>();

                    //use reflection to get propoerties of the class
                    PropertyInfo[] properties = typeof(PreArticleSeasonal).GetProperties();
                    foreach (PropertyInfo property in properties)
                    {
                        BigParameter bp = new BigParameter();
                        bp.ParameterName = property.Name;
                        bp.ParameterType = property.PropertyType.Name;
                        switch (bp.ParameterType)
                        {
                            case "Int32":
                                //parameter is an integer
                                bp.ParameterIntValue = Convert.ToInt32(property.GetValue(SomePreArticleSeasonal, null));
                                break;
                            case "String":
                                //parameter is a string
                                bp.ParameterStringValue = (string)property.GetValue(SomePreArticleSeasonal, null);
                                break;
                        }
                        
                        ListOfBigParameters.Add(bp);

                    }

                    foreach (BigParameter LoopBigParameter in ListOfBigParameters)
                    {
                        SqlParameter p = new SqlParameter();
                        p.ParameterName = "@" + LoopBigParameter.ParameterName;
                        switch (LoopBigParameter.ParameterType)
                        {
                            case "Int32":
                                //parameter is an integer
                                p.Value = LoopBigParameter.ParameterIntValue;
                                break;
                            case "String":
                                //parameter is a string
                                p.Value = LoopBigParameter.ParameterStringValue;
                                break;
                        }
                        cmd.Parameters.Add(p);
                    }

                    //special parameter that isnt in the properties of the class
                    SqlParameter parUser = new SqlParameter()
                    {
                        ParameterName = "@User",
                        Value = AuditUser
                    };
                    cmd.Parameters.Add(parUser);