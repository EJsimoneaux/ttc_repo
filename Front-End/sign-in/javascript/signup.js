
function checkPassword(){
    const username = document.getElementById('username').value;
    const password = document.getElementById('password').value;
    if (username.toUpperCase() === 'ADMIN' && password.toUpperCase() === 'PASSWORD') {
        var url = "http://www.google.com/"; //change to admin page
        window.location.href = url, true; 
        alert("successful");
    }
    else
    {
        alert("unsuccessful");
        document.forms['login'].reset();
    }
}


