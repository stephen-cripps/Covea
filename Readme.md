# Overview
This document covers a brief discussion of the solution architecture, how to run the program locally and the API definition. 

## Architectural Overview
The solution is designed to be easily extensible, allowing for features such as different types of applicant, applicant storage or rate card management. 

The key to the applicant design in the domain was to enable more times of applicant without violating the open closed principle. This was achieved a few key ways. The first was to use an abstract applicant. This way all applicants would have a way of calculating the gross premium, but it could be handled in a way suitable for that applicant type. Secondly, a strategy pattern was used to allow for a modular implementation of cost calculations. Applicants are then instantiated by the applicant factory, which connects all these pieces together, and can be updated as business needs require it. Building this out as a rich domain, rather than just calculating the gross premium directly from the input values, means we have the architecture needed to begin creating a database of applicants. 
I've also created a repository for the rate card. Though this currently only uses dummy data, it could be extended to allow updates to the rate card, giving us scope to create some admin-level endpoints for managing this. 

As for the rest of the architecture, I've opted to use a simple MVC pattern. If I was to be working on this further. I would separate the presentation, application domain and infrastructure into more clearly defined layers as separate projects.  

## Running the Program
### Requirements
[.net Core 3.1](https://dotnet.microsoft.com/download/dotnet/3.1)
### Commands
To run the program run the follwoing commands from the Covea.Application folder.

`dotnet build`

`dotnet run`

The endpoints will be running on localhost:5000
## API Definition
### Get Gross Premium
`GET /Controller/GrossPremium`
#### Query Parameters
Name | Type | Description | Restrictions 
 ---|---|---|---
age |int| age of applicant | between 18 and 65 inclusive
sumassured |int| applicant's sum assured | between 25,000 and 500,000 inclusive

#### Response Structure
```
{
    "age": 0,
    "sumAssured": 0,
    "premium": 0.0
}
```
#### Response Codes
 - 200
 - 400 
 - 503