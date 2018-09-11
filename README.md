# LNLamaScrape

This is a library to parse manga and novel sites.

## Basic Usage
```
var series = await Repositories.ReadLightNovel.GetSeriesAsync();
var chapters = await series.Result[0].GetChaptersAsync();
var pages = await chapters.Result[0].GetPagesAsync();
var pagesWithContent = await pages.Result[0].GetPageContentAsync();
```

## Save to MongoDB
```
var res = await repodb.UpdateDbPagesWithContentAsync(
    new DbConfig(
        connectionString: "YOUR_CONNECTIONSTRING",
        dbName: "YOUR_DBNAME"), pages);
```