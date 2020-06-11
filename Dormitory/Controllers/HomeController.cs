using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Dormitory.Models;
using System.IO;
namespace Dormitory.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/


        /// <summary>
        /// 登录方法体
        /// </summary>
        /// <returns></returns>
        public ActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// 登录实现步骤
        /// </summary>
        /// <param name="admin"></param>
        /// <returns></returns>
        public ActionResult LoginFul(Admin admin)
        {
            try
            {
                using (DormitoryDBEntitie db = new DormitoryDBEntitie())
                {
                    var list = db.Admin.Where(x => x.AdminNumber == admin.AdminNumber && x.AdminPwd == admin.AdminPwd).ToList();
                    if (list.Count > 0)
                    {
                        return View("Index");
                    }
                    else
                    {
                        return Content("<script>alert('账号或密码错误！！！');history.go(-1);</script>");
                    }
                }
            }
            catch (Exception)
            {
                return RedirectToAction("Home", "Error");
            }


        }
        /// <summary>
        /// 首页
        /// </summary>
        /// <returns></returns>
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 首页实现步骤
        /// </summary>
        /// <returns></returns>
        public ActionResult IndexFul()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {

            }
            return View();
        }
        /// <summary>
        /// 报错
        /// </summary>
        /// <returns></returns>
        public ActionResult Error()
        {
            return View();
        }
        /// <summary>
        /// 管理员信息修改
        /// </summary>
        /// <returns></returns>
        public ActionResult Dis_Update()
        {
            return View();
        }
        /// <summary>
        /// 修改方法体
        /// </summary>
        /// <returns></returns>
        public ActionResult Dis_Revise()
        {
            return View();
        }
        /// <summary>
        /// 寝室分配人员情况
        /// </summary>
        /// <returns></returns>
        public ActionResult Distrb()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Distribution.ToList();
                ViewBag.count = list.Count;
            }
            return View();
        }
        /// <summary>
        /// 寝室分配人员情况方法体//查询
        /// </summary>
        /// <returns></returns>
        public ActionResult Dispose()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Distribution.Select(x => new { x.DistributionID, x.DistributionNum, x.DistributionImg, x.DistributionAcademic, x.DistributionGoods, x.DistributionTime, x.DistributionName }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="id">需要删除行的ID</param>
        /// <returns></returns>
        public ActionResult Del(string id)
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                Distribution list = db.Distribution.Find(int.Parse(id));
                db.Distribution.Remove(list);
                if (db.SaveChanges() > 0)
                {
                    return Content("ok");
                }
                else
                {
                    return Content("no");
                }
            }
        }
        /// <summary>
        /// 分配学生         //添加
        /// </summary>
        /// <returns></returns>
        public ActionResult Dis_Add()
        {
            return View();
        }
        public ActionResult Dis_Add_list(Distribution d)
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                HttpFileCollectionBase files = Request.Files;
                HttpPostedFileBase file = files[0];
                //获取文件名后缀
                string extName = Path.GetExtension(file.FileName).ToLower();
                //获取保存目录的物理路径
                if (System.IO.Directory.Exists(Server.MapPath("/Images/")) == false)//如果不存在就创建images文件夹
                {
                    System.IO.Directory.CreateDirectory(Server.MapPath("/Images/"));
                }
                string path = Server.MapPath("/Images/"); //path为某个文件夹的绝对路径，不要直接保存到数据库
                //    string path = "F:\\TgeoSmart\\Image\\";
                //生成新文件的名称，guid保证某一时刻内图片名唯一（文件不会被覆盖）
                string fileNewName = Guid.NewGuid().ToString();
                string ImageUrl = path + fileNewName + extName;
                //SaveAs将文件保存到指定文件夹中
                file.SaveAs(ImageUrl);
                //此路径为相对路径，只有把相对路径保存到数据库中图片才能正确显示（不加~为相对路径）
                string url = "\\Images\\" + fileNewName + extName;
                d.DistributionImg=fileNewName + extName;
                d.DistributionTime = Convert.ToDateTime("yyyy-MM-dd HH:mm:ss");
                db.Distribution.Add(d);
                if (db.SaveChanges() > 0)
                {
                    return Content("ok");
                }
                else {
                    return Content("on");
                }

            }
        }
        /// <summary>
        /// 卫生管理
        /// </summary>
        /// <returns></returns>
        public ActionResult Hygiene()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Hygiene.Select(x => new { x.HygieneID, x.HygieneType, x.Distribution.DistributionNum }).ToList();
                ViewBag.num = list.Count;
            }
            return View();
        }
        /// <summary>
        /// 卫生管理显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Hygiene_list()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Hygiene.Select(x => new { x.HygieneID, x.HygieneType, x.Distribution.DistributionNum }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 通知管理
        /// </summary>
        /// <returns></returns>
        public ActionResult HostelNotice()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.HostelNotice.Select(x => new { x.HostelNoticeTitle, x.HostelNoticeCont, x.HostelNoticeTime }).ToList();
                ViewBag.hos = list[list.Count - 1].HostelNoticeCont;
                ViewBag.tit = list[list.Count - 1].HostelNoticeTitle;
                return View();
            }
        }
        /// <summary>
        /// 通知管理显示
        /// </summary>
        /// <returns></returns>
        public ActionResult HostelNotice_list()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.HostelNotice.Select(x => new { x.HostelNoticeTitle, x.HostelNoticeCont, x.HostelNoticeTime }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 旷寝视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Arrive()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Arrive.ToList();
                ViewBag.count = list.Count;
            }
            return View();
        }
        /// <summary>
        /// 旷寝显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Arrive_list()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Arrive.Select(x => new { x.ArriveID, x.ArriveName, x.ArriveTime, x.Distribution.DistributionNum, x.Distribution.DistributionImg, x.Distribution.DistributionAcademic }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 保修管理视图
        /// </summary>
        /// <returns></returns>
        public ActionResult Repairs()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Repairs.Select(x => new { x.RepairsID, x.RepairsDescribe, x.Distribution.DistributionNum, x.RepairsTime }).ToList();
                ViewBag.count = list.Count;
            }
            return View();
        }
        /// <summary>
        /// 保修管理  显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Repairs_list()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Repairs.Select(x => new { x.RepairsID, x.RepairsDescribe, x.Distribution.DistributionNum, x.RepairsTime }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);

            }
        }
        /// <summary>
        /// 出入登记
        /// </summary>
        /// <returns></returns>
        public ActionResult Register()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Register.Select(x => new { x.RegisterID, x.RegisterName, x.RegisterGoods, x.Distribution.DistributionNum, x.Distribution.DistributionImg, x.Distribution.DistributionAcademic, x.RegisterType, x.RegisterTime }).ToList();
                ViewBag.count = list.Count;
            }
            return View();
        }
        /// <summary>
        /// 出入登记显示
        /// </summary>
        /// <returns></returns>
        public ActionResult Register_list()
        {
            using (DormitoryDBEntitie db = new DormitoryDBEntitie())
            {
                var list = db.Register.Select(x => new { x.RegisterID, x.RegisterName, x.RegisterGoods, x.Distribution.DistributionNum, x.Distribution.DistributionImg, x.Distribution.DistributionAcademic, x.RegisterType, x.RegisterTime }).ToList();
                var newdata = new { code = 0, msg = "", count = list.Count, data = list };
                return Json(newdata, JsonRequestBehavior.AllowGet);
            }
        }
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public ActionResult Admin()
        {

            return View();
        }
    }
}
