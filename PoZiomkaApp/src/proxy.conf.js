const { env } = require('process');

const target = env.API_DOMAIN && env.API_PORT ? `https://${env.API_DOMAIN}:${env.API_PORT}` : 'https://localhost:8081';

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
