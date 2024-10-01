## Express voitures

### Présentation

**Express Voitures** est une application web qui permet de gérer un parc automobile . 
##Index :

-[Fonctionnalités]
-[Technologies utilisées]
-[Mode d'emploi]

### Fonctionnalités

**Gérer les vehicules **
Il est possible de :
  -ajouter des vehicules
  -modifier les données d'un vehicule
  -marquer les vehicules comme vendus ou indisponibles
  -supprimer un vehicule
  -ajouter des réparations
  -modifier les réparations
  -supprimer les réparations

  Bien sûr toutes ces fonctionnalités ne sont disponibles qu'à un profil administrateur
  Seule la lecture est disponibles pour les autres utilisateurs.

  
### Technologies utilsées :

- **ASP.NET Core mvc** :pour créer la base de l'application 
- **Entity Framework Core** : pour permetre les interactions avec la base de données.
- **SQL Server** : base de donnée utilisée.
- **Microssoft sql server management** : pour consulter BDD et gerer les rôles
- **Bootstrap** : Pour la partie css
- **cshtml** : pour l'affichage des vues (Razor pages)
- **JavaScript** : pour un event listener

### Mode d'emploi :

[1] ##IMPORTANT##
Pour lancer ce projet, vous devez avoir installé :

-Visual Studio 2022 : https://visualstudio.microsoft.com/fr/downloads/

-sql server : https://www.microsoft.com/fr-fr/sql-server/sql-server-downloads?msockid=2b33a7ef67dc66b33723b35d66a5670e

-mssql : https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

[2] ##Utilisation##

-[Cloner le dépot] :
 Depuis votre terminal, placez vous à l'emplacement désiré , puis entrez la commande: 
 https://github.com/Xcaliburns/ExpressVoitures.git  ou clonez le repository directement depuis l'interface de visual studio
 
 -[Mettre à jour les packages]
 
 Executez la commande : dotnet restore
 
 -[Executer le projet] :
 
 Depuis visual studio f5 (mode debug) ou bouton démarrer,
 
 Depuis un terminal : dotnet build  puis dotnet run

 [Administration]

 La base de données Server=localhost;Database=express_voitures est créée et peuplée depuis un fichier d'exemple, vous aurez quelques vehicules fictifs que vous pourrer gérer à titre d'exemple. Pour toute mise en production , la création par défaut doit etre supprimée.
 Un administrateur par défaut sera également créé avec les logs suivants :
 
 adresse mail ##admin@example.com##
 
 Mot de passe : ##Admin@123456##

 Vous pourrez créer d'autres utilisateurs, mais ils auront le role de USER et ne pourrons que consulter les annonces.

 La modification des rôles se fera depuis ms sql server management par soucis de sécurité.

 





