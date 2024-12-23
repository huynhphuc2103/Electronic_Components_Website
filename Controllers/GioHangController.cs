using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _02_WebsiteBanLinhKienDienTu.Models;

namespace _02_WebsiteBanLinhKienDienTu.Controllers
{
    public class GioHangController : Controller
    {
        // GET: GioHang
        LinhKienDienTuDataContext db = new LinhKienDienTuDataContext();
        public List<GioHang> LayGioHang()
        {
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list == null)
            {
                list = new List<GioHang>();
                Session["GioHang"] = list;
            }
            return list;
        }

        public ActionResult ThemGioHang(int maLK, string strURL)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.Find(l => l.maLK == maLK);
            if (sp == null)
            {
                sp = new GioHang(maLK);
                list.Add(sp);
                return Redirect(strURL);
            }
            else
            {
                sp.soLuong++;
                return Redirect(strURL);
            }
        }

        private int tinhTongSoLuong()
        {
            int sum = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list != null)
            {
                sum = list.Sum(sp => sp.soLuong);
            }
            return sum;
        }

        private double tinhTongThanhTien()
        {
            double sum = 0;
            List<GioHang> list = Session["GioHang"] as List<GioHang>;
            if (list != null)
            {
                sum = list.Sum(sp => sp.thanhTien);
            }
            return sum;
        }

        public ActionResult GioHangRong()
        {
            return View();
        }

        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("GioHangRong", "GioHang");
            }
            List<GioHang> list = LayGioHang();
            ViewBag.TongSL = tinhTongSoLuong();
            ViewBag.TongTT = tinhTongThanhTien();

            return View(list);
        }

        public ActionResult GioHangPartial()
        {
            ViewBag.TongSL = tinhTongSoLuong();
            return PartialView();
        }

        public ActionResult Xoa1SPTrongGioHang(int maLK)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.Single(l => l.maLK == maLK);
            if (sp != null)
            {
                list.RemoveAll(sa => sa.maLK.Equals(maLK));
                return RedirectToAction("GioHang", "GioHang");
            }
            if (list.Count == 0)
                return RedirectToAction("TrangChu", "LinhKien");
            return RedirectToAction("GioHang", "GioHang");
        }

        public ActionResult XoaALLGioHang()
        {
            List<GioHang> list = LayGioHang();
            list.Clear();
            return RedirectToAction("GioHangRong", "GioHang");
        }

        public ActionResult CapNhatGioHang(int maLK, FormCollection f)
        {
            List<GioHang> list = LayGioHang();
            GioHang sp = list.Single(l => l.maLK == maLK);
            if (sp != null)
            {
                sp.soLuong = int.Parse(f["txtSoLuong"].ToString());
            }
            return RedirectToAction("GioHang", "GioHang");
        }

        [HttpGet]
        public ActionResult DatHang()
        {
            if (Session["TaiKhoan"] == null)
                return RedirectToAction("DangNhap", "NguoiDung");
            if (Session["GioHang"] == null)
                return RedirectToAction("TrangChu", "LinhKien");
            List<GioHang> list = LayGioHang();
            ViewBag.TongSL = tinhTongSoLuong();
            ViewBag.TongTT = tinhTongThanhTien();
            return View(list);
        }

        [HttpPost]
        public ActionResult DatHang(FormCollection f)
        {
            //Thêm đơn hàng
            HOADON hd = new HOADON();
            KHACHHANG kh = (KHACHHANG)Session["TaiKhoan"];
            List<GioHang> gh = LayGioHang();
            hd.MAKH = kh.MAKH;
            hd.NGAYLAPHD = DateTime.Now;
            var ngayGiao = string.Format("{0:MM/dd/yyyy}", f["NgayGiao"]);
            hd.NGAYGIAO = DateTime.Parse(ngayGiao);
            db.HOADONs.InsertOnSubmit(hd);
            db.SubmitChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                CTHOADON ctDH = new CTHOADON();
                ctDH.MAHD = hd.MAHD;
                ctDH.MALK = item.maLK;
                ctDH.SOLUONG = item.soLuong;
                ctDH.THANHTIEN = (decimal)item.donGia;
                db.CTHOADONs.InsertOnSubmit(ctDH);
            }
            db.SubmitChanges();
            Session["GioHang"] = null;
            return RedirectToAction("XacNhanDonHang", "GioHang");
        }

        public ActionResult XacNhanDonHang()
        {
            return View();
        }
    }
}