# Comments micro service

## Generate migration

- Execute `cd data`
- Execute `dotnet ef --startup-project ../app migrations add <MigrationName> -v`

## Docker

- Cleanup `docker rm -f $(docker ps -a -q) || true && docker rmi -f $(docker images -q) || true`

## Build docker image and run

- `docker build -t comments .`
- `docker run -d -p 3030:3030 -p 3031:3031 -e ASPNETCORE_URLS=http://+:3030 -e ASPNETCORE_ENVIRONMENT="Development" --name comments_micro_service comments`

## Build and run for test local test

- `dotnet publish -c Test -o dist`
- `cd dist`
- `ASPNETCORE_ENVIRONMENT="Development" dotnet comments.app.dll`
