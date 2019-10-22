# Vinland

This is a proof-of-concept for an batch-order system. It follows the FIFO principle where a batch is send out once it is complete. A special rule is that a new order containing already a full batch will be send out immediately.

## Web Application

The main application is a ASP.NET Core application including a pipeline for the Angular frontend. For simplicity reasons I have chosen to include the Angular application into the API project, but in a real world scenario they should be split. The SPA frontend application should run on its own server and the API should run on a separate server. (Hint: By server it is not meant to be split logically on different instances, but moreover as separate processes)

### The Backend

**Jorros.Vinland.API**
The *Jorros.Vinland.API* project is a REST API, which doesn't contain any business logic. It only consumes other services, that contain the business logic and access to the data layer.
Therefore it mostly uses AutoMapper to map the service models into api models and vice-versa.

Throughout the services the IOptions interface is used to manage configuration, which is read from the appsettings.json inside the API project.

In order to scale this solution one could move the implementation of the services into their own service solutions and publish an API, which then could be consumed by a new implementation of the services in this solution, allowing specific parts to be scaled. (e.g. in a case where the order processing could be expensive in terms of processing power)

The services can be split into 2 areas:

**Jorros.Vinland.OrderProcessing**
This is the main service containing the batch processing. It communicates to the data layer *Jorros.Vinland.Data* in order to save orders and their states and communicates with the shop *Jorros.Vinland.WineProviders* to handle the order. The implementation for the order logic is inside the *BatchOrderService* class. AutoMapper ensures that the entities are mapped into service models, which are eventually passed down to the controller level.

The name of the shop is called FrenchWinery and the implementation of the provider contains only mock data, so it will always confirm order requests and return a random generated order reference.

The data layer uses the repository pattern. The implementation of the repositories is done using Entity Framework Core. The API project uses the InMemory database provider. Currently the implementation and interfaces reside in the same namespace. In the future this can be split so another implementation for another database can be done, but given the size of this project and its current set of functions it seems to small to abstract the data layer that much.

**Jorros.Vinland.Pricing**
This is a small service that is responsible for calculating the costs of the order and shipping. It contains a simple logic for the calculation and the settings are taken in via IOptions. This service has no knowledge about any specifics of the OrderProcessing logic or the user.

### Unit tests
Unit tests can be found in *Jorros.Vinland.Tests*. Used libraries are NUnit, AutoFixture and moq.
The controller, repositories and the wine provider are not tested. The wine provider is a simple mock, where a suite of unit tests feels over the top. The controllers contain no real logic, it's just calling the services and mapping and the repositories are directly querying the database. For the last two integration/e2e tests would make more sense.

### The Frontend
The frontend is a simple SPA application using Angular. It communicates with the backend. As a 'login' the username is fully sufficient in this sample.
The login is asked in a dialog prior opening the main dashboard. The dashboard offers a form to create a new order and a list of all the orders the user with that name has already made and whether they are confirmed or not.


## How to run
The **Jorros.Vinland.API** is the main entry point to run this app. In case it shows an error message on startup stating that it's missing npm packages, this can be resolved by running `npm install` or `yarn install` inside the Jorros.Vinland.API/ClientApp folder.
