# Sử dụng ổn định nhất của .NET SDK làm cơ sở
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build-env
WORKDIR /app

# Sao chép file csproj và restore các dependencies
COPY *.csproj ./
RUN dotnet restore

# Sao chép toàn bộ mã nguồn và build ứng dụng
COPY . ./
RUN dotnet publish -c Release -o out

# Sử dụng ổn định nhất của .NET runtime làm cơ sở
FROM mcr.microsoft.com/dotnet/SDK:7.0 AS final-env
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "CodePulse.dll"]
