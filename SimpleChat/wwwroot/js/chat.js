"use strict";

var connection = new signalR.HubConnectionBuilder().withUrl("/messagehub").build();

connection.start();


const hubConnection = new signalR.HubConnectionBuilder()
    .withUrl("/chat")
    .build();

let userName = '';
// получение сообщения от сервера
hubConnection.on('Receive', function (message, userName) {

    // создаем элемент <b> для имени пользователя
    let userNameElem = document.createElement("b");
    userNameElem.appendChild(document.createTextNode(userName + ': '));

    // создает элемент <p> для сообщения пользователя
    let elem = document.createElement("p");
    elem.appendChild(userNameElem);
    elem.appendChild(document.createTextNode(message));

    var firstElem = document.getElementById("chatroom").firstChild;
    document.getElementById("chatroom").insertBefore(elem, firstElem);

});

hubConnection.on('Notify', (message) => {
    setTimeout(() => {
        console.log(message);
    }, 2000)
})



// установка имени пользователя
document.getElementById("loginBtn").addEventListener("click", function (e) {
    userName = document.getElementById("userName").value;
    document.getElementById("header").innerHTML = '<h3>Welcome ' + userName + '</h3>';
});
// отправка сообщения на сервер
document.getElementById("sendBtn").addEventListener("click", function (e) {
    let message = document.getElementById("message").value;
    hubConnection.invoke("Send", message, userName);
});

hubConnection.start();