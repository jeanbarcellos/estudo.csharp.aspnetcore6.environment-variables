var builder = WebApplication.CreateBuilder(args);

// Inje��o de Depend�ncia - Adicionar servi�os ao cont�iner

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer(); // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddSwaggerGen();



var app = builder.Build();

// Middleware - Configure the HTTP request pipeline.

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
