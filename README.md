# IoT Edge Logging Module

An Azure IoT Edge Module for collecting edge module logs and pushing them to Azure storage.

This project explored a number of options for achieving centralized logging for IoT Edge modules.

    +--------+
    |Module 1|+----+
    +--------+     |+--------------+     +-------+
                   >Logging Module |+--->| Azure |
    +--------+     |+--------------+     +-------+
    |Module 2|+----+
    +--------+


### Syslog / RSyslog Logging

Syslog is a great mechanism for passing messages since it's natively supported, extremely simple, and super fast.

We used RSyslog inside a container to receive syslog messages, aggregate them, and send them to an output. This could be a file inside the container, or it could be any of many [output modules](https://www.rsyslog.com/doc/v8-stable/configuration/modules/idx_output.html).

More details on configuring a container to use syslog, or for configuring IoT Edge can be [found here](rsyslog-container/usage.md).

Pros:

* Simple
* High performance
* Included by default

### Fluentd Logging

`FluentdClient` contains a proof of concept for using the Fluentd logging driver with Docker and send those logs to a Fluentd listener. This is a robust solution that many are familiar with. More configuration details can be found [here](FluentdClient/usage.md).

Pros:

* Fluentd is already used by many projects
* A wide variety of [output plugins](https://www.fluentd.org/plugins/all) to integrate with your existing solution.

Cons:

* Not supported on Nano Server or Windows IoT Core




## Related Links for Docker Logging
* [Strategies for Docker Logging](https://www.loggly.com/docs/strategies-for-docker-logging/)
* [Docker Logging Through Syslog](https://www.loggly.com/docs/docker-syslog/)
* [RSyslog Logging Adapter](https://www.rsyslog.com/using-the-syslog-receiver-module/)