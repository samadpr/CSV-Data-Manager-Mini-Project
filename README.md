# CSV Data-Manager Mini Project

Introduction

The CSV Data Manager is a mini project that streamlines the process of uploading, managing, and storing CSV data efficiently. It incorporates modern web technologies to handle backend processing, front-end interactions, and asynchronous message queuing.

Technologies Used

.NET Core Web APIs for backend processing

.NET Core MVC for user interface development

RabbitMQ for message queuing

MS SQL as the database for storing and managing data

Console Application to receive messages from the queue and send data to the database through APIs

Test Cases to ensure the reliability and functionality of the APIs

Features

User Registration and Login: Users can register and log in to the system.

CSV File Upload: A dedicated page for uploading CSV files, which are then processed and stored in the database.

Message Queue Integration: Utilizes RabbitMQ for asynchronous processing of CSV data.

Database Management: Data is securely stored and managed in MS SQL.

Setup Instructions

Clone the repository.

Set up the RabbitMQ server.

Configure the MS SQL database.

Run the backend APIs using .NET Core.

Launch the MVC application for the front-end UI.

Run the console application to handle message queue processing.

Execute the provided test cases to validate the functionality.

Future Enhancements

Enhance security measures.

Implement additional data validation.

Provide detailed logging for better traceability.