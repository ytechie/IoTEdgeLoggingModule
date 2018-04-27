## Fluentd Commands:

    docker run -i -d -P -v /Users/jasonyoung/src/IoTEdgeLoggingModule/FluentdClient:/fluentd/etc fluent/fluentd
    
    docker run --log-driver=fluentd --log-opt fluentd-address=127.0.0.1:32790 hello-world