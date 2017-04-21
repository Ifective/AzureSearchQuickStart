# Quickstart zoeken met Azure Search 
Microsoft Azure Search is een cloud oplossing die het mogelijk maakt om een geavanceerde zoekervaring te bieden binnen applicaties. In deze quickstart laten we zien hoe er snel gestart kan worden met Azure Search.

## Kenmerken Azure Search
Azure Search biedt verschillende voordelen:
1.	Schaalbare oplossing
2.	Indexing
3.	Search wildcards
4.	Facetting
5.  Fuzzy Search
6.  Suggestions
7.  Taal analyse
5.  Etc.....

## Opzet quickstart
In deze quickstart gaan we een voorbeelddataset met vastgoeditems doorzoeken. Per vastgoeditem zijn er eigenschappen vastgelegd zoals onder andere de omschrijving in verschillende talen, de locatie, de prijs en de oppervlakte. Microsoft heeft zelf een voorbeeldwebsite waarin deze dataset doorzoekbaar is gemaakt: [Voorbeeld webapplicatie](https://searchsamples.azurewebsites.net/#/homes). Hier wordt een console applicatie in C# gemaakt waarin deze dataset aangeproken wordt.

## Opzetten van de index  
Om gebruik te maken van Azure Search moet er een search service in Azure aangemaakt worden. Hier wordt getoond hoe dit uitgevoerd wordt vanuit het het Azureportaal. Uiteraard kan dit ook uitgevoerd worden met Powershell, CLI of via de API's.  

Hieronder wordt getoond hoe in het Azureportaal een nieuwe searchservice gemaakt kan worden. Voor dit voorbeeld volstaat de pricing tier Free. Voor productiedoeleinden moet uiteraard een betaalde pricing tier gekozen worden.
![alt text](/Content/create_searchservice.png "Aanmaken Search Service")

Er kan uiteraard alleen gezocht worden als er data in de search service aanwezig is. Hiervoor moet er een index gemaakt worden. Er kan een index gemaakt worden volgens een eigen definitie die door middel van code wordt gevuld. Een andere manier om een index te maken is door direct te koppelen aan een bestaande databron zoals een database. De index wordt dan zonder tussenkomst van custom code  gevuld. Op deze manier kan een bestaande database eenvoudig doorzoekbaar gemaakt worden. 

Zodra de searchservice aangemaakt is, kan er op het dashboard van de searchservice op "Import data" geklikt worden. In dit scherm kiezen we er nu voor om gebruik te maken van voorbeelddata (Samples > realestate-us-sample) in plaats van een eigen database.

![alt text](/Content/create_sample_index.png "Aanmaken index op basis van voorbeelddata")


De index met voorbeelddata is nu aangemaakt. Het dashboard van de search service toont nu welke indexes aanwezig zijn en de hoeveelheid data in deze indexes.

![alt text](/Content/search_dashboard.png "Search service dasboard")


Vanuit dit dashboard kunnen de eigenschappen van de voorbeeldindex bekeken worden. In dit eigenschappenscherm zijn de velden van de index met het bijbehorende datatype in te zien. Verder wordt er per veld aangegeven of het veld doorzoekbaar, sorteerbaar of filterbaar is, of het veld uitgelezen kan worden door een client en of het veld als facet gebruikt gebruikt kan worden. Vanuit dit scherm kunnen velden bewerkt of toegevoegd worden. Verder kunnen hier scoring profiles worden toegevoegd. Deze principes worden verder op uitgelegd.

![alt text](/Content/index_overview.png "Index eigenschappen")


Vanuit het eigenschappenscherm van de index kan de search explorer gestart worden. Met deze search explorer kunnen zoekqueries eenvoudig getest worden. De resultaten zijn dan direct zichtbaar. De syntax die gebruikt kan worden staat op de volgende pagina: [Search Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/search-documents). Queryvoorbeelden...

![alt text](/Content/search_explorer.png "Search explorer")

## Gebruik van de voorbeeldcode
Bij deze quickstart is voorbeeldcode te vinden waarmee een eigen search service aangesproken kan worden. Deze code kan gebruikt worden door onze github repository te clonen en en het project te openen met Visual Studio. In de projecteigenschappen (Debug>Application Arguments) kunnen de gegevens van search service opgegeven worden in het volgende formaat: 
`-s <searchservicenaam> -k <admin key> -i realestate-us-sample`

De voorbeeldapplicatie is opgedeeld in verschillende scenarios die hier uitgewerkt worden. Na het starten van de applicatie kan een scenario gekozen worden. In de console wordt dan meteen de uitvoer getoond.

## Aanspreken Search service
Na het installeren van de nuget package.
```C#
var serviceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));
var indexClient = serviceClient.Indexes.GetClient(_indexName);
var result = client.Documents.Search<RealEstate>("Bellevue");
```
## Basis queries

## Filtering
- a
- b