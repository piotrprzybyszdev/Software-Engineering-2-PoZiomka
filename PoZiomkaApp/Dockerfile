FROM node:latest

RUN npm install -g @angular/cli

WORKDIR /PoZiomka/App

COPY . .
RUN npm install

CMD ng serve --host 0.0.0.0 --port 4200
