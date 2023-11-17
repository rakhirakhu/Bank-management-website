
document.getElementById('inputBalance').addEventListener('blur', function () {
    // Get the input value
    var value = this.value.trim();

    // Check if the value is empty
    if (value === "") {
        document.getElementById('errorBalance').innerText = 'Balance is required.';
        return false;
    }

    // Check if the value is a valid number
    if (isNaN(value)) {
        document.getElementById('errorBalance').innerText = 'Balance should only contain numbers.';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorBalance').innerText = '';

    return true;
});
function validateFirstName(input) {
    // Get the input value
    var value = input.value.trim();

    // Check if the value is empty
    if (value === "") {
        document.getElementById('errorFirstName').innerText = 'Account holder name is required.';
        return false;
    }

    // Check if the value contains only letters (characters)
    var regex = /^[a-zA-Z\s]+$/;
    if (!regex.test(value)) {
        document.getElementById('errorFirstName').innerText = 'Account holder name should only contain letters and spaces.';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorFirstName').innerText = '';

    return true;
}

// Add an event listener to call the validation function on focus out
document.getElementById('inputfName').addEventListener('blur', function () {
    validateFirstName(this);
});

document.getElementById('inputPassword').addEventListener('blur', validatePassword);

function validatePassword() {
    // Get the password input value
    var password = document.getElementById('inputPassword').value.trim();

    // Check if the password is valid
    // You can replace this condition with your own password validation logic
    if (password.length < 8) {
        document.getElementById('errorPassword').innerText = 'Password should be at least 8 characters long.';
    } else {
        document.getElementById('errorPassword').innerText = '';
    }
}
function validateIFSCCode(input) {
    var ifscCode = input.value;

    // Regular expression to check for exactly 11 characters starting with "FEDRL" and followed by 6 digits
    var regex = /^FEDRL\d{6}$/;

    if (ifscCode.length !== 11 || !regex.test(ifscCode)) {
        showError("errorIFSCCode", "Please enter a valid IFSC code with 11 characters starting with 'FEDRL' and followed by 6 digits.");
    } else {
        hideError("errorIFSCCode");
    }
}

function showError(spanId, errorMessage) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = errorMessage;
}

function hideError(spanId) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = "";
}
function validateAccountNumber(input) {
    var accountNumber = input.value;

    // Regular expression to check for 14 digits starting with "22"
    var regex = /^22\d{12}$/;

    if (!regex.test(accountNumber)) {
        showError("errorAccountNumber", "Please enter a valid 14 digit account number ");
    } else {
        hideError("errorAccountNumber");
    }
}
function validateEmail(ref) {
    let error = document.getElementById("errorMail");
    const emailPattern = /^[a-zA-Z0-9._-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}$/;
    const email = ref.value.toLowerCase(); // Convert email to lowercase for case-insensitive comparison

    // Check if the email is valid
    if (!email.includes("@gmail") || !emailPattern.test(email)) {
        ref.classList.add("invalid");
        error.innerHTML = "Enter a valid email address";
        document.getElementById("Username").value = "";
    } else {
        ref.classList.remove("invalid");
        error.innerHTML = "";
        // Set the entered email as the username
        document.getElementById("Username").value = email;
    }

    // Event handler for onFocus event to clear validation error on focus
    ref.onfocus = function () {
        if (this.classList.contains("invalid")) {
            this.classList.remove("invalid");
            error.innerHTML = "";
        }
    };
}

function showError(spanId, errorMessage) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = errorMessage;
}

function hideError(spanId) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = "";
}



function validateToAccountNumber(input) {
    var accountNumber = input.value;

    // Check if the account number has exactly 14 digits
    if (/^\d{14}$/.test(accountNumber)) {
        document.getElementById('errorToAccountNumber').innerHTML = '';
    } else {
        document.getElementById('errorToAccountNumber').innerHTML = 'Enter valid 14 digit Account Number';
    }
}

function validateToIFSCCode(input) {
    var ifscCode = input.value;

    // Check if the IFSC code has exactly 11 characters
    if (ifscCode.length === 11) {
        document.getElementById('errorToIFSCCode').innerHTML = '';
    } else {
        document.getElementById('errorToIFSCCode').innerHTML = 'Enter valid Ifsc ';
    }
}

//function validateFirstName(ref) {
//    // Regular expression to check for only alphabetic characters
//    const containsOnlyAlphabets = /^[A-Za-z]+$/;

//    // Check if the first name contains only alphabetic characters
//    if (!containsOnlyAlphabets.test(ref.value)) {
//        ref.classList.add("invalid");
//        errorFirstName.innerHTML = "First name should contain only alphabetic characters";
//    } else {
//        ref.classList.remove("invalid");
//        errorFirstName.innerHTML = "";
//    }

//    // Event handler for onFocus event to clear validation error on focus
//    ref.onfocus = function () {
//        if (this.classList.contains("invalid")) {
//            this.classList.remove("invalid");
//            errorFirstName.innerHTML = "";
//        }
//    };
//}






function validateBranchName(selectElement) {
    var selectedBranch = selectElement.value;

    // Check if a valid branch is selected
    if (selectedBranch === "") {
        showError("errorBranchName", "Please select a branch.");
    } else {
        hideError("errorBranchName");
    }
}

// Function to show error message
function showError(spanId, errorMessage) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = errorMessage;
}

// Function to hide error message
function hideError(spanId) {
    var errorSpan = document.getElementById(spanId);
    errorSpan.innerHTML = "";
}

function setTodayDate() {
    // Get current date
    var today = new Date();
    var yyyy = today.getFullYear();
    var mm = String(today.getMonth() + 1).padStart(2, '0');
    var dd = String(today.getDate()).padStart(2, '0');
    var currentDate = yyyy + '-' + mm + '-' + dd;

    // Set the value of the input field to the current date
    var inputOpenDate = document.getElementById("inputOpenDate");
    inputOpenDate.value = currentDate;

    // Set the minimum and maximum date to the current date
    inputOpenDate.min = currentDate;
    inputOpenDate.max = currentDate;
}

// Call the function when the page loads
window.onload = setTodayDate;

var amountValue = $("input[name='amount']").val();
//// Make sure amountValue is not null or empty before making the AJAX request
//$.ajax({
//    type: "POST",
//    url: "/UserController/Withdraw",  // Adjust the URL accordingly
//    data: { amount: amountValue, /* other parameters */ },
//    success: function (result) {
//        // Handle success
//    },
//    error: function (error) {
//        // Handle error
//    }
//});
function validateOldPassword(input) {
    // Get the input value
    var value = input.value.trim();

    // Check if the value is empty
    if (value === "") {
        document.getElementById('errorOldPassword').innerText = 'Enter your password';
        return false;
    }

    // Clear any previous error message
    document.getElementById('errorOldPassword').innerText = '';

    return true;
}
