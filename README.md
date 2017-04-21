# Quickstart zoeken met Azure Search 
Microsoft Azure Search is een cloud oplossing die het mogelijk maakt om een geavanceerde zoekervaring te bieden binnen applicaties. In deze blog laten we zien hoe er snel gestart kan worden met Azure Search.

## Kenmerken Azure Search
Azure Search biedt verschillende voordelen:
1.	Schaalbare oplossing
2.	Indexing
3.	Search
4.	Facetting
5.  Etc.....

## Opzet quickstart
In deze quickstart gaan we een voorbeelddataset met vastgoeditems doorzoeken. Per vastgoeditem zijn er eigenschappen vastgelegd zoals onder andere de omschrijving in verschillende talen, de locatie, de prijs en de oppervlakte. Microsoft heeft zelf een voorbeeldwebsite waarin deze dataset doorzoekbaar is gemaakt: [Voorbeeld webapplicatie](https://searchsamples.azurewebsites.net/#/homes). Hier wordt een console applicatie in C# gemaakt waarin deze dataset aangeproken wordt.

## Opzetten van de index  
Om gebruik te maken van Azure Search moet er een search service in Azure aangemaakt worden. Hier wordt getoond hoe dit uitgevoerd wordt vanuit het het Azureportaal. Uiteraard kan dit ook uitgevoerd worden met Powershell, CLI of via de API's.  

Hieronder wordt getoond hoe in het Azureportaal een nieuwe searchservice gemaakt kan worden. Voor dit voorbeeld volstaat de pricing tier Free. Voor productiedoeleinden moet uiteraard een betaalde pricing tier gekozen worden.
![alt text](/content/create_searchservice.png "Aanmaken Search Service")

Er kan uiteraard alleen gezocht worden als er data in de search service aanwezig is. Hiervoor moet er een index gemaakt worden. Er kan een index gemaakt worden volgens een eigen definitie die door middel van code wordt gevuld. Een andere manier om een index te maken is door direct te koppelen aan een bestaande databron zoals een database. De index wordt dan zonder tussenkomst van custom code  gevuld. Op deze manier kan een bestaande database eenvoudig doorzoekbaar gemaakt worden. 

Zodra de searchservice aangemaakt is, kan er op het dashboard van de searchservice op "Import data" geklikt worden. In dit scherm kiezen we ervoor om gebruik te maken van voorbeelddata (Samples > realestate-us-sample).
![alt text](/content/create_sample_index.png "Aanmaken index op basis van voorbeelddata")




- a
- b