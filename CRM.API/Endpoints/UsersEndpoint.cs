using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.UsersDTOs;
using static CRM.DTOs.UsersDTOs.SearchResultUsersDTO;

namespace CRM.API.Endpoints
{
    public static class UsersEndpoint
    {
        public static void AddUsersEndpoints(this WebApplication app)
        {
            app.MapPost("/user/search", async (SearchQueryUsersDTO usersDTO, UsersDAL users) =>
            {
                var user = new Users
                {
                    Name = usersDTO.Name_Like ?? string.Empty,
                    LastName = usersDTO.LastName_Like ?? string.Empty,
                };

                // Inicializar una lista de usuarios y una variable para contar las filas
                var usuario = new List<Users>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (usersDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de usuarios y contar las filas
                    usuario = await users.Search(user, skip: usersDTO.Skip, take: usersDTO.Take);
                    if (usuario.Count > 0)
                        countRow = await users.CountSearch(user);
                }
                else
                {
                    // Realizar una búsqueda de usuarios sin contar las filas
                    usuario = await users.Search(user, skip: usersDTO.Skip, take: usersDTO.Take);
                }

                // Crear un objeto 'SearchResultUsersDTO' para almacenar los resultados
                var userResult = new SearchResultUsersDTO
                {
                    Data = new List<SearchResultUsersDTO.UserDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'UserDTO' y agregarlos al resultado
                usuario.ForEach(u =>
                {
                    userResult.Data.Add(new SearchResultUsersDTO.UserDTO
                    {
                        Id = u.Id,
                        Name = u.Name,
                        LastName = u.LastName,
                        Email = u.Email,
                        Phone = u.Phone
                    });
                });

                // Devolver los resultados
                return userResult;
            });

            
        }
    }
}
