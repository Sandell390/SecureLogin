

async function OnLogin(){
    
    let password = document.getElementById("password").value;
    let username = document.getElementById("uname").value;

    let inputBytes = new TextEncoder().encode(password);

    let pepper = await makeRequest('POST', '/Index/RequestPepper', JSON.stringify({username: username}));
    let pepperBytes = new TextEncoder().encode(pepper);
    if (pepper == "failed")
    {
        alert("Failed to login");
        return;
    }
    
    let hashedPassword = await window.crypto.subtle.digest("SHA-256", inputBytes);
    
    let hashedPasswordWithPepper = await window.crypto.subtle.digest("SHA-256", MergeArrayBuffer(hashedPassword, pepperBytes));

    let response = await makeRequest('POST', '/Index/Login', JSON.stringify({ username: username, password: buf2hex(hashedPasswordWithPepper) }));
    
    console.log(response);
    if (response.includes("success")){ 
        
        // Get the pepper from the response
        let pepper = response.split(":")[1];
        let pepperBytes = new TextEncoder().encode(pepper);

        // Hashing the new pepper with the password
        let newHashedPassword = await window.crypto.subtle.digest("SHA-256", MergeArrayBuffer(hashedPassword, pepperBytes));
        
        // Sending the new pepper with the password to the server so the server can update the hashed password with the new pepper
        let response2 = await makeRequest('POST', '/Index/UpdatePepper', JSON.stringify({ username: username, password: buf2hex(newHashedPassword) }));
        window.location.href = response2;
        
    } else if (response == "failed"){
        alert("Failed to login");
    }
}