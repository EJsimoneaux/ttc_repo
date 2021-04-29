function populateDashboard(){
    getBalance(); 
    // getInvoices();
    getCustomerInquries(); 
}

function getInvoices(id){
    const allPostsApiUrl = "https://localhost:5001/api/customer/getinvoices/"+id;
    fetch(allPostsApiUrl).then(function(response){
        console.log(response); 
        return response.json(); 
    }).then(function (json) {
        console.log(json);
        customerInvoices = json; 
        showInvoices(); 
    })
};

function showInvoices(){
    var modal = document.getElementById("editModal"); 
    let html = "<button class =\"close\" onclick= \"closeModal()\"><ion-icon name=\"close-circle-outline\"></ion-icon></button>"
    html += '<div class = "user-info"><h2 id = "acc"> Account Number:  ' +customerInvoices.accountNo+'</h2>'
    html += '<h2> Customer Name:  ' +customerInvoices.fName+' '+customerInvoices.lName+'</h2></div>';
    html += '<h2> Invoices With Balance:</h2>'
    customerInvoices.invoiceList.forEach(invoice => {
        html += '<div class = float-container>'; 
        html += '<div class = "float-child Descrip">';
        html += '<h3><b> Event: '+invoice.eventName + '</b></h3>'; 
        html += '<h4>Invoice No: '+invoice.invoiceNo +'</h4>';
        html += '</div>'; 
        html += '<div class = "float-child Amount">';
        html +='<h4> Amount Due: ' + formatter.format(invoice.amountDue) + "</h4><button onclick= \"getInvoiceInfo("+invoice.invoiceID+")\">"+'<ion-icon name="mail-outline"></ion-icon></button>';
        html += '</div>'; 
        html += '</div>';

    });
    html += '</div>'
    document.querySelector('.modal-content').innerHTML = html;
    modal.style.display = "block"; 

    var button = document.getElementsByClassName("close")[0]; 
}
function closeModal(){
    var modal = document.getElementById("editModal");
    modal.style.display = "none";
}
window.onclick = function(event){
    var modal = document.getElementById("editModal"); 
    if(event.target == modal){
        modal.style.display = "none"; 
    }
}
var formatter = new Intl.NumberFormat('en-US', {
    style: 'currency',
    currency: 'USD',
});

function amountDue(){

    let html = '<div class = BalanceDue>' 
    customersBalance.forEach(customer => {
        html += '<div class = float-container>';
        html += '<div class = "float-child name">';
        html += '<h3>'+ customer.fName +'</h3>'; 
        html += '<h5> Amount Due </h5>';
        html += '</div>';
        html += '<div class = "float-child Amount">';''
        html += "<button title= \"Click To See "+customer.fName+"\ Invoices\" onclick= \"getInvoices("+customer.custID+")\">"+customer.invoiceSummary.numberOfEvents+" Unpaid Event(s)</button>"; 
        html += '<h5>'+ formatter.format(customer.invoiceSummary.amountDue) +'</h5>'; 
        html += '</div>'
        html += '</div>'
    });
    html += '</div>'
    document.querySelector('.Customers-Balance').innerHTML = html;
    
}

function getBalance(){
    const allCustomerApiUrl = "https://localhost:5001/api/customer/getbalance";
    fetch(allCustomerApiUrl).then(function(response){
        console.log(response); 
        return response.json(); 
    }).then(function (json) {
        console.log(json);
        customersBalance = json; 
        amountDue(); 
    })
}

//Make New Customer Method
function makeCustomer(){
    const allCustomersApiUrl = "https://localhost:5001/api/customer";
    const CustFName = document.getElementById("FirstName").value;
    const CustLName = document.getElementById("LastName").value;
    const CustCompany = document.getElementById("Company").value;
    const CustPhone = document.getElementById("Phone").value;
    const CustEmail = document.getElementById("Email").value;
    
    
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
            email: CustEmail
        })
    })
    .then((response) =>{
        console.log(response); 
        document.forms['new-customer'].reset();
        
    })
}

//Send Customer Invoices by Email
function getInvoiceInfo(id){
    const customerApiUrl = "https://localhost:5001/api/event/getinvoice/"+id; 
    fetch(customerApiUrl).then(function(response){
        console.log(response); 
        return response.json(); 
    }).then(function (json) {
        console.log(json);
        invoice = json; 
        sendEmail(); 
    })
}
function sendEmail(){
    Email.send({
        Host: "smtp.gmail.com",
        Username : "ttowncatering@gmail.com",
        Password : "@TtownCatering",
        To : invoice.customer.email,
        From : "ttowncatering@gmail.com",
        Subject : invoice.customer.accountNo+':'+invoice.invoice.invoiceNo+' Invoice Due',
        Body : "This is the invoice content",
        }).then(
            message => alert("mail sent successfully")
        );
}

//Get Customer Inquries
function getCustomerInquries() {
    const allPostsApiUrl = "https://localhost:5001/api/customer/getinquries";
    fetch(allPostsApiUrl).then(function(response){
        console.log(response); 
        return response.json(); 
    }).then(function (json) {
        console.log(json);
        inquiries = json; 
        inquiries.reverse(); 
        populateQueries(); 
    })
}
function populateQueries(){
    let html = '<div class= customer__message><h1 class="form__title">Customer Inquiries</h1>' 
    inquiries.forEach(inquiry => {
       if (inquiry.message.status == 0) {
            html += '<li><h5 id = "name">From: ' + inquiry.fName + ' ' + inquiry.lName + '</h5></li>';
            html += '<li><label for="message" id = "message_label"> Message</label></li>'; 
            html += '<li><div id = "cust_msg" name = "message">'+ inquiry.message.message +'</div></li>'; 
            html += "<li><a id = \"contact_link\" onclick=\"readMessage("+inquiry.custID+")\" href = \"mailto:"+inquiry.email+"\">Reply</a></li>"; 
       }
    });
    html += '</div>'
    document.querySelector('.Customer-Inquiries').innerHTML = html;
}
function readMessage(id) {
    let changedStatus = 1; 
    let custMessage = document.getElementById('cust_msg').value;
    
    
}