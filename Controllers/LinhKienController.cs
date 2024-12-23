using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _02_WebsiteBanLinhKienDienTu.Models;

namespace _02_WebsiteBanLinhKienDienTu.Controllers
{
    public class LinhKienController : Controller
    {
        // GET: LinhKien
        LinhKienDienTuDataContext db = new LinhKienDienTuDataContext();
        public ActionResult TrangChu()
        {
            var list = db.LINHKIENs.OrderBy(sp => sp.TENLK).Take(16).ToList();
            return View(list);
        }

        public ActionResult ChiTietLinhKien(int maLK)
        {
            var list = db.LINHKIENs.Single(l => l.MALK == maLK);
            return View(list);
        }

        public ActionResult LoaiLoaiKienPartial()
        {
            var list = db.LOAILINHKIENs.OrderBy(l => l.TENLOAI).ToList();
            return View(list);
        }

        public ActionResult LinhKienTheoLoai(int maLoai)
        {
            var list = db.LINHKIENs.Where(l => l.MALOAI == maLoai).OrderBy(lk => lk.DONGIA).ToList();
            if (list.Count == 0)
            {
                ViewBag.ThongBao = "Không có linh kiện cho loại này!!";
            }
            return View(list);
        }

        public ActionResult SearchLinhKien(string tenLK)
        {
            var list = db.LINHKIENs.Where(sp => sp.TENLK.Contains(tenLK)).ToList();
            ViewBag.TongSL = list.Count;
            return View(list);
        }
    }
}