FROM ubuntu:14.04
RUN apt-get update -q
RUN apt-get install -y rsyslog
COPY rsyslog.conf /etc
CMD rsyslogd -n
EXPOSE 514