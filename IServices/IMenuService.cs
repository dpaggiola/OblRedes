using System.Collections.Generic;

namespace IServices
{
    public interface IMenuService
    {
        List<string> GetMenuItems(bool isLogged);
    }
}