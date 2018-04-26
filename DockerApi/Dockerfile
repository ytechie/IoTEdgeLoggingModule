FROM microsoft/dotnet:2.0.0-runtime-stretch

WORKDIR /app
COPY out/ .

CMD ["dotnet", "dockerlogs.dll"]

# docker run -it -v /var/run/docker.sock:/var/run/docker.sock tripdubroot/dockerlogs:0.0.4