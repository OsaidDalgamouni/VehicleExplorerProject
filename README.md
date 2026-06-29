# Vehicle Explorer

Vehicle Explorer is a  web application that allows users to search vehicle manufacturers, browse available vehicle types, and display vehicle models by year.

## Technologies Used

### Backend

* ASP.NET Core 8 Web API
* C#
* Memory Cache
* REST API

### Frontend

* Angular 13
* PrimeNG
* Bootstrap 5

### Containerization

* Docker
* Nginx


## Project Structure

```
VehicleExplorer/
│
├── API/
├── Services/
├── Domain/
├── Web/
│   └── AngVehicleExplorer/
├── docker-compose.yml
└── VehicleExplorer.sln
```

---


##url

Angular:

```
(http://13.63.155.112:4200/)
```

API:

```
http://<EC2-PUBLIC-IP>](http://13.63.155.112:5000)
```

---

## Features

* Search vehicle manufacturers
* View vehicle types for a selected manufacturer
* Filter models by manufacturing year
* Cached manufacturer list for better performance
* Responsive UI using Bootstrap and PrimeNG
* Dockerized deployment
* Hosted on AWS EC2

---
