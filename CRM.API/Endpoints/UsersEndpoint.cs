using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
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

            app.MapGet("/User/{id}", async (int id, UsersDAL users) =>
            {
                // Obtener un cliente por ID
                var user = await users.GetById(id);

                // Crear un objeto 'GetIdResultCustomerDTO' para almacenar el resultado
                var UserResult = new GetIdResultUsersDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = user.Password,
                };

                // Verificar si se encontró el cliente y devolver la respuesta correspondiente
                if (UserResult.Id > 0)
                    return Results.Ok(UserResult);
                else
                    return Results.NotFound(UserResult);    
            });

            app.MapPost("/User", async (CreateUsersDTO create, UsersDAL users) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var user = new Users
                {
                    Name = create.Name,
                    LastName = create.LastName,
                    Email= create.Email,
                    Phone = create.Phone,
                    Password = create.Password,
                };

                // Intentar crear el cliente y devolver el resultado correspondiente
                int result = await users.Create(user);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            app.MapPut("/User", async (EditUsersDTO edit, UsersDAL users) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var user = new Users
                {
                    Id = edit.Id,
                    Name = edit.Name,
                    LastName = edit.LastName,
                    Email = edit.Email,
                    Phone = edit.Phone,
                    Password = edit.Password
                    
                };

                // Intentar editar el cliente y devolver el resultado correspondiente
                int result = await users.Edit(user);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            app.MapDelete("/User/{id}", async (int id, UsersDAL users) =>
            {
                // Intentar eliminar el cliente y devolver el resultado correspondiente
                int result = await users.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

        }
    }
}
