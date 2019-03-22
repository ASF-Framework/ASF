FROM nginx AS base
WORKDIR /app
EXPOSE 80

# 开始构建nodejs环境进行发布asf
FROM aqa510415008/yarn:1.0.0 AS build
## 指定一个源代码存放工作空间
WORKDIR /src
COPY . .
RUN yarn install  \
    && yarn run build

FROM base AS final
COPY --from=build /src/dist /usr/share/nginx/html/




