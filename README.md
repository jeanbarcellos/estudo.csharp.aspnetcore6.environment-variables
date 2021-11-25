# ASP.NET Core + Variáveis de Ambiente + Docker

Exemplo de Web API criada com ASP.NET Core 6.0 para testes de leitura de variáveis de ambiente utilizando o tipo IConfiguration.

Referências:

- https://renatogroffe.medium.com/asp-net-core-docker-trabalhando-com-vari%C3%A1veis-de-ambiente-619704bd6819
- https://docs.microsoft.com/pt-br/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0
- https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited
- https://docs.microsoft.com/pt-br/dotnet/core/docker/build-container?tabs=windows
- https://docs.microsoft.com/pt-br/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-6.0#the-dockerfile
- https://digitalinnovation.one/artigos/aspnet-5core-docker-variaveis-de-ambientes

<br>
<br>

## Dockerfile

```bash
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /source

# copy csproj and restore as distinct layers
COPY *.sln .
COPY *.csproj .
RUN dotnet restore

# copy everything else and build app
COPY . .
WORKDIR /source
RUN dotnet publish -c release -o /app --no-restore

# final stage/image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app ./
ENTRYPOINT ["dotnet", "Demo.dll"]
```

Explicação linha por linha:

- `FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build`

  - Constrói uma imagem tendo como base a imagem de `sdk` do `.NET 6`, definindo um _alias_ (apelido) para essa etapa de `“build”`

- `WORKDIR /app`

  - Definine que a pasta de trabalho trabalhando é a `“/source”` (mesmo que não tenha, ele irá criar!)

- `COPY *.csproj ./`

  - Copia o arquivo `csproj` para dentro da imagem, exatamente na pasta que estamos trabalhando, definida no passo anterior (`/source`)

- `RUN dotnet restore`

  - Como está sendo usada uma imagem base do `sdk` do `.NET`, tem-se acesso ao `CLI do dotnet`, e então é executado o restore dos pacotes da aplicação, no caso do arquivo `csproj` copiado no passo anterior

- `COPY . ./`

  - Após o restore ser concluído, então é copiado todos os demais arquivos da aplicação para dentro da imagem, na workdir (`./`), que é a `“/source”`

- `RUN dotnet publish -c Release -o out`

  - Então é executado o comando de `publish` com configuração (`-c`) de `Release` e indicamos que os arquivos compilados (`DLLs`) devem ser gerados numa pasta (`-o`) `“/app”`. O argumento `--no-restore` faz com que não seja executada uma restauração implícita ao executar o comando

Até aqui temos a **primeira fase** (`stage`) do build, que é, no fim das contas, a _aplicação compilada em modo Release_, pronta para `“deploy”`.

Então seguimos para a **segunda etapa** (`stage`) do build, o deploy.

Essa estratégia de dividir o `build` da imagem em várias etapas é chamada de `multistage build` e ajuda a obter melhor aproveitamento do cache interno do docker no momento de criação das imagens.

Seguindo então no nosso build temos:

- `FROM mcr.microsoft.com/dotnet/aspnet:6.0`

  - O segundo `stage` partirá de uma nova imagem base, a imagem `“aspnet”` na versão 6. Esta é uma imagem otimizada para `deploy` de uma aplicação aspnet

- `WORKDIR /app`

  - Dentro deste novo stage também definimos nossa pasta de trabalho nomeada `“/app”`

- `COPY --from=build /app ./`

  - Aqui vem a mágica do multistage build: é pego o resultado final do `publish` em modo Release do stage anterior `“build”` (que foi publicado na pasta `app/`) e copiado para a pasta de trabalho atual (`./`) neste segundo stage (no caso uma pasta app)

- `ENTRYPOINT [ "dotnet", "Demo.dll" ]`

  - Indico qual comando será executado quando um container desta imagem subir, que no nosso caso é a execução da api (já compilada)

---

<br>
<br>
<br>

**Outros comandos**

```
dotnet publish -c Release
```

```
docker build -t api-environment -f Dockerfile .
```

```
docker create --name api-environment api-environment
```
