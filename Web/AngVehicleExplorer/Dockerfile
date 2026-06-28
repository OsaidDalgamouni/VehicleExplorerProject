# Stage 1 - Build Angular application
FROM node:18-alpine AS build

WORKDIR /app

COPY package*.json ./

RUN npm ci

COPY . .

RUN npm run build -- --configuration production

# Stage 2 - Serve with Nginx
FROM nginx:alpine

COPY --from=build /app/dist/AngVehicleExplorer /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]