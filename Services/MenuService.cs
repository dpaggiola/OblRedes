using System.Collections.Generic;
using IDataAccess;
using IServices;

namespace Services
{
    public class MenuService : IMenuService
    {
        public IMenuDataAccess menuDataAccess;

        public MenuService(IMenuDataAccess im)
        {
            menuDataAccess = im;
        }

        public List<string> GetMenuItems(bool isLogged)
        {
            var menuItems = menuDataAccess.GetItems(isLogged);
            return menuItems;
        }
    }
}