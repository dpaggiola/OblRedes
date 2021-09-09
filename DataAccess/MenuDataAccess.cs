using System.Collections.Generic;
using IDataAccess;

namespace DataAccess
{
    public class MenuDataAccess : IMenuDataAccess
    {
        public List<string> GetItems(bool isLogged)
        {
            var ret = new List<string>();
            if (isLogged)
                ret = new List<string>
                {
                    "Listado de Usuarios",
                    "Alta Foto",
                    "Listado de Fotos de un usuario",
                    "Listado de Comentarios de una foto",
                    "Alta Comentario",
                    "Salir"
                };
            else
                ret = new List<string>
                {
                    "Alta Usuario",
                    "Loguear Usuario",
                    "Listado de Usuarios",
                    "Salir"
                };


            return ret;
        }
    }
}