var builder = WebApplication.CreateBuilder(args);

// Injeção de Dependência - Adicionar serviços ao contêiner

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
