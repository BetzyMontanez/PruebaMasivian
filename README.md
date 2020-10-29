# Roulette
This component is responsible for next features:
- Create new roulettes
- Open and close roulettes
- Make bets related to existents roulettes
- List existent roulettes

## Main dependencies
```
MongoDB.Driver==2.11.3
Microsoft.AspNet.WebApi.Core==5.2.7
Microsoft.AspNetCore.Mvc.NewtonsoftJson==3.1.9
```

## Environment variables
```
setx ROULETTE_COLLECTION_NAME Roulettes /m
setx ROULETTE_CONNECTION_STRING mongodb://localhost:27017 /m
setx ROULETTE_DATABASE?NAME RouletteDB /m
```

## API CURLs examples
### Create new roulette
```
curl --location --request POST 'http://localhost:5000/roulettes/'
```
### Open Roulette
```
curl --location --request PUT 'http://localhost:5000/roulettes/open' \
--header 'Content-Type: application/json' \
--data-raw '{
    "rouletteId": "123456"
}'
```
### Bet into a Roulette
```
curl --location --request POST 'http://localhost:5000/roulettes/placeBet' \
--header 'userId: 101010' \
--header 'Content-Type: application/json' \
--data-raw '{
    "rouletteId": "123456",
    "betPlace": "number/colour",
    "amount": 10000
}'
```
### Close Roulette
```
curl --location --request PUT 'http://localhost:5000/roulettes/close' \
--header 'Content-Type: application/json' \
--data-raw '{
    "rouletteId": "123456"
}'
```
### List Roulettes
```
curl --location --request GET 'http://localhost:5000/roulettes/'
```