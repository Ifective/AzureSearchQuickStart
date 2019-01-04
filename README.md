# Quickstart zoeken met Microsoft Azure Search 
**Microsoft Azure Search** is een cloud oplossing die het mogelijk maakt om een geavanceerde zoekervaring te bieden binnen applicaties. Hierbij worden out-of-the-box geavanceerde mogelijkheden geboden, zoals onder andere wildcard search, filtering, facetted navigation, highlighting, custom scoring en geografische sortering. In deze quickstart laten we zien hoe er gestart kan worden met het gebruik van Azure Search.

## Opzet quickstart
Voor deze quickstart maken we gebruik van een voorbeelddataset met vastgoeditems. Per vastgoeditem zijn er eigenschappen vastgelegd zoals onder andere de omschrijving in verschillende talen, de locatie, de prijs en de oppervlakte. Microsoft heeft zelf een voorbeeldwebsite waarin deze dataset doorzoekbaar is gemaakt: [Voorbeeld webapplicatie](https://searchsamples.azurewebsites.net/#/homes). 
![Search Demo](/Content/search_sample.png "Search Demo")

Hier wordt er door middel van een console applicatie in *C#* duidelijk gemaakt hoe deze dataset vanuit code kan worden aangesproken. Om Azure Search te gebruiken binnen een C# applicatie wordt er gebruik gemaakt van de *.NET SDK*. Op andere platformen kan er gebruik gemaakt worden van de *REST API*.

## Opzetten van de index  
Om gebruik te maken van *Azure Search* moet er een search service in Azure aangemaakt worden. Hier wordt getoond hoe dit uitgevoerd wordt vanuit het het Azureportaal. Uiteraard kan dit ook uitgevoerd worden met *Powershell*, *CLI* of via de *Azure API*.  

Hieronder wordt getoond hoe in het Azure portaal een nieuwe searchservice gemaakt kan worden. Voor dit voorbeeld volstaat de pricing tier *Free*. Voor productiedoeleinden moet uiteraard een betaalde pricing tier gekozen worden.
![Aanmaken Search Service](/Content/create_searchservice.png "Aanmaken Search Service")

Er kan alleen gezocht worden als er data in de search service aanwezig is. Hiervoor moet er een index gemaakt en gevuld worden. Er kan een index gemaakt worden volgens een eigen definitie die door middel van code wordt gevuld. Een andere manier om een index te maken is door direct te koppelen aan een bestaande databron zoals een database. De index wordt dan zonder tussenkomst van custom code  gevuld. Op deze manier kan een bestaande database eenvoudig doorzoekbaar gemaakt worden. 

Zodra de search service aangemaakt is, kan er op het dashboard van de searchservice op *Import data* geklikt worden. In dit scherm kiezen we er nu voor om gebruik te maken van voorbeelddata *(Samples > realestate-us-sample)* in plaats van een eigen database.

![Aanmaken index op basis van voorbeelddata](/Content/create_sample_index.png "Aanmaken index op basis van voorbeelddata")


De index met voorbeelddata is nu aangemaakt. Het dashboard van de search service toont nu welke indexes aanwezig zijn en de hoeveelheid data in deze indexes.

![Search service dasboard](/Content/search_dashboard.png "Search service dasboard")


Vanuit dit dashboard kunnen de eigenschappen van de voorbeeldindex bekeken worden. In dit eigenschappenscherm zijn de velden van de index met het bijbehorende datatype in te zien. Verder wordt er per veld aangegeven of het veld doorzoekbaar, sorteerbaar of filterbaar is, of het veld uitgelezen kan worden door een client en of het veld als facet gebruikt gebruikt kan worden. Vanuit dit scherm kunnen velden bewerkt of toegevoegd worden. Verder kunnen hier scoring profiles worden toegevoegd. Deze principes worden verderop nog uitgelegd.

![Index eigenschappen](/Content/index_overview.png "Index eigenschappen")


Vanuit het eigenschappenscherm van de index kan de *search explorer* gestart worden. Met deze search explorer kunnen zoekqueries eenvoudig getest worden. Er kan gezocht worden door een zoekwoord in het veld *Query string* te typen. De resultaten zijn dan direct zichtbaar. De syntax die gebruikt kan worden staat op de volgende Microsoft Docs pagina: [Search Syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/search-documents). We laten hieronder ook een aantal geavanceerde voorbeelden van zoekqueries zien.

![Search explorer](/Content/search_explorer.png "Search explorer")

## Gebruik van de voorbeeldcode
Bij deze quickstart is voorbeeldcode gemaakt waarmee een eigen search service aangesproken kan worden. Deze code kan gebruikt worden door onze github repository te clonen en en het project te openen met Visual Studio. In de projecteigenschappen (Debug > Application Arguments) kunnen de gegevens van search service opgegeven worden in het volgende formaat: 
`-s <searchservicenaam> -k <adminkey> -i realestate-us-sample`

De searchservicenaam is de ingevoerde naam bij het maken van de search service. De adminkey kan gevonden worden in het menu van het dasboard van de searchservice onder de sectie *settings>keys*.

De voorbeeldapplicatie is opgedeeld in verschillende scenarios die hieronder beschreven worden. Na het starten van de applicatie kan een scenario gekozen worden. In de console wordt dan meteen het resultaat getoond. De voorbeelden die hieronder beschreven staan, zijn aanwezig in de voorbeeldapplicatie.

## Aanspreken van de Search service
Om in .NET gebruik te kunnen maken van de search service moet de SDK middels de Nuget-package *Microsoft.Azure.Search* geinstalleerd worden. Deze SDK is een wrapper om de REST API van Azure Search, zodat alle functionaliteiten ook op andere platformen uitgevoerd kunnen worden. Na de installatie van de Nuget-package moet eerst een search service client gemaakt worden. Aan deze client worden de servicenaam en de key meegegeven. De search service client geeft toegang tot alle functionaliteiten van Azure Search, waaronder beheer van de service, het aanmaken van indexes en het uitvoeren van zoekacties. Om het mogelijk te maken om te kunnen zoeken, instantiëren we de indexclient voor de opgegeven indexnaam (realestate-us-sample):

```cs
var serviceClient = new SearchServiceClient(_searchServiceName, new SearchCredentials(_adminApiKey));
var indexClient = serviceClient.Indexes.GetClient(_indexName);
```


## Zoekacties
Met behulp van de indexClient kunen we zoekacties uitvoeren. Dit doen we op de volgende manier:

```cs
var result = indexClient.Documents.Search<RealEstate>("Bellevue");
```

Hiermee wordt er binnen alle kolommen die als doorzoekbaar zijn gemarkeerd gezocht op het het woord (plaatsnaam) *Bellevue*. Deze code levert onder water de volgende aanroep naar de API van de search service:
`https://<searchservicenaam>.search.windows.net/indexes/realestate-us-sample/docs?api-version=2016-09-01&search=Bellevue`.

Uiteraard kan de zoekactie veel uitgebreider gemaakt worden. Dit kan gedaan worden door de zoektekst uit te breiden of door extra parameters mee te geven.

Bij de zoekactie wordt er gebruik gemaakt van een model waarin de resultaten worden opgeslagen. Dit model is een *POCO* waarvan de properties overeenkomen met de veldnamen in de searchindex. De SDK zorgt ervoor dat de zoekresultaten automatisch gemapped worden naar een lijst van dit model. Voor dit voorbeeld is de onderstaande klasse gemaakt.

```cs
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
De zoektekst kan wildcards bevatten waarmee meer controle op de zoekresultaten uitgeoefend kan worden. Als er bijvoorbeeld specifiek gezocht moet worden op *great views* of *beautiful home* kan dit uitgevoerd worden door als zoektest het volgende mee te geven: `("great views") | ("beautiful home")`. Er wordt dan specifiek gezocht op de exacte fragmenten die tussen de haakjes staan. Verder is er een OF-operator toegevoegd, zodat er op meerdere fragmenten gezocht kan worden. Er zijn daarnaast nog meer operators zoals EN (+), NOT (-) en Suffix (*). De laatste operator maakt het mogelijk om te zoeken op tekst die begint met een bepaalde tekst, bijvoorbeeld: `belle*` (wat onder andere bellevue zal vinden).
 
### Parameters
De werking van de zoekquery kan aangepast worden door zoekparameters mee te geven. In het parameter object kan onder andere het volgende gespecificeerd worden:
- Sortering
- Filtering
- Welke velden teruggegeven worden
- Filtering
- Highlighting
- Limitatie van het aantal resultaten

In het onderstaande codevoorbeeld wordt de property IncludeTotalResultCount ingesteld, zodat bij de zoekactie teruggegeven wordt hoeveel documenten in de index aan de zoekquery voldoen. Er worden standaard 50 items teruggegeven door de search service. Door middel van deze optie wordt het mogelijk om het totaal aantal resultaten uit te lezen zonder deze allemaal op te halen. 

Verder wordt in het onderstaande codevoorbeeld bepaald welke velden uit de index terug worden gegeven in het resultaat. Zo kan ervoor gezorgd worden dat alleen de data die nodig is verstuurd wordt. De namen die hier meegegeven worden, komen overeen met de veldnamen in de index.

```cs
var parameters = new SearchParameters()
{
    IncludeTotalResultCount = true,
    Select = new[] { "listingId", "description", "description_nl", "beds", "city", "sqft" }
};
var result = client.Documents.Search<RealEstate>("bellevue" , parameters);
```

De bovenstaande parameters kunnen uiteraard ook aan de REST API worden meegegeven. Voor elke zoekparameter wordt er dan een querystringparameter meegegeven. De volgende querystring kan bijvoorbeeld in *search explorer* gebruikt worden om hetzelfde resultaat te krijgen als de bovenstaande .NET code:
`&search=bellevue&$count=true&$select=listingId,description,description_nl,beds,city,sqft`.

Door middel van de parameters kan verder worden bepaald in welke velden moet worden gezocht. Om er bijvoorbeeld voor te zorgen dat er alleen gezocht wordt in de Nederlandse beschrijving, wordt de volgende property op het parameter object ingesteld: 

```cs
parameters.SearchFields = new List<string>() {"description_nl"};
``` 
Vanaf dan worden de overige velden in de index buiten beschouwing gelaten bij de zoekactie.

### Filtering
De zoekresultaten kunnen gefilterd worden door de filter property in te stellen: 
```cs
parameters.Filter = "beds ge 2 and sqft gt 16000";
```
In dit geval worden de zoekresultaten gefilterd, zodat er alleen vastgoeditems getoond worden die twee of meer bedden hebben en waarvan de oppervlakte meer dan 16000 sq ft is. Voor filtering wordt de [OData Expression syntax](https://docs.microsoft.com/en-us/rest/api/searchservice/odata-expression-syntax-for-azure-search) gebruikt, zodat operators zoals (eq, ne, gt, lt, ge, le, and, or en any) kunnen worden toegepast.

Ditzelfde kan in de *search explorer* bereikt worden door het volgende te typen: `$filter=beds ge 2 and sqft gt 16000` .

### Highlighting
Het is gebruikelijk dat in zoekresultaten gemarkeerd wordt waar de teksten staan die overeenkomen met de gezochte tekst. Hier kan de feature highlighting voor gebruikt worden. Higlighting zorgt ervoor dat de overeenkomstige tekst binnen een HTML-tag wordt geplaatst. In een webpagina kan er dan voor gezorgd worden dat deze tag anders wordt opgemaakt (bijvoorbeeld vet). 

In het onderstaande voorbeeld wordt highlighting ingesteld op de zoekparameters. Als eerste moet er aangegeven worden op welke indexvelden er highlighting wordt uitgevoerd. Verder kunnen de begin- en eindtag ingesteld worden. Hier wordt er gebruik gemaakt van een custom tag, maar uiteraard kan hier elke HTML-tag neergezet worden.
```cs
parameters.HighlightFields = new List<string>() { "description", "description_nl" };
parameters.HighlightPreTag = "<HIGHLIGHT>";
parameters.HighlightPostTag = "</HIGHLIGHT>";
```
De tekst met highlights worden niet in de velden van het model aangepast, maar moet worden uitgelezen uit de Higlights property van de zoekresultaten. 
```cs
result.Highlights.ForEach(h => Console.WriteLine($"-{string.Join("\r\n-", h.Value)}"));
```


### Faceted navigation
Bij veel zoekmachines wordt er gebruik gemaakt van de facet functionaliteit. Deze functionaliteit maakt het mogelijk om de zoekresultaten te categoriseren. De categorisering vindt dan plaats op verschillende waardes in een indexveld of op het bereik van waardes. In het onderstaande voorbeeld staan een aantal facetten van het vastgoedvoorbeeld. Hier wordt onder andere gegroepeerd op type huis en prijsbereik.

![Facetten](/Content/search_sample_facet.png "Facetten")

De facetten kunnen worden gebruikt om te filteren (met de reeds beschreven filterfunctionaliteit). Als er dan op een een facetwaarde geklikt wordt, wordt er een nieuwe zoekquery uitgevoerd waarbij gefilterd wordt op de gekozen facetwaarde.

Om ervoor te zorgen dat de search service facetten aanlevert, moet de property Facets op de zoekparameters worden ingesteld. Deze property bevat een lijst van namen van velden waarvan facetten gemaakt moeten worden. Bij een veldnaam kan eventueel aangegeven van welke type het facet is. In het onderstaande voorbeeld wordt er aangegeven dat het veld sqft gesegmenteerd moet worden in segmenten van 1000. Dit levert in de voorbeeldindex de segmenten 0, 1000, 2000, etc.
```cs
parameters.Facets = new List<string>() { "sqft,interval:1000"};
```
In de *search explorer* kan het volgende gebruikt worden: `facet=sqft,interval:1000` .

De facetten kunnen vervolgens uit de Facets property van de zoekresultaten gelezen worden:

```cs
searchResults.Facets.ForEach(f => Console.WriteLine($"{f.Key}:\r\n{string.Join("\r\n",f.Value.Select(v => $"Value: {v.Value} Count: {v.Count}"))}"));
```
Hierbij worden de facetnaam, de facetwaarde en het aantal zoekresultaten onder een facetwaarde teruggegeven.

### Scoring
Azure Search bepaalt zelf in welke volgorde zoekresultaten teruggegeven worden. Hierbij zullen resultaten waarbij woorden beter overeenkomen of vaker voorkomen hoger in de lijst staan. 

De standaardscoring kan aangepast worden door gebruik te maken van *scoring profiles*. Vanuit het dashboard van de search service in het *Azure portal* kan het scoringprofile gemaakt worden. In het *scoring profile* kunnen gewichten aan velden gekoppeld worden. Dit betekent als er overeenkomsten in het opgegeven veld zijn voor een zoekresultaat, dat het zoekresultaat hoger op de lijst komt te staan dan een zoekresultaat dat geen overeenkomst in dit veld heeft (maar in een ander veld). Het standaardgewicht voor de velden is 1. In het volgende voorbeeld zetten we het gewicht van city op 10. Dit betekent dat overeenkomsten op city dus 10 keer zwaarder wegen dan overeenkomsten op andere velden.  

![Scoring profile](/Content/scoring_profile_city.png "Scoring profile")

Bovenstaande betekent echter niet dat een overeenkomst in het veld city (altijd) tien keer hoger komt te staan in de lijst. De wegingsfactor voor velden is onderdeel van het totale scoringsalgoritme van Azure Search. 

Het bovenstaande scoringprofile kan eenvoudig toegepast worden door de searchparameters in te stellen op de naam van het aangemaakte *scoring profile*:
```cs
parameters.ScoringProfile = "BoostCity";
```
Op deze manier wordt er bij de zoekacties gebruik gemaakt van dit *scoring profile*.

### Geografische scoring
Hierboven hebben we een *scoring profile* gemaakt waarbij gewichten aan velden werden toegekend. Het is daarnaast ook mogelijk om een *scoring functions* te gebruiken. Een *scoring function* voert berekeningen uit op invoer. De invoer een *scoring function* is bijvoorbeeld de waarde van een (numeriek) veld, de recentheid van een item of een geografische afstand. In dit geval maken we een function die de zoekresultaten sorteert op de afstand tot de huidige locatie. 

Dit wordt als volgt ingesteld in het *Azure portal* (vanuit het dashboard van de search service):
![Geo scoring profile](/Content/scoring_profile_location.png "Geo scoring profile")

Er wordt hier een lineair algoritme gebruikt: elke stijging van de invoerwaarde resulteert in een evenredige stijging van het gewicht. Verder wordt hierbij aangegeven wat de maximale score is voor items die het meest in de buurt zijn (15) en wat de maximum afstand is waarop er nog extra gewicht wordt toegekend (50). Dit betekent dus dat een item dat op 0 km staat een gewicht van 15 krijgt, en een item dat op 51 km staat een gewicht van 1 krijgt. De locatie wordt vanuit de consumerende applicatie meegegeven door middel van een parameter met de naam currentLocation.

Het scoringprofile kan als volgt aangesproken worden:
```cs
parameters.ScoringProfile = "BoostNearbyLocation";
parameters.ScoringParameters = new List<ScoringParameter>()
{
    new ScoringParameter("currentLocation", GeographyPoint.Create(47.5679, -122.29))
};
``` 
Hierbij worden dus de naam van het *scoring profile* en een *scoring parameter* meegegeven. De *scoring parameter* bevat de in het portal opgegeven naam en de huidige locatie. De huidige locatie is in dit geval als vaste waarde meegegeven, maar dit kan uiteraard gekoppeld worden aan de echte locatie van een gebruiker.

---
*Heb je vragen of wil je meer weten? Neem dan contact met ons op via telefoon: 085-0443333 of via email: [info@ifective.nl](mailto:info@ifective.nl).*
