using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Reflection;

namespace BusinessLayerLibrary
{
    public class Compare
    {
        //brendan.enrick.com/post/LINQ-Your-Collections-with-IEqualityComparer-and-Lambda-Expressions
        //this is to provide a better comparer for the except part of Linq, to enable a simple exclusion by a=b

        public class LambdaComparer<T> : IEqualityComparer<T>
        {
            private readonly Func<T, T, bool> _lambdaComparer;
            private readonly Func<T, int> _lambdaHash;

            public LambdaComparer(Func<T, T, bool> lambdaComparer) :
                this(lambdaComparer, o => 0)
            {
            }

            public LambdaComparer(Func<T, T, bool> lambdaComparer, Func<T, int> lambdaHash)
            {
                if (lambdaComparer == null)
                    throw new ArgumentNullException("lambdaComparer");
                if (lambdaHash == null)
                    throw new ArgumentNullException("lambdaHash");

                _lambdaComparer = lambdaComparer;
                _lambdaHash = lambdaHash;
            }

            public bool Equals(T x, T y)
            {
                return _lambdaComparer(x, y);
            }

            public int GetHashCode(T obj)
            {
                return _lambdaHash(obj);
            }
        }
    }

    public class GeocoderLocation
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return String.Format("{0}, {1}", Latitude, Longitude);
        }
    }

    public class Coordinates
    
    {
        //www.superstarcoders.com/blogs/posts/geocoding-in-c-sharp-using-google-maps.aspx
        //to return the coordinates from a postcode, using the google API
        public static GeocoderLocation Locate(string AddressQuery)
        {
            //try this next to get the proxy working
            //msdn.microsoft.com/en-gb/library/ms172495(v=vs.90).aspx
            try
            {
                //WebProxy proxy = new WebProxy("smoothwallcolo", 8080);
                //proxy.Credentials = new NetworkCredential("openney", "", "dunelmmill");
                ////request.Proxy = proxy;
                ////request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                //WebRequest.DefaultWebProxy = proxy;

                WebProxy proxyObject = new WebProxy("smoothwallcolo", 800);

                //// Disable proxy use when the host is local.
                //proxyObject.BypassProxyOnLocal = false;

                //// HTTP requests use this proxy information.
                //WebRequest.DefaultWebProxy = proxyObject;

                //--------------------------------------------------------------------------

                AddressQuery = AddressQuery.Replace(" ", String.Empty);

                WebRequest request = WebRequest
                   .Create("http://maps.googleapis.com/maps/api/geocode/xml?sensor=false&address="
                      + HttpUtility.UrlEncode(AddressQuery));

                //----------------------------------
                request.Proxy = null;
                request.Proxy = proxyObject;
                request.Proxy.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                request.Credentials = System.Net.CredentialCache.DefaultNetworkCredentials;
                //----------------------------------

                using (WebResponse response = request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        XDocument document = XDocument.Load(new StreamReader(stream));

                        XElement longitudeElement = document.Descendants("lng").FirstOrDefault();
                        XElement latitudeElement = document.Descendants("lat").FirstOrDefault();

                        if (longitudeElement != null && latitudeElement != null)
                        {
                            return new GeocoderLocation
                            {
                                Longitude = Double.Parse(longitudeElement.Value, CultureInfo.InvariantCulture),
                                Latitude = Double.Parse(latitudeElement.Value, CultureInfo.InvariantCulture)
                            };
                        }
                    }
                }

                return new GeocoderLocation
                {
                    Longitude = 0,
                    Latitude = 0
                };
            }
            catch (Exception x)
            {
                return new GeocoderLocation
                {
                    Longitude = 0,
                    Latitude = 0
                };
            }


        }
    }

    public class ReflectionTool
    {
        public IEnumerable<SqlParameter> SqlParameters(Type t, Object o, string AuditUser="")
        {
            List<SqlParameter> ListOfSqlParameters = new List<SqlParameter>();
            //use BigParameter, a bespoke class to form a list of parameters
            List<BigParameter> ListOfBigParameters = new List<BigParameter>();

            //use reflection to get properties of the class
            PropertyInfo[] properties = t.GetProperties();
            foreach (PropertyInfo property in properties)
            {
                BigParameter bp = new BigParameter();
                bp.ParameterName = property.Name;
                bp.ParameterType = property.PropertyType.Name;
                switch (bp.ParameterType)
                {
                    case "Int32":
                        //parameter is an integer
                        bp.ParameterIntValue = Convert.ToInt32(property.GetValue(o, null));
                        break;
                    case "String":
                        //parameter is a string
                        bp.ParameterStringValue = (string)property.GetValue(o, null);
                        break;
                    case "Decimal":
                        //parameter is a string
                        bp.ParameterDecimalValue = Convert.ToDecimal(property.GetValue(o, null));
                        break;
                    case "DateTime":
                        //parameter is a string
                        bp.ParameterDateTimeValue = Convert.ToDateTime(property.GetValue(o, null));
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
                    case "Decimal":
                        //parameter is a string
                        p.Value = LoopBigParameter.ParameterDecimalValue;
                        break;
                    case "DateTime":
                        //parameter is a string
                        p.Value = LoopBigParameter.ParameterDateTimeValue;
                        break;
                }
                ListOfSqlParameters.Add(p);
            }
            //finally, the audit user as
            if (AuditUser!="")
            {
                SqlParameter parUser = new SqlParameter()
                {
                    ParameterName = "@User",
                    Value = AuditUser
                };
                ListOfSqlParameters.Add(parUser);
            }
            return ListOfSqlParameters;

        }
    }

}
