FROM  mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR webapp

EXPOSE 80
EXPOSE 5400
EXPOSE 5401

#COPIAR ARCHIVOS DE PROYECTO
COPY ./collabBackend ./
RUN dotnet restore collabBackend.csproj

COPY ./collabDB ./
RUN dotnet restore collabDB.csproj


#Contruir la imagen
FROM  mcr.microsoft.com/dotnet/sdk:7.0 
WORKDIR /webapp
COPY	--from=build /webapp/out .
ENTRYPOINT ["donet","collabBackend.dll"]
