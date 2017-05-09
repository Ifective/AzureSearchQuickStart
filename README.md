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
8.  Geo Search

## Opzet quickstart
In deze quickstart gaan we een voorbeelddataset met vastgoeditems doorzoeken. Per vastgoeditem zijn er eigenschappen vastgelegd zoals onder andere de omschrijving in verschillende talen, de locatie, de prijs en de oppervlakte. Microsoft heeft zelf een voorbeeldwebsite waarin deze dataset doorzoekbaar is gemaakt: [Voorbeeld webapplicatie](https://searchsamples.azurewebsites.net/#/homes). 
![alt text](/Content/searchsample.png "Search Demo")

In deze quickstart wordt een console applicatie in C# gemaakt waarin deze dataset aangeproken wordt. Om Azure Search aan te spreken wordt gebruik gemaakt van de .NET SDK. Op andere platformen kan er gebruik gemaakt worden van de REST API.

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
Bij deze quickstart is voorbeeldcode gemaakt waarmee een eigen search service aangesproken kan worden. Deze code kan gebruikt worden door onze github repository te clonen en en het project te openen met Visual Studio. In de projecteigenschappen (Debug > Application Arguments) kunnen de gegevens van search service opgegeven worden in het volgende formaat: 
`-s <searchservicenaam> -k <adminkey> -i realestate-us-sample`

De searchservicenaam is de ingevoerde naam bij het maken van de search service. De adminkey kan gevonden worden in het menu van het dasboard van de searchservice onder de sectie settings > keys.

De voorbeeldapplicatie is opgedeeld in verschillende scenarios die hieronder beschreven worden. Na het starten van de applicatie kan een scenario gekozen worden. In de console wordt dan meteen het resultaat getoond.

## Aanspreken van de Search service
Om in .NET gebruik te kunnen maken van de search service moet de SDK middels de Nuget-package "Microsoft.Azure.Search" geinstalleerd worden. Deze SDK is een wrapper om de REST API van Azure Search, zodat alle functionaliteiten ook op andere platformen uitgevoerd kunnen worden. Na het installatie van de Nuget-package moet eerst een seach service client gemaakt worden. Aan deze client worden de servicenaam en de key meegegeven. De search service client geeft toegang tot alle functionaliteiten van Azure Search, waaronder beheer van de service, het aanmaken van indexes en het uitvoeren zoekacties. Om het mogelijk te maken om te kunnen zoeken, halen we de indexclient voor de opgegeven indexnaam (realestate-us-sample) op.

```C#
var serviceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));
var indexClient = serviceClient.Indexes.GetClient(_indexName);
```

## Zoekacties
Met behulp van de indexClient kunen we zoekacties uitvoeren. Dit doen we op de volgende manier:

```C#
var result = indexClient.Documents.Search<RealEstate>("Bellevue");
```

Hiermee wordt er binnen alle kolommen die als doorzoekbaar zijn gemarkeerd gezocht op het het woord "Bellevue". Deze code levert onder water de volgende aanroep naar de API van de search service:
`https://<searchservicenaam>.search.windows.net/indexes/realestate-us-sample/docs?api-version=2016-09-01&search=Bellevue`.

Uiteraard kan de zoekactie veel uitgebreider gemaakt worden. Dit kan gedaan worden door de zoektekst uit te breiden of door extra parameters mee te geven.

Bij de zoekactie wordt er gebruik gemaakt van een model waarin de resultaten worden gezet. Dit model is een POCO waarvan de properties overeenkomen met de veldnamen in de searchindex. De SDK zorgt ervoor dat de zoekresultaten automatisch gemapped worden naar een lijst van dit model. Voor dit voorbeeld is de onderstaande klasse gemaakt.

```C#
public class RealEstate
{
    public string ListingId { get; set; }
    public int Beds { get; set; }
    public int Bads { get; set; }
    public string Description { get; set; }
    public string Description_nl { get; set; }
    public string City { get; set; }
    public string Sqft { get; set; }
}
```

### Wildcards
De zoektekst kan wildcards bevatten waarmee meer controle op de zoekresultaten uitgeoefend kan worden. Als er bijvoorbeeld specifiek gezocht moet worden op "great views" of "beautiful home" kan dit uitgevoerd worden door als zoektest het volgende mee te geven: `("great views") | ("beautiful home")`. Er wordt dan specifiek gezocht op de exacte fragmenten die tussen de haakjes staan. Verder is er een OF-operator toegevoegd, zodat er op meerdere fragmenten gezocht kan worden. Er zijn daarnaast nog veel meer operators zoals EN (+), NOT (-) en Suffix (*). De laatste operator maakt het mogelijk om te zoeken op tekst die begint met een bepaalde tekst, bijvoorbeeld: `belle*` (wat onder andere bellevue zal vinden).
 
### Parameters
De werking van de zoekquery kan aangepast worden door zoekparameters mee te geven. In deze parameters kan onder andere het volgende gespecificeerd worden:
- Sortering
- Filtering
- Welke velden teruggegeven worden
- Filtering
- Highlighting
- Limitatie van het aantal resultaten

De parameters kunnen op de onderstaande manier mee gegeven worden aan de zoekquery. In het onderstaande voorbeeld wordt de property IncludeTotalResultCount ingesteld, zodat bij de zoekactie teruggegeven wordt hoeveel documenten in de index aan de zoekquery voldoen. Er worden standaard 50 items teruggegeven. Door middel van deze optie wordt het mogelijk om het totaal aantal resultaten uit te lezen zonder deze op te halen. Verder wordt in het onderstaande voorbeeld bepaald welke velden uit de index terug worden gegeven in het resultaat. Zo kan ervoor gezorgd worden dat alleen de data die nodig is verstuurd wordt. De velnamen die hier meegegeven worden, komen overeen met de veldnamen in de index.
```C#
var parameters = new SearchParameters()
{
    IncludeTotalResultCount = true,
    Select = new[] { "listingId", "description", "description_nl", "beds", "city", "sqft" }
};
var result = client.Documents.Search<RealEstate>("bellevue" , parameters);
```

De bovenstaande parameters kunnen uiteraard ook aan de REST API worden meegegeven. Voor elke zoekparameter wordt er dan een querystringparameter meegegeven. De volgende querystring kan bijvoorbeeld in search explorer gebruikt worden om hetzelfde resultaat te krijgen als de bovenstaande .NET code:
`&search=bellevue&$count=true&$select=listingId,description,description_nl,beds,city,sqft`.

Door middel van de parameters kan verder worden bepaald in welke velden moet worden gezocht. Om er bijvoorbeeld voor te zorgen dat er alleen gezocht wordt in de Nederlandse beschrijving, wordt de volgende property ingesteld: `parameters.SearchFields = new List<string>() {"description_nl"};`. Vanaf dan worden de overige velden in de index buiten beschouwing gelaten bij de zoekactie.

### Filtering
De zoekresultaten kunnen gefilterd worden door de filter property in te stellen: `parameters.Filter = "beds ge 2 and sqft gt 16000";`. In dit geval worden de zoekresultaten gefilterd, zodat er alleen vastgoeditems getoond worden die twee of meer bedden hebben en waarvan de oppervlakte meer dan 16000 sq ft is. Voor filtering wordt de [OData Expression syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/odata-expression-syntax-for-azure-search) gebruikt, zodat operators zoals (eq, ne, gt, lt, ge, le, and, or en any) kunnen worden toegepast.

Ditzelfde kan in de search explorer bereikt worden door: `$filter=beds ge 2 and sqft gt 16000`

### Highlighting
Het is gebruikelijk dat in zoekresultaten gemarkeerd wordt waar de teksten staan die overeenkomen met de gezochte tekst. Hier kan de feature highlighting voor gebruikt worden. Higlighting zorgt ervoor dat de overeenkomstige tekst binnen een HTML-tag wordt geplaatst. In een webpagina kan er dan voor gezorgd worden dat deze tag anders wordt opgemaakt (bijvoorbeeld vet). 

In het onderstaande voorbeeld wordt highlighting ingesteld op de zoekparameters. Als eerste moet er aangegeven worden op welke indexvelden er highlighting wordt uitgevoerd. Verder kunnen de begin- en eindtag ingesteld worden. Hier wordt er gebruik gemaakt van een customtag, maar uiteraard kan hier elke HTML-tag neergezet worden.
```C#
parameters.HighlightFields = new List<string>() { "description", "description_nl" };
parameters.HighlightPreTag = "<HIGHLIGHT>";
parameters.HighlightPostTag = "</HIGHLIGHT>";
```
De tekst met highlights worden niet in de velden van het model aangepast, maar moet worden uitgelezen uit de Higlights property van de zoekresultaten. `result.Highlights.ForEach(h => Console.WriteLine($"-{string.Join("\r\n-", h.Value)}"));`


### Faceted navigation
Bij veel zoekmachines wordt er gebruik gemaakt facet functionaliteit. Deze functionaliteit maakt het mogelijk om de zoekresultaten te categoriseren. De categorisering vindt dan plaats op verschillende waardes in een indexveld of op het bereik van waardes. In het volgende voorbeeld staan de facetten van het vastgoedvoorbeeld.
![alt text](/Content/searchsample_facet.png "Facetten")


