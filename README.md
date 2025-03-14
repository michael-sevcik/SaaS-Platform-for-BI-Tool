# 222-Sevcik-Michael

Repozitář obsahuje soubory související s ročníkovým projektem a bakalářskou prací na téma tvorba podnikového online reporting nástroje s důrazem na jednoduchost nasazení u středních až větších firem zabývajících se výrobou.

## Přiblížení projektu

Výše zmíněné firmy již většinou používají nějaký ERP systém, ale často nevyužívají plného potenciálu dat v něm se skrývajících. Zde vyvíjený BI nástroj si klade za cíl poskytnout generický pohled (datový model) na tyto data a pomocí něj umožnit velmi rychlou vizualizaci různých výrobních a ekonomických metrik, mimo to nástroj provede základní kontrolu importovaných dat a upozorní na výskyt některých častých druhů chyb.

## Tématické okruhy související s vývojem tohoto projektu

Níže je základní přehled témat, s kterými jsem se setkal při práci na tomto projektu, či se jim v brzké budoucnosti budu věnovat:

- Srovnání nástrojů pro vizualizaci
    - Vybrán metabase
- Srovnání možností importu dat s důrazem na jednoduchost
    - ETL nástroje
    - Vlastní mapper a ETL infrastruktura
    - Pravděpodobně vlastní řešení s ohledeme na jednoduchost použití. 
- Generický datový model výrobce
    - Datové modely zákazníků
    - Generalizace
- Vlastní mapper
    - Knihovny pro mapování
    - Implementace
- Kontrola dat - spíše implementace
- Generické nástěnky
- Deploy BI nástroje v Kubernets

## Running the application
If you want to run this application on windows, you need to set git before cloning the repository to not convert LF to CRLF (`git config --global core.autocrlf false`), otherwise it will discrupt shell scripts. 

Run in the repository root directory:

```sh
docker compose up --build
```

This could take sevarl minutes, but not much more than 20, depending on your computer power and internet connection speed.

Wait until container `server-1` is running, this is the server and it waits for all other containers to become ready. Due to the big size of example customer data (containers `customer-db-1-1` and `customer-db-2-1`), it takes quete some time to initialize the example databases.

### Creating first customer and deploying Metabase


Admin:
	email: admin@admin.cz
	password: Admin123*
	
customer1:
	email: customer@example.cz
	password: Customer123*

Open http://localhost:8080/
1. Log in as Admin, go to Users -> Customers -> Add Customer and enter customer details (e.g. customer@example.cz and customer1 as name)
2. Log out and go to http://localhost:5080/ to see sent emails.
3. Open the invitation email and click on the invitation link.
4. Enter a password, e.g. Customer123*, and click `Set password`. Now you should be logged in as a customer.
5. Go to Data Integation -> Database connection configuration and fill in following details:
```
	Database connection configuration:
		Select your database provider: SqlServer
		Data Source: 10.5.0.3,1433
		Initial Catalog: CostumerExampleData
		User name: sa     # must be entered
		Password: password123!
		Connect Timeout: 30
		Encrypt: false
		Trust Server Certificate: true
		Multi Subnet Failover: false
```

6. Click Save, you should see a message: Connection established successfully
7. Click `Create model`, you should see a message: Model created successfully.
8. Navigate to Data Integration -> Mapper, You should see a green message:
	Please map the target entities one by one, don't forget to save your progress after each entity mapped. Fully mapped entities are marked with ✅, othrerwise ❌. After mapping all entities and saving each mapping, Continu button will appear
9.  Close the message if you want to.
10. Move the Workplaces entity to the right side by dragging its header.
11. Click `Add source table` and pick `TabCisZam`, click `Continue`.
12. Map columns of `TabCisZam` to columns of  target table`Employees` by clicking on columns of the source table and dragging them on the target columns:
	mapping:
		ID -> Id
		Cislo -> PersonalID
		ID -> ExternalId
		Jmeno -> FirstName
		Prijmeni -> Lastname
	
13. Double click `TabCisZam` to deselect unused columns (optional).
14. Click `Save`, you should see: Mapping was saved.
15. Click on selection input `dbo.Employees`, select `dbo.Workplaces`. You should see `Workplaces` target table entity.
16. Click `Vybrat soubor`.
17. pick {RepositoryRoot}/ExampleMappings/WorkPlacesMapping.json
18. You should see mapping of Workplaces, click save.
19. Continue to `dbo.WorkReports`
20. Add `TabPrikazMzdyAZmetky` source table.
21. Map the columns:
	mapping:
		ID -> ID
		IDPrikaz -> OrderId
		DokladPrPostup -> ProductionOperationId
		IDPracoviste -> WorkPlaceId
		kusy_odv -> Quantity
		Zamestnanec -> WorkerId
		Nor_cas -> ExpectedTime
		datum -> DateTime
	
