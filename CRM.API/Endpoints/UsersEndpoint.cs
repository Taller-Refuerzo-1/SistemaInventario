using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
using CRM.DTOs.UsersDTOs;
using Microsoft.AspNetCore.Authorization;

namespace CRM.API.Endpoints
{
    public static class UsersEndpoint
    {
        public static void AddUsersEndpoints(this WebApplication app)
        {
            // Endpoint de búsqueda de usuarios con autorización
            app.MapPost("/User/search", async (SearchQueryUsersDTO usersDTO, UsersDAL users) =>
            {
                var user = new Users
                {
                    Name = usersDTO.Name_Like ?? string.Empty,
                    LastName = usersDTO.LastName_Like ?? string.Empty,
                };

                var usuario = new List<Users>();
                int countRow = 0;

                if (usersDTO.SendRowCount == 2)
                {
                    usuario = await users.Search(user, skip: usersDTO.Skip, take: usersDTO.Take);
                    if (usuario.Count > 0)
                        countRow = await users.CountSearch(user);
                }
                else
                {
                    usuario = await users.Search(user, skip: usersDTO.Skip, take: usersDTO.Take);
                }

                var userResult = new SearchResultUsersDTO
                {
                    Data = new List<SearchResultUsersDTO.UserDTO>(),
                    CountRow = countRow
                };

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

                return userResult;
            });

            // Endpoint de obtención de un usuario por ID con autorización
            app.MapGet("/User/{id}", async (int id, UsersDAL users) =>
            {
                var user = await users.GetById(id);

                var UserResult = new GetIdResultUsersDTO
                {
                    Id = user.Id,
                    Name = user.Name,
                    LastName = user.LastName,
                    Email = user.Email,
                    Phone = user.Phone,
                    Password = user.Password,
                };

                if (UserResult.Id > 0)
                    return Results.Ok(UserResult);
                else
                    return Results.NotFound(UserResult);
            });

            // Endpoint de creación de un usuario con autorización
            app.MapPost("/User", async (CreateUsersDTO create, UsersDAL users) =>
            {
                var user = new Users
                {
                    Name = create.Name,
                    LastName = create.LastName,
                    Email = create.Email,
                    Phone = create.Phone,
                    Password = create.Password,
                };

                int result = await users.Create(user);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Endpoint de edición de un usuario con autorización
            app.MapPut("/User", async (EditUsersDTO edit, UsersDAL users) =>
            {
                var user = new Users
                {
                    Id = edit.Id,
                    Name = edit.Name,
                    LastName = edit.LastName,
                    Email = edit.Email,
                    Phone = edit.Phone,
                    Password = edit.Password
                };

                int result = await users.Edit(user);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });

            // Endpoint de eliminación de un usuario con autorización
            app.MapDelete("/User/{id}", async (int id, UsersDAL users) =>
            {
                int result = await users.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            });
        }
    }
}
