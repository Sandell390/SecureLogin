document.getElementById("loginbtn").disabled = true;

function OnInputChanged(){
    let password = document.getElementById("password").value;
    let username = document.getElementById("uname").value;
    
    let res = ValidPassword(password);
    
    if (res){
        document.getElementById("password").style.backgroundColor = "rgba(0, 255, 0, 0.2)";
        
        if (username != ""){
            document.getElementById("loginbtn").disabled = false;
        }
        
    } else if (password != ""){
        document.getElementById("password").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
    }
}

async function OnLogin(){
    
    let password = document.getElementById("password").value;
    let username = document.getElementById("uname").value;

    let inputBytes = new TextEncoder().encode(password);

    let hashedPassword = await window.crypto.subtle.digest("SHA-256", inputBytes);
    
    let salt = await makeRequest('POST', '/Index/RequestSalt', JSON.stringify({ username: username, password: "" }));
    
    if (salt == "Username does not exist"){
        document.getElementById("uname").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("loginbtn").disabled = true;
        document.getElementById("uname").value = "";
        document.getElementById("uname").placeholder = "Username does not exist";
        return;
    }
    
    let response = await makeRequest('POST', '/Index/Login', JSON.stringify({ username: username, password: buf2hex(hashedPassword) + salt }));
    
    console.log(response);
    if (response == "success"){ 
        window.location.href = "/Privacy";
    } else if (response == "failed"){
        document.getElementById("password").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("loginbtn").disabled = true;
        document.getElementById("password").value = "";
        document.getElementById("password").placeholder = "Incorrect password";
    }
}