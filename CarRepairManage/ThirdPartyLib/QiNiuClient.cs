using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Qiniu.Http;
using Qiniu.Storage;
using Qiniu.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThirdPartyLib
{
    public class QiNiuClient
    {

        private static readonly string AccessKey = (ConfigurationManager.AppSettings["QiNiu:AppID"] ?? "").Trim();
        private static readonly string SecretKey = (ConfigurationManager.AppSettings["QiNiu:AppSecret"] ?? "").Trim();
        // 存储空间名
        private static readonly string Bucket = (ConfigurationManager.AppSettings["QiNiu:Bucket"] ?? "").Trim();

        public static QiNiuUploadResult Upload(byte[] bytes, string fileName)
        {
            QiNiuUploadResult model = new QiNiuUploadResult();
            try
            {
                Config config = new Config();
                // 空间对应的机房
                config.Zone = Zone.ZONE_CN_East;
                // 是否使用https域名
                config.UseHttps = true;
                // 上传是否使用cdn加速
                config.UseCdnDomains = true;

                Mac mac = new Mac(AccessKey, SecretKey);

                // 设置上传策略，详见：https://developer.qiniu.com/kodo/manual/1206/put-policy
                PutPolicy putPolicy = new PutPolicy();
                putPolicy.Scope = Bucket;
                putPolicy.SetExpires(3600);
                putPolicy.DeleteAfterDays = 1;
                string token = Auth.CreateUploadToken(mac, putPolicy.ToJsonString());
                config.ChunkSize = ChunkUnit.U512K;
                // 表单上传
                FormUploader target = new FormUploader(config);
                HttpResult result = target.UploadData(bytes, fileName, token, null);

                if (result.Code == 200)
                {
                    model.result = 1;
                    JObject jo = (JObject)JsonConvert.DeserializeObject(result.Text);
                    model.hash = (jo["hash"]?.ToString());
                    model.key = (jo["key"]?.ToString());
                }
                else
                {
                    model.message = "Qiniu  UploadData 上传 文件错误";
                    model.result = 0;
                }
            }
            catch (Exception ex)
            {
                model.message = ex.Message;
                model.result = 0;
            }
            return model;
        }



    }




    #region Model

    public class QiNiuUploadResult
    {
        public QiNiuUploadResult()
        {
            this.result = 1;
            this.message = "OK";
        }

        public int result { get; set; }
        public string message { get; set; }
        public string hash { get; set; }
        public string key { get; set; }
    }

    #endregion
}
