FROM node:22-alpine as builder

WORKDIR /app

COPY package*.json ./

# Instalar dependencias
RUN npm ci --only=production=false

COPY . .

# Construir la aplicación para producción
RUN npm run build


# Etapa de producción con nginx
FROM nginx:alpine as production

# Copiar los archivos construidos desde la etapa anterior
COPY --from=builder /app/dist /usr/share/nginx/html

# Copiar configuración personalizada de nginx (opcional)
COPY nginx.conf /etc/nginx/conf.d/default.conf

EXPOSE 80

# Comando para iniciar nginx
CMD ["nginx", "-g", "daemon off;"]
