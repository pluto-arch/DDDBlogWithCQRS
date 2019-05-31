using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using D3.Blog.Domain.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace D3.BlogApi.Controllers
{
    /// <summary>
    /// valueController
    /// </summary>
    [Route("api/Values")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IUser _user;
        public ValuesController(IUser accessor)
        {
            _user = accessor;
        }

        /// <summary>
        /// GET api/values
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { _user.Name, _user.Id ,_user.ClientIP};
        }
        


        /// <summary>
        /// GET api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
//        [Authorize(Roles ="admin")]
        [HttpGet("auth")]
        public ActionResult<string> auth(int id)
        {
            return "这里是需要授权验证的资源";
        }

        /// <summary>
        /// POST api/values
        /// </summary>
        /// <param name="value"></param>
        [HttpPost("DealHttpBody")]
        public void DealHttpBody()
        {
            string inputBody;
            using (var reader = new System.IO.StreamReader(
                Request.Body, System.Text.Encoding.UTF8))
            {
                inputBody = reader.ReadToEnd();
            }
            Dictionary<string,string> dd=new Dictionary<string, string>();
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(inputBody);
            XmlNode rootNode = xmlDoc.SelectSingleNode("xml");
            foreach (XmlNode xxNode in rootNode.ChildNodes)
            {
                string dsf = xxNode.InnerText;
                string sdf = xxNode.Name;
                dd.Add(sdf, dsf);
            }

        }

        /// <summary>
        /// PUT api/values/5
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        /// <summary>
        /// DELETE api/values/5
        /// </summary>
        /// <param name="id"></param>
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
