using FluentValidation;
using ToDoList.Bll.DTOs;
using ToDoList.Bll.Services;
using ToDoList.Bll.Validations;
using ToDoList.Repository.Services;

namespace ToDoList.Server.Configurations
{
    public static class DependicyInjectionConfigurations
    {
        public static void Configure(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IToDoListRepository, ToDoListRepository>();
            //builder.Services.AddScoped<IToDoListRepository, AdoNetWithSp>();

            builder.Services.AddScoped<IToDoItemService, ToDoItemService>();

            builder.Services.AddScoped<ToDoItemUpdateDtoValidator, ToDoItemUpdateDtoValidator>();
            builder.Services.AddScoped<ToDoItemCreateDtoValidator, ToDoItemCreateDtoValidator>();

            builder.Services.AddScoped<IValidator<ToDoItemCreateDto>, ToDoItemCreateDtoValidator>();
        }
    }
}
