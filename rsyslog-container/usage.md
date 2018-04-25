syslog is a great mechanism for passing messages since it's widely supported, extremely simple, and super fast.

We used rsyslog inside a container to receive syslog messages, aggregate them, and send them to an output. This could be a file inside the container, or it could be any of many [output modules](https://www.rsyslog.com/doc/v8-stable/configuration/modules/idx_output.html).


Creating the logging container:

    docker build -t syslog .

Starting the logging container:

    docker run -i -d -P syslog

Use `docker ps` to see the mapped port number. In my case, it's 32794. We'll need to tell the containers to send their logs to that port.

From MacOS, if you want to send a test message to the syslog server:

    syslog -s -l INFO "Hello, world." -r 127.0.0.1:32794

To start a container and direct its logs to the logging container:

    docker run --log-driver syslog --log-opt syslog-address=tcp://127.0.0.1:32795 hello-world

You can use the rsyslog.conf file to configure the output for rsyslog.