using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Service;
using ViewModels;
using ViewModels.CarRepair;
using System.IO;
using HelperLib;
using LogLib;
using System.Text;
using ThirdPartyLib;

namespace CarRepairWeb.Controllers
{
    /// <summary>
    ///  车 相关
    /// </summary>
    public class MediaController : BaseController
    {
        ShortVideoService _ShortVideoService = new ShortVideoService();

        #region 上传 文件 之 七牛
        /// <summary>
        /// 上次 图片
        /// </summary>
        /// <param name="upImg"></param>
        /// <returns></returns>
        public ActionResult Upload(HttpPostedFileBase upImg)
        {
            QiNiuUploadResult result = UpLoad_Image(upImg);
            string ShowURL = CommonUtil.QiniuShowDomain+result.key;
            return Json(new { result ,ShowURL= ShowURL }, JsonRequestBehavior.AllowGet);
        }

        //将File文件保存到本地，返回物理的相对地址
        private QiNiuUploadResult UpLoad_Image(HttpPostedFileBase file)
        {
            string fileName = Path.GetFileName(file.FileName);
            var bytes = FileHelper.FileToByte(file);
            var result  = QiNiuClient.Upload(bytes, fileName);
            return result;
        }
        #endregion

        #region 短视频

        // 列表 短视频
        public ActionResult ShortVideoList( DateTime startTime, DateTime? endTime, string keyword = "",int pageIndex = 1, int pageSize = 10)
        {
            PageInfoModel page = new PageInfoModel() { PageIndex = pageIndex, PageSize = pageSize };
            endTime = endTime ?? DateTime.Now.Date;
            List<ShortVideoModel> models = _ShortVideoService.GetListByPage(keyword, startTime, Convert.ToDateTime(endTime).AddDays(1), ref page);
            ViewBag.page = page;
            ViewBag.keyword = keyword;
            ViewBag.startTime = startTime;
            ViewBag.endTime = endTime;
            ViewBag.Models = models;


            return View();
        }

        //详情 短视频
        public ActionResult ShortVideoDetail(int id = 0)
        {
            ShortVideoModel model = (id == 0 ? new ShortVideoModel() : _ShortVideoService.GetByID(id));

            ViewBag.Model = model;
            return View();
        }

        //保存 短视频
        public ActionResult SaveShortVideo(ShortVideoModel model)
        {
            long id = 0;
            if (model.ID==0)
            {
                 id = _ShortVideoService.Save(model);
            }
            else
            {
                ShortVideoModel saveModel = _ShortVideoService.GetByID(model.ID);
                saveModel.FileHash = model.FileHash;
                saveModel.FileKey = model.FileKey;
                saveModel.Title = model.Title;
                saveModel.URL = model.URL;
                id = _ShortVideoService.Save(saveModel);
            }
            return Json(id, JsonRequestBehavior.AllowGet);
        }

        //删除  短视频
        public ActionResult DeleteShortVideo(long ID)
        {
            int flag = _ShortVideoService.DeleteByID(ID);
            return Json(flag, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}