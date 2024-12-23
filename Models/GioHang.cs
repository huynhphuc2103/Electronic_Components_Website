using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using _02_WebsiteBanLinhKienDienTu.Models;

namespace _02_WebsiteBanLinhKienDienTu.Models
{
    public class GioHang
    {
        LinhKienDienTuDataContext db = new LinhKienDienTuDataContext();
        public int maLK { get; set; }
        public string tenLK { get; set; }
        public string hinhAnh { get; set; }
        public double donGia { get; set; }
        public int soLuong { get; set; }
        public double thanhTien
        {
            get { return soLuong * donGia; }
        }
        public GioHang(int MALK)
        {
            maLK = MALK;
            LINHKIEN lk = db.LINHKIENs.Single(l => l.MALK == maLK);
            tenLK = lk.TENLK;
            hinhAnh = lk.HINHANH;
            donGia = double.Parse(lk.DONGIA.ToString());
            soLuong = 1;
        }
    }
}