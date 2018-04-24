var logger = require('fluent-logger');

logger.configure('fluentd.test', {
    host: 'localhost',
    port: 32790,
    timeout: 3.0,
    reconnectInterval: 600000 // 10 minutes
  });


setInterval(() => {
    logger.emit('follow', {from: 'userA', to: 'userB'});
}, 1000);

