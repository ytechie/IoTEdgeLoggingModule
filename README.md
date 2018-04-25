# IoT Edge Logging Module

An Azure IoT Edge Module for collecting edge module logs and pushing them to Azure storage.

## First Tasks
* Determine a way to read the logs from Docker ("docker logs")
* Access those logs from within **this** module
* Push those logs to Azure storage

## Related Links for Docker Logging
* [Strategies for Docker Logging](https://www.loggly.com/docs/strategies-for-docker-logging/)
* [Docker Logging Through Syslog](https://www.loggly.com/docs/docker-syslog/)
* [RSyslog Logging Adapter](https://www.rsyslog.com/using-the-syslog-receiver-module/)

## Similar Architecture
![](https://www.loggly.com/wp-content/uploads/2014/11/Loggly_Docker_container_diagram.png)