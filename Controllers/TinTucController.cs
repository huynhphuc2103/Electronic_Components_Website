using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _02_WebsiteBanLinhKienDienTu.Models;

namespace _02_WebsiteBanLinhKienDienTu.Controllers
{
    public class TinTucController : Controller
    {
        // GET: TinTuc
        LinhKienDienTuDataContext db = new LinhKienDienTuDataContext();
        public ActionResult TinTucList()
        {
            var list = db.TINTUCs.OrderBy(t => t.TENTIEUDE).ToList();
            return View(list);
        }
        
        public ActionResult TinTucPartial()
        {
            var list = db.LOAITINTUCs.OrderBy(t => t.TENLOAITIN).ToList();
            return View(list);
        }

        public ActionResult TinTucTheoLoai(int maLoai)
        {
            var list = db.TINTUCs.Where(t => t.MALOAITIN == maLoai).OrderBy(lk => lk.TENTIEUDE).ToList();
            if (list.Count == 0)
            {
                ViewBag.ThongBao = "Không có tin tức loại này!!";
            }
            return View(list);
        }
    }
}