#使用dotnet 6作为基础镜像，起一个别名为build
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
#设置工作目录为/app
WORKDIR /app

# 复制内容 生成
COPY . .
RUN dotnet publish -c Release -o out

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS runtime

# 将字体文件复制到镜像中的/usr/share/fonts目录
RUN apt-get update
RUN apt-get install -y xfonts-utils fontconfig
# RUN mkdir -p /usr/share/fonts
COPY Fonts /usr/share/fonts 
# 更新字体缓存
RUN fc-cache -f -v 

WORKDIR /app
COPY --from=build /app/out ./

#暴露80和443端口
EXPOSE 80

ENTRYPOINT ["dotnet", "Wee_XYYY.dll"]
