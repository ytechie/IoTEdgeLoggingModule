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

`FluentdClient` contains a proof of concept for using the [Fluentd](https://www.fluentd.org/) logging driver with Docker and send those logs to a Fluentd listener. This is a robust solution that many are familiar with. More configuration details can be found [here](FluentdClient/usage.md).

Pros:

* Fluentd is already used by many projects
* A wide variety of [output plugins](https://www.fluentd.org/plugins/all) to integrate with your existing solution.

Cons:

* Not supported on Nano Server or Windows IoT Core


## IoT Edge Configuration

In your [deployment manifest](https://docs.microsoft.com/en-us/azure/iot-edge/module-composition#deployment-manifest-example), look for the `runtime.settings.loggingOptions` property. This is a stringified JSON containing the logging options for the Edge agent container:

    {
        "log-driver": "syslog",
        "log-opts": {
            "syslog-address": "tcp://127.0.0.1:32795"
        }
    }

## Custom Logging Driver

Unfortunately, custom [logging drivers](https://docs.docker.com/engine/extend/plugins_logging/#logdriverstartlogging) for Docker are not supported on Windows.

## Related Links for Docker Logging
* [Strategies for Docker Logging](https://www.loggly.com/docs/strategies-for-docker-logging/)
* [Docker Logging Through Syslog](https://www.loggly.com/docs/docker-syslog/)
* [RSyslog Logging Adapter](https://www.rsyslog.com/using-the-syslog-receiver-module/)