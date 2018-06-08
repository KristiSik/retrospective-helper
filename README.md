# RETROSPECTIVE HELPER

[![AppVeyor](https://ci.appveyor.com/api/projects/status/8mlgtyidag6gonae?svg=true)](https://ci.appveyor.com/project/mzwierzchlewski/retrospective-helper)

Deployed at: [retro-helper.azurewebsites.net](https://retro-helper.azurewebsites.net/)

## Team
Arkadiusz Sielecki

Maciej Talaga

Maciej Zwierzchlewski

# Project Documentation
## Description
Retrispective Helper is a web application created to make Sprint Retrospectives easier and better.
## Features
Account creation and Authentication.

Users are able to create projects, add its members and admins.

## Authentication
Authentication uses Bearer Tokens. After getting a token (see API reference), all requests need Bearer Token authentication headers.

## Requirements
.NET Framework 4.6.1
Visual Studio (preferably)

## Setting up a developer instance
1. Clone repo
2. Open project in Visual Studio
3. Build & run

In case of database problems - create an initial database migration and update the database.

## Deployment
### Azure
Create Azure resource - App Service with SQL - Deploy via Visual Studio by following the prompts.

### IIS
1. Publish to folder
2. Create IIS Site pointing to that folder
3. Add HTTP bindings

# API Documentation
## Account management
### Registration
Route: `POST api/account/register`

Content-Type: `application/json`

Body:
```
{
    "fullname": "Terry Jeffords",
    "email": "tjeffords@police.gov",
    "password": "yoghurt99",
    "confirmpassword": "yoghurt99"
} 
```

### Get User Information [Authorized]
Route: `GET api/account/userinfo`

### Change User Information [Authorized]
Route: `PUT api/account/changeinfo`

Content-Type: `application/json`

Body:
```
{
    "fullname": "Jake Peralta",
    "email": "tjeffords@police.gov",
} 
```

### Change Password [Authorized]
Route: `PUT api/account/changepassword`

Content-Type: `application/json`

Body:
```
{
    "oldpassword": "yoghurt99",
    "password": "amy99",
    "confirmpassword": amy99"
} 
```

### Logging Out [Authorized]
Route: `GET api/account/logout`

### Delete Account [Authorized]
Route: `DELETE api/account/deleteaccount`

## Authentication
Route: `GET Token`

Content-Type: `application/x-www-form-urlencoded`

Body:

**grant_type** `password`

**username** `tjeffords@police.gov`

**password** `yoghurt99`

Result: Bearer Token

## Project Management
#### List Projects [Authorized]
Route: `GET api/projects`

### Create Project [Authorized]
Route: `POST api/projects`

Content-Type: `application/json`

Body:
```
{
    "name": "Nine-Nine"
} 
```

### Delete Project [Authorized]
Route: `DELETE api/projects`

Content-Type: `application/json`

Body:
```
{
    "id": 1
} 
```

### Update Project [Authorized]
Route: `PUT api/projects`

Content-Type: `application/json`

Body:
```
{
    "id": 1,
    "name": "Linetti"
} 
```

### Add Member to Project [Authorized]
Route: `POST api/projects/addmember`

Content-Type: `application/json`

Body:
```
{
    "email": "rholt@police.gov",
    "projectid": "1"
}
```

### Add Admin to Project [Authorized]
Route: `POST api/projects/addadmin`

Content-Type: `application/json`

Body:
```
{
    "email": "rholt@police.gov",
    "projectid": "1"
}
```

### Remove Member from Project [Authorized]
Route: `DELETE api/projects/removemember`

Content-Type: `application/json`

Body:
```
{
    "email": "rholt@police.gov",
    "projectid": "1"
}
```

### Leave Project [Authorized]
Route: `DELETE api/projects/leave`

Content-Type: `application/json`

Body:
```
{
    "projectid": "1"
}
```
