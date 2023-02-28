# Installer dotnet ef

```dotnet tool install --global dotnet-ef --version 6.0.11```

# Changer le fichier 

local.settings.json
```json 
{
  "IsEncrypted": false,
  "Values": {
    "SqlConnectionString": "Server=localhost,1433;Initial Catalog=IdentityServer;Persist Security Info=False;User ID=sa;Password=Tkm@akpRYh4m?qo4;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;"
  },
  "Host": {
    "CORS": "*"
  }
}
```
Changer la chaine de connexion

# Creer une migration
```shell
dotnet ef migrations add Role
```

# Lancer les migrations
```shell
dotnet ef database update
```


Run le projet en mode debug pour utiliser le seeder