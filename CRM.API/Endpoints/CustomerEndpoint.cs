﻿using CRM.API.Models.DAL;
using CRM.API.Models.EN;
using CRM.DTOs.CustomerDTOs;
using Microsoft.AspNetCore.Authorization;

namespace CRM.API.Endpoints
{
    
    public static class CustomerEndpoint
    {
        
        // Método para configurar los endpoints relacionados con los clientes
        public static void AddCustomerEndpoints(this WebApplication app)
        {
            // Configurar un endpoint de tipo POST para buscar clientes
            app.MapPost("/customer/search", async (SearchQueryCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var customer = new Customer
                {
                    Name = customerDTO.Name_Like != null ? customerDTO.Name_Like : string.Empty,
                    LastName = customerDTO.LastName_Like != null ? customerDTO.LastName_Like : string.Empty
                };

                // Inicializar una lista de clientes y una variable para contar las filas
                var customers = new List<Customer>();
                int countRow = 0;

                // Verificar si se debe enviar la cantidad de filas
                if (customerDTO.SendRowCount == 2)
                {
                    // Realizar una búsqueda de clientes y contar las filas
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);
                    if (customers.Count > 0)
                        countRow = await customerDAL.CountSearch(customer);
                }
                else
                {
                    // Realizar una búsqueda de clientes sin contar las filas
                    customers = await customerDAL.Search(customer, skip: customerDTO.Skip, take: customerDTO.Take);
                }

                // Crear un objeto 'SearchResultCustomerDTO' para almacenar los resultados
                var customerResult = new SearchResultCustomerDTO
                {
                    Data = new List<SearchResultCustomerDTO.CustomerDTO>(),
                    CountRow = countRow
                };

                // Mapear los resultados a objetos 'CustomerDTO' y agregarlos al resultado
                customers.ForEach(s => {
                    customerResult.Data.Add(new SearchResultCustomerDTO.CustomerDTO
                    {
                        Id = s.Id,
                        Name = s.Name,
                        LastName = s.LastName,
                        Address = s.Address
                    });
                });

                // Devolver los resultados
                return customerResult;
            }).RequireAuthorization();

            // Configurar un endpoint de tipo GET para obtener un cliente por ID
            app.MapGet("/customer/{id}", async (int id, CustomerDAL customerDAL) =>
            {
                // Obtener un cliente por ID
                var customer = await customerDAL.GetById(id);

                // Crear un objeto 'GetIdResultCustomerDTO' para almacenar el resultado
                var customerResult = new GetIdResultCustomerDTO
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    LastName = customer.LastName,
                    Address = customer.Address
                };

                // Verificar si se encontró el cliente y devolver la respuesta correspondiente
                if (customerResult.Id > 0)
                    return Results.Ok(customerResult);
                else
                    return Results.NotFound(customerResult);
            });

            // Configurar un endpoint de tipo POST para crear un nuevo cliente
            app.MapPost("/customer", async (CreateCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var customer = new Customer
                {
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                // Intentar crear el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Create(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            }).RequireAuthorization();

            // Configurar un endpoint de tipo PUT para editar un cliente existente
            app.MapPut("/customer", async (EditCustomerDTO customerDTO, CustomerDAL customerDAL) =>
            {
                // Crear un objeto 'Customer' a partir de los datos proporcionados
                var customer = new Customer
                {
                    Id = customerDTO.Id,
                    Name = customerDTO.Name,
                    LastName = customerDTO.LastName,
                    Address = customerDTO.Address
                };

                // Intentar editar el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Edit(customer);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            }).RequireAuthorization();

            // Configurar un endpoint de tipo DELETE para eliminar un cliente por ID
            app.MapDelete("/customer/{id}", async (int id, CustomerDAL customerDAL) =>
            {
                // Intentar eliminar el cliente y devolver el resultado correspondiente
                int result = await customerDAL.Delete(id);
                if (result != 0)
                    return Results.Ok(result);
                else
                    return Results.StatusCode(500);
            }).RequireAuthorization();
        }
    }
}
