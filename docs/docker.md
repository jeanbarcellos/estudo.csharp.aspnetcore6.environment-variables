Manual

```bash
FROM mcr.microsoft.com/dotnet/aspnet:6.0

COPY bin/Release/net6.0/publish/ App/
WORKDIR /App
ENV DOTNET_EnableDiagnostics=0
ENTRYPOINT ["dotnet", "Demo.dll"]
```

Dockerfile

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


```bash
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/out .

EXPOSE 80

ENTRYPOINT [ "dotnet", "Demo.dll" ]
```


Vamos entender cada linha aqui:

- `FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build`

  - Iremos começar a construção de nossa imagem tendo como base a imagem de sdk do .NET 5 e vamos dar um alias (apelido) para essa etapa de “build”

- `WORKDIR /app`

  - Estamos definindo que a pasta que estarei trabalhando é a “app” (mesmo que não tenha, ele irá criar!)

- `COPY *.csproj ./`

  - Copiamos o arquivo csproj para dentro da imagem, exatamente na pasta que estamos trabalhando, definida no step anterior (app)

- `RUN dotnet restore`

  - Como estamos usando uma imagem base do sdk do .NET, temos acesso ao dotnet CLI, e então executamos o restore dos pacotes da aplicação, no caso do arquivo csproj copiado no step anterior

- `COPY . ./`

  - Após o restore ser concluído, então copiamos todos os demais arquivos da nossa aplicação para dentro da imagem, na nossa workdir (./), que é a “app”

- `RUN dotnet publish -c Release -o out`

  - Então executamos o comando de publish com configuração (-c) de Release e indicamos que os arquivos compilados (DLLs) devem ser gerados numa pasta (-o) “out”.

Até aqui temos a primeira fase (stage) do nosso build, que é no fim das contas a aplicação compilada em modo Release, pronta para “deploy”. Então seguimos para a segunda etapa (stage) do build. Essa estratégia de dividir o build da imagem em várias etapas é chamada de multistage build e ajuda a obter melhor aproveitamento do cache interno do docker no momento de criação das imagens.

Seguindo então no nosso build temos:

- `FROM mcr.microsoft.com/dotnet/aspnet:6.0`

  - Para nosso segundo stage partiremos de uma nova imagem base, a imagem “aspnet” na versão 6, esta é uma imagem otimizada para deploy de uma aplicação aspnet

- `WORKDIR /app`

  - Dentro deste novo stage também definimos nossa pasta de trabalho nomeada “app” (não é a mesma app do stage anterior)

- `COPY --from=build /app/out .`

  - Aqui vem a mágica do multistage build: eu pego o resultado final do meu publish em modo Release do stage anterior “build” (que foi publicado na pasta app/out/) e copio ele para a minha pasta de trabalho atual (.) neste segundo stage (no caso uma pasta app)

- `EXPOSE 80`

  - Indico qual a porta que o container desta imagem irá esperar requests, para conseguir vincular uma porta do meu host no momento de execução

- `ENTRYPOINT [ "dotnet", "Demo.dll" ]`

  - Indico qual comando será executado quando um container desta imagem subir, que no nosso caso é a execução da api (já compilada)
