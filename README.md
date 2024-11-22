# On The Beach Technical Test

This repo includes two projects for a Holiday Search. Both projects target .NET 8.0

- HolidaySearch 
    - Contains the business logic for the search function

- HolidaySearchTests
    - Contains the Unit tests for business logic
    - Contains the Integration tests using the provided json files with the test cases described in the PDF


The holiday search was written taking in to account the following assumptions:

    - The prices in the test data were whole numbers but this may not always be the case
    - The dates (arrival and departure) needed to account for timezones offsets as the comparison between a BST and UTC time could potentially mean a different date
    - The hotel price is Price per night multiplied by the duration (nights)
    - Total Price in the response is worked out by summing the hotel price with the flight price 
    - The best value trip, i,e: the first one returned in the response - is the trip that best matches the search parameters whilst having the lowest total price




