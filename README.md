# RESTful

A sample RESTful API.

This repository contains the source code used for the article [How to build a RESTful API — A Deep Dive into REST APIs](https://itnext.io/how-to-build-a-restful-api-a-deep-dive-into-rest-apis-215188f80854) that covers in deep elements of the web architecture that are fundamental pieces of REST architecture.

## REST Pizza

A sequence of REST requests can be made to simulate the basic flow of an order to get a pizza being made.

To keep the focus on how REST can be implemented in practice, issues related to validations and complex business rules were ignored, even with the source code well organized and structured to address the problem of this application.

The application has two main domain entities, which are *Pizza* and *Order*.

You can kick off the little journey by POST-ing some pizzas and paying attention to details such as the `Location's header`, and `HTTP Status Codes`. Then try to do some Orders using the Pizza' Ids newly created and checking the `Hypermedia Links` at the server responses.

> There is so much to talk about Hypermedia As The Engine Of Application State - I recommend you take a look at the article, buddy!


## Instructions

Although it is a .NET Core application, it runs in Docker, so you can simply run the commands below and play around with some pizzas!

`docker build -t resftulapi .`

`docker run -d -p 5001:80 --name restpizza resftulapi`

In your browser: `http://localhost:5001/swagger/index.html`
