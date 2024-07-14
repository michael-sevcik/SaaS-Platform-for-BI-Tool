# Microsoft sql server dockerize

# Create, run and connect
```
podman-compose -f docker-compose.yml up -d 

podman exec -it db_mssql_server_1 bash

/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P "password123!" -Q 'SELECT 1'
```

## Autostart with systemd service
```
podman generate systemd --new --name db_mssql_server_1 > mssql.service
mkdir -p ~/.config/systemd/user/
cp mssql.service ~/.config/systemd/user/
systemctl --user daemon-reload
systemctl --user enable mssql.service 
systemctl --user start mssql.service
systemctl --user status mssql.service 
```


