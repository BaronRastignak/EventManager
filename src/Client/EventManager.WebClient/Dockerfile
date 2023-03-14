FROM node AS build
WORKDIR /app
COPY "Client/EventManager.WebClient/package*.json" .
RUN npm ci
COPY "Client/EventManager.WebClient" .
RUN npm run build

FROM nginx AS final
COPY --from=build /app/build /usr/share/nginx/html/
COPY "Client/EventManager.WebClient/nginx.conf" /etc/nginx/conf.d/default.conf
ENV NODE_ENV production
EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]