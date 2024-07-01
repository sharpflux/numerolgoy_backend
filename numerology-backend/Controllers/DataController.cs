using OmsSolution.Helpers;
using OmsSolution.Models;
using OmsSolution.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace OmsSolution.Controllers
{
    [EnableCors("DefaultCorsPolicy")]
    [Route("api/[controller]")]
    [ApiController]
    public class DataController : ControllerBase
    {

        //private ISiteService _siteService;

        //public DataController(ISiteService siteService)
        //{
        //    _siteService = siteService;
        //}


        //[HttpGet("coffeedata")]
        //public async Task<IActionResult> SaveDataAsync([FromQuery] string data)
        //{
         
        //    try
        //    {
        //        string decodedData = HttpUtility.UrlDecode(data);
        //        List<DataModel> dataList = JsonConvert.DeserializeObject<List<DataModel>>(decodedData);
        //        //var serializer = new XmlSerializer(typeof(DataModel));
        //        //var stringWriter = new StringWriter();
        //        //serializer.Serialize(stringWriter, data);
        //        //string xml = stringWriter.ToString();


        //        //var serializer = new XmlSerializer(typeof(List<DataModel>));
        //        //var stringWriter = new StringWriter();
        //        //serializer.Serialize(stringWriter, dataList);
        //        //string xml = stringWriter.ToString();

        //        var serializer = new XmlSerializer(typeof(List<DataModel>));
        //        var settings = new XmlWriterSettings
        //        {
        //            OmitXmlDeclaration = true, // Exclude XML declaration
        //            Indent = true // Optional: Format the XML with indentation
        //        };

        //        using (var stringWriter = new StringWriter())
        //        using (var xmlWriter = XmlWriter.Create(stringWriter, settings))
        //        {
        //            serializer.Serialize(xmlWriter, dataList);
        //            string xml = stringWriter.ToString();
        //            //var response = await _siteService.SitePunchData(xml);
        //            //if (response == false)
        //            //    return BadRequest(new { message = "Bad Request" });

        //            //return Ok(response);

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }

        //}

         
    }
}
