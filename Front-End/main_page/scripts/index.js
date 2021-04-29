function makeCustomer(){
    const allCustomersApiUrl = "https://localhost:5001/api/customer";
    const CustFName = document.getElementById("fName").value;
    const CustLName = document.getElementById("lName").value;
    const CustCompany = document.getElementById("company").value;
    const CustEmail = document.getElementById("email").value;
    const CustPhone = document.getElementById("phone").value;
    const CustMessage = document.getElementById("message").value;

    
    fetch(allCustomersApiUrl, {
        method: "POST",
        headers: {
            "Accept": 'application/json',
            "Content-Type": 'application/json'
        },
        body: JSON.stringify({
            fName: CustFName,
            lName: CustLName,
            company: CustCompany,
            phone: CustPhone,
            email: CustEmail,
            message: {
                message:CustMessage
            }
        })
    }).then((response) =>{
        console.log(response); 
        document.forms['new-customer'].reset();
    })
}