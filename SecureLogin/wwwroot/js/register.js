document.getElementById("registerbtn").disabled = true;

async function OnRegister(){
    console.log("Client");

    let username = document.getElementById("uname").value;


    let password = document.getElementById("password1").value;

    let inputBytes = new TextEncoder().encode(password);

    let hashedPassword = await window.crypto.subtle.digest("SHA-256", inputBytes);

    let salt = await makeRequest('POST', '/Register/RequestSalt', null);

    let response = await makeRequest('POST', '/Register/RegisterUser', JSON.stringify({ username: username, password: buf2hex(hashedPassword) + salt }));

    console.log(response);
    if (response == "Success"){
        window.location.href = "/Index";
    }else if (response == "Username already exists"){
        document.getElementById("uname").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("registerbtn").disabled = true;
        document.getElementById("uname").value = "";
        document.getElementById("uname").placeholder = "Username already exists";
    }
}




// regex for username: must be 3 to 20 characters long, and must not contain any spaces, special characters, or emoji.
function OnInputChanged() {
    let password1 = document.getElementById("password1").value;
    let password2 = document.getElementById("password2").value;
    let username = document.getElementById("uname").value;
    
    let res = ValidPassword(password1);
    let res2 = ValidPassword(password2);
    let res3 = RegExp(/^[a-zA-Z0-9]{3,20}$/).test(username);
    
    if (res){
        document.getElementById("password1").style.backgroundColor = "rgba(0, 255, 0, 0.2)";
       
    }else if (password1 != ""){
        document.getElementById("password1").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("registerbtn").disabled = true;
    }

    if (res2){
        document.getElementById("password2").style.backgroundColor = "rgba(0, 255, 0, 0.2)";
    }else if (password2 != ""){
        document.getElementById("password2").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("registerbtn").disabled = true;
    }
    
    if (res3){
        document.getElementById("uname").style.backgroundColor = "rgba(0, 255, 0, 0.2)";
    }else if (username != ""){
        document.getElementById("uname").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("registerbtn").disabled = true;
    }
    
    if (res && res2 && res3){
        MatchPassword();
    }
}

function MatchPassword(){
    let password1 = document.getElementById("password1").value;
    let password2 = document.getElementById("password2").value;

    if (password1 == password2){
        document.getElementById("password2").style.backgroundColor = "rgba(0, 255, 0, 0.2)";
        document.getElementById("registerbtn").disabled = false;
        document.getElementById("matchPass").innerHTML = "";
    }else {
        document.getElementById("password2").style.backgroundColor = "rgba(255, 0, 0, 0.2)";
        document.getElementById("registerbtn").disabled = true;
        document.getElementById("matchPass").innerHTML = "Passwords do not match";
    }
}

