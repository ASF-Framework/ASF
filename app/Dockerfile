FROM nginx AS base
WORKDIR /app
EXPOSE 80

# 开始构建nodejs环境进行发布asf
FROM node:12.10.0-alpine AS build
## 指定一个源代码存放工作空间
WORKDIR /src
COPY . .
RUN npm install  \
    && npm run build

FROM base AS final
COPY --from=build /src/dist /usr/share/nginx/html/




