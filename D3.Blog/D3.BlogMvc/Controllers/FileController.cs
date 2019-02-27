using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Renci.SshNet;

namespace D3.BlogMvc.Controllers
{
    /// <summary>
    /// 图片文件上传
    /// </summary>
    [Authorize]
    public class FileController : Controller
    {
        private const string remoteSavePath = "/usr/share/nginx/images"; //linux上图片文件存储路径
        private const string remoteHost = "34.80.75.250";//服务器地址
        private const string remoteUser = "root";//用户名
        private const string remotePwd = "970307Lbx";//密码

        public IActionResult UpLoadFile([FromQuery]string flag)
        {
            var res=new Object();
            switch (flag)
            {
                 case "md":
                     res=  DealMarkDownFile(HttpContext.Request.Form.Files);
                     break;
                 case "we":
                     res= DealWangEditerFile(HttpContext.Request.Form.Files);
                     break;
            }
            return new JsonResult(res);
        }
        /// <summary>
        /// 富文本编辑器文件处理
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private Object DealWangEditerFile(IFormFileCollection files)
        {
            var res = UpLoadFiles(files);
            List<string> fileurls=new List<string>();
            var returnres = new
            {
                errno = 1,           // 0 表示上传失败，1 表示上传成功
                data=fileurls
            };
            if (!res.Item1)
            {
                return returnres;
            }
            else
            {
                fileurls.Add("http://34.80.75.250/images/"+res.Item2);
                returnres = new
                {
                    errno = 0,           // 0 表示上传失败，1 表示上传成功
                    data=fileurls       // 上传成功时才返回
                };
                return returnres;
            }
        }
        /// <summary>
        /// markdown文件处理
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>

        private Object DealMarkDownFile(IFormFileCollection files)
        {
            var res = UpLoadFiles(files);
            var returnres = new
            {
                success = 0,           // 0 表示上传失败，1 表示上传成功
                message = "文件不正确，请重新选择",
                url     =""        // 上传成功时才返回
            };
            if (!res.Item1)
            {
                return returnres;
            }
            else
            {
                returnres = new
                {
                    success = 1,           // 0 表示上传失败，1 表示上传成功
                    message = "上传成功",
                    url     ="http://34.80.75.250/images/"+res.Item2        // 上传成功时才返回
                };
                return returnres;
            }
        }

        /// <summary>
        /// 上传文件到linux服务器
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private Tuple<bool,string> UpLoadFiles(IFormFileCollection files)
        {
            var file = files[0];
            string fileExtName = Path.GetExtension(file.FileName); //获取扩展名
            string newfilename=Guid.NewGuid().ToString("N")+"."+fileExtName;//保存的文件名称
            if (files.Count<=0)
            {
                return new Tuple<bool, string>(false,"");
            }
            else
            {
                //将文件上传到linux文件服务器
                try
                {
                    SftpClient client=new SftpClient(remoteHost,remoteUser,remotePwd);
                    client.Connect();
                    client.UploadFile(files[0].OpenReadStream(), remoteSavePath+"/"+newfilename);
                    client.Disconnect();
                }
                catch (Exception ex)
                {                   
                    return new Tuple<bool, string>(false,"");
                }
                return new Tuple<bool, string>(true,newfilename);
            }
        }
    }

}