22. Click `Add custom query`
23. Fill in custom query details:
```
	Unique name: WorkReportTypeCustomQuery
	Query:
```

```sql
SELECT [TabPrikazMzdyAZmetky].ID as TabPrikazMzdyAZmetkyID,
	CAST(
		CASE
		-- Forbidden prefixes for [TabKmenZbozi].RegCis "17", "18", "37", "38" -- and [TabKmenZbozi_EXT]._pracoviste_filtr equals one of: "R", "R+cell", "R+podia", "T+R", "R+Přípraváři"
		WHEN ([TabKmenZbozi].RegCis LIKE '17%' OR [TabKmenZbozi].RegCis LIKE '18%' OR [TabKmenZbozi].RegCis LIKE '37%' OR [TabKmenZbozi].RegCis LIKE '38%') AND [TabKmenZbozi_EXT]._pracoviste_filtr IN ('R', 'R+cell', 'R+podia', 'T+R', 'R+Přípraváři')
		THEN 'Type1'
		-- Forbidden prefixes for [TabKmenZbozi].RegCis "17", "18", "37", "38" -- and [TabKmenZbozi_EXT]._pracoviste_filtr equals one of: "TR", "TR+podia", "T+R", "TR+Přípraváři"
		WHEN ([TabKmenZbozi].RegCis LIKE '17%' OR [TabKmenZbozi].RegCis LIKE '18%' OR [TabKmenZbozi].RegCis LIKE '37%' OR [TabKmenZbozi].RegCis LIKE '38%') AND [TabKmenZbozi_EXT]._pracoviste_filtr IN ('TR', 'TR+podia', 'T+R', 'TR+Přípraváři')
		THEN 'Type2' ELSE 'Other' END AS nvarchar(100) 
	) AS ProductType
	FROM [CostumerExampleData].[dbo].[TabPrikazMzdyAZmetky]
	LEFT JOIN [TabKmenZbozi]
	ON TabPrikazMzdyAZmetky.IDTabKmen = [TabKmenZbozi].ID LEFT JOIN [TabKmenZbozi_EXT] ON TabPrikazMzdyAZmetky.IDTabKmen = [TabKmenZbozi_EXT].ID
```
```
		Selected columns:
			name: TabPrikazMzdyAZmetkyID
			type: Integer
			Is nullable: false
			
			name: ProductType
			type: nvchar(lenght)
			Is nullable: false
			lenght: 100
```
24.  Click `Continue`, `Join definition` modal should pop up
25.  Enter join details:
	Left column: TabPrikazMzdyAZmetky_ID
	Operator: Equals
	Right column: WorkReportTypeCustomQuery_TabPrikazMzdyAZmetkyID
26.  Click `Continue`, `WorkReportTypeCustomQuery` source entity should appear.
27.  Drag the entity to a free space
28.  Map `ProductType` source column to `ProductType` target column
29.  Click `Save`, now you mapping should be complete and a `Continue to deployment` button should appear in a upper part of the page.
30.  Click `Continue to deployment`. You should be redirected to page http://localhost:8080/Deployment/DeployMetabase
31.  (optional) Click `Generate Views` to see what SQL commands will be executed to deploy database views.
32.  Click `Automatically deploy views`, you should see a message: `Views deployed successfully.`
33. Enter Metabase Admin credentials, random email and strong password, e.g.:
	Email used to log in to metabase: customer@example.cz
	Password: Customer123*
34. Click `Deploy Metabase`, you should see spinner to pop up and a message:
		This could take several minutes. You can close this page and you will be notified when the metabase is successfully deployed.
	This operation should not take more than 10 minutes, around 1 GB needs to be downloaded for the first deployment.
35. Either wait for the spinner to finish or you can keep checking http://localhost:5080/ for an email with link
36. Follow the link to Metabase and use the entered Metabase Admin credentials to log in. After logging in you should see dashboard with graphs. If nothing is displayed, try refreshing the page
37. From there you can start using your own preconfigured instance of Metabase
	

### Deploying second metabase for second customer
- The process is the same with the different data source in `Database connection configuration`:

Database connection configuration 2:
```
		Select your database provider: SqlServer
		Data Source: 10.5.0.7,1433
		Initial Catalog: CostumerExampleData
		User name: sa     # must be entered
		Password: password123!
		Connect Timeout: 30
		Encrypt: false
		Trust Server Certificate: true
		Multi Subnet Failover: false
```
- The second deployment of Metabase should not take more than 2 minutes. On average one minute.

	
### Deleting customer
1. Log in as a Admin
2. Navigate to Users -> Customers and click on ID of a customer you want to delete.
3. Click `Delete` and confirm you intention by clicking `Confirm`
4. You should see that the customer is no longer in the list and whithin few seconds their deployment of Metabase, if they had any, become inaccessible.
