using System;
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
using System.Threading.Tasks;

namespace Service
{
    public class SqlDBHelper
    {
        public static string EncKey = "*!@#$%^SharpFlux^%$#@!*";
        public readonly IHttpContextAccessor _contextAccessor;
        public static string CONNECTION_STRING;

        protected readonly IConfiguration Configuration;

        public SqlDBHelper(IConfiguration _configuration, IHttpContextAccessor contextAccessor)
        {
            Configuration = _configuration;
            _contextAccessor = contextAccessor;
            CONNECTION_STRING = this.Configuration.GetConnectionString("DefaultConnection");
        }

        public string GetBaseUrl()
        {
            var request = _contextAccessor.HttpContext.Request;
            var host = request.Host.ToUriComponent();
            var pathBase = request.PathBase.ToUriComponent();
            return $"{request.Scheme}://{host}{pathBase}";
        }

        public  class Encrypt_Decrypt
        {
            private static TripleDESCryptoServiceProvider DES = new TripleDESCryptoServiceProvider();
            private static MD5CryptoServiceProvider MD5 = new MD5CryptoServiceProvider();

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

            public static string Encryptdata(string password)
            {
                string strmsg = string.Empty;
                byte[] encode = new byte[password.Length];
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


        public static async Task<DataTable> ExecuteDataTableWithParametersAsync(string CommandName, CommandType cmdType, SqlParameter[] param)
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
                            await con.OpenAsync();
                        }

                        using (SqlDataReader reader = await cmd.ExecuteReaderAsync())
                        {                  
                            table.Load(reader);
                           
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

        public static async Task<DataSet> ExecuteDataSetWithParametersAsync(string CommandName, CommandType cmdType, SqlParameter[] param)
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
                            await con.OpenAsync();
                        }

                        using (SqlDataAdapter adapter = new SqlDataAdapter(cmd))
                        {
                            adapter.Fill(table);
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

        public static async Task<int> ExecuteReaderIntAsync(string CommandName, CommandType cmdType, SqlParameter[] pars)
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
                            await con.OpenAsync();
                        }

                        using (SqlDataReader da = await cmd.ExecuteReaderAsync())
                        {
                            while (await da.ReadAsync())
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


        public static async Task<int> ExecuteNonQuery(string CommandName, CommandType cmdType, SqlParameter[] pars)
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
                            await con.OpenAsync();
                        }

                        return await cmd.ExecuteNonQueryAsync();
                    }
                    catch(Exception ex)
                    {
                        throw ex;
                    }
                }
            }
        }


        public static async Task<int> ExecuteNonQueryWithTableParameter(string CommandName, CommandType cmdType, DataTable table, string TableParameter, string TypeName)
        {
            using (SqlConnection con = new SqlConnection(CONNECTION_STRING))
            {
                using (SqlCommand cmd = con.CreateCommand())
                {
                    cmd.CommandType = cmdType;
                    cmd.CommandText = CommandName;

                    // Create a SqlParameter for the table-valued parameter
                    SqlParameter tableParameter = cmd.Parameters.AddWithValue(TableParameter, table);
                    tableParameter.SqlDbType = SqlDbType.Structured;
                    tableParameter.TypeName = TypeName; // Replace with the actual type name

                    try
                    {
                        if (con.State != ConnectionState.Open)
                        {
                            await con.OpenAsync();
                        }

                        return await cmd.ExecuteNonQueryAsync();
                    }
                    catch
                    {
                        throw;
                    }
                }
            }
        }
    }
}
