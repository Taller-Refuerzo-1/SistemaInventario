using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CRM.DTOs.UsersDTOs
{
    public class LoginDTO
    {
        /// <summary>
        /// Nombre de usuario.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Contraseña del usuario.
        /// </summary>
        public string Password { get; set; }
    }
}
