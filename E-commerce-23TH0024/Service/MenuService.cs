using E_commerce_23TH0024.Data;
using E_commerce_23TH0024.Models;
using Microsoft.EntityFrameworkCore;

namespace E_commerce_23TH0024.Service
{
    public class MenuService: BaseService
    {
        public MenuService(ApplicationDbContext context) : base(context)
        {
        }
        public IEnumerable<MenuViewModels> GetMenus(string? IdNhomMenu = null)
        {
            var menus = from menu in _context.Menu.Include(m => m.NhomMenu)
                        join loai in _context.LoaiSanPham
                        on menu.MaLoaiMenu equals loai.Id into gj
                        from loaiSanPham in gj.DefaultIfEmpty()
                        where (IdNhomMenu == null || menu.IdNhomMenu == IdNhomMenu)
                        select new MenuViewModels
                        {
                            Id = menu.Id,
                            IdNhomMenu = menu.IdNhomMenu,
                            NhomMenu = menu.NhomMenu,
                            LoaiMenu = menu.LoaiMenu,
                            MaLoaiMenu = menu.MaLoaiMenu,
                            LoaiSanPham = loaiSanPham
                        };

            return menus.ToList();
        }

    }
}
