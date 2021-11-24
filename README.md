# ASP.NET Core + Variáveis de Ambiente + Docker

Exemplo de Web API criada com ASP.NET Core 6.0 para testes de leitura de variáveis de ambiente utilizando o tipo IConfiguration.

Referências:

- https://renatogroffe.medium.com/asp-net-core-docker-trabalhando-com-vari%C3%A1veis-de-ambiente-619704bd6819
- https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0
- https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited
- https://docs.microsoft.com/pt-br/dotnet/core/docker/build-container?tabs=windows

<br>
<br>

**Anotações**

```
dotnet publish -c Release
```

```
docker build -t api-environment -f Dockerfile .
```

```
docker create --name api-environment api-environment
```
