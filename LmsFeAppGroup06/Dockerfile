FROM node:14.17.4 as node
WORKDIR /app
COPY . .
RUN npm install
RUN npm run build --prod

FROM nginx:alpine

COPY ./nginx/nginx.conf /etc/nginx/nginx.conf
COPY --from=node /app/dist/LmsFeAppGroup06  /usr/share/nginx/html