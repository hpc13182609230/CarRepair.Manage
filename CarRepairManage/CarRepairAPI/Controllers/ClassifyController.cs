using Em.Future._2017.Common;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ViewModels;
using ViewModels.CarRepair;

namespace CarRepairAPI.Controllers
{
    [RoutePrefix("api/Classify")]
    public class ClassifyController : ApiController
    {
        //根据一级分类 获取分类列表
        [Route("GetPartsClassifyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyList(long OptionID,string keyword)
        {
            DataResultModel result = new DataResultModel();
            PartsClassifyService service = new PartsClassifyService();
            try
            {
                List<PartsClassify> models = service.SearchAllByParentIDThenOrder(OptionID,keyword);
                result.data = models;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //获取子分类 下面的配件商 
        [Route("GetPartsClassifyCompanyList")]
        [HttpGet]
        public DataResultModel GetPartsClassifyCompanyList(long partsClassifyID, string keyword)
        {
            keyword = keyword ?? "";
            PageInfoModel page = new PageInfoModel() { PageIndex=1,PageSize=100};
            DateTime start= new DateTime(2017, 1, 1, 0, 00, 00, 001); //正确
            DataResultModel result = new DataResultModel();
            PartsCompanyService service = new PartsCompanyService();
            try
            {
                List<PartsCompany> data = service.GetListByPage(keyword, partsClassifyID, start, DateTime.Now, ref page);
                foreach (var item in data)
                {
                    item.Content = xxHTML(item.Content);
                }
                result.data = data;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        //获取配件商 的详情
        [Route("GetPartsCompany")]
        [HttpGet]
        public DataResultModel GetPartsCompany(long id)
        {
            DataResultModel result = new DataResultModel();
            PartsCompanyService service = new PartsCompanyService();
            try
            {
                PartsCompany model = service.GetByID(id);
                result.data = model;
            }
            catch (Exception ex)
            {
                result.result = 0;
                result.message = ex.Message;
            }
            return result;
        }

        #region 内部方法
        public string xxHTML(string html)
        {

            html = html.Replace("(<style)+[^<>]*>[^\0]*(</style>)+", "");
            html = html.Replace(@"\<img[^\>] \>", "");
            html = html.Replace(@"<p>", "");
            html = html.Replace(@"</p>", "");


            System.Text.RegularExpressions.Regex regex0 =
            new System.Text.RegularExpressions.Regex("(<style)+[^<>]*>[^\0]*(</style>)+", System.Text.RegularExpressions.RegexOptions.Multiline);
            System.Text.RegularExpressions.Regex regex1 = new System.Text.RegularExpressions.Regex(@"<script[\s\S] </script *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex2 = new System.Text.RegularExpressions.Regex(@" href *= *[\s\S]*script *:", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex3 = new System.Text.RegularExpressions.Regex(@" on[\s\S]*=", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex4 = new System.Text.RegularExpressions.Regex(@"<iframe[\s\S] </iframe *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex5 = new System.Text.RegularExpressions.Regex(@"<frameset[\s\S] </frameset *>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex6 = new System.Text.RegularExpressions.Regex(@"\<img[^\>] \>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex7 = new System.Text.RegularExpressions.Regex(@"</p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex8 = new System.Text.RegularExpressions.Regex(@"<p>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            System.Text.RegularExpressions.Regex regex9 = new System.Text.RegularExpressions.Regex(@"<[^>]*>", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            html = regex1.Replace(html, ""); //过滤<script></script>标记  
            html = regex2.Replace(html, ""); //过滤href=javascript: (<A>) 属性   
            html = regex0.Replace(html, ""); //过滤href=javascript: (<A>) 属性   


            //html = regex10.Replace(html, "");  
            html = regex3.Replace(html, "");// _disibledevent="); //过滤其它控件的on...事件  
            html = regex4.Replace(html, ""); //过滤iframe  
            html = regex5.Replace(html, ""); //过滤frameset  
            html = regex6.Replace(html, ""); //过滤frameset  
            html = regex7.Replace(html, ""); //过滤frameset  
            html = regex8.Replace(html, ""); //过滤frameset  
            html = regex9.Replace(html, "");
            //html = html.Replace(" ", "");  
            html = html.Replace("</strong>", "");
            html = html.Replace("<strong>", "");
            html = html.Replace(" ", "");
            return html;
        }
        #endregion

    }
}
