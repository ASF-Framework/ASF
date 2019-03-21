FROM nginx AS base
WORKDIR /app
EXPOSE 80

# 开始构建nodejs环境进行发布asf
FROM node:11.11.0 AS build
## 指定一个源代码存放工作空间
WORKDIR /src
COPY . .
RUN npm install yarn -g && yarn install && yarn run build
RUN ls

FROM build AS publish
WORKDIR /app
COPY --from=build /src/dist .
RUN cd .. && ls







