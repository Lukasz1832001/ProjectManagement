# ProjectManagement

Wymagania wstępne:
 - Visual Studio 2022 lub nowszy z ASP.NET i programowaniem w sieci Web
 - .NET 7
 - MS SQL Server Express

Aplikacja wymaga zainstalowanych pakietów: 
  - "Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="7.0.5" 
  - "Microsoft.AspNetCore.Identity.UI" Version="7.0.4" 
  - "Microsoft.EntityFrameworkCore" Version="7.0.5" 
  - "Microsoft.EntityFrameworkCore.SqlServer" Version="7.0.5" 
  - "Microsoft.EntityFrameworkCore.Tools" Version="7.0.5"

W celu prawidłowej konfiguracji aplikacji z bazą danych MS SQL Server Express przy wdrożeniu należy poprawnie zastąpić connectionString znajdujący się w pliku appsettings.json.
Następnie otworzyć konsolę menadżera pakietów w Visual Studio i wpisać komendę do tworzenia kodu migracji: „add-migration MigrationName”.
Stworzony skrypt migracji odpowiada za utworzenie bazy danych i jej tabeli. Następnie należy wykonać polecenie „update-database” w konsoli menadżera pakietów w celu stworzenia rzeczywistej bazy danych i tabel.
