const { env } = require('process');

const target = env.API_DOMAIN && env.API_PORT ? `http://${env.API_DOMAIN}:${env.API_PORT}` : 'http://localhost:8080';

const PROXY_CONFIG = [
  {
    context: [
      "/weatherforecast",
    ],
    target,
    secure: false
  }
]

module.exports = PROXY_CONFIG;
