using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using _02_WebsiteBanLinhKienDienTu.Models;

namespace _02_WebsiteBanLinhKienDienTu.Controllers
{
    public class NguoiDungController : Controller
    {
        // GET: NguoiDung
        LinhKienDienTuDataContext db = new LinhKienDienTuDataContext();
        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(KHACHHANG k, FormCollection f)
        {
            var ht = f["HoTenKH"];
            var dc = f["DiaChi"];
            var dt = f["DienThoai"];
            var gt = f["GioiTinh"];
            var ns = string.Format("{0:MM/DD/YYYY}", f["NgaySinh"]);
            var tdn = f["TenDN"];
            var mk = f["MatKhau"];
            var nlmk = f["NLMatKhau"];
            var email = f["Email"];

            if (String.IsNullOrEmpty(ht))
                ViewData["HTErorr"] = "Họ tên không được bỏ trống!!";
            if (String.IsNullOrEmpty(tdn))
                ViewData["TDNErorr"] = "Tên đăng nhập không được bỏ trống!!";
            if (String.IsNullOrEmpty(mk))
                ViewData["MKErorr"] = "Mật khẩu không được bỏ trống!!";
            if (String.IsNullOrEmpty(nlmk))
                ViewData["NLMKErorr"] = "Nhập lại mật khẩu không được bỏ trống!";
            if (String.IsNullOrEmpty(dt))
                ViewData["DTErorr"] = "Điện thoại không được bỏ trống";
            if (!String.IsNullOrEmpty(ht) && !String.IsNullOrEmpty(tdn) && !String.IsNullOrEmpty(mk) && !String.IsNullOrEmpty(nlmk) && !String.IsNullOrEmpty(dt))
            {
                k.TENKH = ht;
                k.DIACHI = dc;
                k.SDT = dt;
                k.GIOITINH = gt;
                k.NGAYSINH = DateTime.Parse(ns);
                k.TAIKHOAN = tdn;
                k.MATKHAU = mk;
                k.EMAIL = email;
                db.KHACHHANGs.InsertOnSubmit(k);
                db.SubmitChanges();
                ViewBag.TB = "Đăng ký thành công đặt hàng thôi nào (*^.^*)";
            }
            return View();
        }

        [HttpGet]
        public ActionResult DangNhap()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhap(FormCollection f)
        {
            var tk = f["TenDN"];
            var mk = f["MatKhau"];
            if (String.IsNullOrEmpty(tk))
                ViewData["TKErorr"] = "Tài khoản không được bỏ trống";
            if (String.IsNullOrEmpty(mk))
                ViewData["MKErorr"] = "Mật khẩu không được bỏ trống";
            if (!String.IsNullOrEmpty(tk) && !String.IsNullOrEmpty(mk))
            {
                KHACHHANG k = db.KHACHHANGs.SingleOrDefault(n => n.TAIKHOAN.Equals(tk) && n.MATKHAU.Equals(mk));
                if (k != null)
                {
                    ViewBag.TB = "Đăng nhập thành công đặt hàng thôi nào (*^.^*)";
                    Session["TaiKhoan"] = k;
                }
                else
                    ViewBag.TB = "Sai tên tài khoản hoặc sai mật khẩu, Vui lòng nhập lại!!";
            }
            return View();
        }
        //Admin
        public ActionResult QuanTriAdmin()
        {
            var list = db.LINHKIENs.OrderBy(sp => sp.TENLK).ToList();
            return View(list);
        }

        [HttpGet]
        public ActionResult DangNhapAdmin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangNhapAdmin(FormCollection f)
        {
            var tk = f["TenDN"];
            var mk = f["MatKhau"];
            if (String.IsNullOrEmpty(tk))
                ViewData["TKErorr"] = "Tài khoản không được bỏ trống";
            if (String.IsNullOrEmpty(mk))
                ViewData["MKErorr"] = "Mật khẩu không được bỏ trống";
            if (!String.IsNullOrEmpty(tk) && !String.IsNullOrEmpty(mk))
            {
                QUANTRI qt = db.QUANTRIs.SingleOrDefault(n => n.TAIKHOAN.Equals(tk) && n.MATKHAU.Equals(mk));
                if (qt != null)
                {
                    return RedirectToAction("QuanTriAdmin", "NguoiDung");
                }
                else
                    ViewBag.TB = "Sai tên tài khoản hoặc sai mật khẩu, Vui lòng nhập lại!!";
            }
            return RedirectToAction("DangNhapAdmin", "NguoiDung");
        }
        //Thêm 
        [HttpGet]
        public ActionResult ThemLinhKien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ThemLinhKien(LINHKIEN k, FormCollection f)
        {
            var maLoai = f["maLoai"];
            var tenLK = f["tenLK"];
            var hsx = f["hsx"];
            var moTa = f["moTa"];
            var bh = f["bh"];
            var ngayCN = string.Format("{0:YYYY/MM/DD}", f["ngayCN"]);
            var donGia = f["donGia"];
            var hinh = f["hinh"];
            if (!String.IsNullOrEmpty(maLoai) && !String.IsNullOrEmpty(tenLK) && !String.IsNullOrEmpty(hsx) && !String.IsNullOrEmpty(bh) && !String.IsNullOrEmpty(donGia))
            {
                k.MALOAI = int.Parse(maLoai);
                k.TENLK = tenLK;
                k.HANGSX = hsx;
                k.MOTA = moTa;
                k.TGBH = int.Parse(bh);
                k.NGAYCAPNHAT = DateTime.Parse(ngayCN);
                k.DONGIA = decimal.Parse(donGia);
                k.HINHANH = hinh;
                db.LINHKIENs.InsertOnSubmit(k);
                db.SubmitChanges();
            }
            ViewBag.TB = "Thêm sản phẩm thành công (*^.^*)";
            return View();
        }

        [HttpGet]
        public ActionResult XoaLinhKien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XoaLinhKien(int maLK)
        {
            LINHKIEN l = db.LINHKIENs.Single(ma => ma.MALK == maLK);
            if (l == null)
            {
                return HttpNotFound();
            }
            else
            {
                db.LINHKIENs.DeleteOnSubmit(l);
                db.SubmitChanges();
            }
            ViewBag.TB = "Xoá sản phẩm thành công (*^.^*)";
            return View();
        }

        [HttpGet]
        public ActionResult SuaLinhKien()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SuaLinhKien(LINHKIEN k, FormCollection f, int maLK)
        {
            k = db.LINHKIENs.Single(ma => ma.MALK == maLK);
            var maLoai = f["maLoai"];
            var tenLK = f["tenLK"];
            var hsx = f["hsx"];
            var moTa = f["moTa"];
            var bh = f["bh"];
            var ngayCN = string.Format("{0:YYYY/MM/DD}", f["ngayCN"]);
            var donGia = f["donGia"];
            var hinh = f["hinh"];
            if (!String.IsNullOrEmpty(maLoai) && !String.IsNullOrEmpty(tenLK) && !String.IsNullOrEmpty(hsx) && !String.IsNullOrEmpty(bh) && !String.IsNullOrEmpty(donGia))
            {
                k.MALOAI = int.Parse(maLoai);
                k.TENLK = tenLK;
                k.HANGSX = hsx;
                k.MOTA = moTa;
                k.TGBH = int.Parse(bh);
                k.NGAYCAPNHAT = DateTime.Parse(ngayCN);
                k.DONGIA = decimal.Parse(donGia);
                k.HINHANH = hinh;
                db.SubmitChanges();
            }
            ViewBag.TB = "Sửa linh kiện thành công (*^.^*)";
            return View();
        }
    }
}