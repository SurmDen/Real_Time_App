
let userName;
let accessToken ;
let hubConnection  = new signalR.HubConnectionBuilder().withUrl('/chat', {accessTokenFactory: ()=>accessToken}).build();

const urlParams = new URLSearchParams(window.location.search);
const receiverId = urlParams.get('id');

if(sessionStorage.getItem('token')!== null){

    accessToken = sessionStorage.getItem('token');

    userName = sessionStorage.getItem('name')

    hubConnection.start();

}

let loginBox = document.querySelector('.container');
let messageBox = document.querySelector('.messanger')

function Displayer(){

    if (accessToken !== undefined) {

        loginBox.classList.add('displayer');
        
        if(messageBox.classList.contains('displayer')){
            messageBox.classList.remove('displayer');
        }
    }
    else{
    
        if(loginBox.classList.contains('displayer')){
            loginBox.classList.remove('displayer');
        }
        
        messageBox.classList.add('displayer');
    }
}

Displayer();


document.querySelector('.login-btn').addEventListener("click", async (e)=>{

    e.preventDefault(false);

    let name = document.querySelector(".name-group input").value;
    let password = document.querySelector(".password-group input").value;
    console.log('name ' + name);
    console.log('passw ' + password);


    let response =  await fetch('http://localhost:5000/api/user/login', {
        method: 'POST',
        headers: {'Content-Type' : 'application/json'},
        body : JSON.stringify({
            'Name' : name,
            'Password' : password
        })
    })

    if (response.ok) {
        let result = await response.json();

        userName = result.name;
        accessToken = result.token;

        sessionStorage.setItem('token', accessToken);

        sessionStorage.setItem('name', userName);

        console.log(accessToken);

        hubConnection.start();

        await hubConnection.invoke('Init', receiverId);

        Displayer();

        return;

    }

    console.log('error occured!!!');

});

document.querySelector('.sender').addEventListener("click", ()=>{

    let message = document.querySelector('.send-group input').value;

    document.querySelector('.send-group input').value = '';

    hubConnection.invoke('Send', message);

});

document.querySelector('.sender').addEventListener("mousedown", (e)=>{

    document.querySelector('.sender').style.backgroundColor = 'aqua';
    
});

document.querySelector('.sender').addEventListener("mouseup", (e)=>{

    document.querySelector('.sender').style.backgroundColor = 'rgb(68, 70, 71)';
    
});



document.querySelector('.login-btn').addEventListener("mousedown", (e)=>{

    document.querySelector('.login-btn').style.backgroundColor = 'aqua';
    
});

document.querySelector('.login-btn').addEventListener("mouseup", (e)=>{

    document.querySelector('.login-btn').style.backgroundColor = 'rgb(2, 51, 81)';
    
});

hubConnection.on('Receive', function(message, name){

    let output = document.createElement('div');

    if(name === userName){
        output.innerHTML = `You: ${message}`;
        output.classList.add('self-output');
    }else{
        output.innerHTML = `${name}: ${message}`;
        output.classList.add('user-output');
    }

    document.querySelector('.window').appendChild(output);

});

setTimeout(()=>{
    console.log("init started");
    
}, 3000);




