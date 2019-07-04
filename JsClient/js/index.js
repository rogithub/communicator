"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("http://localhost:53505/communicator").build();

//Disable send button until connection is established

connection.on("ReceiveMessage", function (user, message) {
    alert("Message " + message);
});

connection.start().then(function(){
    document.getElementById("sendButton").disabled = false;
}).catch(function (err) {
    return console.error(err.toString());
});

document.getElementById("sendButton").addEventListener("click", function (event) {
    var user = "Rodrigo";
    var message = "Hola";
    
    connection.invoke("SendMessage", user, message).catch(function (err) {
	return console.error(err.toString());
    });
    event.preventDefault();
});


