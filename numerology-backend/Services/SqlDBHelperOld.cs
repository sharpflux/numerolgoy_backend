using System;

// Encryption
using System.Text;
using System.Security.Cryptography;

using System.Collections.Generic;

using System.Data;
using SqlParameter = System.Data.SqlClient.SqlParameter;
using SqlConnection = System.Data.SqlClient.SqlConnection;
using SqlCommand = System.Data.SqlClient.SqlCommand;
using SqlDataAdapter = System.Data.SqlClient.SqlDataAdapter;
using SqlDataReader = System.Data.SqlClient.SqlDataReader;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Service
{
    public class SqlDBHelperOld
    {
        public static string EncKey = "*!@#$%^SharpFlux^%$#@!*";
        public readonly IHttpContextAccessor _contextAccessor;
        //public static string CONNECTION_STRING = "Data Source=DESKTOP-DJN08C6; initial catalog=ProEx; integrated security=true; Pooling=false;Trusted_Connection=True;MultipleActiveResultSets=True;Encrypt=True;TrustServerCertificate=True";
       public static string CONNECTION_STRING;

        protected readonly IConfiguration Configuration;


      

     
        public SqlDBHelperOld(IConfiguration _configuration, IHttpContextAccessor contextAccessor)
        {
            Configuration = _configuration;
            _contextAccessor = contextAccessor;
          //  CONNECTION_STRING = this.Configuration.GetConnectionString("DefaultConnection");
        }


        public string GetBaseUrl()
        {
            var request = _contextAccessor.HttpContext.Request;

            var host = request.Host.ToUriComponent();

            var pathBase = request.PathBase.ToUriComponent();

            return $"{request.Scheme}://{host}{pathBase}";
        }


 
 
        public class Encrypt_Decrypt
        {
            // Fields
            private static TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            private static MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

            // Methods
            public static string Decrypt(string encryptedString, string key)
            {
                string Decrypt;
                try
                {
                    DES.Key = MD5Hash(key);
                    DES.Mode = CipherMode.ECB;
                    byte[] Buffer = Convert.FromBase64String(encryptedString);
                    Decrypt = Encoding.ASCII.GetString(DES.CreateDecryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
                }
                catch
                {
                    Decrypt = "";
                    return Decrypt;
                }
                return Decrypt;
            }

            public static string Encrypt(string stringToEncrypt, string key)
            {
                DES.Key = MD5Hash(key);
                DES.Mode = CipherMode.ECB;
                byte[] Buffer = Encoding.ASCII.GetBytes(stringToEncrypt);
                return Convert.ToBase64String(DES.CreateEncryptor().TransformFinalBlock(Buffer, 0, Buffer.Length));
            }

            public static byte[] MD5Hash(string value)
            {
                return MD5.ComputeHash(Encoding.ASCII.GetBytes(value));
            }



            //.......................................//
            public static string Encryptdata(string password)
            {
                string strmsg = string.Empty;
                byte[] encode = new
                byte[password.Length];
                encode = Encoding.UTF8.GetBytes(password);
                strmsg = Convert.ToBase64String(encode);
                return strmsg;
            }
            public static string Decryptdata(string encryptpwd)
            {
                string decryptpwd = string.Empty;

                UTF8Encoding encodepwd = new UTF8Encoding();
                System.Text.Decoder utf8Decode = encodepwd.GetDecoder();
                byte[] todecode_byte = Convert.FromBase64String(encryptpwd);
                int charCount = utf8Decode.GetCharCount(todecode_byte, 0, todecode_byte.Length);
                char[] decoded_char = new char[charCount];
                utf8Decode.GetChars(todecode_byte, 0, todecode_byte.Length, decoded_char, 0);
                decryptpwd = new String(decoded_char);
                return decryptpwd;

            }

        }

        internal static DataTable ExecuteDataTableWithParameters(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }
        internal static int ExecuteReaderInt(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {


            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            while (da.Read())
                            {
                                return Convert.ToInt32(da[0].ToString());
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return 0;
        }

        /// <summary>
        /// Returns Int32 result from a specified stored procedure
        /// </summary>
        /// <param name="SPName">Stored Procedure Name</param>
        /// <param name="Parameters">Array of SqlParameters</param>
        /// <returns></returns>
        internal static int GetInt32(string SPName, params SqlParameter[] Parameters)
        {
            int output = 0;
            SqlConnection cn = new SqlConnection(CONNECTION_STRING);
            SqlCommand cmd = new SqlCommand(SPName, cn);

            cmd.CommandType = CommandType.StoredProcedure;

            if (Parameters != null)
                foreach (SqlParameter item in Parameters)
                    cmd.Parameters.Add(item);

            cn.Open();

            try
            {
                SqlDataReader dreader = cmd.ExecuteReader();
                if (dreader.Read())
                    if (dreader.GetValue(0) != DBNull.Value)
                        output = Convert.ToInt32(dreader.GetValue(0));

                dreader.Close();
            }
            catch
            {
                if (cn != null) cn.Close();
                throw;
            }

            cn.Close();
            cmd = null;
            cn = null;

            return output;
        }
        internal static String ExecuteReaderWithoutParametes(string CommandName, CommandType cmdType)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    //  cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            while (da.Read())
                            {
                                return da[0].ToString();
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return "";
        }

        internal static List<string> ListExecuteWithParameters(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {
            List<string> list = new List<string>();
            DataTable tables = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tables);

                            for (int i = 0; i < tables.Rows.Count; i++)
                            {
                                list.Add(tables.Rows[i][0].ToString());
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return list;
        }
        internal static List<string> ListExecuteWithoutParameters(string CommandName, CommandType cmdType)
        {
            List<string> list = new List<string>();
            DataTable tables = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    //cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(tables);

                            for (int i = 0; i < tables.Rows.Count; i++)
                            {
                                list.Add(tables.Rows[i][1].ToString());
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return list;
        }
        internal static String ExecuteReader(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {


            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            while (da.Read())
                            {
                                return da[0].ToString();
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return "";
        }
        internal static DataSet ExecuteWithParametersDataSet(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataSet table = new DataSet();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }

        internal static DataTable ExecuteWithParametersDataTable(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }
        public static String GetReader(string commandText, CommandType cmdType, SqlParameter[] parameters)
        {

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {

                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = commandText;
                    cmd.Parameters.AddRange(parameters);
                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataReader da = cmd.ExecuteReader())
                        {
                            while (da.Read())
                            {
                                return da[0].ToString();
                            }
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return "";


        }
        internal static DataTable ExecuteSelectCommand(string CommandName, CommandType cmdType)
        {
            DataTable table = null;
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            table = new DataTable();
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }



        // This function will be used to execute R(CRUD) operation of parameterized commands
        internal static DataTable ExecuteParamerizedSelectCommand(string CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }

        // This function will be used to execute CUD(CRUD) operation of parameterized commands
        internal static bool ExecuteNonQuery(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {

            int result = 0;

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return (result > 0);
        }



        // This function will be used to execute CUD(CRUD) operation of parameterized commands
        internal static string ExecuteNonQueryString(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {

            string result = "";

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery().ToString();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return result;
        }

        // This function will be used to execute CUD(CRUD) operation of parameterized commands
        internal static Int32 ExecuteNonQueryGetInt32(string CommandName, CommandType cmdType, SqlParameter[] pars)
        {

            Int32 result = 0;

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = Convert.ToInt32(cmd.ExecuteScalar());
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return result;
        }

        internal static DataTable ExecuteWithoutParameters(string CommandName, CommandType cmdType/*, SqlParameter[] param*/)
        {
            DataTable table = new DataTable();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    //cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }
        internal static DataSet ExecuteWithoutParametersDataSet(string CommandName, CommandType cmdType/*, SqlParameter[] param*/)
        {
            DataSet table = new DataSet();

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    //cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return table;
        }
        internal static DataTable GetName(String CommandName, CommandType cmdType, SqlParameter[] param)
        {
            DataTable table = new DataTable();
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(param);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                        {
                            da.Fill(table);
                        }
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
            return table;

        }









        internal static bool ExecuteNonQueryWithConnection(string CommandName, CommandType cmdType, SqlParameter[] pars, string CONNECTION_STRING1)
        {


            int result = 0;

            using (SqlConnection con = new SqlConnection(CONNECTION_STRING1))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;
                    cmd.Parameters.AddRange(pars);

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            con.Open();
                        }

                        result = cmd.ExecuteNonQuery();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }

            return (result > 0);
        }
    }
}
