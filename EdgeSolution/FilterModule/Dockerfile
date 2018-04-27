FROM microsoft/dotnet:2.0-sdk AS build-env
WORKDIR /app

COPY *.csproj ./
RUN dotnet restore

COPY . ./
RUN dotnet publish -c Release -o out

FROM microsoft/dotnet:2.0-runtime
WORKDIR /app
COPY --from=build-env /app/out ./
ENTRYPOINT ["dotnet", "FilterModule.dll"]

FROM ubuntu:14.04
RUN apt-get update -q
RUN apt-get install -y rsyslog
COPY rsyslog.conf /etc
CMD rsyslogd -n
EXPOSE 514