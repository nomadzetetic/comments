### Generate migration

- [x] Execute `cd data`
- [x] Execute `dotnet ef --startup-project ../app migrations add <MigrationName> -v`

### Docker

- [x] Cleanup `docker rm -f $(docker ps -a -q) || true && docker rmi -f $(docker images -q) || true`
